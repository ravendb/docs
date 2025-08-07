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
        var agent = new AiAgentConfiguration("document-summarizing-assistant", connectionString.Name,
            "You are an AI agent of a university library, summarizing documents from a documentation database. " +
            "you get from a student a subject that the student wants to learn about, and do as follows: " +
            "1.Use a query tool to retrieve from the database a list of document IDs and titles where the titles " +
            "include the student's subject. " +
            "2.Go through the document titles and decide which document is the most helpful for the student. " +
            "3.Retrieve the chose article's text from the database by triggering the relevant action tool. " +
            "4.Summarize the document and hand the student the document's title and summary.");
        #endregion

        #region ai-agents_agent-identifier
        // Agent ID
        agent.Identifier = "document-summarizing-assistant";
        #endregion

        #region ai-agents_AiAgentParameter_function
        //  Agent parametera
        agent.Parameters.Add(new AiAgentParameter("subject", "the subject provided by the student"));
        #endregion

        #region ai-agents_agent_sampleObjectString
        // Set sample object
        agent.SampleObject = "{" +
                                "\"DocumentName\": \"The document's original title\", " +
                                "\"DocumentSummary\": \"The summarized document\"" +
                             "}";
        #endregion

        #region ai-agents_agent_query-tool-sample
        agent.Queries =
        [   // Set query tool
            new AiAgentToolQuery
            {
                Name = "retrieveDocumentTitles",
                Description = "Use this query to load all the IDs and titles of documents whose title " +
                              "includes the student's subject",
                Query = "from DocumentationPages where search(Title, $subject) order by id() select id(), Title",
                ParametersSampleObject = "{\"ID\": \"The ID of the retrieved document\", " +
                                         "\"Title\": \"The Title of the retrieved document\"}"
            }
        ];
        #endregion

        #region ai-agents_agent_action-tool-sample
        
        agent.Actions =
        [   // set action tool
            new AiAgentToolAction
            {
                Name = "retrieveDocumentText",
                Description = "Trigger a user query that will load the text of the document you selected for the student",
                ParametersSampleObject = "{ \"DocumentID\": \"The ID of the selected document\" }"
            }
        ];
        #endregion

        #region ai-agents_CreateAgentAsync_example
        var createResult = await store.AI.CreateAgentAsync(agent);
        #endregion

        #region ai-agents_Conversation_example
        // Set chat
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "summaries/",
            new AiConversationCreationOptions().AddParameter("subject", "indexing"));
        
        // Set user prompt
        chat.SetUserPrompt("Tell me about indexing of counters in RavenDB");
        
        // Run the chat
        var reply = await chat.RunAsync<OutputSchem>(CancellationToken.None);
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
        var agent = new AiAgentConfiguration("document-summarizing-assistant", connectionString.Name,
            "You are an AI agent of a university library, summarizing documents from a documentation database. " +
            "you get from a student a subject that the student wants to learn about, and do as follows: " +
            "1.Use a query tool to retrieve from the database a list of document IDs and titles where the titles " +
            "include the student's subject. " +
            "2.Go through the document titles and decide which document is the most helpful for the student. " +
            "3.Retrieve the chose article's text from the database by triggering the relevant action tool. " +
            "4.Summarize the document and hand the student the document's title and summary.");

        // Agent ID
        agent.Identifier = "document-summarizing-assistant";

        //  Agent parametera
        agent.Parameters.Add(new AiAgentParameter("subject", "the subject provided by the student"));

        // Set sample object
        agent.SampleObject = "{" +
                                "\"DocumentName\": \"The document's original title\", " +
                                "\"DocumentSummary\": \"The summarized document\"" +
                             "}";

        agent.Queries =
        [   // Set query tool
            new AiAgentToolQuery
            {
                Name = "retrieveDocumentTitles",
                Description = "Use this query to load all the IDs and titles of documents whose title " +
                              "includes the student's subject",
                Query = "from DocumentationPages where search(Title, $subject) order by id() select id(), Title",
                ParametersSampleObject = "{\"ID\": \"The ID of the retrieved document\", " +
                                         "\"Title\": \"The Title of the retrieved document\"}"
            }
        ];

        agent.Actions =
        [   // set action tool
            new AiAgentToolAction
            {
                Name = "retrieveDocumentText",
                Description = "Trigger a user query that will load the text of the document you selected for the student",
                ParametersSampleObject = "{ \"DocumentID\": \"The ID of the selected document\" }"
            }
        ];

        var createResult = await store.AI.CreateAgentAsync(agent);
        // Set chat
        var chat = store.AI.Conversation(
            createResult.Identifier,
            "summaries/",
            new AiConversationCreationOptions().AddParameter("subject", "indexing"));

        // Set user prompt
        chat.SetUserPrompt("Tell me about indexing of counters in RavenDB");

        // Run the chat
        var r = await chat.RunAsync<OutputSchem>(CancellationToken.None);
    }

    public class OutputSchem
    {
        public static OutputSchem Instance = new();

        public string DocumentName = "The document's original title";

        public string DocumentSummary = "The summarized document";
    }
    #endregion


    private interface IFoo
    {
        /*
        #region ai-agents_AiAgentConfiguration_definition
        public AiAgentConfiguration(string name, string connectionStringName, string systemPrompt);
        #endregion
        */

        #region ai-agents_AiAgentConfiguration-class_definition
        public class AiAgentConfiguration
        {
            public string Identifier { get; set; }
            public string Name { get; set; }
            public string ConnectionStringName { get; set; }
            public string SystemPrompt { get; set; }
            public string SampleObject { get; set; }
            public string OutputSchema { get; set; }
            public List<AiAgentToolQuery> Queries { get; set; } = new List<AiAgentToolQuery>();
            public List<AiAgentToolAction> Actions { get; set; } = new List<AiAgentToolAction>();
            public List<AiAgentParameter> Parameters { get; set; } = new List<AiAgentParameter>();
            public AiAgentChatTrimmingConfiguration ChatTrimming { get; set; } = new AiAgentChatTrimmingConfiguration(new AiAgentSummarizationByTokens());
            public int? MaxModelIterationsPerCall { get; set; }
        }
        #endregion

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
    }
}
