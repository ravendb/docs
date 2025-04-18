# Generating Embeddings - Overview
---

{NOTE: }

* RavenDB can serve as a vector database, see [Why choose RavenDB as your vector database](../../ai-integration/vector-search/ravendb-as-vector-database#why-choose-ravendb-as-your-vector-database).

* Vector search can be performed on:   
   * Raw text stored in your documents.   
   * Pre-made embeddings that you created yourself and stored using these [Data types](../../ai-integration/vector-search/data-types-for-vector-search#numerical-data).  
   * Pre-made embeddings that are automatically generated from your document content by RavenDB's tasks  
     using external service providers, as explained below.

---

* In this article:
  * [Embeddings generation - overview](../../ai-integration/generating-embeddings/overview#embeddings-generation---overview)
     * [Embeddings generation - process flow](../../ai-integration/generating-embeddings/overview#embeddings-generation---process-flow)
     * [Supported providers](../../ai-integration/generating-embeddings/overview#supported-providers)
  * [Creating an embeddings generation task](../../ai-integration/generating-embeddings/overview#creating-an-embeddings-generation-task)
  * [Monitoring the tasks](../../ai-integration/generating-embeddings/overview#monitoring-the-tasks)

{NOTE/}

---

{PANEL: Embeddings generation - overview}

{CONTENT-FRAME: }

#### Embeddings generation - process flow
---

* **Define an Embeddings Generation Task**:  
  Specify a [connection string](../../ai-integration/connection-strings/connection-strings-overview) that defines the AI provider and model for generating embeddings.  
  Define the source content - what parts of the documents will be used to create the embeddings.  

* **Source content is processed**:  
  1. The task extracts the specified content from the documents.  
  2. If a processing script is defined, it transforms the content before further processing.  
  3. The text is split according to the defined chunking method; a separate embedding will be created for each chunk.  
  4. Before contacting the provider, RavenDB checks the [embeddings cache](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-cache-collection)
     to determine whether an embedding already exists for the given content from that provider.
  5. If a matching embedding is found, it is reused, avoiding unnecessary requests.  
     If no cached embedding is found, the transformed and chunked content is sent to the configured AI provider.  

* **Embeddings are generated by the AI provider**:  
  The provider generates embeddings and sends them back to RavenDB.  
  If quantization was defined in the task, RavenDB applies it to the embeddings before storing them.

* **Embeddings are stored in your database**:  
  * Each embedding is stored as an attachment in a [dedicated collection](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-collection).  
  * RavenDB maintains an [embeddings cache](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-cache-collection),
    allowing reuse of embeddings for the same source content and reducing provider calls.
    Cached embeddings expire after a configurable duration.

* **Perform vector search:**  
  Once the embeddings are stored, you can perform vector searches on your document content by:  
  * Running a [dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query#querying-pre-made-embeddings-generated-by-tasks), which automatically creates an auto-index for the search.  
  * Defining a [static index](../../ai-integration/vector-search/vector-search-using-static-index#indexing-pre-made-text-embeddings) to store and query embeddings efficiently.  

      The query search term is split into chunks, and each chunk is looked up in the cache.  
      If not found, RavenDB requests an embedding from the provider and caches it.  
      The embedding (cached or newly created) is then used to compare against stored vectors. 

* **Continuous processing**:  
  * Embeddings generation tasks are [Ongoing Tasks](../../studio/database/tasks/ongoing-tasks/general-info) that process documents as they change.  
    Before contacting the provider after a document change, the task first checks the cache to see if a matching embedding already exists, avoiding unnecessary requests.
  * The requests to generate embeddings from the source text are sent to the provider in batches.  
    The batch size is configurable, see the [Ai.Embeddings.MaxBatchSize](../../server/configuration/ai-integration-configuration#ai.embeddings.maxbatchsize) configuration key.  
  * A failed embeddings generation task will retry after the duration set in the  
    [Ai.Embeddings.MaxFallbackTimeInSec](../../server/configuration/ai-integration-configuration#ai.embeddings.maxfallbacktimeinsec) configuration key.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Supported providers
---

* The following service providers are supported for auto-generating embeddings using tasks:

    * [OpenAI & OpenAI-compatible providers](../../ai-integration/connection-strings/open-ai)
    * [Azure Open AI](../../ai-integration/connection-strings/azure-open-ai)
    * [Google AI](../../ai-integration/connection-strings/google-ai)
    * [Hugging Face](../../ai-integration/connection-strings/hugging-face)
    * [Ollama](../../ai-integration/connection-strings/ollama)
    * [Mistral AI](../../ai-integration/connection-strings/mistral-ai)
    * [bge-micro-v2](../../ai-integration/connection-strings/embedded) (a local embedded model within RavenDB)

{CONTENT-FRAME/}

![flow chart](images/embeddings-generation-task-flow.png)

![flow chart](images/vector-search-flow.png)

{PANEL/}

{PANEL: Creating an embeddings generation task}

* An embeddings generation tasks can be created from:
    * The **AI Tasks view in the Studio**, where you can create, edit, and delete tasks. Learn more in [AI Tasks - list view](../../ai-integration/ai-tasks-list-view).
    * The **Client API** - see [Configuring an embeddings generation task - from the Client API](../../ai-integration/generating-embeddings/embeddings-generation-task#configuring-an-embeddings-generation-task---from-the-client-api)

---

* From the Studio:  

     ![Add ai task 1](images/add-ai-task-1.png "Add AI Task")

     1. Go to the **AI Hub** menu.
     2. Open the **AI Tasks** view.
     3. Click **Add AI Task** to add a new task.

     ![Add ai task 2](images/add-ai-task-2.png "Add a new Embeddings Generation Task")

* See the complete details of the task configuration in the [Embeddings generation task](../../ai-integration/generating-embeddings/embeddings-generation-task) article.

{PANEL/}

{PANEL: Monitoring the tasks}

* The status and state of each embeddings generation task are visible in the [AI Tasks - list view](../../ai-integration/ai-tasks-list-view).

* Task performance and activity over time can be analyzed in the _AI Tasks Stats_ view,  
  where you can track processing duration, batch sizes, and overall progress.  
  Learn more about the functionality of the stats view in the [Ongoing Tasks Stats](../../studio/database/stats/ongoing-tasks-stats/overview) article.

* The number of embeddings generation tasks across all databases can also be monitored using [SNMP](../../server/administration/SNMP/snmp).  
  The following SNMP OIDs provide relevant metrics:
  * [5.1.11.25](../../server/administration/SNMP/snmp#5.1.11.25) – Total number of enabled embeddings generation tasks.
  * [5.1.11.26](../../server/administration/SNMP/snmp#5.1.11.26) – Total number of active embeddings generation tasks.

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../../ai-integration/vector-search/ravendb-as-vector-database)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index)
- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query)

### Embeddings Generation

- [Embeddings generation task](../../ai-integration/generating-embeddings/embeddings-generation-task)
- [The Embedding Collections](../../ai-integration/generating-embeddings/embedding-collections)

### AI Connection Strings

- [Connection strings - overview](../../ai-integration/connection-strings/connection-strings-overview)
