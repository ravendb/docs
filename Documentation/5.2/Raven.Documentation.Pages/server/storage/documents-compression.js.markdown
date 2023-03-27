# Documents Compression
---

{NOTE: }

* The __Documents Compression__ feature employs the [Zstd compression algorithm](https://github.com/facebook/zstd)  
  to achieve more efficient data storage with constantly improving compression ratios.

* Documents compression can be set for all collections or for selected collections.  
  Revisions compression is turned on for all collections by default.

* When turned on, compression will be applied to:  
  * __New documents__:  
      * A new document that is saved will be compressed.  
  * __Existing documents__:  
      * Existing documents that are modified and saved will be compressed.  
      * Existing documents that are Not modified are compressed when executing the    
        [compact database operation](../../client-api/operations/server-wide/compact-database#compaction-triggers-compression).  

* Compression can be set from the [Studio](../../studio/database/settings/documents-compression#database-storage-report),  
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

{PANEL/}

{PANEL: Compression -vs- Compaction}

* The following table summarizes the differences between Compression and Compaction:

| __Compression__ | |
| - | - |
| Action: | Reduce storage space using the Zstd compression algorithm |
| Items that can be compressed: | __-__ Documents in collections that are configured for compression<br>__-__ Revisions for all collections |
| Triggered by: | The server |
| Triggered when: | Compression feature is configured,<br> __and__ when either of the following occurs for the configured collections:<br>&nbsp;&nbsp;&nbsp;__-__ Storing new documents<br>&nbsp;&nbsp;&nbsp;__-__ Modifying & saving existing documents<br>&nbsp;&nbsp;&nbsp;__-__ Compact operation is triggered, existing documents will be compressed |

| __Compaction__ | |
| - | - |
| Action: | Remove empty gaps on disk that still occupy space after deletes |
| Items that can be compacted: | Documents and/or indexes on the specified database |
| Triggered by: | Client API code |
| Triggered when: | Explicitly calling [compact database operation](../../client-api/operations/server-wide/compact-database) |

{PANEL/}

{PANEL: Set compression for all collections}

{CODE:nodejs compress_all@Server/Storage/documentsCompression.js /}

{PANEL/}

{PANEL: Set compression for selected collections}

{CODE:nodejs compress_specific@Server/Storage/documentsCompression.js /}

{PANEL/}

{PANEL: Syntax}

* Documents compression is configured using the following object in the database record:  

{CODE:nodejs syntax@Server/Storage/documentsCompression.js /}  

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
