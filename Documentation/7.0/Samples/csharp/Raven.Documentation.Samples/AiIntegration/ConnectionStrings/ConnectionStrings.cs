using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;

namespace Raven.Documentation.Samples.AiIntegration.ConnectionStrings;

public class CreateConnectionStrings
{
    public async Task Examples()
    {
        #region create_connection_string_azure_open_ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to Azure OpenAI
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToAzureOpenAI", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Azure OpenAI connection settings
                AzureOpenAiSettings = new AzureOpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://your-resourse-name.openi.azure.com",
                    model: "text-embedding-3-small",
                    deploymentName: "your-deployment-name")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_google_ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to Google AI
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToGoogleAI", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Google AI connection settings
                GoogleSettings = new GoogleSettings(
                    apiKey: "your-api-key",
                    model: "text-embedding-004",
                    aiVersion: GoogleAIVersion.V1)
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_hugging_face
        using (var store = new DocumentStore())
        {
            // Define the connection string to Hugging Face
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToHuggingFace", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Hugging Face connection settings
                HuggingFaceSettings = new HuggingFaceSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api-inference.huggingface.co/pipeline/feature-extraction",
                    model: "sentence-transformers/all-MiniLM-L6-v2")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_ollama
        using (var store = new DocumentStore())
        {
            // Define the connection string to Ollama
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToOllama", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Ollama connection settings
                OllamaSettings = new OllamaSettings(
                    uri: "http://localhost:11434/api/embed",
                    model: "mxbai-embed-large")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_open_ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to OpenAI
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToOpenAI", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // OpenAI connection settings
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    model: "text-embedding-3-small")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_mistral_ai
        using (var store = new DocumentStore())
        {
            // Define the connection string to Mistral AI
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToMistralAI", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Mistral AI connection settings
                MistralAiSettings = new MistralAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.mistral.ai/v1",
                    model: "mistral-embed")
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
        
        #region create_connection_string_embedded
        using (var store = new DocumentStore())
        {
            // Define the connection string to the embedded model
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToEmbedded", 
                Identifier = "identifier-to-the-connection-string", // optional
                
                // Embedded model settings
                // No user configuration is required for the embedded model,
                // as it uses predefined values managed internally by RavenDB.
                EmbeddedSettings = new EmbeddedSettings()
            };
            
            // Deploy the connection string to the server
            var operation = new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(operation);
        }
        #endregion
    }
}

#region azure_open_ai_settings
public class AzureOpenAiSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string Model { get; set; }
    public string DeploymentName { get; set; }
    public int? Dimensions { get; set; }
}
#endregion

#region google_ai_settings
public class GoogleSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public GoogleAIVersion? AiVersion { get; set; }
    public int? Dimensions { get; set; }
}

public enum GoogleAIVersion
{
    V1, // Represents the "v1" version of the Google AI API
    V1_Beta // Represents the "v1beta" version of the Google AI API
}
#endregion

#region hugging_face_settings
public class HuggingFaceSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string Model { get; set; }
}
#endregion

#region ollama_settings
public class OllamaSettings
{
    public string Uri { get; set; }
    public string Model { get; set; }
}
#endregion

#region open_ai_settings
public class OpenAiSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string Model { get; set; }
    public int? Dimensions { get; set; }
    public string OrganizationId { get; set; }
    public string ProjectId { get; set; }
}
#endregion

#region mistral_ai_settings
public class MistralAiSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string Model { get; set; }
}
#endregion
