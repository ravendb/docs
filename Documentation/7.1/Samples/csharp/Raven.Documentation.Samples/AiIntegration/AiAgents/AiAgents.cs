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

    public async Task createAndRunAiAgent()
    {
        var store = new DocumentStore();

        // Set a connection string to OpenAI
        var connectionString = new AiConnectionString
        {
            Name = "open-ai-cs",
            ModelType = AiModelType.Chat,

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
        // Start setting an agent configuration
        var agent = new AiAgentConfiguration("reward-productive-employee", connectionString.Name,
            "You work for a human experience manager. " +
            "The manager uses your services to find which employee earned the largest profit for the company " +
            "and suggest a reward for this employee. " +
            "The manager provides you with the name of a country, or with the word \"everything\" to indicate all countries. " +
            "then you: " +
            "1. use a query tool to load all the orders sent to the selected countery, " +
            "or a query tool to load all orders sent to all countries. " +
            "2. calculate which employee made the largest profit." +
            "3. use a query tool to learn in what region the employee lives." +
            "4. find suitable vacation sites or other rewards based on the employee's residence area." +
            "5. use an action tool to store in the database the employee's ID, profit, and your reward suggestions." +
            "When you're done, return these details in your answer to the user as well."
            );
        #endregion

        #region ai-agents_agent-identifier
        // Set agent ID
        agent.Identifier = "reward-productive-employee";
        #endregion

        #region ai-agents_AiAgentParameter_function
        //  Set agent parametera
        agent.Parameters.Add(new AiAgentParameter("country", "A specific country that orders were shipped to, " +
                                                  "or \"everywhere\" to look for orders shipped to all countries"));
        #endregion

        #region ai-agents_agent_query-tool-sample
        agent.Queries =
        [
            // Set query tool that triggers the agent to retrieve all the orders sent everywhere
            new AiAgentToolQuery
                {
                    Name = "retrieve-orders-sent-to-all-countries",
                    Description = "a query tool that allows you to retrieve all orders sent to all countries.",
                    Query = "from Orders as O select O.Employee, O.Lines.Quantity",
                    ParametersSampleObject = "{}"
                },
            
                // Set query tool that triggers the agent to retrieve all the orders sent to a specific country
                new AiAgentToolQuery
                {
                    Name = "retrieve-orders-sent-to-a-specific-country",
                    Description = "a query tool that allows you to retrieve all orders sent to a specific country",
                    Query = "from Orders as O where O.ShipTo.Country == $country select O.Employee, O.Lines.Quantity",
                    ParametersSampleObject = "{}"
                },

                // Set query tool that triggers the agent to retrieve the performer's
                // residence region details (country, city, and region) from the database
                new AiAgentToolQuery
                {
                    Name = "retrieve-performer-living-region",
                    Description = "a query tool that allows you to retrieve an employee's country, city, and region, by the employee's ID",
                    Query = "from Employees as E where id() == $employeeId select E.Address.Country, E.Address.City, E.Address.Region",
                    ParametersSampleObject = "{" +
                                                "\"employeeId\": \"embed the employee's ID here\"" +
                                             "}"
                }
        ];
        #endregion

        #region ai-agents_agent_action-tool-sample
        agent.Actions =
        [
            // Set action tool that triggers the client to store the performer's details
            new AiAgentToolAction
                {
                    Name = "store-performer-details",
                    Description = "an action tool that allows you to store the ID of the employee that made " +
                                  "the largest profit, the profit, and your suggestions for a reward, in the database.",
                    ParametersSampleObject = "{" +
                                                "\"employeeID\": \"embed the employee’s ID here\"," +
                                                "\"profit\": \"embed the employee’s profit here\"," +
                                                "\"suggestedReward\": \"embed your suggestions for a reward here\"" +
                                             "}"
                }
        ];
        #endregion

        #region ai-agents_trimming-configuration_example
        // Set chat trimming configuration
        AiAgentSummarizationByTokens summarization = new AiAgentSummarizationByTokens()
        {
            // When the number of tokens stored in the conversation exceeds this limit
            // summarization of old messages will be triggered.
            MaxTokensBeforeSummarization = 32768,
            // The maximum number of tokens that the conversation is allowed to contain
            // after summarization. 
            MaxTokensAfterSummarization = 1024
        };

        agent.ChatTrimming = new AiAgentChatTrimmingConfiguration(summarization);
        #endregion

        #region ai-agents_MaxModelIterationsPerCall_function
        // Limit the number of times the LLM can request for tools in response to a single user prompt
        agent.MaxModelIterationsPerCall = 3;
        #endregion

        #region ai-agents_CreateAgentAsync_example
        // Create the agent
        // Pass it an object for its response
        var createResult = await store.AI.CreateAgentAsync(agent, new Performer
        {
            employeeID = "the ID of the employee that made the largest profit",
            profit = "the profit the employee made",
            suggestedReward = "your suggestions for a reward"
        });
        #endregion

        #region ai-agents_Conversation_example
        // Set the chat
        // Pass it the agent's ID, a prefix for conversations stored in @Conversations,
        // and agent parameters' values
        // Here, the country is simply set to "France" for the example.
        // A user would pick a country, or enter "everyehwre" for all countries.
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "Performers/",
            new AiConversationCreationOptions().AddParameter("country", "France"));
        #endregion

        #region ai-agents_Conversation_handle-for-action-tool
        // Handle action tool.
        // In this example, the action tool is requested to store an employee's
        // details in the database. 
        chat.Handle("store-performer-details", (Performer performer) =>
        {
            using (var session1 = store.OpenSession())
            {
                // These values are passed to the client by the action tool
                Performer rewarded = new Performer
                {
                    employeeID = performer.employeeID,
                    profit = performer.profit,
                    suggestedReward = performer.suggestedReward
                };

                // store the values in the Performers collection in the database
                session1.Store(rewarded);
                session1.SaveChanges();
            }
        
            // return to the agent an indication that the action went well.
            return "done";
        });
        #endregion

        #region ai-agents_Conversation_user-prompt-and-run
        // Set the user prompt and run the chat
        chat.SetUserPrompt("send a few suggestions to reward the employee that made the largest profit");

        var LLMResponse = await chat.RunAsync<Performer>(CancellationToken.None);

        if (LLMResponse.Status == AiConversationResult.Done)
        {
            // The LLM successfully processed the user prompt and returned its response.
            // The performer's ID, profit, and suggested rewards were stored in the Performers
            // collection by the action tool, and are also returned in the final LLM response.
        }
        #endregion
    }
 
    #region ai-agents_Conversation_action-tool-data-object
    // An object for the LLM response
    public class Performer
    {
        public string employeeID;
        public string profit;
        public string suggestedReward;
    }
    #endregion

    #region ai-agents-full-example
    public async Task createAndRunAiAgent_full()
    {
        var store = new DocumentStore();

        // Define the connection string to OpenAI
        var connectionString = new AiConnectionString
        {
            // Connection string name & identifier
            Name = "open-ai-cs",

            ModelType = AiModelType.Chat,

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

         // Start setting an agent configuration
        var agent = new AiAgentConfiguration("reward-productive-employee", connectionString.Name,
            "You work for a human experience manager. " +
            "The manager uses your services to find which employee earned the largest profit for the company " +
            "and suggest a reward for this employee. " +
            "The manager provides you with the name of a country, or with the word \"everything\" to indicate all countries. " +
            "then you: " +
            "1. use a query tool to load all the orders sent to the selected countery, " +
            "or a query tool to load all orders sent to all countries. " +
            "2. calculate which employee made the largest profit." +
            "3. use a query tool to learn in what region the employee lives." +
            "4. find suitable vacation sites or other rewards based on the employee's residence area." +
            "5. use an action tool to store in the database the employee's ID, profit, and your reward suggestions." +
            "When you're done, return these details in your answer to the user as well."
            );

        // Set agent ID
        agent.Identifier = "reward-productive-employee";

        //  Set agent parametera
        agent.Parameters.Add(new AiAgentParameter("country", "A specific country that orders were shipped to, " +
                                                  "or \"everywhere\" to look for orders shipped to all countries"));

        agent.Queries =
        [
            // Set query tool that triggers the agent to retrieve all the orders sent everywhere
            new AiAgentToolQuery
                {
                    Name = "retrieve-orders-sent-to-all-countries",
                    Description = "a query tool that allows you to retrieve all orders sent to all countries.",
                    Query = "from Orders as O select O.Employee, O.Lines.Quantity",
                    ParametersSampleObject = "{}"
                },
            
                // Set query tool that triggers the agent to retrieve all the orders sent to a specific country
                new AiAgentToolQuery
                {
                    Name = "retrieve-orders-sent-to-a-specific-country",
                    Description = "a query tool that allows you to retrieve all orders sent to a specific country",
                    Query = "from Orders as O where O.ShipTo.Country == $country select O.Employee, O.Lines.Quantity",
                    ParametersSampleObject = "{}"
                },

                // Set query tool that triggers the agent to retrieve the performer's
                // residence region details (country, city, and region) from the database
                new AiAgentToolQuery
                {
                    Name = "retrieve-performer-living-region",
                    Description = "a query tool that allows you to retrieve an employee's country, city, and region, by the employee's ID",
                    Query = "from Employees as E where id() == $employeeId select E.Address.Country, E.Address.City, E.Address.Region",
                    ParametersSampleObject = "{" +
                                                "\"employeeId\": \"embed the employee's ID here\"" +
                                             "}"
                }
        ];

        agent.Actions =
        [
            // Set action tool that triggers the client to store the performer's details
            new AiAgentToolAction
                {
                    Name = "store-performer-details",
                    Description = "an action tool that allows you to store the ID of the employee that made " +
                                  "the largest profit, the profit, and your suggestions for a reward, in the database.",
                    ParametersSampleObject = "{" +
                                                "\"employeeID\": \"embed the employee’s ID here\"," +
                                                "\"profit\": \"embed the employee’s profit here\"," +
                                                "\"suggestedReward\": \"embed your suggestions for a reward here\"" +
                                             "}"
                }
        ];

        // Set chat trimming configuration
        AiAgentSummarizationByTokens summarization = new AiAgentSummarizationByTokens()
        {
            // When the number of tokens stored in the conversation exceeds this limit
            // summarization of old messages will be triggered.
            MaxTokensBeforeSummarization = 32768,
            // The maximum number of tokens that the conversation is allowed to contain
            // after summarization. 
            MaxTokensAfterSummarization = 1024
        };

        agent.ChatTrimming = new AiAgentChatTrimmingConfiguration(summarization);

        // Limit the number of times the LLM can request for tools in response to a single user prompt
        agent.MaxModelIterationsPerCall = 3;

        var createResult = await store.AI.CreateAgentAsync(agent, new Performer
        {
            employeeID = "the ID of the employee that made the largest profit",
            profit = "the profit the employee made",
            suggestedReward = "your suggestions for a reward"
        });

        // Set chat ID, prefix, and agent parameters.
        // In this example the "country" agent parameter is set to "France",
        // to trigger a query that retrieves orders sent to a particular country.
        // Providing "everywhere" instead, will trigger another query that retrieves all orders.
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "Performers/",
            new AiConversationCreationOptions().AddParameter("country", "France"));

        // Handle the action tool that the LLM uses to store the performer's details in the database
        chat.Handle("store-performer-details", (Performer performer) =>
        {
            using (var session1 = store.OpenSession())
            {
                // These values are passed to the client by the action tool
                Performer rewarded = new Performer
                {
                    employeeID = performer.employeeID,
                    profit = performer.profit,
                    suggestedReward = performer.suggestedReward
                };

                // store the values in the Performers collection in the database
                session1.Store(rewarded);
                session1.SaveChanges();
            }
            return "done";
        });

        // Set the user prompt and run the chat
        chat.SetUserPrompt("send a few suggestions to reward the employee that made the largest profit");

        var LLMResponse = await chat.RunAsync<Performer>(CancellationToken.None);

        if (LLMResponse.Status == AiConversationResult.Done)
        {
            // The LLM successfully processed the user prompt and returned its response.
            // The performer's ID, profit, and suggested rewards were stored in the Performers
            // collection by the action tool, and are also returned in the final LLM response.
        }
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

            // The trimming configuration defines if and how the chat history is summarized, 
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
        // Create the agent with just the defined configuration
        CreateAgentAsync(configuration);

        // Create the agent with just the defined configuration
        CreateAgentAsync(AiAgentConfiguration configuration, CancellationToken token = default(CancellationToken));
        
        // Create the agent while passing it a response object
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
            // The maximum number of tokens allowed before summarization is triggered.
            public long? MaxTokensBeforeSummarization { get; set; }

            // The maximum number of tokens allowed in the generated summary.
            public long? MaxTokensAfterSummarization { get; set; }
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

        /*
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
        */
    }
}
