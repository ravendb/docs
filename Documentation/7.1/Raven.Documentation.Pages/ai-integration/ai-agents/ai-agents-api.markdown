# AI agents: API
---

{NOTE: }

* In this article:
   * [AI Agents: overview](../../ai-integration/ai-agents/ai-agents-api#create-ai-agent)  
   * [Creating a connection string](../../ai-integration/ai-agents/ai-agents-api#create-ai-agent)  
   * [Defining and running an AI agent](../../ai-integration/ai-agents/ai-agents-api#configure-basic-settings)
      * [Define agent configuration](../../ai-integration/ai-agents/ai-agents-api#configure-basic-settings)
      * [Create the agent](../../ai-integration/ai-agents/ai-agents-api#configure-basic-settings)
   * [Conversations](../../ai-integration/ai-agents/ai-agents-api#set-agent-parameters)
   * [Defining agent tools](../../ai-integration/ai-agents/ai-agents-api#define-agent-tools)
      * [Query tools](../../ai-integration/ai-agents/ai-agents-api#add-new-query-tool)
      * [Action tools](../../ai-integration/ai-agents/ai-agents-api#add-new-action-tool)
   * [Get an agent](../../ai-integration/ai-agents/ai-agents-api#set-chat-persistence)
{NOTE/}

---

{PANEL: AI Agents: overview}

an overall look at the process of the creation of an AI agent

{PANEL/}

{PANEL: Creating a connection string}

to create a connection string that your agent will use to connect the LLM, Create an 
`AiConnectionString` connection string using the `PutConnectionStringOperation` operation.  

You can connect a local `Ollama` application if your considerations are mainly speed, cost, 
open-source, or security, or a remote `OpenAI` service for its additional resources and capabilities.  

---

### Example:

{CODE-TABS}
{CODE-TAB:csharp:open-ai-cs ai-agents_create-connection-string_open-ai@AiIntegration\AiAgents\AiAgents.cs /}
{CODE-TAB:csharp:ollama-cs ai-agents_create-connection-string_ollama@AiIntegration\AiAgents\AiAgents.cs /}
{CODE-TABS/}

---

### Syntax:

{CODE-TABS}
{CODE-TAB:csharp:open-ai-cs-syntax ai-agents_connection-string_syntax_open-ai@AiIntegration\AiAgents\AiAgents.cs /}
{CODE-TAB:csharp:ollama-cs-syntax ai-agents_connection-string_syntax_ollama@AiIntegration\AiAgents\AiAgents.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Defining and running an AI agent}

## Define agent configuration

To define an AI agent create a new `AiAgentConfiguration` class.  
While creating the class, pass its constructor the agent's Name, a reference to the 
[connection string]() you created, and a System prompt.  
The agent will send the system prompt you define here to the LLM, to define the LLM's role 
and explain it how this role should be fulfilled.  

In the following example we assign the LLM the role of a simple assistant in a university 
library that has access to a database of documents and can select and summarize for its user 
a single document in a requested subject.

{CODE ai-agents_AiAgentConfiguration_example@AiIntegration\AiAgents\AiAgents.cs /}

* Method definition:  
  {CODE ai-agents_AiAgentConfiguration_definition@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentConfiguration` class definition:
  {CODE ai-agents_AiAgentConfiguration-class_definition@AiIntegration\AiAgents\AiAgents.cs /}

---

Once the agent configuration is created, we need to add it a few additional elements.  

## Agent ID

Use the `Identifier` property to provide the agent with a unique ID that the 
system will recognize it by.  
{CODE ai-agents_agent-identifier@AiIntegration\AiAgents\AiAgents.cs /}

## Add agent parameters

Agent parameters are optional variables, whose values are provided by the client 
to the agent when chats are initiated. They are then passed by the agent to the LLM, 
allowing user input to take part in the interaction. They can be big help in focusing 
LLM querying and considerations on user input or live client data.  

Add agent parameters using the `AiAgentParameter` method.  
Pass it the name of the parameter you want to define, and instructions for the LLM, 
written in natural language.  

* Example:
  {CODE ai-agents_AiAgentParameter_function@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentParameter` Definition:
  {CODE ai-agents_AiAgentParameter_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Add Sample object or Schema

At the end of a chat, when the LLM is done with all its interactions and work, 
it returns its reply through the agent to the client in an object that we need 
to prepare beforehand and send the LLM when the agent is started.  

We can prepare this object in two different formats.  
The first format is the formal **JSON-based schema** in which the LLM expects 
the structure to arrive.  
The second is a friendlier **sample object** format, that RavenDB will turn 
to a schema behind the scenes for us.  
Normally the simpler way is to prepare a sample object and let RavenDB do 
the schema-preperation work for us, but both options are still available.  

If we prepare both a sample object and a schema, the schema will be used.  

Example:
{CODE-TABS}
{CODE-TAB-BLOCK:json:sample-object}
{
    "DocumentName": "The document's original title",
    "DocumentSummary": "The summarized document"
}
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:json:schema}
{
  "name": "QSt6RSthRGQ5OEVhNTBETTJSOHhUaWc5VDRocXV6MjM0OU85M2tJYnhMbz0",
  "strict": true,
  "schema": {
    "type": "object",
    "properties": {
      "DocumentName": {
        "type": "string",
        "description": "The document\u0027s original title"
      },
      "DocumentSummary": {
        "type": "string",
        "description": "The summarized document"
      }
    },
    "required": [
      "DocumentName",
      "DocumentSummary"
    ],
    "additionalProperties": false
  }
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

To prepare a sample object, use the agent's `SampleObject` string property.  
To prepare a schema, use `OutputSchema`.  
{CODE ai-agents_agent_sampleObjectString@AiIntegration\AiAgents\AiAgents.cs /}


## Add agent tools

You can enhance your agent with Query and Action tools, that allow the LLM 
to query your database and trigger client actions.  

After defining agent tools and submitting them to the LLM, it is up to the LLM 
to decide if and when to use them. 

### Query tools

Query tools provide the LLM the ability to acecess the database. Once defined, 
it uses them of its own accord. They include a natural language descrition that 
explain the LLM what they are for, and an RQL query that the agent, not the LLM, 
will apply if the LLM request it to use a tool.  
They also include a schema and a sample object for retrieved results. Here too, 
these objects are applied by the agent, which runs the query, retrieves the results 
into the schema or the sample object, and sends the results to the LLM.  
You can choose for yourself whether to apply a schema or a sample object, but if 
you define both only the schema will be used.  


* Example: 
  The following query tool is used by an agent that functions as a students library assistant.  
  The tool uses the `$subject` agent parameter, defined earlier (whose value a user typed 
  in or a client passed to the agent), and searches a documentation database for documents 
  whose names include this subject. The tool then loads the titles and IDs of these documents.  
  The system prompt, defined above, will use this tool to prepare a list of documents and 
  an action tool, defined below, to load the text of one of them.  
  {CODE ai-agents_agent_query-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  Query tools are defined in a list of `AiAgentToolQuery` classes.  
  {CODE ai-agents_AiAgentToolQuery_definition@AiIntegration\AiAgents\AiAgents.cs /}


### Action tools

Action tools allow the LLM to trigger the client to perform actions like modifying, adding, 
or removing documents, or whatever other operations the client is permitted to perform. 
The LLM cannot modify the database itself, it can only request the agent to do so.  
An action tool includes no query, but just a natural language description that informs 
the LLM of this tool's capabilities, and a schema or a sample object that the LLM can 
populate and send the agent as a request to apply the action.  

* The following action tool gets from the LLM an ID of a single document (retrieved by 
  the query tool defined above), and requests the client to load the document's text to 
  the LLM.  
  The system prompt, defined above, combines all these operations: retrieves a selected 
  document's ID using a query tool, retrieves the document's text using this action tool, 
  and summarizes the document for the user.  
  {CODE ai-agents_agent_action-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  Action tools are defined in a list of `AiAgentToolAction` classes.  
  {CODE ai-agents_AiAgentToolAction_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Create the agent

Now that the agent configuration is ready, we can create the agent using the `CreateAgentAsync` operation.  
`CreateAgentAsync` has several overloads that we can use, including two that use the return schema defined 
within the agent configuration and one that passes the schema to the method as part of the agent creation.  

* Example:  
  {CODE ai-agents_CreateAgentAsync_example@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:  
  {CODE ai-agents_CreateAgentAsync_overloads@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

{PANEL: Conversations}

### Chats
A chat is a communication session between the client, the agent, and the LLM, 
during which the LLM may use agent tools to interact with the database and the client.  

If [agent parameters]() were defined, the agent will wait for their values when the 
chat begins. Parameter values can be provided by the client application, or a user may 
be using the client to enter them.  

### Continuous conversations
The LLM does not record its chats, and opening a new chat means losing the chat history.  
The AI agent allows a continuous, multi-chat conversation, by storing chats history in 
the `@conversations` collection. When a new chat starts, it continues where it left off 
because the agent sends the conversation history to the LLM.  

### Stored conversations' Prefix and IDs
Conversations are kept in the `@conversations` collection with a prefix (such as `chats/`) 
that can be set when the conversation is initiated. The conversation ID after the set prefix 
is given to the conversation automatically, similarly to the automatic IDs given to documents.  

## Set a chat session and run it

- Set a chat session using the `Conversation` method.  
  Pass it the **agent ID**, the **ID of the conversation** you want to continue - 
  or just the prefix so a new conversation will be automatically created, and 
  **agent parameters' values** in a `AiConversationCreationOptions` class.  
- Set the user prompt using the `SetUserPrompt`method.  
  The user prompt updates the agent with the user's expectations for this session.  
- Use the value returned by `Conversation` to run the chat.

In the example below a new conversation is set by providing no conversation ID, 
using an empty prefix, 
without an ID and a new conversation will be stored in `@conversations`.  

* Example: 
  {CODE ai-agents_Conversation_example@AiIntegration\AiAgents\AiAgents.cs /}

* `Conversation` Definition:  
  {CODE ai-agents_Conversation_definition@AiIntegration\AiAgents\AiAgents.cs /}

* `SetUserPrompt` Definition:  
  {CODE ai-agents_SetUserPrompt_definition@AiIntegration\AiAgents\AiAgents.cs /}
  
{PANEL: Get an agent}
{PANEL/}

{PANEL: Full Example}

{CODE ai-agents_full-example@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}
