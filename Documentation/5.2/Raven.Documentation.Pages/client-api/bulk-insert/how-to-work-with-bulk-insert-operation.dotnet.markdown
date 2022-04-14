# Bulk Insert: How to Work With Bulk Insert Operation

{NOTE: }

* `BulkInsert` is useful when inserting a large quantity of data from the client to the server.  
* It is an optimized time-saving approach with a few [limitations](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#limitations)
  such as transactionality and the minor possibility of interruptions during the operation.

In this page:

* [Syntax](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#syntax)
* [BulkInsertOperation](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoperation) 
  * [Methods](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#methods)
  * [Limitations](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#limitations)
  * [Example](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#example)
* [BulkInsertOptions](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions)
  * [CompressionLevel](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#compressionlevel)
  * [SkipOverwriteIfUnchanged](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#skipoverwriteifunchanged)

{NOTE/}

{PANEL: Syntax}

{CODE bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of database for which bulk operation should be performed. If `null` then the `Database` from DocumentStore will be used. |
| **token** | CancellationToken | Cancellation token used in order to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| BulkInsertOperation| Instance of BulkInsertOperation used for interaction. |

---

{CODE bulk_inserts_2@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **database** | string | Name of database for which bulk operation should be performed. If `null` then the `Database` from DocumentStore will be used. |
| **options** | BulkInsertOptions | [Options](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) to configure BulkInsert. |
| **token** | CancellationToken | Cancellation token used in order to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| BulkInsertOperation| Instance of BulkInsertOperation used for interaction. |

---

{CODE bulk_inserts_3@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **options** | BulkInsertOptions | [Options](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) to configure BulkInsert. |
| **token** | CancellationToken | Cancellation token used in order to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| BulkInsertOperation| Instance of BulkInsertOperation used for interaction. |

{PANEL/}

{PANEL: BulkInsertOperation}

The following methods can be used when creating a bulk insert.

### Methods

| Signature | Description |
| ----------| ----- |
| **void Abort()** | Abort the operation |
| **void Store(object entity, IMetadataDictionary metadata = null)** | Store the entity, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. |
| **void Store(object entity, string id, IMetadataDictionary metadata = null)** | Store the entity, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity.|
| **void StoreAsync(object entity, IMetadataDictionary metadata = null)** | Store the entity in an async manner, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. |
| **void StoreAsync(object entity, string id, IMetadataDictionary metadata = null)** | Store the entity in an async manner, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity.|
| **void Dispose()** | Dispose an object |
| **void DisposeAsync()** | Dispose an object in an async manner |

### Limitations

* BulkInsert is designed to efficiently push high quantities of data.  
  As such, data is streamed and **processed by the server in batches**.  
  Each batch is fully transactional, but there are no transaction guarantees between the batches. The operation as a whole is non-transactional. 
  If your bulk insert is interrupted mid-way, some of your data might be persisted on the server while some of it might not.  
  * Make sure that your logic accounts for the possibility of an interruption where some of your data has not yet persisted on the server.
  * If the operation was interrupted and you choose to re-insert the whole dataset in a new operation, 
    you can configure [SkipOverwriteIfUnchanged](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#skipoverwriteifunchanged) as `true`.  
    It only overwrites existing documents if a change has been made since the last insertion.
  * **If you need full transactionality**, the [session](../../client-api/session/what-is-a-session-and-how-does-it-work) may be a better option.  
    If using the session, because all of the data is processed in one transaction, your machine's RAM must be able to handle the 
    entire data-set included in the transaction.  
* Bulk insert is **not thread-safe**, meaning that it isn't transactional.  
  A single bulk insert should not be accessed concurrently.  
  * The use of multiple bulk inserts concurrently on the same client is supported.  
  * Also the use in an async context is supported.

### Example

#### Create bulk insert

Here we create a bulk insert operation and insert a million documents of type Employee:
{CODE-TABS}
{CODE-TAB:csharp:sync bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.cs /}
{CODE-TAB:csharp:async bulk_inserts_5@ClientApi\BulkInsert\BulkInserts.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: BulkInsertOptions}

The following options can be configured for BulkInsert.

### CompressionLevel

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **Optimal** | string | Compression level to be used when compressing static files. |
| **Fastest** | string | Compression level to be used when compressing HTTP responses with GZip or Deflate. |
| **NoCompression** | string | Does not compress. |


### SkipOverwriteIfUnchanged

Prevent overriding documents if there are no changes when compared to the already existing ones.  

Enabling this can avoid a lot of additional work including triggering re-indexation, subscriptions, and ETL processes.  
It introduces slight overlay into bulk insert process because of the need to compare the existing documents with the ones that are being inserted. 

{CODE bulk_insert_option_SkipOverwriteIfUnchanged@ClientApi\BulkInsert\BulkInserts.cs /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work)

### Document Extensions

- [Bulk Insert: How to Add Attachments](../../document-extensions/attachments/bulk-insert)
- [Bulk Insert: How to Append Time Series](../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk)

