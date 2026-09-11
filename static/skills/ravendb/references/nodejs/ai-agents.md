# AI Agents from Node.js (client 7.2+)

The npm client wraps the AI-agent HTTP surface under `store.ai` — prefer it over raw HTTP. Concepts, endpoint shapes, and embeddings setup: `../ai-agents.md`.

## Define the agent

```js
const result = await store.ai.createAgent({
    name: "dealadvisor",
    connectionStringName: "open-ai-chat",
    systemPrompt: "You are a deal advisor. ...",
    parameters: [{ name: "clientId", description: "Client document id" }],
    queries: [{
        name: "clientDeals",
        description: "Open deals for the client",
        query: "from Deals where ClientId == $clientId",
        parametersSampleObject: "{}"
    }],
    actions: [],
}, { recommendation: "narrative text", riskLevel: "low|medium|high" });   // sampleObject → output schema
const agentId = result.identifier;
```

Connection string setup uses the standard `PutConnectionStringOperation` with `{ type: "Ai", modelType: "Chat", name, openAiSettings: { apiKey, endpoint, model } }` via `store.maintenance.send(...)`.

## Converse

```js
const chat = store.ai.conversation(agentId, "chats/dealadvisor|", {
    parameters: { clientId: "clients/1-A" }
});
chat.setUserPrompt("Summarize this deal and recommend next steps.");
chat.handle("sendQuote", async (args) => ({ sent: true }));   // one handler per configured action
const answer = await chat.run();       // loops tool calls until status "Done"
// answer.answer = typed object per sampleObject; answer.usage, answer.elapsed
// follow-up: chat.setUserPrompt("What about pricing?"); await chat.run();
```

Streaming variant: `chat.stream("recommendation", chunk => ...)`.

## Vector search

```js
session.query({ collection: "Deals" })
    .vectorSearch(f => f.withText("Summary").usingTask("deals-embeddings"),
                  v => v.byText("late delivery"));
```
