using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.AI;
using Raven.Client.Documents.Operations.ConnectionStrings;

namespace Raven.Documentation.Samples.AiIntegration.GeneratingEmbeddings;

public class CreateEmbeddingsGenerationTask
{    
    public async Task Examples()
    {
        // This example shows using PATHS:
        // ===============================
        
        using (var store = new DocumentStore())
        {
            #region create_embeddings_task_1
            // Define a connection string that will be used in the task definition:
            // ====================================================================
            
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
        
        // This example shows using a SCRIPT:
        // ==================================
        
        using (var store = new DocumentStore())
        {
            var connectionString = new AiConnectionString
            {
                Name = "ConnectionStringToOpenAI", 
                Identifier = "id-for-open-ai-connection-string",
        
                OpenAiSettings = new OpenAiSettings(
                    apiKey: "your-api-key",
                    endpoint: "https://api.openai.com/v1",
                    model: "text-embedding-3-small")
            };
            
            var putConnectionStringOp = 
                new PutConnectionStringOperation<AiConnectionString>(connectionString);
            var putConnectionStringResult = store.Maintenance.Send(putConnectionStringOp);
            
            var embeddingsTaskConfiguration = new EmbeddingsGenerationConfiguration
            {
                Name = "GetEmbeddingsFromOpenAI",
                Identifier = "id-for-task-open-ai",
                ConnectionStringName = "ConnectionStringToOpenAI",
                Disabled = false,
                
                #region create_embeddings_task_2
                // Source collection:
                Collection = "Categories",
                
                // Use 'EmbeddingsTransformation':
                EmbeddingsTransformation = new EmbeddingsTransformation()
                {
                    // Define the script:
                    Script = 
                        @"embeddings.generate({

                            // Process the document 'Name' field using method text.split()
                            Name: text.split(this.Name, 2048),

                            // Process the document 'Description' field using method text.splitLines()
                            Description: text.splitLines(this.Description, 2048)
                        });"
                },
                #endregion
                
                Quantization = VectorEmbeddingType.Single,
                EmbeddingsCacheExpiration = TimeSpan.FromDays(90),
                
                ChunkingOptionsForQuerying = new()
                {
                    ChunkingMethod = ChunkingMethod.PlainTextSplit,
                    MaxTokensPerChunk = 2048
                },
                
                EmbeddingsCacheForQueryingExpiration = TimeSpan.FromDays(14)
            };
            
            var addEmbeddingsGenerationTaskOp =
                new AddEmbeddingsGenerationOperation(embeddingsTaskConfiguration);
            var addAiIntegrationTaskResult = store.Maintenance.Send(addEmbeddingsGenerationTaskOp);
        }
    }
}

/*
#region syntax_1
// The 'EmbeddingsGenerationConfiguration' class inherits from 'EtlConfiguration'
// and provides the following specialized configurations for the embeddings generation task:
// =========================================================================================

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
#endregion

#region syntax_2
public class EmbeddingPathConfiguration
{
    public string Path { get; set; }
    public ChunkingOptions ChunkingOptions { get; set; }
}

public class ChunkingOptions
{
    public ChunkingMethod ChunkingMethod { get; set; } // Default is PlainTextSplit
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

#region syntax_3
public AddEmbeddingsGenerationOperation(EmbeddingsGenerationConfiguration configuration);
#endregion
*/


