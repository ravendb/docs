# Documents Compression
---

{NOTE: }

* The **Documents Compression** feature uses the `Zstd` compression algorithm to 
  continuously create a more efficient data model with better compression ratios.  

* RavenDB will compress documents in selected collections or in all collections when storing those documents.  
  The compression will be applied to:  
  * Newly created documents  
  * Existing documents that are modified and saved  

* If opened just for reading, **existing documents will not be compressed** .  
  * If you also want to compress existing documents without editing them, 
    after configuring the collections to compress you can use the [CompactDatabaseOperation](../../client-api/operations/server-wide/compact-database).
    While removing empty gaps which occupy space in your database, this operation will also trigger compression on collections that were configured.  
     * Note: Compression and compaction are two different methods. Compression reduces the amount of storage that data uses, 
       while compaction removes empty gaps that still occupy space after deletes.  

* Compression is configured in the [Database Record](../../studio/database/settings/database-record).  

* In this page:  
  * [Overview](../../server/storage/documents-compression#overview)  
  * [Syntax](../../server/storage/documents-compression#syntax)  

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

The `Zstd` algorithm is trained by each compression operation and continuously improves 
its compression ratio.  

Reading and querying compressed large datasets is usually at least as fast as reading 
and querying their uncompressed versions because the compressed data is loaded much 
faster. Compression and decompression is fully transparent to the user.  

{PANEL/}

{PANEL: Syntax}

Documents compression is configured using the `DocumentsCompressionConfiguration` 
option in the `DatabaseRecord`.  

{CODE:csharp Syntax_0@Server/Storage/DocumentsCompression.cs /}  

In this example, we configure compression to be active on the collection `Orders` 
and on the revisions of all collections:  

{CODE:csharp Example_0@Server/Storage/DocumentsCompression.cs /}  



{PANEL/}

## Related Articles

### Client API

- [What are Operations](../../client-api/operations/what-are-operations)
- [To Compact a Database](../../client-api/operations/server-wide/compact-database)

### Server

- [Database Configuration - Compress Revisions Default](../../server/configuration/database-configuration#databases.compression.compressrevisionsdefault)

### Studio

- [Documents Compression](../../studio/database/settings/documents-compression)
- [Database Record](../../studio/database/settings/database-record)
