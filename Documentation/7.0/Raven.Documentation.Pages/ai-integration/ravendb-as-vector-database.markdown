# RavenDB as a Vector Database
---

{NOTE: }

* In this page:
    * [What is a vector database](../ai-integration/ravendb-as-vector-database#what-is-a-vector-database)
    * [Why choose RavenDB as your vector database](../ai-integration/ravendb-as-vector-database#why-choose-ravendb-as-your-vector-database)
    
{NOTE/}

---

{PANEL: What is a vector database}

**Embeddings**:  

* A vector database stores data as high-dimensional vectors in a high-dimensional space.  
  These vectors, known as **embeddings**, are mathematical representations of your data.

* Each embedding is an array of numbers, where each dimension corresponds to specific characteristics of the data, capturing its semantic or contextual meaning.
  Words, phrases, entire documents, images, audio, and other types of data can all be vectorized.

* The raw data is converted into embeddings using [transformers](https://huggingface.co/docs/transformers).  
  To reduce storage and computation, transformers can encode embeddings with lower-precision data types, such as 8-bit integers, through a technique called **quantization**.

**Indexing embeddings and semantic searching**:  

* The embedding vectors are indexed and stored in a vector space. Their positions in the space reflect relationships and characteristics of the data.
  The distance between two embeddings in the vector space correlates with the semantic similarity of their original inputs. 
  
* Vectors representing similar data are located close to each other in the vector space.  
  This is achieved using algorithms such as [HNSW](https://en.wikipedia.org/wiki/Hierarchical_navigable_small_world), a vector search algorithm designed for indexing and querying embeddings.
  HNSW constructs a graph-based structure that efficiently navigates and retrieves approximate nearest neighbors in high-dimensional spaces.

* This architecture enables **similarity searches**. Instead of conventional keyword-based queries,  
  a vector database allows you to search for relevant data based on semantic and contextual meaning.

{PANEL/}

{PANEL: Why choose RavenDB as your vector database}

##### An integrated solution:  

* RavenDB provides an integrated solution that combines high-performance NoSQL capabilities with advanced vector indexing and querying features,
  enabling efficient storage and management of high-dimensional vector data.

##### Data privacy and ownership:  

* With RavenDB, your data remains private. 
  There's no need to integrate with external vector databases, keeping your sensitive data secure within your own infrastructure.

##### AI integration:  

* You can use RavenDB as the vector database for your AI-powered applications, including large language models (LLMs).
  This eliminates the need to transfer data to expensive external services for vector similarity search,
  providing a cost-effective and efficient solution for vector-based operations.

##### Multiple field types in indexes:  

* An index can combine different field types, e.g., standard fields, spatial fields, full-text search fields,  
  and **vector-fields**, allowing queries to retrieve data from all these field types.
  This flexibility allows you to work with complex documents containing various data types and retrieve meaningful insights efficiently.

* Document attachments can also be indexed as vector fields, and Map-Reduce indexes can incorporate vector fields in their reduce phase, 
  further extending the versatility of your data processing and search capabilities.

##### Built-in embedding support:

* **Textual input**:  
  RavenDB uses the [bge-micro-v2](https://huggingface.co/TaylorAI/bge-micro-v2) model to embed **textual input** from your documents into 384-dimensional dense vectors.
  This highly efficient sentence-transformer model ensures precise and compact vector representations.

* **Numerical arrays input**:  
  Documents in RavenDB can contain numerical arrays with **pre-made embeddings** created elsewhere.  
  Use RavenDB's dedicated data type, `RavenVector`, to store these embeddings in your document entities.  
  This type is highly optimized to reduce storage space and enhance the speed of reading arrays from disk.

* **HNSW algorithm usage**:  
  All embeddings, whether generated from textual input or pre-made numerical arrays,  
  are indexed and searched for using the [HNSW](https://en.wikipedia.org/wiki/Hierarchical_navigable_small_world) algorithm.

* **Optimize storage via quantization**:  
  RavenDB allows you to select the quantization format for embeddings when creating the index.  
  Choose the format that best fits your needs:  
  * `Single` (32-bit floating point per dimension):  
     Provides precise vector representations.  
     Cosine similarity is used for searching and matching.  
  * `Int8` (8-bit integer per dimension):  
     Reduces storage requirements while maintaining good performance.  
     Cosine similarity is used for searching and matching.  
  * `Binary` (1-bit per dimension):  
     Minimizes storage usage, suitable for use cases where binary representation suffices.  
     Hamming distance is used for searching and matching.

{PANEL/}

## Related Articles

### Client API

- [RQL](../client-api/session/querying/what-is-rql) 
- [Query overview](../client-api/session/querying/how-to-query)

### Vector Search

- [Vector search using a dynamic query](../ai-integration/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../ai-integration/vector-search-using-static-index.markdown)

### Server

- [indexing configuration](../server/configuration/indexing-configuration)
