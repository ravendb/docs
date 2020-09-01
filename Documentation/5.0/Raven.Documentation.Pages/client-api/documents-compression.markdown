# Documents Compression
---

{NOTE: }

* The **Documents Compression** feature uses the Zstd compression algorithm to 
continuously create a more efficient data model with better compression ratios.  

* It is configured in the [Database Record](../studio/database/settings/database-record).  

* In this page:  
  * [Overview](../client-api/documents-compression#overview)  
  * [Syntax](../client-api/documents-compression#syntax)  

{NOTE/}

---

{PANEL: Overview}

Despite the many benefits of the schema-less nature of document databases, one drawback 
is that it requires us to manage the structure of our data on a per-document basis. In 
extreme cases, the majority of the data you store is the documents' structure.  

**Documents Compression** uses the top of the line [Zstd compression algorithm](https://github.com/facebook/zstd) 
to learn your data model and create dictionaries that represent the redundant structural 
data across documents. Compression is applied at the collection rather than the document 
level, to eliminate these cross-document duplications. RavenDB continuously inspects your 
documents as they change to retrain the algorithm and maintain the most efficient 
compression model. In many datasets, this can reduce the storage space by more than 50%.  

The Zstd algorithm is trained by each compression operation and continuously improves 
its compression ratio.  

Reading and querying compressed large datasets is usually at least as fast as reading 
and querying their uncompressed versions because the compressed data is loaded much 
faster. Compression and decompression is fully transparent to the user.  

{PANEL/}

{PANEL: Syntax}

Documents compression is configured using the `DocumentsCompressionConfiguration` 
option in the `DatabaseRecord`.  

{CODE:csharp Syntax_0@ClientApi/DocumentsCompression.cs /}  

In this example, we configure compression to be active on the collection `Orders` 
and on the revisions of all collections:  

{CODE:csharp Example_0@ClientApi/DocumentsCompression.cs /}  

{PANEL/}
