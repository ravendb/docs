# RavenDB AI Agents (7.1+)

How to define an AI agent in RavenDB and run conversations with it — concepts and HTTP surface; client-library wrappers live in your language's directory (`nodejs/ai-agents.md`, `dotnet/ai-agents.md`, `python/ai-agents.md`).

## Concepts

| Piece | What it is |
|---|---|
| AI connection string | Named config pointing at one model provider (`type: "Ai"`, `modelType: "Chat"`). Exactly one of: `openAiSettings`, `azureOpenAiSettings`, `ollamaSettings`, `googleSettings`, `mistralAiSettings`, `huggingFaceSettings`, `vertexSettings`, `embeddedSettings`. |
| Agent configuration | Server-stored: `name`, `connectionStringName`, `systemPrompt`, output shape (`sampleObject` or `outputSchema`), `parameters`, `queries`, `actions`, optional `subAgents`, `chatTrimming`, `maxModelIterationsPerCall`. Creating returns an `identifier` — use it for all later calls. |
| Parameters | Values the caller must supply when opening a conversation; referenced by the agent's queries (`$param`). `sendToModel: false` hides a value from the LLM while queries can still use it. |
| Queries | RQL the *server* runs when the model asks (database-side tools): `{ name, description, query, parametersSampleObject | parametersSchema }`. |
| Actions | Client-side tools: the model calls them, *your app* computes the result and sends it back. Same shape minus `query`. |
| Conversation | A document (id like `chats/dealadvisor/`... or `prefix|` for server-generated id) holding chat state; each run returns a `changeVector` for optimistic concurrency. |

Requires v7.1 or later (endpoints absent in 7.0). **License-gated** (`HasAiAgent`) — verify with `GET /license/status` before use; a disabled license reports the feature unavailable. Embeddings generation is separately gated (`HasEmbeddingsGeneration`), as is GenAI (`HasGenAi`).

**Agents are server-side objects: the connection string and every agent configuration must be PUT to the server, not just written into app code.** The run endpoint 404s on an agent that was never created. A stub model provider is fine for tests (`embeddedSettings` or a stub connection string), but it still has to exist on the server.

Before inventing provider settings, look for a working connection string already on the server with the right `modelType` and provider — `GET /databases` for the names, then `GET /databases/{db}/admin/connection-strings` — and mirror it.

## HTTP endpoints

| Route | Verb | Purpose |
|---|---|---|
| `/databases/{db}/admin/connection-strings` | PUT | Store the AI connection string |
| `/databases/{db}/admin/ai/agent` | PUT | Create/update agent (body = agent configuration); returns `{ Identifier, RaftCommandIndex }` |
| `/databases/{db}/admin/ai/agent?agentId={id}` | GET / DELETE | Read (`{ AiAgents: [...] }`) / delete |
| `/databases/{db}/ai/agent?conversationId={cid}&agentId={id}[&changeVector=..][&streaming=true&streamPropertyPath=..]` | POST | Run one conversation turn |

Run-turn body / response:

```json
{ "UserPrompt": [{ "Type": "text", "Text": "..." }],
  "ActionResponses": [{ "ToolId": "...", "Content": "json string" }],
  "ArtificialActions": null,
  "CreationOptions": { "Parameters": { "clientId": { "Value": "clients/1-A", "SendToModel": true } },
                       "ExpirationInSec": null, "MaxModelIterationsPerCall": null } }
```

```json
{ "ConversationId": "...", "ChangeVector": "...",
  "Response": { "your output schema": "..." },
  "ActionRequests": [{ "Name": "...", "ToolId": "...", "Arguments": "json string" }],
  "Usage": {}, "TotalUsage": {}, "Elapsed": "00:00:02" }
```

`ActionRequests` non-empty means: run each action, POST again with matching `ActionResponses` and no new prompt. With `streaming=true` the response is line-delimited: JSON-string chunks of the streamed property, then one final `{...}` result object.

## Embeddings + vector search (brief) — server 7.0+, embeddings generation license-gated (`HasEmbeddingsGeneration`)

Same `AiConnectionString` with `modelType: "TextEmbeddings"` feeds an embeddings-generation task (`AddEmbeddingsGenerationOperation`, PUT `/databases/{db}/admin/etl`): `{ EtlType: "EmbeddingsGeneration", name, identifier, connectionStringName, collection, embeddingsPathConfigurations, quantization, chunkingOptionsForQuerying }`. Over raw HTTP the `EtlType` discriminator is mandatory — omit it and the PUT fails with `"ETL configuration must have EtlType field"`, and a wrong value gives `"Unknown ETL type"`. `"GenAi"` is a **different** feature on the same endpoint with its own licence flag (`HasGenAi`), so a 402 naming Gen AI means you sent the wrong type, not that embeddings generation is unavailable — that one is `HasEmbeddingsGeneration`. Query with RQL:

```
from "Deals" where vector.search(embedding.text(Summary, ai.task('deals-embeddings')), $q)
```

Working applications built on this surface — agents with sub-agents, GenAI tasks, streaming, conversation storage — are listed in [samples.md](samples.md); `samples-fit` and `samples-verity` are the deepest.
