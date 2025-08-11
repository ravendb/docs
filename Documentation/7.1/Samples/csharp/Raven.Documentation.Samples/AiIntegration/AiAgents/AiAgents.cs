using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Akka.Dispatch.SysMsg;
using Akka.Routing;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.AI;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.AI.Agents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Http;
using Raven.Client.Json;
using Raven.Client.Json.Serialization;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Sparrow.Json.Parsing;
using Xunit;
using Xunit.Abstractions;

namespace Raven.Documentation.Samples.AiIntegration.ConnectionStrings;

public class AiAgents
{
    public async Task Examples()
    {
        #region ai-agents_create-connection-string_open-ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to OpenAI
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "open-ai-cs",

                // OpenAI connection settings
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    // LLM model for text generation
                    model: "gpt-4.1")
            };

            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion

        #region ai-agents_create-connection-string_ollama
        using (var store = new DocumentStore())
        {
            // Define the connection string to Ollama
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ollama-cs",

                // Ollama connection settings
                OllamaSettings = new OllamaSettings(
                    // LLM Ollama model for text generation
                    model: "llama3.2",
                    // local URL
                    uri: "http://localhost:11434/")
            };

            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion

    }

    public async Task useActionToolsToTriggerUserQuery()
    {
        var store = new DocumentStore();

        // Define the connection string to OpenAI
        var connectionString = new AiConnectionString
        {
            // Connection string name & identifier
            Name = "open-ai-cs",

            // OpenAI connection settings
            OpenAiSettings = new OpenAiSettings(
                apiKey: "your-api-key",
                endpoint: "https://api.openai.com/v1",
                // LLM model for text generation
                model: "gpt-4.1")
        };

        // Deploy the connection string to the server
        var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);

        var putConnectionStringResult = store.Maintenance.Send(operation);

        using var session = store.OpenAsyncSession();

        #region ai-agents_AiAgentConfiguration_example
        // Agent configuration
        var agent = new AiAgentConfiguration("congratulate-employee-with-present", connectionString.Name,
            "You work for a human experience manager. " +
            "The manager uses your help to find which employee made the largest profit and thank this employee. " +
            "The manager can guide you to choose between employees that sent orders to a particular country, " +
            "or include all employees. " +
            "To help the manager thank the employee, you are requested to find good vacation sites or other " +
            "presents based on the employee's living area that the company can reward them with." +
            "To help the manager thank the employee, you are requested to find good vacation sites or other presents " +
            "based on the employee's living area that the company can reward them with. " +
            "You are equipped with: " +
            "1.a query tool that allows you to retrieve all orders sent to all countries.use this tool to retrieve " +
            "all orders sent to all countries and calculate which employee made the largest profit. " +
            "2.A query tool that allows you to retrieve all the orders sent to a particular country.use this tool if " +
            "the user prompt specifies a country that orders were sent to, and calculate which employee that sent products " +
            "to this country made the largest profit. " +
            "3.An action tool that you can provide the employee's ID with to get the employee's living region." +
            "When you're done. return the employee ID, the profit the employee made, and the suggested rewards."
            );
        #endregion

        #region ai-agents_agent-identifier
        // Agent ID
        agent.Identifier = "congratulate-employee-with-present";
        #endregion

        #region ai-agents_AiAgentParameter_function
        //  Agent parametera
        agent.Parameters.Add(new AiAgentParameter("country", "A specific country that orders were shipped to, or \"everywhere\" to look for orders shipped to all countries"));
        #endregion

        #region ai-agents_agent_sampleObjectString
        // Set sample object
        agent.SampleObject = "{" +
                                "\"EmployeeID\": \"embed the employee’s ID here\"" +
                                "\"EmployeeProfit\": \"embed the profit made by the employee here\"," +
                                "\"Suggestions\": \"embed suggested rewards here\"" +
                             "}";

        #endregion

        #region ai-agents_agent_query-tool-sample
        agent.Queries =
        [   
            // Set first query tool
            new AiAgentToolQuery
            {
                Name = "retrieve-orders-sent-to-all-countries",
                Description = "a query tool that allows you to retrieve all orders sent to all countries.",
                Query = "from \"Orders\" " +
                        "select Employee, Lines.Quantity",
                ParametersSampleObject = "{" +
                                            "\"Employee\": \"employee ID\"," +
                                            "\"Lines.Quantity\": \"an array of profits made by this employee\"" +
                                         "}"
            },
            
            // Set second query tool
            new AiAgentToolQuery
            {
                Name = "retrieve-orders-sent-to-a-particular-country",
                Description = "a query tool that allows you to retrieve all orders sent to a particular country",
                Query = "from \"Orders\" where ShipTo.Country == $country " +
                        "select Employee, Lines.Quantity",
                ParametersSampleObject = "{" +
                                            "\"Employee\": \"employee ID\"," +
                                            "\"Lines.Quantity\": \"an array of profits made by this employee\"" +
                                         "}"
            }
        ];
        #endregion

        #region ai-agents_agent_action-tool-sample
        agent.Actions =
        [   // set action tool
            new AiAgentToolAction
            {
                Name = "request-employee-details-by-ID",
                Description = "an action tool that allows you to provide the user the ID of the employee that made " +
                              "the largest profit so the user will send you a prompt with the employee’s living region",
                ParametersSampleObject = "{" +
                                            "\"EmployeeID\": \"embed the employee’s ID here\"" +
                                         "}"
            }
        ];
        #endregion

        #region ai-agents_trimming-configuration_example
        // Set chat trimming configuration
        AiAgentSummarizationByTokens summarization = new AiAgentSummarizationByTokens()
        {
            SummarizationTaskBeginningPrompt = "Summarize the conversation so far.",
            SummarizationTaskEndPrompt = "Generate a summary of the conversation.",
            ResultPrefix = "Summary: ",
            MaxTokensBeforeSummarization = 10000,
            MaxTokensAfterSummarization = 10000
        };

        AiAgentHistoryConfiguration history = new AiAgentHistoryConfiguration()
        {
            HistoryExpirationInSec = 60 * 60 * 24 // 1 day
        };

        agent.ChatTrimming = new AiAgentChatTrimmingConfiguration(summarization, history);
        #endregion

        #region ai-agents_MaxModelIterationsPerCall_function
        // Limit the number of times the LLM can request for tools in response to a single user prompt
        agent.MaxModelIterationsPerCall = 3;
        #endregion

        #region ai-agents_CreateAgentAsync_example
        var createResult = await store.AI.CreateAgentAsync(agent);
        #endregion

        #region ai-agents_Conversation_example
        // Set chat
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "suggestions/",
            new AiConversationCreationOptions().AddParameter("country", "France"));

        // Set user prompt and run the chat
        chat.SetUserPrompt("check which employee made the largest profit");
        var LLMResponse = await chat.RunAsync<OutputSchem>(CancellationToken.None);

        Employee employee;
        if (LLMResponse.Status == AiConversationResult.ActionRequired)
        {
            // Handle action required case

            // The LLM response indicates that an action is required to fetch the employee's ID
            // Extract the employee ID from the LLM response
            var employeeId = LLMResponse.Answer.EmployeeID;

            employee = (Employee)session.Advanced
                .AsyncDocumentQuery<Employee>()
                .WhereEquals(x => x.Id, employeeId);

            // Run the chat again and send as user prompt the details requested by the LLM
            chat.SetUserPrompt("{\"City\": " + employee.Address.City +
                               "{\"Region\": " + employee.Address.Region +
                               "{\"Country\": " + employee.Address.Country);
            LLMResponse = await chat.RunAsync<OutputSchem>(CancellationToken.None);

            if (LLMResponse.Status == AiConversationResult.Done)
            {
                // The LLM has successfully processed the action and returned the final response
                // Find it in LLMResponse.Answer.EmployeeID, LLMResponse.Answer.EmployeeProfit, 
                // and LLMResponse.Answer.SuggestedRewards
            }
        }
        #endregion
    }

    #region ai-agents_full-example
    public async Task FullAIAgentsExample()
    {
        var store = new DocumentStore();

        // Define the connection string to OpenAI
        var connectionString = new AiConnectionString
        {
            // Connection string name & identifier
            Name = "open-ai-cs",

            // OpenAI connection settings
            OpenAiSettings = new OpenAiSettings(
                apiKey: "your-api-key",
                endpoint: "https://api.openai.com/v1",
                // LLM model for text generation
                model: "gpt-4.1")
        };

        // Deploy the connection string to the server
        var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);

        var putConnectionStringResult = store.Maintenance.Send(operation);

        using var session = store.OpenAsyncSession();

        // Agent configuration
        var agent = new AiAgentConfiguration("congratulate-employee-with-present", connectionString.Name,
            "You work for a human experience manager. " +
            "The manager uses your help to find which employee made the largest profit and thank this employee. " +
            "The manager can guide you to choose between employees that sent orders to a particular country, " +
            "or include all employees. " +
            "To help the manager thank the employee, you are requested to find good vacation sites or other " +
            "presents based on the employee's living area that the company can reward them with." +
            "To help the manager thank the employee, you are requested to find good vacation sites or other presents " +
            "based on the employee's living area that the company can reward them with. " +
            "You are equipped with: " +
            "1.a query tool that allows you to retrieve all orders sent to all countries.use this tool to retrieve " +
            "all orders sent to all countries and calculate which employee made the largest profit. " +
            "2.A query tool that allows you to retrieve all the orders sent to a particular country.use this tool if " +
            "the user prompt specifies a country that orders were sent to, and calculate which employee that sent products " +
            "to this country made the largest profit. " +
            "3.An action tool that you can provide the employee's ID with to get the employee's living region." +
            "When you're done. return the employee ID, the profit the employee made, and the suggested rewards."
            );

        // Agent ID
        agent.Identifier = "congratulate-employee-with-present";

        //  Agent parametera
        agent.Parameters.Add(new AiAgentParameter("country", "A specific country that orders were shipped to, or \"everywhere\" to look for orders shipped to all countries"));

        // Set sample object
        agent.SampleObject = "{" +
                                "\"EmployeeID\": \"embed the employee’s ID here\"" +
                                "\"EmployeeProfit\": \"embed the profit made by the employee here\"," +
                                "\"Suggestions\": \"embed suggested rewards here\"" +
                             "}";

        agent.Queries =
        [   
            // Set first query tool
            new AiAgentToolQuery
            {
                Name = "retrieve-orders-sent-to-all-countries",
                Description = "a query tool that allows you to retrieve all orders sent to all countries.",
                Query = "from \"Orders\" " +
                        "select Employee, Lines.Quantity",
                ParametersSampleObject = "{" +
                                            "\"Employee\": \"employee ID\"," +
                                            "\"Lines.Quantity\": \"an array of profits made by this employee\"" +
                                         "}"
            },
            
            // Set second query tool
            new AiAgentToolQuery
            {
                Name = "retrieve-orders-sent-to-a-particular-country",
                Description = "a query tool that allows you to retrieve all orders sent to a particular country",
                Query = "from \"Orders\" where ShipTo.Country == $country " +
                        "select Employee, Lines.Quantity",
                ParametersSampleObject = "{" +
                                            "\"Employee\": \"employee ID\"," +
                                            "\"Lines.Quantity\": \"an array of profits made by this employee\"" +
                                         "}"
            }
        ];

        agent.Actions =
        [   
            // set action tool
            new AiAgentToolAction
            {
                Name = "request-employee-details-by-ID",
                Description = "an action tool that allows you to provide the user the ID of the employee that made " +
                              "the largest profit so the user will send you a prompt with the employee’s living region",
                ParametersSampleObject = "{" +
                                            "\"EmployeeID\": \"embed the employee’s ID here\"" +
                                         "}"
            }
        ];

        // Set chat trimming configuration
        AiAgentSummarizationByTokens summarization = new AiAgentSummarizationByTokens()
        {
            SummarizationTaskBeginningPrompt = "Summarize the conversation so far.",
            SummarizationTaskEndPrompt = "Generate a summary of the conversation.",
            ResultPrefix = "Summary: ",
            MaxTokensBeforeSummarization = 10000,
            MaxTokensAfterSummarization = 10000
        };
        agent.ChatTrimming = new AiAgentChatTrimmingConfiguration(summarization);

        // Limit the number of times the LLM can request for tools in response to a single user prompt
        agent.MaxModelIterationsPerCall = 3;

        var createResult = await store.AI.CreateAgentAsync(agent);
        // Set chat
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "suggestions/",
            new AiConversationCreationOptions().AddParameter("country", "France"));

        // Set user prompt and run the chat
        chat.SetUserPrompt("check which employee made the largest profit");
        var LLMResponse = await chat.RunAsync<OutputSchem>(CancellationToken.None);

        Employee employee;
        if (LLMResponse.Status == AiConversationResult.ActionRequired)
        {
            // Handle action required case

            // The LLM response indicates that an action is required to fetch the employee's ID
            // Extract the employee ID from the LLM response
            var employeeId = LLMResponse.Answer.EmployeeID;

            employee = (Employee)session.Advanced
                .AsyncDocumentQuery<Employee>()
                .WhereEquals(x => x.Id, employeeId);

            // Run the chat again and send as user prompt the details requested by the LLM
            chat.SetUserPrompt("{\"City\": " + employee.Address.City +
                               "{\"Region\": " + employee.Address.Region +
                               "{\"Country\": " + employee.Address.Country);
            LLMResponse = await chat.RunAsync<OutputSchem>(CancellationToken.None);

            if (LLMResponse.Status == AiConversationResult.Done)
            {
                // The LLM has successfully processed the action and returned the final response
                // Find it in LLMResponse.Answer.EmployeeID, LLMResponse.Answer.EmployeeProfit, 
                // and LLMResponse.Answer.SuggestedRewards
            }
        }
    }

    public class OutputSchem
    {
        public static OutputSchem Instance = new();
        public string EmployeeID = "The employee's ID";
        public string EmployeeProfit = "The profit made by the employee";
        public string SuggestedRewards = "Suggested rewards for the employee";
    }
    #endregion


    private interface IFoo
    {
        /*
        #region ai-agents_AiAgentConfiguration_definition
        public AiAgentConfiguration(string name, string connectionStringName, string systemPrompt);
        #endregion
        */

        /*
        #region ai-agents_AiAgentConfiguration-class_definition
        public class AiAgentConfiguration
        {
            // A unique identifier given to the AI agent configuration
            public string Identifier { get; set; }

            // The name of the AI agent configuration
            public string Name { get; set; }

            // Connection string name
            public string ConnectionStringName { get; set; }

            // The system promnpt that defines the role and purpose of the agent and the LLM
            public string SystemPrompt { get; set; }

            // An example object that sets the layout for the LLM's response to the user.
            // The object is translated to a schema before we send it to the LLM.
            public string SampleObject { get; set; }

            // A schema that sets the layout for the LLM's response to the user.
            // If both a sample object and a schema are defined, only the schema is used.
            public string OutputSchema { get; set; }

            // A list of Query tools that the LLM can use (through the agent) to access the database
            public List<AiAgentToolQuery> Queries { get; set; } = new List<AiAgentToolQuery>();

            // A list of Action tools that the LLM can use to trigger the user to action
            public List<AiAgentToolAction> Actions { get; set; } = new List<AiAgentToolAction>();

            // Agent parameters whose value the client passes to the LLM each time a chat is started, 
            // for stricter control over queris initiated by the LLM and as a means for interaction 
            // between the client and the LLM.  
            public List<AiAgentParameter> Parameters { get; set; } = new List<AiAgentParameter>();

            // The trimming configuration defines if and how the chat history is compacted or truncated, 
            // to minimize the amount of data passed to the LLM when a chat is started.  
            public AiAgentChatTrimmingConfiguration ChatTrimming { get; set; } = new AiAgentChatTrimmingConfiguration(new AiAgentSummarizationByTokens());
            
            // Control over the number of times that the LLMis allowed to use agent tools to handle a user prompt.  
            public int? MaxModelIterationsPerCall { get; set; }
        }
        #endregion
        */

        /*
        #region ai-agents_connection-string_syntax_open-ai
        public class AiConnectionString
        {
            public string Name { get; set; }
            public string Identifier { get; set; }
            public OpenAiSettings OpenAiSettings { get; set; }
            ...
        }

        public class OpenAiSettings : AbstractAiSettings
        {
            public string ApiKey { get; set; }
            public string Endpoint { get; set; }
            public string Model { get; set; }
            public int? Dimensions { get; set; }
            public string OrganizationId { get; set; }
            public string ProjectId { get; set; }
        }
        #endregion

        #region ai-agents_connection-string_syntax_ollama
        public class AiConnectionString
        {
            public string Name { get; set; }
            public string Identifier { get; set; }
            public OllamaSettings OllamaSettings { get; set; }
            ...
        }

        public class OllamaSettings : AbstractAiSettings
        {
            public string Model { get; set; }
            public string Uri { get; set; }
        }
        #endregion

        */

        /*
        #region ai-agents_AiAgentParameter_definition
        public AiAgentParameter(string name, string description);
        #endregion
        */

        /*
        #region ai-agents_CreateAgentAsync_overloads
        // Create the agent with just the defined agent configuration
        CreateAgentAsync(configuration);

        CreateAgentAsync(AiAgentConfiguration configuration, CancellationToken token = default(CancellationToken));
        
        // Create the agent with the agent configuration,         
        CreateAgentAsync<TSchema>(AiAgentConfiguration configuration, TSchema sampleObject, CancellationToken token = default(CancellationToken));
        #endregion
        */

        /*
        #region ai-agents_Conversation_definition
        public IAiConversationOperations Conversation(string agentId, string conversationId, AiConversationCreationOptions creationOptions, string changeVector = null)
        #endregion
        */

        #region ai-agents_AiAgentToolQuery_definition
        public class AiAgentToolQuery
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Query { get; set; }
            public string ParametersSampleObject { get; set; }
            public string ParametersSchema { get; set; }
        }
        #endregion

        #region ai-agents_AiAgentToolAction_definition
        public class AiAgentToolAction
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ParametersSampleObject { get; set; }
            public string ParametersSchema { get; set; }

        }
        #endregion

        #region ai-agents_SetUserPrompt_definition
        void SetUserPrompt(string userPrompt);
        #endregion

        /*
        #region ai-agents_MaxModelIterationsPerCall_definition
        public int? MaxModelIterationsPerCall
        #endregion
        */

        /*
        #region ai-agents_trimming-configuration_syntax
        public class AiAgentSummarizationByTokens
        {
            // Sent with the system prompt and appears before any summary content.
            // Customize it to influence the structure, tone, or depth of the generated summary.
            public string SummarizationTaskBeginningPrompt { get; set; }

            // A concise request to generate the summary according to the guidelines set by 
            // SummarizationTaskBeginningPrompt. Sent after the system prompt.
            // Adjust its phrasing to change how explicitly the model understands the summarization task.
            public string SummarizationTaskEndPrompt { get; set; }

            // An introductory prefix that appears before the generated summary of the previous conversation.
            // Customize this prefix to change how the summary is introduced when producing conversation summaries.
            public string ResultPrefix { get; set; }

            // The maximum number of tokens allowed before summarization is triggered.
            public long? MaxTokensBeforeSummarization { get; set; }

            // The maximum number of tokens allowed in the generated summary.
            public long? MaxTokensAfterSummarization { get; set; }
        }

        public class AiAgentTruncateChat
        {
            // The maximum number of messages allowed before deleting old messages
            public int MessagesLengthBeforeTruncate { get; set; }

            // The number of messages after deleting old messages
            public int MessagesLengthAfterTruncate { get; set; } = DefaultMessagesLengthBeforeTruncate / 2;
        }
        */

        /*
        public class AiAgentHistoryConfiguration
        {
            // Enables history for the AI agents conversations.
            public AiAgentHistoryConfiguration()

            // Enables history for the AI agents conversations.
            // <param name="expiration">The timespan after which history documents expire.</param>
            public AiAgentHistoryConfiguration(TimeSpan expiration)

            // The timespan after which history documents expire.
            public int? HistoryExpirationInSec { get; set; }
        }
        #endregion
        */

        #region ai-agents_AiAnswer
        public class AiAnswer<TAnswer>
        {
            // The answer content produced by the AI
            public TAnswer Answer;

            // The status of the conversation
            public AiConversationResult Status;
        }

        public enum AiConversationResult
        {
            // The conversation has completed and a final answer is available
            Done,
            // Further interaction is required, such as responding to tool requests
            ActionRequired
        }
        #endregion
    }
}
