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
      * [Runtime view and Test results](../../ai-integration/ai-agents/ai-agents-studio#runtime-view-and-test-results)

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

5. **Defined agent**  
   After defining an agent it is listed in this view, allowing you to run, edit, or remove the agent.

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
   When your agent connects the LLM it will pass it your prompt, your sample object or 
   schema to shape its response by, your agent tools, and if you connect an existing 
   conversation - the entire conversaion history.  

5. **Sample response object** and **Response JSON schema**  
   Click "Sample response object" or "response JSON schema" to switch between these two tabs.  
   
    * Provide a JSON-based **sample response object** that defines the layout of the AI model's reply.  
      Design it so LLM responses would include the data you need and be easy to process 
      by the client.  

         Behind the scenes, RavenDB will translate your sample object to a JSON schema format 
         before sending it to the LLM. If you prefer it, you can skip the translation phase by 
         defining the response JSON schema yourself (see below).  
      
         **If you define both a sample response object and an explicit schema, only the schema will be used.**  

    * **Response JSON schema**  
      Enter a JSON schema that defines the layout of LLM responses.  
            
         If you design a sample object and then open the schema tab, you will be given the option to 
         translate the sample object to a scheme by clicking the "View schema" button.  

         **If you define both a sample response object and an explicit schema, only the schema will be used.**  
      
         ![Configure basic settings](images/ai-agents_config-basic-settings_schema.png "Configure basic settings")
    
{PANEL/}

{PANEL: Set agent parameters}

Define **agent parameters**.  
Values for agent parameters you define will be provided by the client when it runs the agent.  
When the agent runs a [query tool]() that includes an agent parameter, the value provided by 
the client will be placed in the query instead of the parameter name.  

![Set agent parameters](images/ai-agents_set-agent-params.png "Set agent parameters")

1. **Add new parameter**  
   Click to add an agent parameter.  
   
2. **Name**  
   Enter agent parameter name.  

3. **Description**  
   Explain the parameter so the LLM will understand its role.  

4. **Remove parameter**  
   Removea defined parameter from the list.

{PANEL/}

{PANEL: Define agent tools}

Define **Query** and **Action** agent tools.  
When the agent is executed, it will pass the LLM the list of tools you define here.  
The LLM will then be able to trigger the execution of these tools to retrieve needed 
data from the database or apply changes like the creation or the modification of documents.  

Agent tools are **not** applied directly by the LLM, but triggered by the LLM and applied 
by the agent or by the client.  
When the LLM needs to apply a **query tool**, it will notify the agent which will execute 
the query and pass its results to the LLM in the sample object you define for the tool.  
When the LLM needs to apply an **action tool**, it will request the agent to run it.  

![Define agent tools](images/ai-agents_define-agent-tools.png "Define agent tools")

1. **Query tools**  
   Click to add a new query tool.  

2. **Action tools**  
   Click to add a new action tool.  

---

### Add new query tool

![Add new query tool](images/ai-agents_define-agent-tools_add-query-tool.png "Add new query tool")


1. **Add new query tool**  

2. **Cancel**  
   Cancel your changes or Save the query tool when it is ready.  

3. **Expandq/Colllapse tool**

4. **Tool name**  
   Give your query tool a meaningful name.  

5. **Description**  
   Write a description that will explain to the LLM in natural language what the attached query can be used for.  
   E.g., `apply this query when you need to retrieve all the companies that reside in a certain country`  

6. **Query**  
   Write the query that the agent will run when the LLM requests it to use this tool.  

7. **Sample response object** and **Response JSON schema**  
   Click "Sample response object" or "response JSON schema" to switch between the two tabs.  
   Define an object that the agent will fill with the data that it retrieves from the database 
   when this query tool is used.  
   If you define both a sample response object and a schema, only the schema will be used.  
    
---

### Add new action tool

![Add new action tool](images/ai-agents_define-agent-tools_add-action-tool.png "Add new action tool")

1. **Add new query tool**  

2. **Cancel**  
   Cancel your changes or Save the action tool when it is ready.  

3. **Expandq/Colllapse tool**

4. **Tool name**  
   Give your action tool a meaningful name.  

5. **Description**  
   Write a description that explains to the LLM in natural language when this action tool should be applied.  
   E.g., `apply this action tool when you need to create a new summary document`  

6. **Sample response object** and **Parameters JSON schema**  
   Click "Sample response object" or "response JSON schema" to switch between the two tabs.  
   Define an object that the agent will fill with data needed for the client's action.  
   
      If you define both a sample response object and a schema, only the schema will be used.  

{PANEL/}

{PANEL: Configure chat trimming}
The LLM keeps no record of previous conversations it conducted.  
To allow a continuous conversation, the agent sends the LLM the entire conversation history 
when a chat is started. To minimize these transfers, you can configure the agent to summarize 
messages before sending the history to the LLM.  

![Configure chat trimming](images/ai-agents_config-chat-trimming.png "Configure chat trimming")

1. **Summarize chat**  
   Use this option to limit the size of the conversation history. If its size breaches this 
   limit, chat history will be summarized before it is sent to the LLM.  

2. **Max tokens Before summarization**  
   If the conversation contains a total number of tokens larger than the limit you set here, 
   the conversation will be summarized.  

3. **Max tokens After summarization**  
   Set the maximum number of tokens that will be left in the conversation after it is summarized.  
   Messages exceeding the set limit will be removed, starting with the oldest.  
   
1. **History**  
     * **Enable history**  
       When history is enabled the chat sent to the LLM will be summarized, but copies of the 
       original chats will be kept in a dedicated documents in the `@conversations-history` collection.  
     * **Set history expiration**  
       When this option is enabled, conversations will be deleted from the `@conversations-history` collection 
       when the their age exceeds the the period you set in the time boxes.  
        
{PANEL/}

{PANEL: Save and Run your agent}

When you're done configuring your agent, save it using the **save** button at the bottom.  

![Save your agent](images/ai-agents_save-agent.png "Save your agent")

You will find your agent in the main **AI Agents** view, where you can run or edit it.  

![Your agent](images/ai-agents_your-agent.png "Your agent")

1. **Start new chat**  
   Click to run your agent.  

2. **Edit agent**  
   Click to edit the agent.  

## Start new chat

Starting a new chat will open the chat window, where you can provide values 
for the parameters you defined for this agent and enter a user prompt that explains 
the agent what you expect from this conversation.  

![Run agent](images/ai-agents_run-agent.png "Run agent")

1. **Conversation ID or prefix**  
    - Entering a prefix (e.g. `Chats/`) will start a new conversation, with the prefix 
      preceding an automatically-created conversation ID.  
    - Entering the ID of a conversation that doesn't exist will start a new conversation as well.  
    - Entering the ID of an existing conversation will send its history 
      to the LLM and allow you to continue where you left off.  
 
2. **Set expiration**  
   Enable this option and set an expiration period to automatically delete conversations 
   from the `@Conversations` collection when their age exceeds the set period.  
 
3. **Agent parameters**  
   Enter a value for each parameter defined in the agent configuration.  
   The LLM will embed these values in query tools RQL queries where you 
   included agent parameters.  
   E.g., If you enter `France` here as the value for `Country`, 
   a query tool's `from "Orders" where ShipTo.Country == $country` RQL query 
   will be executed as `from "Orders" where ShipTo.Country == "France"`.  

4. **User prompt**  
   Use the user prompt to explain the agent your expectations from it in 
   this conversation.  

## Agent interaction 
        
Running the agent presents its components and interactions.  

Agent parameters and their values:
![Parameters](images/ai-agents_running_params.png "Parameters")

The system prompt set for the LLM and the user prompt:
![System and User prompts](images/ai-agents_running_prompts.png "System and User prompts")

The query tools and their activity:
![Query tool](images/ai-agents_running_query-tool.png "Query tool")

You can view the raw data of the agent's activity in JSON form as well:  
![Raw data](images/ai-agents_running_raw-data.png "Raw data")

### Action tool dialog

If the agent runs action tools, you will be given a dialog that shows you the 
information provided by the LLM when it requests the action, and a dialog inviting 
you to enter the results of the action when you finish performing it.  

![Action tool waiting for client response](images/ai-agents_running_action-tool.png "Action tool waiting for client response")

### Agent results

And finally, when the AI model finishes its work and negotiations, you will be able to see its response.
As with all other dialog boxes, you can expand the view to see the content or minimize it to see it in its context.

![LLM answer: minimized](images/ai-agents_running_llm-response-minimized.png "LLM answer: minimized")

{PANEL/}

{PANEL: Test your agent}

You can test your agent while creating or editing it, to examine its configuration 
and operability before you deploy it. The test interface resembles the one you see 
when you run your agent normally via Studio, but conversationa are not kept in the 
`@conversations` or `@conversations-history`.  

To test your agent, click **Test** at the bottom of the agent configuration view.  

![Test Agent](images/ai-agents_test-agent_test-button.png "Test Agent")

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
   Click to pass your agent your parameter values and user prompt and run the test.  
   You can keep sending the agent prompts and getting its replies in 
   a continuous conversation.  
   
---

### Runtime view and Test results

You will see the components that take part in the agent's run and be able 
to enter and send requested information for action tools. Each tool can be 
minimized to see it in context or expanded to view the data it carries.  

![Runtime view](images/ai-agents_runtime-view.png "Runtime view")

When the LLM finishes processing, you will see its response.  

![Test results: minimized](images/ai-agents_test-results_minimized.png "Test results: minimized")

You can expand the dialog or copy the content to see the response in detail.
{CODE-BLOCK:json}
{
  "EmployeeID": "employees/1-A",
  "EmployeeProfit": "1760",
  "SuggestedRewards": "The employee lives in Redmond, WA, USA. For a special reward, consider a weekend getaway to the Pacific Northwest's scenic sites such as a stay at a luxury resort in Seattle or a relaxing wine tasting tour in Woodinville. Alternatively, you could offer gift cards for outdoor excursions in the Cascade Mountains or tickets to major cultural events in the Seattle area."
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### AI Agents

- [AI Agents API](../../ai-integration/ai-agents/ai-agents-api)
