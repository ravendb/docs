# Documents Compression
---

{NOTE: }

* The **Documents Compression** feature employs the [Zstd compression algorithm](https://github.com/facebook/zstd)  
  to achieve more efficient data storage with constantly improving compression ratios.  

* Documents compression can be set for all collections, selected collections, and revisions.  
  Default compression settings are [configurable](../../server/configuration/database-configuration#databases.compression.compressallcollectionsdefault).  

* When turned on, compression will be applied to:  
  * **New documents**:  
      * A new document that is saved will be compressed.  
  * **Existing documents**:  
      * Existing documents that are modified and saved will be compressed.  
      * Existing documents that are Not modified will only be compressed when executing the    
        [compact database operation](../../client-api/operations/server-wide/compact-database#compaction-triggers-compression).  

* Compression can be set from the [Studio](../../studio/database/settings/documents-compression),  
  or by updating the database record from the Client API, see below.

* In this page:  
  * [Overview](../../server/storage/documents-compression#overview)
  * [Compression -vs- Compaction](../../server/storage/documents-compression#compression--vs--compaction)
  * [Set compression for all collections](../../server/storage/documents-compression#set-compression-for-all-collections)  
  * [Set compression for selected collections](../../server/storage/documents-compression#set-compression-for-selected-collections)
  * [Syntax](../../server/storage/documents-compression#syntax)

{NOTE/}

---

{PANEL: Overview}

* As a document database, RavenDB's schema-less nature presents many advantages,  
  however, it requires us to manage the data structure on a per-document basis.  
  In extreme cases, the majority of the data you store is the documents' structure.

* The [Zstd compression algorithm](https://github.com/facebook/zstd) is used to learn your data model, identify common patterns,  
  and create dictionaries that represent the redundant structural data across documents in a collection.  

* The algorithm is trained by each compression operation and continuously improves its compression ratio  
  to maintain the most efficient compression model.  
  In many datasets, this can reduce the storage space by more than 50%.

* Compression and decompression are fully transparent to the user.  
  Reading and querying compressed large datasets is usually as fast as reading and querying  
  their uncompressed versions because the compressed data is loaded much faster.  

* Compression is Not applied to attachments, counters, and time series data,  
  only to the content of documents and revisions.  

* Detailed information about the database's physical storage is visible in the [Storage Report view](../../studio/database/stats/storage-report).

{PANEL/}

{PANEL: Compression -vs- Compaction}

* The following table summarizes the differences between Compression and Compaction:

| **Compression** | |
| - | - |
| Action: | Reduce storage space using the `Zstd` compression algorithm |
| Items that can be compressed: | **-** Documents in collections that are configured for compression<br>**-** Revisions for all collections |
| Triggered by: | The server |
| Triggered when: | Compression feature is configured,<br> **and** when either of the following occurs for the configured collections:<br>&nbsp;&nbsp;&nbsp;**-** Storing new documents<br>&nbsp;&nbsp;&nbsp;**-** Modifying & saving existing documents<br>&nbsp;&nbsp;&nbsp;**-** Compact operation is triggered, existing documents will be compressed |

| **Compaction** | |
| - | - |
| Action: | Remove empty gaps on disk that still occupy space after deletes |
| Items that can be compacted: | Documents and/or indexes on the specified database |
| Triggered by: | Client API code |
| Triggered when: | Explicitly calling [compact database operation](../../client-api/operations/server-wide/compact-database) |

{PANEL/}

{PANEL: Set compression for all collections}

{CODE-TABS}
{CODE-TAB:csharp:Sync compress_all@Server/Storage/DocumentsCompression.cs /}  
{CODE-TAB:csharp:Async compress_all_async@Server/Storage/DocumentsCompression.cs /}  
{CODE-TABS/}

{PANEL/}

{PANEL: Set compression for selected collections}

{CODE-TABS}
{CODE-TAB:csharp:Sync compress_specific@Server/Storage/DocumentsCompression.cs /}  
{CODE-TAB:csharp:Async compress_specific_async@Server/Storage/DocumentsCompression.cs /}  
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

* Documents compression is configured using the `DocumentsCompressionConfiguration` class in the database record.  

{CODE:csharp syntax@Server/Storage/DocumentsCompression.cs /}  

{PANEL/}

## Related Articles

### Client API

- [What are Operations](../../client-api/operations/what-are-operations)
- [To Compact a Database](../../client-api/operations/server-wide/compact-database)

### Server

- [Database Configuration - Compress Collections Default](../../server/configuration/database-configuration#databases.compression.compressallcollectionsdefault)
- [Database Configuration - Compress Revisions Default](../../server/configuration/database-configuration#databases.compression.compressrevisionsdefault)

### Studio

- [Documents Compression](../../studio/database/settings/documents-compression)
- [Database Record](../../studio/database/settings/database-record)
