# Configuration: AI Integration
---

{NOTE: }

* The following configuration keys apply to integrating **AI-powered embeddings generation**:
 
  * Embeddings can be generated from your document content via [AI-powered tasks](../../todo) and stored in a dedicated collection in the database.  
  * When performing vector search queries, embeddings are also generated from the search term to compare against the stored vectors.

* In this page:
   * [Ai.Embeddings.Generation.Querying.Batching.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.maxbatchsize)  
   * [Ai.Embeddings.Generation.Querying.Batching.MaxConcurrentBatches](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.maxconcurrentbatches)  
   * [Ai.Embeddings.Generation.Querying.Batching.MaxRetries](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.maxretries)  
   * [Ai.Embeddings.Generation.Querying.Batching.RetryDelayInMs](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.retrydelayinms)  
   * [Ai.Embeddings.Generation.Querying.Batching.TimeoutInMs](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.batching.timeoutinms)  
   * [Ai.Embeddings.Generation.Querying.Caching.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.querying.caching.maxbatchsize)  
   * [Ai.Embeddings.Generation.Task.FallbackModeStrategy](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.fallbackmodestrategy)  
   * [Ai.Embeddings.Generation.Task.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.maxbatchsize)  
   * [Ai.Embeddings.Generation.Task.MaxFallbackTimeInSec](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.maxfallbacktimeinsec)  
   * [Ai.Embeddings.Generation.Task.RetryDelayInSec](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.retrydelayinsec)    


{NOTE/}

---

{PANEL: Ai.Embeddings.Generation.Querying.Batching.MaxBatchSize}

The maximum number of query embedding requests to include in a single batch sent to the AI provider.  
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

{PANEL: Ai.Embeddings.Generation.Querying.Batching.MaxRetries}

The maximum number of retry attempts for failed query embedding generation requests before giving up.  
Retries are performed using an exponential backoff strategy, where the wait time between attempts increases exponentially.

- **Type**: `int`
- **Default**: `3`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Querying.Batching.RetryDelayInMs}

The **base delay** (in milliseconds) between retry attempts for failed query embedding requests.  
Actual delay increases exponentially with each retry attempt.  
For example, with a base delay of 200ms, retries would occur after 200ms, 400ms, 800ms, etc.

- **Type**: `int`
- **Default**: `200`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Ai.Embeddings.Generation.Querying.Batching.TimeoutInMs}

The time (in milliseconds) to wait for additional query embedding requests before sending the current batch to the AI provider.
Lower values decrease latency for query embedding generation but may reduce throughput.

- **Type**: `int`
- **Default**: `200`
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

{PANEL: Ai.Embeddings.Generation.Task.FallbackModeStrategy}

* The strategy to use for retry intervals when embeddings generation fails.  
  Determines how the delay between retries increases after a connection failure or failed embedding request.

* When set to `Linear` - fixed intervals are used between retries, e.g., 15s, 30s, 60s, ...  
  When set to `Exponential` - the wait time increases exponentially after each failure, e.g., 15s, 225s, 3375s, ...

---

- **Type**: `"Linear"` or `"Exponential"`
- **Default**: `"Exponential"`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.MaxBatchSize}

The maximum number of documents processed in a single batch by an embeddings generation task.  
Higher values may improve throughput but require more resources and higher limits from the AI service.

- **Type**: `int?`
- **Default**: `128`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.MaxFallbackTimeInSec}

The maximum time (in seconds) the embeddings generation task remains suspended (fallback mode) following a connection failure to the AI provider.
Once this time expires, the system will retry the connection automatically.

- **Type**: `int`
- **Default**: `60 * 15`
- **Scope**:  Server-wide or per database

{PANEL/}

{PANEL: Ai.Embeddings.Generation.Task.RetryDelayInSec}

* The **base delay** (in seconds) before retrying a failed embeddings generation task.  
  This applies to both connection failures and failures of individual embedding requests.
 
* The actual wait time between retry attempts depends on the configured [FallbackModeStrategy](../../server/configuration/ai-integration-configuration#ai.embeddings.generation.task.fallbackmodestrategy).  
  When using the `Linear` strategy, the delay increases linearly with each retry attempt (e.g., 15s, 30s, 45s).  
  When using the `Exponential` strategy, the delay increases exponentially with each retry attempt (e.g., 15s, 225s, 3375s).

---

- **Type**: `int`
- **Default**: `15`
- **Scope**:  Server-wide or per database

{PANEL/}
