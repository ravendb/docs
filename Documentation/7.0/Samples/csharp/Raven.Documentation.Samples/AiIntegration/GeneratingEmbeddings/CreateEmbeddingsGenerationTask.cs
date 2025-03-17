using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;

namespace Raven.Documentation.Samples.AiIntegration.GeneratingEmbeddings;

public class CreateEmbeddingsGenerationTask
{    
    public async Task Examples()
    {
        using (var store = new DocumentStore())
        {
            #region create_embeddings_task_1
            // Define a connection string:
            // ===========================
            
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToOpenAI", 
                Identifier = "id-for-open-ai-connection-string",
        
                // OpenAI connection settings
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    model: "text-embedding-3-small")
            };
            
            // Deploy the connection string to the server:
            // ===========================================
            var putConnectionStringOp = 
                new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(putConnectionStringOp);
            
            
            // Define the embeddings generation task:
            // ======================================
            var embeddingsTaskConfiguration = new EmbeddingsGenerationConfiguration
            {
                // General info:
                Name = "GetEmbeddingsFromOpenAI",
                Identifier = "id-for-task-open-ai",
                ConnectionStringName = "ConnectionStringToOpenAI",
                Disabled = false,
                
                // Embeddings source & chunking methods - using PATHS configuration:
                Collection = "Categories",
                EmbeddingsPathConfigurations = [
                    new EmbeddingPathConfiguration() { 
                        Path = "Name", 
                        ChunkingOptions = new()
                        {
                            ChunkingMethod = ChunkingMethod.PlainTextSplit,
                            MaxTokensPerChunk = 2048
                        }
                    },
                    new EmbeddingPathConfiguration()
                    {
                        Path = "Desription", 
                        ChunkingOptions = new()
                        {
                            ChunkingMethod = ChunkingMethod.PlainTextSplitLines,
                            MaxTokensPerChunk = 2048
                        }
                    },
                ],
                
                // Quantization & expiration -
                // for embeddings generated from source documents:
                Quantization = VectorEmbeddingType.Single,
                EmbeddingsCacheExpiration = TimeSpan.FromDays(90),
                
                // Chunking method and expiration -
                // for the embeddings generated from search term in vector search query:
                ChunkingOptionsForQuerying = new()
                {
                    ChunkingMethod = ChunkingMethod.PlainTextSplit,
                    MaxTokensPerChunk = 2048
                },
                
                EmbeddingsCacheForQueryingExpiration = TimeSpan.FromDays(14)
            };
            
            // Deploy the connection string to the server:
            // ===========================================
            var addEmbeddingsGenerationTaskOp =
                new AddEmbeddingsGenerationOperation(embeddingsTaskConfiguration);
            var addAiIntegrationTaskResult = store.Maintenance.Send(addEmbeddingsGenerationTaskOp);
            #endregion
        }
        
        using (var store = new DocumentStore())
        {
            // Define a connection string:
            // ===========================
            
            var connectionString = new AiConnectionString
            {
                // Connection string name & identifier
                Name = "ConnectionStringToOpenAI", 
                Identifier = "id-for-open-ai-connection-string",
        
                // OpenAI connection settings
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    model: "text-embedding-3-small")
            };
            
            // Deploy the connection string to the server:
            // ===========================================
            var putConnectionStringOp = 
                new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(putConnectionStringOp);
            
            
            // Define the embeddings generation task:
            // ======================================
            var embeddingsTaskConfiguration = new EmbeddingsGenerationConfiguration
            {
                // General info:
                Name = "GetEmbeddingsFromOpenAI",
                Identifier = "id-for-task-open-ai",
                ConnectionStringName = "ConnectionStringToOpenAI",
                Disabled = false,
                
                #region create_embeddings_task_2
                // Embeddings source & chunking methods - using SCRIPT configuration:
                Collection = "Categories",
                EmbeddingsTransformation = new EmbeddingsTransformation()
                {
                    Script = @"embeddings.generate({
                                   Name: text.split(this.Name, 2048),
                                   Description: text.splitLines(this.Description, 2048)
                             });"
                },
                #endregion
                
                // Quantization & expiration -
                // for embeddings generated from source documents:
                Quantization = VectorEmbeddingType.Single,
                EmbeddingsCacheExpiration = TimeSpan.FromDays(90),
                
                // Chunking method and expiration -
                // for the embeddings generated from search term in vector search query:
                ChunkingOptionsForQuerying = new()
                {
                    ChunkingMethod = ChunkingMethod.PlainTextSplit,
                    MaxTokensPerChunk = 2048
                },
                
                EmbeddingsCacheForQueryingExpiration = TimeSpan.FromDays(14)
            };
            
            // Deploy the connection string to the server:
            // ===========================================
            var addEmbeddingsGenerationTaskOp =
                new AddEmbeddingsGenerationOperation(embeddingsTaskConfiguration);
            var addAiIntegrationTaskResult = store.Maintenance.Send(addEmbeddingsGenerationTaskOp);
        }
    }
}

/*
#region syntax
// The 'EmbeddingsGenerationConfiguration' class inherits from 'EtlConfiguration'
// and provides specialized configurations for the embeddings tasks:
// ==============================================================================

public class EmbeddingsGenerationConfiguration : EtlConfiguration<AiConnectionString>
{
    public string Identifier { get; set; }
    public string Collection { get; set; }
    public List<EmbeddingPathConfiguration> EmbeddingsPathConfigurations { get; set; }
    public EmbeddingsTransformation EmbeddingsTransformation { get; set; }
    public VectorEmbeddingType Quantization { get; set; }
    public ChunkingOptions ChunkingOptionsForQuerying { get; set; }
    public TimeSpan EmbeddingsCacheExpiration { get; set; } = TimeSpan.FromDays(90);
    public TimeSpan EmbeddingsCacheForQueryingExpiration { get; set; } = TimeSpan.FromDays(14);
}

public class EmbeddingPathConfiguration
{
    public string Path { get; set; }
    public ChunkingOptions ChunkingOptions { get; set; }
}

public class ChunkingOptions
{
    public ChunkingMethod ChunkingMethod { get; set; }
    public int MaxTokensPerChunk { get; set; } = 512;
}

public enum ChunkingMethod
{
    PlainTextSplit,
    PlainTextSplitLines,
    PlainTextSplitParagraphs,
    MarkDownSplitLines,
    MarkDownSplitParagraphs,
    HtmlStrip
}

public class EmbeddingsTransformation
{
    public string Script { get; set; }
}

public enum VectorEmbeddingType
{
    Single,
    Int8,
    Binary,
    Text
}
#endregion
*/
