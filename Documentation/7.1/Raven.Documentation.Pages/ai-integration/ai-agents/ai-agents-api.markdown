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
(You can also create a connection string using Studio, see [here](../../ai-integration/ai-agents/ai-agents-studio#configure-basic-settings))

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
- The [connection string](../../ai-integration/ai-agents/ai-agents-api#creating-a-connection-string) you created  
- A System prompt  

The agent will send the system prompt you define here to the LLM, to define the LLM's role 
and explain it how this role should be fulfilled.  

In the following example we create a system prompt that assigns the LLM the role of a helper 
to a human experience manager, who wants to reward the employee who made the biggest profit.

The LLM will retrieve from the Orders collection the IDs of employees and the sums they made 
for each order, to find which employee has made the biggest profit. Then, the LLM will retrieve 
additional information about this employee from the database and suggest the manager a reward.  

We are very careful and selective with the details we send the LLM about employees and orders.  

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

In the example shown below, an agent parameter is used to determine what area 
of the world a query will handle. Another example can be a student that uses 
a librarian agent, providing the agent with a subject and having the agent fetch 
and summarize documents related to this subject.  

To add an agent parameter create an `AiAgentParameter` instance, initialize it with 
the parameter's **name** and **description** (explaining the LLM what the parameter 
is for), and pass this instance to the `agent.Parameters.Add` method.  

* Example:
  {CODE ai-agents_AiAgentParameter_function@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentParameter` Definition:
  {CODE ai-agents_AiAgentParameter_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Set maximum number of iterations

You can limit the number of times that the LLM is allowed to request the usage of 
agent tools in response to a single user prompt.  
To change this limit use `MaxModelIterationsPerCall`.  

* Example:
  {CODE ai-agents_MaxModelIterationsPerCall_function@AiIntegration\AiAgents\AiAgents.cs /}

* `AiAgentParameter` Definition:
  {CODE ai-agents_MaxModelIterationsPerCall_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Set chat trimming configuration

Since the LLM keeps no history of previous chats, we include in every new message we send 
it the entire conversation from its start.  
To save traffic and tokens, you can summarize older messages.  
This can be helpful when transfer rate and cost are a concern or the context may become 
too large to handle efficiently.  

To summarize old messages create an `AiAgentChatTrimmingConfiguration` instance, 
use this instance to configure your trimming strategy, and set the agent's `ChatTrimming` property 
with the instance.  

When creating the instance, pass its constructor a summarization strategy using 
a `AiAgentSummarizationByTokens` class.  

A history of the original messages, before they were summarized, can optionally be 
kept in the `@conversations-history` collection.  
To determine whether to keep the original messages and for how long, also pass the 
`AiAgentChatTrimmingConfiguration` constructor an `AiAgentHistoryConfiguration` instance 
with your history settings.  

* Example:
  {CODE ai-agents_trimming-configuration_example@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  {CODE ai-agents_trimming-configuration_syntax@AiIntegration\AiAgents\AiAgents.cs /}


## Add Sample object or Schema

At the end of a chat, when the LLM is done processing and negotiating, 
it returns a structured output reply through the agent to the client in an 
object whose layout we need to prepare beforehand.  

The layout object we prepare is sent to the LLM when the agent is started.  
You can prepare this object in two different formats: a formal **JSON-based 
schema**, or a friendlier **sample object** format, that RavenDB will turn 
to a schema behind the scenes.  
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
Once defined, the LLM can use them of its own accord.  

A query tool includes a a natural-language **description** that explain the LLM what 
the tool is for, and an **RQL query**.  
To use a query tool, the LLM will requests the agent to apply it, and the agent will 
run the query and send the LLM the results.  

The RQL query can include parameters.  
E.g., `where Country == $country`
When these parameters are agent parameters, their values are provided by the user 
when the chat is initiated.  
But a query tool also includes a **tool parameters schema** that defines parameters 
that the LLM can set and request the agent to use in the query.  

You can choose for yourself whether to define the tool parameters schema as 
a **sample object** or **schema**, just be aware that if you define both only 
the schema will be used.  

Query tools are for **read only operations**. To make changes in the database, 
use an action tool.  

* **Example**:  
  Two of the query tools shown here retrieve the orders that were sent to a certain 
  country if the manager provided (in an agent parameter) a country, or in all countries 
  if the parameter says "everywhere".  
  The third tool retrieves the general location of an employee that the LLM picked.  
  {CODE ai-agents_agent_query-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* **Syntax**:
  Query tools are defined in a list of `AiAgentToolQuery` classes.  
  {CODE ai-agents_AiAgentToolQuery_definition@AiIntegration\AiAgents\AiAgents.cs /}


### Action tools

Action tools allow the LLM to trigger the client to perform actions like modifying, adding, 
or removing documents.  

Unlike a query tool, an action tool does not include a query. It only includes a description 
and a schema.  
The description informs the LLM in natural language what the tool is capable of.  
The schema is filled by the LLM with values when it requests the agent to apply the action.  

When the agent sends the client a request to use an action tool, the client will need to 
perform the action, and reply to the agent when it's done.  

* The following action tool sends the client an ID so the client will store it in the 
  databse.  
  {CODE ai-agents_agent_action-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  Action tools are defined in a list of `AiAgentToolAction` classes.  
  {CODE ai-agents_AiAgentToolAction_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Create the agent

Now that the agent configuration is ready, we can create the agent using the `CreateAgentAsync` operation.  
`CreateAgentAsync` has multiple overloads that we can use, including two that use the return schema defined 
within the agent configuration and one that passes to the method a sample object that will define the schema.  

* Example:  
  {CODE ai-agents_CreateAgentAsync_example@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:  
  {CODE ai-agents_CreateAgentAsync_overloads@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

{PANEL: Conversations}

A conversation is a communication session between the client, the agent, and the LLM, 
during which the LLM may use agent tools to interact with the database and the client.  

If [agent parameters](../../ai-integration/ai-agents/ai-agents-api#add-agent-parameters) 
were defined, the agent will start the conversation only when they are provided.  

### Continuous conversations
The LLM does not record its chats, starting a new chat means losing the chat history.  
The AI agent allows a continuous conversation by storing conversations history in the 
`@conversations` collection. When a new chat starts, it continues where it left off 
because the agent sends the conversation history to the LLM.  

### Stored conversations' Prefix and IDs
Conversations are kept in the `@conversations` collection with a prefix (such as `Chats/`) 
that can be set when the conversation is initiated. The conversation ID that follows the prefix 
is created automatically by RavenDB, similarly to the creation of automatic IDs for documents.  

You can:  

* Provide a full ID  
* Provide a prefix that ends with `/` or `|` to trigger automatic ID creation.  


## Set the conversation

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

* Example: 
  {CODE ai-agents_Conversation_example@AiIntegration\AiAgents\AiAgents.cs /}

* `Conversation` Definition:  
  {CODE ai-agents_Conversation_definition@AiIntegration\AiAgents\AiAgents.cs /}

* `SetUserPrompt` Definition:  
  {CODE ai-agents_SetUserPrompt_definition@AiIntegration\AiAgents\AiAgents.cs /}

### Handling action tools

Handle action tools messages by passing the chat's `Handle` method the name of the 
action toolyou want it to handle. When the LLM sends your agent action tool requests, 
the data they include will reach the `Handle` method.  
Also pass `Handle` an object to receive the incoming data, in the same structure 
that you gave the action tool's sample object.  
When you finish handling the requested action `return` the LLM an indication that 
it was done.  

{CODE ai-agents_Conversation_handle-for-action-tool@AiIntegration\AiAgents\AiAgents.cs /}

{CODE ai-agents_Conversation_action-tool-data-object@AiIntegration\AiAgents\AiAgents.cs /}

### Chat reply

LLM replies are returned by the agent to the client in an `AiAnswer` object.  
The conversation status is indicated by `AiAnswer.AiConversationResult`.   

* `AiAnswer`syntax:
  {CODE ai-agents_AiAnswer@AiIntegration\AiAgents\AiAgents.cs /}

### Set user prompt and run the chat

Set the user prompt using the `SetUserPrompt` method, and run the chat with the
`RunAsync` method.
{CODE ai-agents_Conversation_user-prompt-and-run@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

{PANEL: Full example}

The agent in this example helps a human experience manage to reward employees.  
It searches, using query tools, the orders sent to a certain country or (if the 
manager prompts it "everywhere") to all countries, and finds the employee that 
made the larget profit. It then uses another query tool to find in which region 
this employee lives, and finds rewards based on this location. Finally, it 
uses an action tool to store the employee's ID in a `Performers` collection, 
and returns, in its final response, the employee's ID, profits, and suggested rewards.  

{CODE ai-agents-full-example@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

## Related Articles

### AI Agents

- [AI Agents Studio](../../ai-integration/ai-agents/ai-agents-studio)
