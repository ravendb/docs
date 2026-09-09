# AI Agents from Python (client 7.2+; sub-agents and typed parameters client 7.2.3+)

The Python client wraps the AI-agent HTTP surface under `store.ai` — prefer it over raw HTTP. Concepts, endpoint shapes, and embeddings setup: `../ai-agents.md`.

## Connection string

```python
from ravendb.documents.operations.ai import AiConnectionString, AiModelType
from ravendb.documents.operations.ai.open_ai_settings import OpenAiSettings
from ravendb.documents.operations.connection_string.put_connection_string_operation import (
    PutConnectionStringOperation,
)

store.maintenance.send(PutConnectionStringOperation(AiConnectionString(
    name="open-ai-chat",
    identifier="open-ai-chat",
    model_type=AiModelType.CHAT,
    openai_settings=OpenAiSettings(api_key="sk-...", endpoint="https://api.openai.com/v1", model="gpt-4o-mini"),
)))
```

## Define the agent

```python
from ravendb import AiAgentConfiguration, AiAgentParameter, AiAgentToolQuery, AiAgentToolAction

result = store.ai.add_or_update_agent(AiAgentConfiguration(
    name="dealadvisor",
    connection_string_name="open-ai-chat",
    system_prompt="You are a deal advisor. ...",
    sample_object='{"recommendation": "narrative text", "riskLevel": "low|medium|high"}',
    parameters=[AiAgentParameter("clientId", "Client document id")],
    queries=[AiAgentToolQuery(
        name="clientDeals",
        description="Open deals for the client",
        query="from Deals where ClientId == $clientId",
        parameters_sample_object="{}",
    )],
    actions=[AiAgentToolAction(
        name="sendQuote",
        description="Send a quote to the client",
        parameters_sample_object='{"amount": 100}',
    )],
))
agent_id = result.identifier
```

Output shape is `sample_object` (JSON string) or `output_schema`. Other knobs: `max_model_iterations_per_call`, `chat_trimming`, `sub_agents`. `store.ai.get_agents()` / `store.ai.delete_agent(identifier)` manage existing agents.

## Converse

```python
from ravendb import AiConversationCreationOptions
from ravendb.documents.ai.ai_conversation import AiHandleErrorStrategy

chat = store.ai.conversation(
    agent_id, "chats/dealadvisor/",       # trailing "/": server assigns the id tail
    creation_options=AiConversationCreationOptions(parameters={"clientId": "clients/1-A"}),
)
chat.set_user_prompt("Summarize this deal and recommend next steps.")
# one handler per configured action; return value is sent back to the model
chat.handle("sendQuote", lambda args: {"sent": True}, AiHandleErrorStrategy.SEND_ERRORS_TO_MODEL)

answer = chat.run()      # loops tool calls until status DONE
# answer.answer = dict per sample_object; answer.status, answer.usage, answer.elapsed
# follow-up on same chat: chat.set_user_prompt("What about pricing?"); chat.run()
```

- `handle(name, fn, error_strategy)` — the error-strategy argument is required (`SEND_ERRORS_TO_MODEL` or `RAISE_IMMEDIATELY`).
- `receive(name, fn(request, args))` gives the raw `AiAgentActionRequest`; respond with `chat.add_action_response(request.tool_id, result)`.
- Streaming: `chat.stream("recommendation", on_chunk=lambda s: ...)`.
- Attachments: `chat.add_attachment(name, bytes_or_stream)`, `chat.copy_attachment_from(doc_id, file_name)`.
- Continue an existing conversation: `store.ai.conversation(agent_id, conversation_id)`; history via `store.ai.get_conversation_messages(conversation_id)`.
- Requires RavenDB 7.1+ server and a license with the AI-agents feature; a missing agent parameter raises `MissingAiAgentParameterException`.

## Vector search

Server-side embeddings task: `AddEmbeddingsGenerationOperation(EmbeddingsGenerationConfiguration(...))` via `store.maintenance.send` (needs a second connection string with `model_type=AiModelType.TEXT_EMBEDDINGS`). Then:

```python
results = list(
    session.query_collection("Deals", Deal)
    .vector_search_text("Summary", "late delivery",
                        embedding_generation_task_identifier="deals-embeddings")
)
```

`vector_search(field, [0.1, ...])` queries a float32 embedding field directly; `_i8` / `_i1` / `_with_base64` variants cover quantized and pre-encoded vectors. Static-index vector fields: `self._vector(field, VectorOptions(...))` on an `AbstractIndexCreationTask` with `CreateVector(...)` in the C# map.
