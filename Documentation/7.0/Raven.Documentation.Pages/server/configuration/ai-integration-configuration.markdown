# Configuration: AI Integration
---

{NOTE: }

* The following configuration keys apply to integrating **AI-powered embeddings generation**:
 
  * Embeddings can be generated from your document content via [AI-powered tasks](../../ai-integration/generating-embeddings/overview) and stored in a dedicated collection in the database.  
  * When performing vector search queries, embeddings are also generated from the search term to compare against the stored vectors.

* In this page:
   * [Ai.Embeddings.Generation.Querying.Batching.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.maxbatchsize)  
   * [Ai.Embeddings.Generation.Querying.Batching.MaxConcurrentBatches](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.maxconcurrentbatches)  
   * [Ai.Embeddings.Generation.Querying.Caching.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.caching.maxbatchsize)  
   * [Ai.Embeddings.Generation.Task.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.maxbatchsize)  
   * [Ai.Embeddings.Generation.Task.MaxFallbackTimeInSec](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.maxfallbacktimeinsec)  
   * [Ai.Embeddings.Generation.Task.RetryDelayInSec](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.retrydelayinsec)  
   * [Ai.Embeddings.Generation.Task.RetryStrategy](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.retrystrategy)


{NOTE/}

---

{PANEL: Ai.Embeddings.Generation.Querying.Batching.MaxBatchSize}

The maximum number of query embedding requests to include in a single batch sent to the embeddings generation service.
Optimal values depend on the provider's rate limits and pricing model.

- **Type**: `int`
- **Default**: `128`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Querying.Batching.MaxConcurrentBatches}

The maximum number of query embedding batches that can be processed concurrently.  
This setting controls the degree of parallelism when sending query embedding requests to AI providers.  
Higher values may improve throughput but can increase resource usage and may trigger rate limits.  

- **Type**: `int`
- **Default**: `4`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Querying.Caching.MaxBatchSize}

* Maximum number of embeddings generated from query terms during vector searches that can be stored in the embeddings cache collection in a single batch operation. 

* Caching these embeddings reduces redundant processing and improves retrieval efficiency for identical queries.

---

- **Type**: `int`
- **Default**: `128`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.MaxBatchSize}

The maximum number of documents processed in a single batch by an embeddings generation task.  
Higher values may improve throughput but can increase latency and require more resources and higher limits from the embeddings generation service.

- **Type**: `int?`
- **Default**: `128`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.MaxFallbackTimeInSec}

The maximum time (in seconds) the embeddings generation task remains suspended (fallback mode) following a connection failure to the embeddings generation service.
Once this time expires, the system will retry the connection automatically.

- **Type**: `int`
- **Default**: `60 * 15`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.RetryDelayInSec}

* The **base delay** (in seconds) before retrying a failed embeddings generation task.  
  This applies to both connection failures and failures of individual embedding requests.
 
* The actual wait time between retry attempts depends on the configured [Retry Strategy](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.retrystrategy).  
  When using the `Linear` strategy, the delay increases linearly with each retry attempt (e.g., 15s, 30s, 45s).  
  When using the `Exponential` strategy, the delay increases exponentially with each retry attempt (e.g., 15s, 60s, 120s, 240s).

---

- **Type**: `int`
- **Default**: `15`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.RetryStrategy}

* The strategy to use for retry intervals when embeddings generation fails.  
  Determines how the delay between retries increases after a connection failure or failed embedding request.

* When set to `Linear` - fixed intervals are used between retries, e.g., 15s, 30s, 60s, ...  
  When set to `Exponential` - the wait time increases exponentially after each failure, e.g., 15s, 60s, 120s, 240s ...  
  (with base 15s).

---

- **Type**: `"Linear"` or `"Exponential"`
- **Default**: `"Exponential"`
- **Scope**:  Server-wide or per database

{PANEL/}
