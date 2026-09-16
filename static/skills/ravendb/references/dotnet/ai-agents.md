# AI Agents from .NET (client 7.2+; sub-agents client 7.2.2+)

The C# client wraps the AI-agent HTTP surface under `store.AI` (`AiOperations`) — prefer it over raw HTTP. Concepts, endpoint shapes, and embeddings setup: `../ai-agents.md`.

## Connection string

```csharp
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;

await store.Maintenance.SendAsync(new PutConnectionStringOperation<AiConnectionString>(
    new AiConnectionString
    {
        Name = "open-ai-chat",
        ModelType = AiModelType.Chat,
        OpenAiSettings = new OpenAiSettings(apiKey, "https://api.openai.com/v1", "gpt-4.1")
    }));
```

Exactly one settings property may be set: `OpenAiSettings`, `AzureOpenAiSettings`, `OllamaSettings`, `EmbeddedSettings`, `GoogleSettings`, `HuggingFaceSettings`, `MistralAiSettings`, `VertexSettings`.

## Define the agent

```csharp
using Raven.Client.Documents.Operations.AI.Agents;

public class AnswerSchema
{
    public string Answer = "Answer to the user question";
    public List<string> RelevantDealIds = ["Deal ids relevant to the answer"];
}

var agent = new AiAgentConfiguration("deal advisor", "open-ai-chat",
    "You are a deal advisor. When discussing deals, include their ids.");

agent.Parameters.Add(new AiAgentParameter("clientId", "Client document id"));
agent.Queries =
[
    new AiAgentToolQuery
    {
        Name = "ClientDeals",
        Description = "Open deals for the current client",
        Query = "from Deals where ClientId == $clientId",
        ParametersSampleObject = "{}"
    }
];
agent.Actions =
[
    new AiAgentToolAction("SendQuote", "Send a quote to the client")
    {
        ParametersSampleObject = "{\"amount\": 100}"
    }
];

var agentId = (await store.AI.CreateAgentAsync(agent, new AnswerSchema())).Identifier;
```

- The `sampleObject` argument serializes into the agent's output schema; every `RunAsync<T>` answer deserializes into that type. Alternatively set `agent.SampleObject` / `agent.OutputSchema` (JSON strings) and call `CreateAgentAsync(agent)`.
- `new AiAgentParameter(name, description, sendToModel: false)` hides a sensitive value from the model while queries still use it.
- Also on `AiOperations`: `GetAgentAsync(id)`, `GetAgentsAsync()`, `DeleteAgentAsync(id)`, `ForDatabase(name)`; sync twins of everything exist.
- Extras on `AiAgentConfiguration`: `SubAgents`, `ChatTrimming`, `MaxModelIterationsPerCall`, `Disabled`. A sub-agent's action surfaces to your code as `"subAgentName/ActionName"`.

## Converse

```csharp
using Raven.Client.Documents.AI;

var chat = store.AI.Conversation(agentId, "chats/deals/",
    new AiConversationCreationOptions().AddParameter("clientId", "clients/1-A"));

chat.SetUserPrompt("Summarize this client's open deals and recommend next steps.");
chat.Handle<SendQuoteArgs, SendQuoteResult>("SendQuote",
    async args => new SendQuoteResult { Sent = true });

var answer = await chat.RunAsync<AnswerSchema>();
// answer.Status == AiConversationResult.Done; answer.Answer is a typed AnswerSchema
// answer.Usage (tokens), answer.Elapsed; chat.Id, chat.ChangeVector

chat.SetUserPrompt("What about pricing?");   // follow-up turn, same conversation document
answer = await chat.RunAsync<AnswerSchema>();
```

- `RunAsync` loops model↔tool calls internally: registered `Handle` callbacks execute and their results go back to the model until `Done`. Sync `Run<T>()` exists.
- Conversation id `"chats/deals/"` gets a server-assigned tail; a fixed id resumes that conversation. Pass the stored `ChangeVector` to the `Conversation(...)` overload for optimistic concurrency.
- Deferred tools: `Receive<TArgs>(name, ...)` observes the call but leaves it open — `RunAsync` returns `Status == AiConversationResult.ActionRequired`; inspect `RequiredActions()`, later call `AddActionResponse(toolId, response)` (even from a new `Conversation` instance) and run again.
- Unregistered action from the model → `OnUnhandledAction` event fires; with no subscriber it throws. Handler errors go back to the model by default (`AiHandleErrorStrategy.SendErrorsToModel`) or throw (`RaiseImmediately`).
- Multimodal input: `AddAttachment(name, stream, contentType)`, `CopyAttachmentFrom(sourceDocumentId, fileName)`; multiple prompts via `AddUserPrompt(...)`.
- `AddArtificialActionWithResponse(toolId, response)` injects a fake tool call + result into the context (programmatic prompting).

## Streaming

```csharp
var answer = await chat.StreamAsync<AnswerSchema>(x => x.Answer,
    async chunk => Console.Write(chunk));
```

Streams one string property while the turn runs; the full typed answer still returns. The streamed property should be a simple string, ideally the first in the schema.

## Vector search (LINQ)

```csharp
var similar = await session.Query<Deal>()
    .VectorSearch(f => f.WithText(x => x.Summary).UsingTask("deals-embeddings"),
                  v => v.ByText("late delivery"),
                  minimumSimilarity: 0.75f)
    .ToListAsync();
```

`WithText` takes a property selector or field name; `.UsingTask(id)` points at a deployed embeddings-generation task (setup: `../ai-agents.md`). Value side: `ByText(text)` or `ByEmbedding(float[])`. Optional args: `minimumSimilarity`, `numberOfCandidates`, `isExact`.
