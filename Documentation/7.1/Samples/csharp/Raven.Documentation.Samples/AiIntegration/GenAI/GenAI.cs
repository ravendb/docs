using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Sparrow.Json;
using static Akka.Streams.Implementation.Fusing.GraphInterpreter;

namespace Raven.Documentation.Samples.AiIntegration.ConnectionStrings;

public class GenAI
{
    public async Task Examples()
    {
        #region gen-ai-create-connection-string_ollama
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
                    // local Ollama mode URL
                    uri: "http://localhost:11434/")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion

        #region gen-ai_create-connection-string-open-ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to Ollama
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "open-ai-cs",

                // Ollama connection settings
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    // LLM Ollama model for text generation
                    model: "gpt-4o-mini")
            };

            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion

        

        using (var store = new DocumentStore())
        {
            #region gen-ai_define-gen-ai-task
            GenAiConfiguration config = new GenAiConfiguration
            {
                // Task name
                Name = "FilterSpam",

                // Unique task identifier
                Identifier = "FilterSpam",

                // Connection string to AI model
                ConnectionStringName = "ollama-cs",

                // Task is enabled
                Disabled = false,

                // Collection associated with the task
                Collection = "Posts",

                // Context generation script - format for objects to be sentto the AI model
                GenAiTransformation = new GenAiTransformation {
                    Script = @"
                        for(const comment of this.Comments)
                        {
                            ai.genContext({Text: comment.Text, Author: comment.Author, Id: comment.Id});}"
                },

                // AI model Prompt - the instructions sent to the AI model
                Prompt = "Check if the following blog post comment is spam or not",

                // JSON schema - a sample response object to format AI model replies by
                SampleObject = JsonConvert.SerializeObject(
                    new
                    {
                        Blocked = true,
                        Reason = "Concise reason for why this comment was marked as spam or ham"
                    }),

                // Update script - specifies what to do with AI model replies
                UpdateScript = @"    
                        // Find the comment
                        const idx = this.Comments.findIndex(c => c.Id == $input.Id);  
                        // Was detected as spam
                        if($output.Blocked)
                        {
                            // Remove this comment
                            this.Comments.splice(idx, 1);
                        }",

                // Max concurrent connections to AI model
                MaxConcurrency = 4
            };

            // Run the task
            var GenAiOperation = new AddGenAiOperation(config);
            var addAiIntegrationTaskResult = store.Maintenance.Send(GenAiOperation);
            #endregion
        }

    }
}

/*
#region ollama_settings
public class AiConnectionString
{
    public string Name { get; set; }
    public string Identifier { get; set; }
    public OllamaSettings OllamaSettings { get; set; }
}

public class OllamaSettings : AbstractAiSettings
{
    public string Model { get; set; }
    public string Uri { get; set; }
}
#endregion

#region open-ai_settings
public class AiConnectionString
{
    public string Name { get; set; }
    public string Identifier { get; set; }
    public OpenAiSettings OpenAiSettings { get; set; }
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

public class AbstractAiSettings
{
    public int? EmbeddingsMaxConcurrentBatches { get; set; }
}
#endregion

*/
