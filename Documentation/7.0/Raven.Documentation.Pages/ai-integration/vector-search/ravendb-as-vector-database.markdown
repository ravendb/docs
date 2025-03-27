# RavenDB as a Vector Database
---

{NOTE: }

* In this article:
    * [What is a vector database](../../ai-integration/vector-search/ravendb-as-vector-database#what-is-a-vector-database)
    * [Why choose RavenDB as your vector database](../../ai-integration/vector-search/ravendb-as-vector-database#why-choose-ravendb-as-your-vector-database)
    
{NOTE/}

---

{PANEL: What is a vector database}

* A vector database stores data as high-dimensional numerical representations (embedding vectors),  
  enabling searches based on contextual meaning and vector similarity rather than exact keyword matches.  
  Instead of relying on traditional indexing, it retrieves relevant results by measuring how close vectors are in a multi-dimensional space.

* Vector databases are widely used in applications such as:  

   * Semantic search – Finding documents based on meaning rather than exact words.
   * Recommendation engines – Suggesting products, media, or content based on similarity.
   * AI and machine learning – Powering LLMs, multi-modal search, and object detection.

**Embeddings**:  

* A vector database stores data as high-dimensional vectors in a high-dimensional space.  
  These vectors, known as **embeddings**, are mathematical representations of your data.

* Each embedding is an array of numbers (e.g. [0.45, 3.6, 1.25, 0.7, ...]), where each dimension represents specific characteristics of the data, capturing its contextual meaning.  
  Words, phrases, entire documents, images, audio, and other types of data can all be vectorized.

* The raw data is converted into embeddings using [transformers](https://huggingface.co/docs/transformers).  
  To optimize storage and computation, transformers can encode embeddings with lower-precision data types, such as 8-bit integers, through a technique called [quantization](../../ai-integration/vector-search/vector-search-using-dynamic-query#quantization-options).

**Indexing embeddings and searching**:  

* The embedding vectors are indexed and stored in a vector space.
  Their positions reflect relationships and characteristics of the data as determined by the model that generated them.
  The distance between two embeddings in the vector space correlates with the similarity of their original inputs within that model's context.
  
* Vectors representing similar data are positioned close to each other in the vector space.  
  This is achieved using algorithms such as [HNSW](https://en.wikipedia.org/wiki/Hierarchical_navigable_small_world), which is designed for indexing and querying embeddings.
  HNSW constructs a graph-based structure that efficiently retrieves approximate nearest neighbors in high-dimensional spaces.

* This architecture enables **similarity searches**. Instead of conventional keyword-based queries,  
  a vector database lets you find relevant data based on semantic and contextual meaning.

{PANEL/}

{PANEL: Why choose RavenDB as your vector database}

##### An integrated solution:  

* RavenDB provides an integrated solution that combines high-performance NoSQL capabilities with advanced vector indexing and querying features,
  enabling efficient storage and management of high-dimensional vector data.

##### Reduced infrastructure complexity:

* RavenDB's built-in vector search eliminates the need for external vector databases,  
  simplifying your infrastructure and reducing maintenance overhead.

##### AI integration:  

* You can use RavenDB as the **vector database** for your AI-powered applications, including large language models (LLMs).
  This eliminates the need to transfer data to expensive external services for vector similarity search,
  providing a cost-effective and efficient solution for vector-based operations.

##### Multiple field types in indexes:  

* An index can consist of multiple index-fields, each having a distinct type, such as a standard field, a spatial field, a full-text search field, or a **vector field**.
  This flexibility allows you to work with complex documents containing various data types and retrieve meaningful insights by querying the index across all these fields.  
  An example is available in [Indexing multiple field types](../../ai-integration/vector-search/vector-search-using-static-index#indexing-multiple-field-types).

* Document [attachments](../../ai-integration/vector-search/indexing-attachments-for-vector-search) can also be indexed as vector fields, and Map-Reduce indexes can incorporate vector fields in their reduce phase, 
  further extending the versatility of your data processing and search capabilities.

##### Built-in embedding support:

* **Textual input**:  
  Embeddings can be automatically generated from textual content within your documents by defining  
  [Embeddings generation tasks](../../ai-integration/generating-embeddings/overview).
  These tasks connect to external embedding providers such as **Azure OpenAI, OpenAI, Hugging Face, Google AI, Ollama, or Mistral AI**.
  If no task is specified, embeddings will be generated using the built-in [bge-micro-v2](https://huggingface.co/TaylorAI/bge-micro-v2) model.
  
     When querying with a phrase, RavenDB generates an embedding for the search term using the same model applied to the document data
     and compares it against the indexed embeddings.

* **Numerical arrays input**:  
  Documents in RavenDB can also contain numerical arrays with **pre-made embeddings** created elsewhere.  
  Use RavenDB's dedicated data type, [RavenVector](../../ai-integration/vector-search/data-types-for-vector-search#ravenvector), to store these embeddings in your document entities.  
  This type is highly optimized to reduce storage space and enhance the speed of reading arrays from disk.

* **HNSW algorithm usage**:  
  All embeddings, whether generated from textual input or pre-made numerical arrays,  
  are indexed and searched for using the [HNSW](https://en.wikipedia.org/wiki/Hierarchical_navigable_small_world) algorithm.

* **Optimize storage via quantization**:  
  RavenDB allows you to select the quantization format for the generated embeddings when creating the index.  
  Learn more in [Quantization options](../../ai-integration/vector-search/vector-search-using-dynamic-query#quantization-options).

* **Perform vector search**:  
  Leverage RavenDB's [Auto-indexes](../../ai-integration/vector-search/vector-search-using-dynamic-query)
  and [Static indexes](../../ai-integration/vector-search/vector-search-using-static-index) to perform a vector search,  
  retrieving documents based on contextual similarity rather than exact word matches.

{PANEL/}

## Related Articles

### Client API

- [RQL](../../client-api/session/querying/what-is-rql) 
- [Query overview](../../client-api/session/querying/how-to-query)

### Vector Search

- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index.markdown)
- [Data types for vector search](../../ai-integration/vector-search/data-types-for-vector-search)

### Server

- [indexing configuration](../../server/configuration/indexing-configuration)
