# AI agents: API
---

{NOTE: }

* A RavenDB **AI Agent** can be used by a RavenDB client to invoke a chat or 
  a continuous conversation between the client, an AI model, and a RavenDB database.  

* The AI agent can provide the AI model with a set of query and action tools that 
  the LLM can use freely to access the database or trigger the user to action.  

* In this article:
   * [Creating a connection string](../../ai-integration/ai-agents/ai-agents-api#creating-a-connection-string)  
   * [Defining and running an AI agent](../..ai-integration/ai-agents/ai-agents-api#defining-and-running-an-ai-agent)
      * [Define agent configuration](../../ai-integration/ai-agents/ai-agents-api#define-agent-configuration)
      * [Set Agent ID](../../ai-integration/ai-agents/ai-agents-api#set-agent-id)
      * [Add agent parameters](../../ai-integration/ai-agents/ai-agents-api#add-agent-parameters)
      * [Set maximum number of iterations](../../ai-integration/ai-agents/ai-agents-api#set-maximum-number-of-iterations)
      * [Set chat trimming configuration](../../ai-integration/ai-agents/ai-agents-api#set-chat-trimming-configuration)
      * [Add Sample object or Schema](../../ai-integration/ai-agents/ai-agents-api#add-sample-object-or-schema)
      * [Add agent tools](../../ai-integration/ai-agents/ai-agents-api#add-sample-object-or-schema)
         * [Query tools](../../ai-integration/ai-agents/ai-agents-api#query-tools)
         * [Action tools](../../ai-integration/ai-agents/ai-agents-api#query-tools)
      * [Create the agent](../../ai-integration/ai-agents/ai-agents-api#query-tools)
   * [Conversations](../../ai-integration/ai-agents/ai-agents-api#conversations)
      * [Chats](../../ai-integration/ai-agents/ai-agents-api#chats)
      * [Continuous conversations](../../ai-integration/ai-agents/ai-agents-api#chats)
      * [Stored conversations' Prefix and IDs](../../ai-integration/ai-agents/ai-agents-api#stored-conversations-prefix-and-ids)
      * [Set and start the chat](../../ai-integration/ai-agents/ai-agents-api#set-and-start-the-chat)
      * [Chat reply](../../ai-integration/ai-agents/ai-agents-api#chat-reply)
   * [Full Example](../../ai-integration/ai-agents/ai-agents-api#full-example)
{NOTE/}

---

{PANEL: Creating a connection string}

Your agent will need a connection string to connect with the LLM. Create a connection string 
using an `AiConnectionString` instance and the `PutConnectionStringOperation` operation.  

You can use a local `Ollama` model if your considerations are mainly speed, cost, open-source, or security,  
Or you can use a remote `OpenAI` service for its additional resources and capabilities.  

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

To create an AI agent you need to prepare an **agent configuration** and populate it with 
your settings and tools.  

Start by creating a new `AiAgentConfiguration` instance.  
While creating the instance, pass its constructor:  

- The agent's Name  
- The [connection string]() you created  
- A System prompt  

The agent will send the system prompt you define here to the LLM, to define the LLM's role 
and explain it how this role should be fulfilled.  

In the following example we create a system prompt that assigns the LLM the role of a helper 
to a human experience manager, who wants to learn which employee made the largest profit so 
the company can reward them.  

The LLM will retrieve from the Orders collection the IDs of employees and the sums they made 
in each order, and calculate which employee has made the biggest profit. Then, the LLM will 
find what kind of vacations or other rewards are available in the employee's region, and 
suggest them in its reply.  

We are careful not to send the LLM details about the employees or about the orders they made.  

{CODE ai-agents_AiAgentConfiguration_example@AiIntegration\AiAgents\AiAgents.cs /}

* Method definition:  
  {CODE ai-agents_AiAgentConfiguration_definition@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentConfiguration` definition:
  {CODE ai-agents_AiAgentConfiguration-class_definition@AiIntegration\AiAgents\AiAgents.cs /}

---

Once the agent configuration is created, we need to add it a few additional elements.  

## Set agent ID

Use the `Identifier` property to provide the agent with a unique ID that the 
system will recognize it by.  
{CODE ai-agents_agent-identifier@AiIntegration\AiAgents\AiAgents.cs /}

## Add agent parameters

Agent parameters are optional variables, whose values are provided by the client 
(or by a user through the client) to the agent when a chat is started.  
The values are then embedded by the client in query tools, and used when these 
tools are applied. Users and clients can provide their selections and preferences 
using agent parameters when the chat starts, and focus the queries and the whole 
interaction on their selections.  

In the example shown below, for example, an agent parameter is used to determine 
what are of the world a query will handle. Another example can be a student that 
uses a librarian agent, providing the agent with a subject and having the agent 
fetch and summarize documents related to this subject.  

To add an agent parameter create an `AiAgentParameter` instance, initialize it with 
the parameter's **name** and **description** (explaining the LLM what the parameter 
is for), and pass this instance to the `agent.Parameters.Add` method.  

* Example:
  {CODE ai-agents_AiAgentParameter_function@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentParameter` Definition:
  {CODE ai-agents_AiAgentParameter_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Set maximum number of iterations

You can limit the number of times that the LLM is allowed to request the usage of 
agent tools in response to a user prompt.  
To change this limit use `MaxModelIterationsPerCall`.  

* Example:
  {CODE ai-agents_MaxModelIterationsPerCall_function@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentParameter` Definition:
  {CODE ai-agents_MaxModelIterationsPerCall_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Set chat trimming configuration

Since the LLM keeps no history of previous chats, RavenDB updates it with the history 
of the conversation when you choose to start a chat from where you left off.  
You can reduce the size of the prompt that is sent to the LLM with the conversation history 
by either summarizing or truncating older messages.  
This can be helpful when transfer rate and cost are a concern or the context may become 
too large to handle efficiently.  

To summarize or truncate old messages create an `AiAgentChatTrimmingConfiguration` instance, 
use this instance to configure your trimming strategy, and set the agent's `ChatTrimming` property 
with the instance.  

When creating the instance, pass its constructor either a `Truncate` or a `Summarize` strategy.
**Summarization strategy** is set using a `AiAgentSummarizationByTokens` class.  
**Truncation strategy** is set using a `AiAgentTruncateChat` class.  
Note that you need to select a single strategy, you cannot use both strategies at the same time.  

A history of the original messages, before summarizing or truncating them, can optionally 
be kept in the `@conversations-history` collection.  
To determine whether to keep the original messages and for how long, also pass the 
`AiAgentChatTrimmingConfiguration` constructor an `AiAgentHistoryConfiguration` instance 
with your history settings.  

* Example:
  {CODE ai-agents_trimming-configuration_example@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  {CODE ai-agents_trimming-configuration_syntax@AiIntegration\AiAgents\AiAgents.cs /}


## Add Sample object or Schema

At the end of a chat, when the LLM is done with all its interactions and work, 
it returns its reply through the agent to the client in an object whose layout 
we need to prepare beforehand. The layout object we prepare is sent to the LLM 
when the agent is started.  

You can prepare this object in two different formats.  
The first format is the formal **JSON-based schema** in which the LLM expects 
the structure to arrive.  
The second is a friendlier **sample object** format, that RavenDB will turn 
to a schema behind the scenes for us.  
Normally the simpler way is to prepare a sample object and let RavenDB translate 
it to a schema for you.  

If you prepare both a sample object and a schema, the schema will be used.  

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

Query tools provide the LLM with the ability to retrieve data from the database.  
Once defined, the LLM uses them of its own accord.  
A query tool include a a natural-language **descrition** that explain the LLM 
what it is for, and an **RQL query** that the agent will run when the LLM requests 
it apply thetool. The LLM does not have access to the RQL query and cannot change 
it, and the only way to change values in these queries is via agent parameters.  

A query tool also includes a schema or a sample object that set the layout for 
the results that it retrieves. The agent will run the query, retrieve the results 
into the schema or the sample object, and send them to the LLM.  
You can choose for yourself whether to apply a schema or a sample object, just 
be aware that if you define both only the schema will be used.  


* **Example**:  
  The following query tools are used by an agent that helps a human experience manager 
  learn which employee made the largest profit. They use a `$country` agent parameter whose 
  value the manager provides when the chat is started. If the parameter's value is "everywhere" 
  the first tool is applied, and if it is a specific country the second tool is applied.  
  The tools retrieve all the orders that were sent to the selected destination.  
  As instructed in the system prompt, the LLM will then calculate which employee 
  made the largest profit, and send its ID to the user through an action tool (explained below).  
  {CODE ai-agents_agent_query-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* **Syntax**:
  Query tools are defined in a list of `AiAgentToolQuery` classes.  
  {CODE ai-agents_AiAgentToolQuery_definition@AiIntegration\AiAgents\AiAgents.cs /}


### Action tools

Action tools allow the LLM to trigger the client to perform actions like modifying, adding, 
or removing documents. They can also be used to make the user query the database using values 
determined by the LLM or by query toold. Since query tools do not interact with each other, 
and the LLM can not alter queries, an easy way to query a value that was found on the fly 
is to apply an action tool and let the client perform whatever action is needed before resuming 
the chat.  

Unlike a query tool, an action tool does not include a query. It includes a natural language 
description that informs the LLM what its capable of, and a schema or a sample object that 
the LLM will fill with values and send to the agent as a request to apply the action.  

When the LLM sends the agent a request to use an action tool, the chat will halt and 
wait for the agent's response. The agent will perform the action (or not), populate 
the object whose layout is determined by the sample object or the schema, and reply 
to the LLMwith the response.  

* The following action tool gets from the LLM an ID of an employee and transfers an 
  object with this ID to the client. The client will need to fetch an employee's living 
  region and resume the chat with a prompt that includes the requested data.  
  As defined by the system prompt, this data will be used by the LLM to find a vacation 
  or another present for the employee based on its location, in reward for the employee's 
  performance.  
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
chat begins. Parameter values can be provided directly by the client application, or 
a user may use the client to enter them.  

### Continuous conversations
The LLM does not record its chats, and opening a new chat means losing the chat history.  
The AI agent allows a continuous, multi-chat conversation, by storing chats history in 
the `@conversations` collection. When a new chat starts, it continues where it left off 
because the agent sends the conversation history to the LLM.  

### Stored conversations' Prefix and IDs
Conversations are kept in the `@conversations` collection with a prefix (such as `chats/`) 
that can be set when the conversation is initiated. The conversation ID that follows the prefix 
is created automatically by RavenDB, similarly to the creation of automatic IDs for documents.  

## Set and start the chat

- Set a chat using the `store.AI.Conversation` method.  
  Pass `Conversation`:  
   - The **agent ID**  
   - The **conversaion ID**  
     If you pass the method the ID of an existing conversation (e.g. `"Chats/0000000000000008883-A"`) 
     the conversation will be retrieved from storage and continued where you left off.  
     If you provide an empty prefix (e.g. `"Chats/`), a new conversation will start.  
   - **agent parameter values** in an `AiConversationCreationOptions` instance.  
- Set the user prompt using the `SetUserPrompt`method.  
  The user prompt informs the agent with the user's requests and expectations for this chat.  
- Use the value returned by the `Conversation` method to run the chat.

### Chat reply

LLM replies are returned by the agent to the client in an `AiAnswer` object.  
The conversation status is indicated by `AiAnswer.AiConversationResult`.   

* `AiAnswer`syntax:
  {CODE ai-agents_AiAnswer@AiIntegration\AiAgents\AiAgents.cs /}

First, check the conversation status to see if this is the agent's final response 
or whether it just requests the client to apply an action tool.  

- A request for action is relayed by an `AiConversationResult.ActionRequired` status, 
  with any additional details stored in `AiAnswer.Answer` - within the return object 
  that you defined for the action tool.  
  To send requested data to the agent, use `chat.SetUserPrompt` to set the data as 
  the user prompt and use `chat.RunAsync` to resume the chat. The prompt will be sent 
  to the LLM by the agent and the chat will continue.  

- A final response from the LLM is relayed by a `AiConversationResult.Done` status, 
  with the LLM's message stored in `AiAnswer.Answer` - within the return object that 
  you defined for the agent.  

In the example below the chat is initiated with a user prompt and an agent parameter 
from the human experience manager, requesting the LLM to check which employee made 
the largest profit with orders sent to the selected country (or "everywhere" for the 
entire world).  
The reply is then checked:  
If this is an action request, the ID of the highest-grossing employee is retrieved 
from the answer, additional data is retrieved from the database based on this ID, and 
the chat is resumed with the retrieved data as a user prompt.  
If this is the final LLM response, the LLM suggestions regarding a reward for the 
employee are also provided in the answer and can be handled further.  

* Example: 
  {CODE ai-agents_Conversation_example@AiIntegration\AiAgents\AiAgents.cs /}

* `Conversation` Definition:  
  {CODE ai-agents_Conversation_definition@AiIntegration\AiAgents\AiAgents.cs /}

* `SetUserPrompt` Definition:  
  {CODE ai-agents_SetUserPrompt_definition@AiIntegration\AiAgents\AiAgents.cs /}
  
{PANEL/}

{PANEL: Full example}

The agent in this example is a library assistant with access to a documentation database.  
Its users pass it a subject, and the agent searches the database for documents in this 
subject by their title. When it finds a document that suits the user it gives the user 
asummary of the document. It uses a query tool to search the documents, and an action 
tool to trigger the user to retrieve for it the text from the document.  

{CODE ai-agents_full-example@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

## Related Articles

### AI Agents

- [AI Agents Studio](../../ai-integration/ai-agents/ai-agents-studio)
