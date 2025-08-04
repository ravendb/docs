# AI Agents Integration: Studio
---

{NOTE: }

* In this article:
   * [Create AI Agent](../../ai-integration/ai-agents/ai-agents-studio#create-ai-agent)  
   * [Configure basic settings](../../ai-integration/ai-agents/ai-agents-studio#configure-basic-settings)
   * [Set agent parameters](../../ai-integration/ai-agents/ai-agents-studio#set-agent-parameters)
   * [Define agent tools](../../ai-integration/ai-agents/ai-agents-studio#define-agent-tools)
      * [Add new query tool](../../ai-integration/ai-agents/ai-agents-studio#add-new-query-tool)
      * [Add new action tool](../../ai-integration/ai-agents/ai-agents-studio#add-new-action-tool)
   * [Set chat persistence](../../ai-integration/ai-agents/ai-agents-studio#set-chat-persistence)
   * [Configure chat trimming](../../ai-integration/ai-agents/ai-agents-studio#configure-chat-trimming)
   * [Save and Run your agent](../../ai-integration/ai-agents/ai-agents-studio#save-and-run-your-agent)
   * [Test your agent](../../ai-integration/ai-agents/ai-agents-studio#test-your-agent)
      * [Test results](../../ai-integration/ai-agents/ai-agents-studio#test-results)

{NOTE/}

---

{PANEL: Create AI Agent}

To create an AI agent, open **AI hub > AI Agents** and click **Add new agent**.

![AI Agents View](images/ai-agents_ai-agents-view.png "AI Agents View")

1. **AI Hub**  
   Click to open the [AI Hub view](../../ai-integration/ai-tasks-list-view).  
   Use this view to handle AI connection strings and tasks, and to view task statistics.  
2. **AI Agents**  
   Click to open the AI Agents view.  
   Use this view to list, configure, or remove your agents.  
3. **Add new agent**  
   Click to add an AI agent.  

      The **Create AI Agent** dialog will open, allowing you to define and test your agent.  

      ![Create AI Agent](images/ai-agents_create-ai-agent.png "Create AI Agent")

      Use the buttons at the bottom bar to Cancel, Save, or [Test]() your changes.  

4. **Filter by name**  
   When multiple agents are created, you can filter them by a string you enter here.  

{PANEL/}

{PANEL: Configure basic settings}

![Configure basic settings](images/ai-agents_config-basic-settings.png "Configure basic settings")

1. **Agent name**  
   Give your agent a meaningful name that would clarify its purpose even when multiple 
   agents are defined.  
   E.g., **CustomerSupportAgent**  

2. **Identifier**  
   Give your agent a unique identifier.  
   You can provide the identifier yourself, or click **Regenerate** to create it automatically.  

3. **Connection String**  

      ![Select or Create a Connection String](images/ai-agents_connection-string_select-or-create.png "Select or Create a Connection String")
   
      **Select** an existing [connection string](../../ai-integration/connection-strings/connection-strings-overview) 
      that the agent will use to connect your LLM of choice,  
      or click **Create a new AI connection string** to define a new string.  
      Your agent can use a local LLM like Ollama, or an external model like OpenAI.  
   
      ![Connection String](images/ai-agents_connection-string.png "Connection String")

4. **System prompt**  
   Provide a prompt that determines the LLM's role and purpose.  

      When your agent connects the LLM, it will pass it your prompt, either a sample 
      response object or a JSON schema to shape its responses by, and database tools 
      to work with.  

5. **Sample response object**  
   Provide a JSON-based **sample response object** that defines the layout of the AI model's reply.  
   Design it so LLM responses would include the data you need and be easy to process 
   by the client.  

      Behind the scenes, RavenDB will translate your sample object to a JSON schema format 
      before sending it to the LLM. If you prefer it, you can skip the translation phase by 
      defining an explicit response schema yourself (see below).  
      If you define both a sample response object and an explicit schema, only the schema will be used.  

6. **Response JSON schema**  
   Enter a JSON schema that defines the layout of LLM responses.  
   Design it so LLM responses would include the data you need and be easy to process 
   by the client.  

      If you define both a sample response object and an explicit schema, only the schema will be used.  

{PANEL/}

{PANEL: Set agent parameters}

Define **agent parameters**.  
Values for agent parameters you define will be provided by the client when it runs the agent.  
When the LLM runs a [query tool]() that includes an agent parameter, the parameter will be replaced 
with the value provided by the client.  
The additional control gained by agent parameters can be used, for example, to limit the scope 
of a query and give the LLM access only to documents related to a specific company whose ID was 
provided by the client.  

![Set agent parameters](images/ai-agents_set-agent-params.png "Set agent parameters")

1. **Name**  
   Define agent parameter name.  

2. **Description**  
   Optionally add a description that explains the parameter.  

3. **Add parameter**  
   Click to add the agent parameter.  
   
      Defined parameters are listed at the lower part of this section.  

      ![Agent parameters list](images/ai-agents_set-agent-params_params-list.png "Agent parameters list")

{PANEL/}

{PANEL: Define agent tools}

Define **Query** and **Action** agent tools.  
When the agent is executed, it will pass the LLM the list of tools you define here.  
The LLM will then be able to trigger the execution of these tools to retrieve needed 
data from the database or apply changes like the creation or the modification of documents.  

Agent tools are **not** applied directly by the LLM, but triggered by the LLM and applied 
by the agent or by the client.  
When the LLM needs to apply a **query tool**, it notifies the agent which executes the query 
and passes its results to the LLM.  
When the LLM needs to apply an **action tool**, the client is triggered to do it and will 
notify the LLM when it's done.  

![Define agent tools](images/ai-agents_define-agent-tools.png "Define agent tools")

1. **Query tools**  
   Click to add a new query tool.  

2. **Action tools**  
   Click to add a new action tool.  

---

### Add new query tool

![Add new query tool](images/ai-agents_define-agent-tools_add-query-tool.png "Add new query tool")

1. **Cancel or Save**  
   Cancel your changes or Save the query tool when it is ready.  

2. **Tool name**  
   Give your query tool a meaningful name.  

3. **Description**  
   Write a description that will explain to the LLM in natural language what the attached query can be used for.  
   E.g.,  
   `apply this query when you need to retrieve all the companies that reside in a certain country`  

4. **Query**  
   Write the query that the agent will run when the LLM triggers it to use this query tool.  
   E.g.,  
   `from Companies where Address.Country = country`

5. **Sample response object**  
   Define the layout of a JSON object that the LLM will send to the agent when it requests 
   to run this query tool.  
   The LLM will populate the object with values that it needs the agent to embed in the query.  
   
      In the response object:  
      Specify the query parameter you want to replace with a value sent by the LLM, as a property name.  
      Phrase what the LLM is expected to search for in natural language, as the property's value.  

      E.g., 
      {CODE-BLOCK:json}
{
  "country": "the name of the country to search for"
}
      {CODE-BLOCK/}

      Behind the scenes, RavenDB will translate your sample object to a JSON schema format 
      before sending it to the LLM. If you prefer it, you can skip the translation phase by 
      defining an explicit response schema yourself (see below).  
      If you define both a sample response object and an explicit schema, only the schema will be used.  

6. **Response JSON schema**  
   Define a JSON schema that the LLM will send to the agent along with a request to run this query tool.  
   The LLM will populate the schema with values that it needs the agent to embed in the query.  

      E.g., 
      {CODE-BLOCK:json}
{
  "type": "object",
  "properties": {
    "Country": {
      "type": "string",
      "description": "the name of the country to search"
    }
  },
  "required": [
    "Country"
  ],
  "additionalProperties": false
}
      {CODE-BLOCK/}

      If you define both a sample response object (see above) and an explicit schema, only the schema will be used.  

---

### Add new action tool

![Add new action tool](images/ai-agents_define-agent-tools_add-action-tool.png "Add new action tool")

1. **Cancel or Save**  
   Cancel your changes or Save the query tool when it is ready.  

2. **Tool name**  
   Give your query tool a meaningful name.  

3. **Description**  
   Write a description that explains to the LLM in natural language when this action tool should be applied.  
   E.g.,  
   `apply this action tool when you need to create a new summary document`  

4. **Sample parameters object**
   Define the layout of a JSON object that the LLM will send to the agent when it 
   requests to run this action tool.  
   The LLM may populate the properties that you define for this object according 
   to instructions it was given by the agent and the action tool, data retrieved 
   from the database, and the ongoing interaction.  
   
      The populated object can then be used by the client to apply the action tool.  
   
      The LLM expects a response to this message, that relays the results of the 
      attempt to apply the action tool. The response can be phrased in natural language,   
      and if no additional data is needed from the LLM it can also be an empty object.  

      E.g., 
      {CODE-BLOCK:json}
{
  "userId": "ID of the user that provided this suggestion",
  "suggestionSummary": "Provide a summary of the user's suggestion"
}
{CODE-BLOCK/}

      Behind the scenes, RavenDB will translate your sample object to a JSON schema format 
      before sending it to the LLM. If you prefer it, you can skip the translation phase by 
      defining an explicit response schema yourself (see below).  
      If you define both a sample response object and an explicit schema, only the schema will be used.     

5. **Parameters JSON schema**  
   Define a JSON schema that the LLM will send to the agent along with a request to run this query tool.  
   The LLM will populate the schema with values that it needs the agent to embed in the query.  

      E.g., 
      {CODE-BLOCK:json}
{
  "type": "object",
  "properties": {
    "userId": {
      "type": "string",
      "description": "ID of the user that provided this suggestion"
    },
    "suggestionSummary": {
      "type": "string",
      "description": "Provide a summary of the user\u0027s suggestion"
    }
  },
  "required": [
    "userId",
    "suggestionSummary"
  ],
  "additionalProperties": false
}
      {CODE-BLOCK/}

      If you define both a sample response object (see above) and an explicit schema, only the schema will be used.  

{PANEL/}

{PANEL: Set chat persistence}
While interacting with the LLM, the agent automatically keeps a record of its ongoing chats in 
a dedicated `@conversations` collection. This section includes settings related to these records.  

![Set chat persistence](images/ai-agents_set-chat-persistence.png "Set chat persistence")

1. **Conversation ID prefix**  
   Set a prefix for IDs of documents stored in the `@conversations` collection.  

2. **Enable document expiration**  
   Enable this option to automatically delete documents from the `@conversations` collection 
   after a set time period.  
   
      Enabling the document expiration setting will allow you to set the time period after 
      which documents are deleted.  

      ![Set expiration period](images/ai-agents_set-chat-persistence_set-expiration.png "Set expiration period")

{PANEL/}

{PANEL: Configure chat trimming}
The LLM keeps no record of previous conversations it conducted.  
To allow a continuous conversation, the agent resends the whole chat history to the 
LLM each time it approaches the model, which, for longconversations, may become costly 
in bandwidth and expenses.  
Use these settings to reduce this cost by summarizing or truncating the chat history 
before sending it to the LLM.  

![Configure chat trimming](images/ai-agents_config-chat-trimming.png "Configure chat trimming")

1. **Summarize chat**  
   Use this option to set a total conversation size limit.  
   When the size of the whole conversation history exceeds the set limit, chat history will 
   be summarized before it is sent to the LLM.  

      ![Summarization settings](images/ai-agents_config-chat-trimming_summarization-settings.png "Summarization settings")

      * **A.** **Max tokens Before summarization**  
        If the conversation contains a total number of tokens larger than the set limit the conversation 
        will be summarized.  
      * **B.** **Max tokens After summarization**  
        Set the maximum number of tokens that will be left in the conversation after it is summarized.  
        Messages exceeding the set limit will be removed, starting with the oldest.  
      * **C.** **Enable history**  
        When history is enabled, the chat sent to the LLM is trimmed but a copy of the original 
        chat is kept in a dedicated document in the `@conversations-history` collection.  
        
        To choose a time period after which history documents will be deleted, enable **Set history expiration**.  
        
        ![Set history expiration](images/ai-agents_config-chat-trimming_summarization-settings_history-expiration.png "Set history expiration")
        

2. **Truncate chat**  
   Use this option to limit the number of messages.  
   When the number of messages exceeded the set limit, older messages will be truncated 
   and only the set number of messages will be sent to the LLM.  

      ![Truncation settings](images/ai-agents_config-chat-trimming_truncation-settings.png "Truncation settings")

      * **A.** **Messages length before truncate**  
        Truncate messages when their number exceeds the set limit.  
      * **B.** **Messages length after truncate**  
        The number of messages to keep after older messages are truncated.  
      * **C.** **Enable history**  
        When history is enabled, the chat sent to the LLM is trimmed but a copy of the original 
        chat is kept in a dedicated document in the `@conversations-history` collection.  
        
        To choose a time period after which history documents will be deleted, enable **Set history expiration**.  
        
        ![Set history expiration](images/ai-agents_config-chat-trimming_summarization-settings_history-expiration.png "Set history expiration")

{PANEL/}

{PANEL: Save and Run your agent}

When you're done configuring your agent, save it using the **save** button at the bottom.  

![Save your agent](images/ai-agents_save-agent.png "Save your agent")

You will find your agent in the main **AI Agents** view, where you can run or edit it.  

![Your agent](images/ai-agents_your-agent.png "Your agent")

1. **Edit agent**  
   Click to edit the agent.  

2. **Start new chat**  
   Click to run your agent.  

      Starting a new chat will open the chat window, where you can provide values 
      for the parameters you defined for this agent and enter a prompt that explains 
      the agent what this session is about.  

      ![Run agent](images/ai-agents_run-agent.png "Run agent")

      * **A.** **New Chat**  
        Click to start a new chat  
      * **B.** **Cancel**  
        Click to return to the AI Agents view.  
      * **C.** **Enter parameter value**  
        Enter a value for each parameter defined in the agent configuration.  
        The LLM will be able to replace these parameters with fixed values 
        when it uses query or action tools in which these parameters are embedded.  
      * **D.** **Agent prompt**  
        Explain the agent in natural language what this session is about.  
      * **E.** **Send prompt**  
        Click to pass your agent your parameter values and prompt and run it.  
        You can keep sending the agent prompts and getting its replies in 
        a continuous conversation.  
        
        The entire conversation history is kept in a document in the `@conversations` collection.  
        
        ![Run agent](images/ai-agents_conversations.png "Run agent")
       
        To start a new conversation, click **New chat** above.  

{PANEL/}

{PANEL: Test your agent}
The **Test agent** area is a secluded environment that allows you to safely run your agent, 
check its conduct with parameters and instructions you provide, examine its interactions with 
the LLM and the database, and view its results.  

The history of test chats is **not** logged in the `@conversations` or `@conversations-history` 
collections.  

Please be aware that fees related to your agent usage of the LLM model will still apply.  
To avoid them, you can test your agent with a local, free LLM like Ollama.  

To test your agent, click **Test** at the bottom of the agent configuration view.  

![Test Agent](images/ai-agents_test-agent_test-button.png "Test Agent")

When The test environment opens, provide it with values for the parameters you defined 
for this agent and enter a prompt that explains the agent what this session is about.  

![Run test](images/ai-agents_test-agent_run-test.png "Run test")


1. **New Chat**  
   Click to start a new chat  
2. **Close**  
   Click to return to the AI Agents configuration view.  
3. **Enter parameter value**  
   Enter a value for each parameter defined in the agent configuration.  
   The LLM will be able to replace these parameters with fixed values 
   when it uses query or action tools in which these parameters are embedded.  
4. **Agent prompt**  
   Explain the agent in natural language what this session is about.  
5. **Send prompt**  
   Click to pass your agent your parameter values and prompt and run the test.  
   You can keep sending the agent prompts and getting its replies in 
   a continuous conversation.  
   
      A test's conversation history is **not** documented in the `@conversations` collection.  
   
      To start a new conversation, click **New chat** above.  

---

### Test results

The results returned by the test include the agent's configuration, agent patameters and the 
values that the client provided for them, the history of the interactions between the agent 
and the LLM, the information requested by the system prompt if it was retrieved or created, 
and varioud other optional details like, for example, data retrieved by query tools.  

You can view the results in the test area, or expand the window foryour comfort.  

![Test results: minimized](images/ai-agents_test-results_minimized.png "Test results: minimized")

Below is an example for expanded results returned by an agent designated to retrieve 
and summarize documents related to a subject determined by the user by an agent parameter.  
{CODE-BLOCK:plain}
Summary of previous conversation: **Context & Identifiers:**
- User asked for security principles in RavenDB (parameters: subject=security).
- All messages from the user; assistant acts as a helper with access to RavenDB documentation.

**Conversation Flow:**
- 1st message (user, subject=security, no timestamp): Request for a summary of security principles in RavenDB.
- Assistant used internal tools to search relevant documentation, targeted titles: Security in RavenDB, Authentication and Authorization, Certificate Authentication, Enforcing Security Policies, Server Security Configuration.
- Fetched and reviewed top 5 documentation entries, summarized as follows:
    - "Security: Overview" page: Explains that RavenDB security is based on X.509 certificates for both authentication and authorization, enables TLS/SSL for encrypted communications, only explicitly registered client certificates are trusted, and includes database encryption at rest by default with high-grade cryptography.
    - "Authorization: Security Clearance and Permissions": Describes how certificate-based roles (Cluster Admin, Operator, User) with fine-grained permissions are assigned per database by admins, controlling what actions are possible at server and database levels.
- The assistant then presented two structured summaries, correlating to the most critical documents for quickly learning the underlying principles.

**Decisions & Tone:**
- Assistant chose official, high-level documentation directly relevant to the user's broad topic.
- Tone: Factual, concise, and instructional, with no filler or unrelated commentary.
- No changes in user intent or assistant direction.

**References (titles, codes if available):**
- "Security: Overview"
- "Authorization: Security Clearance and Permissions"

**Outcome:**
- User received a concise, linear overview highlighting that RavenDB's security uses X.509 certificate-based authentication/authorization, strictly limits trust to registered client certificates for encrypted operations, and supports fine-grained, role-based permissions management per database.
{CODE-BLOCK/}



{PANEL/}

## Related Articles

### GenAI Integration

- [GenAI Overview](../../ai-integration/gen-ai-integration/gen-ai-overview)
- [GenAI API](../../ai-integration/gen-ai-integration/gen-ai-api)  
- [GenAI Security Concerns](../../ai-integration/gen-ai-integration/security-concerns)

### Vector Search

- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index.markdown)
- [Data types for vector search](../../ai-integration/vector-search/data-types-for-vector-search)
