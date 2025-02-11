# Indexing Attachments for Vector Search
---

{NOTE: }

* This article explains how to index attachments using a **static-index** to enable vector search on their content.  
  Note: Vector search on attachment content is not available when making a [dynamic query](../ai-integration/vector-search-using-dynamic-query).  
  
* **Prior to this article**, refer to the [Vector search using a static index](../ai-integration/vector-search-using-static-index) article for general knowledge about  
  indexing a vector field.

* In this page:
    * [Overview](../ai-integration/indexing-attachments-for-vector-search#overview)
    * [Indexing TEXT attachments](../ai-integration/indexing-attachments-for-vector-search#indexing-text-attachments)
    * [Indexing NUMERICAL attachments](../ai-integration/indexing-attachments-for-vector-search#indexing-numerical-attachments)
      * [LINQ index](../ai-integration/indexing-attachments-for-vector-search#linq-index)
      * [JS index](../ai-integration/indexing-attachments-for-vector-search#js-index)
    * [Indexing ALL attachments](../ai-integration/indexing-attachments-for-vector-search#indexing-all-attachments)

{NOTE/}

---

{PANEL: Overview}

{CONTENT-FRAME: }

#### Attachments in RavenDB

---

* Attachments in RavenDB allow you to associate binary files with your JSON documents.  
  You can use attachments to store images, PDFs, videos, text files, or any other format.  

* Attachments are stored separately from documents, reducing document size and avoiding unnecessary duplication.
  They are stored as **binary data**, regardless of content type.

* Attachments are handled as streams, allowing efficient upload and retrieval.  
  Learn more in: [What are attachments](../document-extensions/attachments/what-are-attachments).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Indexing attachment content for vector search

---

You can index attachment content in a vector field within a static-index,    
enabling vector search on text or numerical data that is stored in the attachments:

* **Attachments with TEXT**:  
    * During indexing, RavenDB processes the text into a single embedding per attachment using the built-in  
      [bge-micro-v2](https://huggingface.co/TaylorAI/bge-micro-v2) model.

* **Attachments with NUMERICAL data**:  
    * While attachments can store any file type, RavenDB does Not generate embeddings from images, videos, or other non-textual content.  
      Each attachment must contain a **single** precomputed embedding vector, generated externally.  
    * RavenDB indexes the embedding vector from the attachment in and can apply [quantization](../ai-integration/vector-search-using-dynamic-query#quantization-options)
      (e.g., index it in _Int8_ format) if this is configured.  
    * All embeddings indexed within the same vector-field in the static-index must be vectors of the **same dimension** to ensure consistency in indexing and search.
      They must also be created using the **same model**.

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Indexing TEXT attachments}
 
* The following index defines a **vector field** named `VectorFromAttachment`.

* It indexes embeddings generated from the text content of the `description.txt` attachment.  
  This applies to all _Company_ documents that contain an attachment with that name.

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_1@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:JS_index index_2@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:IndexDefinition index_3@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Storing_text_attachments store_attachments_1@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TABS/}

Execute a vector search using the index:  
Results will include _Company_ documents whose attachment contains text similar to `"chinese food"`.

{CODE-TABS}
{CODE-TAB:csharp:Query query_1@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Query_async query_1_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_2@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery_async query_2_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery query_3@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery_async query_3_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByVector/FromTextAttachment"
where vector.search(VectorFromAttachment, $searchTerm, 0.8)
{ "searchTerm" : "chinese food" }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing NUMERICAL attachments}

### LINQ index

* The following index defines a **vector field** named `VectorFromAttachment`.  

* It indexes embeddings generated from the numerical data stored in the `vector.raw` attachment.  
  This applies to all _Company_ documents that contain an attachment with that name.

* Each attachment contains raw numerical data in 32-bit floating-point format.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_4@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:IndexDefinition index_5@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Storing_numerical_attachments store_attachments_2@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TABS/}

Execute a vector search using the index:  
Results will include _Company_ documents whose attachment contains vectors similar to the query vector.

{CODE-TABS}
{CODE-TAB:csharp:Query query_4@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Query_async query_4_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_5@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery_async query_5_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery query_6@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery_async query_6_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByVector/FromNumericalAttachment"
where vector.search(VectorFromAttachment, $queryVector)
{ "queryVector" : [0.1, 0.2, 0.3, 0.4] }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

### JS index

* The following is the JavaScript index format equivalent to the [LINQ index](../ai-integration/indexing-attachments-for-vector-search#linq-index) shown above.

* The main difference is that JavaScript indexes do Not support `getContentAsStream()` on attachment objects:
  * Because of this, embedding vectors must be stored in attachments as **Base64-encoded strings**.  
  * Use `getContentAsString()` to retrieve the attachment content as a string, as shown in this example.

{CODE-TABS}
{CODE-TAB:csharp:JS_index index_6@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Storing_numerical_attachments_as_base64 store_attachments_3@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TABS/}

Execute a vector search using the index:  
Results will include _Company_ documents whose attachment contains vectors similar to the query vector.

{CODE-TABS}
{CODE-TAB:csharp:RawQuery query_7@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery_async query_7_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByVector/FromNumericalAttachment/JS"
where vector.search(VectorFromAttachment, $queryVector)
{ "queryVector" : [0.1, 0.2, 0.3, 0.4] }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing ALL attachments}

* The following index defines a vector field named `VectorFromAttachment`.

* It indexes embeddings generated from the numerical data stored in ALL attachments of all _Company_ documents.

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_7@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:IndexDefinition index_8@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Storing_numerical_attachments store_attachments_4@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TABS/}

Execute a vector search using the index:  
Results will include Company documents whose attachments contains vectors similar to the query vector.

{CODE-TABS}
{CODE-TAB:csharp:Query query_8@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:Query_async query_8_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_9@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery_async query_9_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery query_10@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB:csharp:RawQuery_async query_10_async@AiIntegration\VectorSearchWithAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Companies/ByVector/AllAttachments"
where vector.search(VectorFromAttachment, $queryVector)
{ "queryVector" : [0.1, 0.2, -0.7, -0.8] }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../ai-integration/ravendb-as-vector-database)
- [Vector search using a dynamic query](../ai-integration/vector-search-using-dynamic-query)
- [Vector search using a static index](../ai-integration/vector-search-using-static-index)

### Querying

- [Query overview](../client-api/session/querying/how-to-query)
- [Full-text search](../client-api/session/querying/text-search/full-text-search)
