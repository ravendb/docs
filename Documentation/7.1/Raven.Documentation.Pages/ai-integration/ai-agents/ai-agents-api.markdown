# AI agents: API
---

{NOTE: }

* A RavenDB **AI Agent** can be used by a RavenDB client to invoke a chat or 
  a continuous conversation between the client, an AI model, and a RavenDB database.  

* The AI agent can provide the AI model with a set of query and action tools that 
  the LLM can use freely to access the database or trigger the user to action.  

* In this article:
   * [Creating a connection string](../../ai-integration/ai-agents/ai-agents-api#creating-a-connection-string)  
   * [Defining and running an AI agent](../../ai-integration/ai-agents/ai-agents-api#defining-and-running-an-ai-agent)
      * [Define agent configuration](../../ai-integration/ai-agents/ai-agents-api#define-agent-configuration)
      * [Set Agent ID](../../ai-integration/ai-agents/ai-agents-api#set-agent-id)
      * [Add agent parameters](../../ai-integration/ai-agents/ai-agents-api#add-agent-parameters)
      * [Set maximum number of iterations](../../ai-integration/ai-agents/ai-agents-api#set-maximum-number-of-iterations)
      * [Set chat trimming configuration](../../ai-integration/ai-agents/ai-agents-api#set-chat-trimming-configuration)
      * [Add Sample object or Schema](../../ai-integration/ai-agents/ai-agents-api#add-sample-object-or-schema)
      * [Add agent tools](../../ai-integration/ai-agents/ai-agents-api#add-agent-tools)
         * [Query tools](../../ai-integration/ai-agents/ai-agents-api#query-tools)
         * [Action tools](../../ai-integration/ai-agents/ai-agents-api#action-tools)
      * [Create a Response object and the Agent](../../ai-integration/ai-agents/ai-agents-api#create-a-response-object-and-the-agent)
   * [Conversations](../../ai-integration/ai-agents/ai-agents-api#conversations)
      * [Continuous conversations](../../ai-integration/ai-agents/ai-agents-api#continuous-conversations)
      * [Stored conversations' Prefix and IDs](../../ai-integration/ai-agents/ai-agents-api#stored-conversations-prefix-and-ids)
      * [Set the conversation](../../ai-integration/ai-agents/ai-agents-api#set-the-conversation)
      * [Action tools handlers](../../ai-integration/ai-agents/ai-agents-api#action-tools-handlers)
      * [Conversation reply](../../ai-integration/ai-agents/ai-agents-api#conversation-reply)
      * [Set user prompt and run the conversation](../../ai-integration/ai-agents/ai-agents-api#set-user-prompt-and-run-the-conversation)
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

Read in the example below what role we assign to LLM.  
Throughout the code, we are careful with the details we send to the LLM about employees and orders.  

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
The values are then embedded by the agent in query tools when the tools are used 
by the LLM. Users and clients can provide their selections and preferences through 
agent parameters, to focus the queries and the whole interaction on their needs.  

In the example below, an agent parameter is used to determine what area 
of the world a query will handle. 

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

The LLM doesn't keep the history of previous chats. To allow a continuous conversation, 
we include in every new message we send to the LLM the entire conversation since it started.  
To save traffic and tokens, you can summarize older messages.  
This can be helpful when transfer rate and cost are a concern or the context may become 
too large to handle efficiently.  

To summarize old messages create an `AiAgentChatTrimmingConfiguration` instance, 
use this instance to configure your trimming strategy, and set the agent's `ChatTrimming` property 
with the instance.  

When creating the instance, pass its constructor a summarization strategy using 
a `AiAgentSummarizationByTokens` class.  

The original conversation history, before it was summarized, can optionally be 
kept in the `@conversations-history` collection.  
To determine whether to keep the original messages and for how long, also pass the 
`AiAgentChatTrimmingConfiguration` constructor an `AiAgentHistoryConfiguration` instance 
with your history settings.  

* Example:
  {CODE ai-agents_trimming-configuration_example@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  {CODE ai-agents_trimming-configuration_syntax@AiIntegration\AiAgents\AiAgents.cs /}

## Add agent tools

You can enhance your agent with Query and Action tools, that allow the LLM 
to query your database and trigger client actions.  

After defining agent tools and submitting them to the LLM, it is up to the LLM 
to decide if and when to use them. 

### Query tools

Query tools provide the LLM with the ability to retrieve data from the database.  

A query tool includes a natural-language **description** that explains the LLM what 
the tool is for, and an **RQL query**.  
To use a query tool, the LLM will request the agent to apply it, and the agent will 
run the query and pass the results to the LLM.  

Both the user and the LLM can provide values to the RQL query, using parameters.  
In the query, a parameter is defined with a `$`.  
E.g., `where Country == $country`

- To include user parameters in the query, include in the query **agent parameters** - 
  the parameters whose values the user provides when the chat is started.  
- To include LLM parameters in the query, define the parameters in the tool's 
  **parameters schema**. The LLM will pass their values each time it requests 
  the agent to run the tool.  

You can define a parameters schema as either a **sample object** or a **schema**.  
Be aware that if you define both a sample object and a schema, only the schema will be used.  

Query tools are allowed to include in their RQL queries only queries for **read operations**.  
To make changes in the database, use an action tool instead.  

* **Example**:  
  * The first  query tool will be used by the LLM when it needs to retrieve all the 
    orders sent to any place in the world. (the system prompt instructs it to use this 
    tool when the user enters "everywhere" when the chat is started.  
  * The second query tool will be used by the LLM when it needs to retrieve all the 
    orders thaty were sent to a particular country. It uses the "country" agent parameter.  
  * The third tool retrieves from the database the general location of an employee.  
    To do this it uses a $employeeId parameter, whose value is set by the LLM in its 
    request to run this tool.  
  {CODE ai-agents_agent_query-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* **Syntax**:
  Query tools are defined in a list of `AiAgentToolQuery` classes.  
  {CODE ai-agents_AiAgentToolQuery_definition@AiIntegration\AiAgents\AiAgents.cs /}


### Action tools

Action tools allow the LLM to trigger the client to actions like modifying or adding 
documents, or any other operation that the client is permitted to perform.  

Unlike a query tool, an action tool does not include a query. It only includes a 
description and a parameters schema.  
The description informs the LLM in natural language what the tool is capable of.  
The schema is filled by the LLM with values when it requests the agent to apply 
the action.  

In the example below, the action tool is requested to store employee's details 
in the database. The LLM will provide these details when it requests the agent 
to apply the tool. 

When the client finishes performing the action, it is required to send the LLM 
a response that explains how it went, e.g. `done`.  

* The following action tool sends to the client employee details that the tool 
  needs to store in the database.  
  {CODE ai-agents_agent_action-tool-sample@AiIntegration\AiAgents\AiAgents.cs /}

* Syntax:
  Action tools are defined in a list of `AiAgentToolAction` classes.  
  {CODE ai-agents_AiAgentToolAction_definition@AiIntegration\AiAgents\AiAgents.cs /}

## Create a Response object and the Agent

The agent configuration is almost ready.  
The only part still missing is an object for the LLM's response, when it 
finishes its work and needs to reply.  

Create a response object class with the fields that you want the LLM to fill in its response.  
Then, create the agent using the `CreateAgentAsync` method and pass it a new 
instance of your response object.  
Set each response-object property with a natural-language explanation to the LLM, indicating 
what the LLM should embed in it.

* **Example**:  
  {CODE ai-agents_CreateAgentAsync_example@AiIntegration\AiAgents\AiAgents.cs /}
  {CODE ai-agents_Conversation_action-tool-data-object@AiIntegration\AiAgents\AiAgents.cs /}

---

Alternatively, you can set the agent configiration's `SampleObject` or `OutputSchema` 
properties with, respectively, a sample object or a schema string and use a `CreateAgentAsync` 
overload that creates the agent without passing it a response object.  

A sample object is a JSON object whose properties indicate in natural language 
what values the LLM should embed in them. A schema follows the formal format used 
by the LLM.  

A sample object is normally easier to create. Note that if you define a sample object 
the agent will translate it to a schema in any case before passing it to the LLM.  

If you define both a sample object and a schema, only the schema will be used.  


* Example:  
  {CODE-BLOCK:json}
// Set sample object
agent.SampleObject = "{" +
                        "\"employeeID\": \"the ID of the employee that made the largest profit\", " +
                        "\"profit\": \"the profit the employee made\"" +
                        "\"suggestedReward\": \"your suggestions for a reward\"" +
                     "}";
{CODE-BLOCK/}


* `CreateAgentAsync` overloads:  
  {CODE ai-agents_CreateAgentAsync_overloads@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

{PANEL: Conversations}

A conversation is a communication session between the client, the agent, and the LLM, 
during which the LLM may use agent tools to interact with the database and the client.  

If [agent parameters](../../ai-integration/ai-agents/ai-agents-api#add-agent-parameters) 
were defined, the agent will start the conversation only when they are entered.  

### Continuous conversations
The LLM doesn't record its chats, but starts a new chat each time.  
The AI agent allows a continuous conversation by storing conversations history in the 
`@conversations` collection. When a new chat is started using the ID of a stored conversation, 
the agent will fetch the entire conversation, send it to the LLM, and the conversation 
will be resumed where you left off.  

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

### Action tools handlers

Handle an action tools request by passing the chat's `Handle` method the name of 
the action tool you want to handle. When the LLM sends your agent an action request, 
any data included in the request reaches the handler and the client can to use it.  

Pass `Handle` an object to populate with the request's data. The object should 
have the same structure you defined for the action tool's parameters schema.  

When you finish handling the requested action `return` the LLM an indication that 
it was done.  

{CODE ai-agents_Conversation_handle-for-action-tool@AiIntegration\AiAgents\AiAgents.cs /}

{CODE ai-agents_Conversation_action-tool-data-object@AiIntegration\AiAgents\AiAgents.cs /}

### Conversation reply

LLM replies are returned by the agent to the client in an `AiAnswer` object.  
The conversation status is indicated by `AiAnswer.AiConversationResult`.   

* `AiAnswer`syntax:
  {CODE ai-agents_AiAnswer@AiIntegration\AiAgents\AiAgents.cs /}

### Set user prompt and run the conversation

Set the user prompt using the `SetUserPrompt` method, and run the chat with the
`RunAsync` method.
{CODE ai-agents_Conversation_user-prompt-and-run@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

{PANEL: Full example}

The agent in this example helps a human experience manage to reward employees.  
It searches, using query tools, the orders sent to a certain country or (if the 
manager prompts it "everywhere") to all countries, and finds the employee that 
made the largest profit.  
It then uses another query tool to find, by the employee's ID (fetched from the 
orders) the employee's residence region, and finds rewards based on this location.  
Finally, it uses an action tool to store the employee's ID, profit, and reward 
suggestions in the `Performers` collection in the database, and returns the same 
details in its final response as well.  

{CODE ai-agents-full-example@AiIntegration\AiAgents\AiAgents.cs /}

{PANEL/}

## Related Articles

### AI Agents

- [AI Agents Studio](../../ai-integration/ai-agents/ai-agents-studio)
