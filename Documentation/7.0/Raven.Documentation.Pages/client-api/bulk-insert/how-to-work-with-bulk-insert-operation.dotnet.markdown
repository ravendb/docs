# Bulk Insert: How to Work With Bulk Insert Operation

{NOTE: }

* `BulkInsert` is useful when inserting a large quantity of data from the client to the server.  
* It is an optimized time-saving approach with a few 
  [limitations](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#limitations) 
  like the possibility that interruptions will occure during the operation.  

In this page:

* [Syntax](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#syntax)
* [`BulkInsertOperation`](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoperation) 
  * [Methods](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#methods)
  * [Limitations](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#limitations)
  * [Example](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#example)
* [`BulkInsertOptions`](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions)
  * [`CompressionLevel`](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#section)
  * [`SkipOverwriteIfUnchanged`](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#section-1)

{NOTE/}

{PANEL: Syntax}

{CODE bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | `string` | The name of the database to perform the bulk operation on.<br>If `null`, the DocumentStore `Database` will be used. |
| **token** | `CancellationToken` | Cancellation token used to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| `BulkInsertOperation`| Instance of `BulkInsertOperation` used for interaction. |

---

{CODE bulk_inserts_2@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **database** | `string` | The name of the database to perform the bulk operation on.<br>If `null`, the DocumentStore `Database` will be used. |
| **options** | `BulkInsertOptions` | [Options](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) to configure BulkInsert. |
| **token** | `CancellationToken` | Cancellation token used to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| `BulkInsertOperation`| Instance of `BulkInsertOperation` used for interaction. |

---

{CODE bulk_inserts_3@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **options** | `BulkInsertOptions` | [Options](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) to configure BulkInsert. |
| **token** | `CancellationToken` | Cancellation token used to halt the worker operation. |

| Return Value | |
| ------------- | ----- |
| `BulkInsertOperation`| Instance of `BulkInsertOperation` used for interaction. |

{PANEL/}

{PANEL: `BulkInsertOperation`}

The following methods can be used when creating a bulk insert.

### Methods

| Signature | Description |
| ----------| ----- |
| **void Abort()** | Abort the operation |
| **void Store(object entity, IMetadataDictionary metadata = null)** | Store the entity, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. |
| **void Store(object entity, string id, IMetadataDictionary metadata = null)** | Store the entity, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity.|
| **void StoreAsync(object entity, IMetadataDictionary metadata = null)** | Store the entity in an async manner, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. |
| **void StoreAsync(object entity, string id, IMetadataDictionary metadata = null)** | Store the entity in an async manner, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity.|
| **void Dispose()** | Dispose of an object |
| **void DisposeAsync()** | Dispose of an object in an async manner |

### Limitations

* BulkInsert is designed to efficiently push high quantities of data.  
  Data is therefore streamed and **processed by the server in batches**.  
  Each batch is fully transactional, but there are no transaction guarantees between the batches 
  and the operation as a whole is non-transactional.  
  If the bulk insert operation is interrupted mid-way, some of your data might be persisted 
  on the server while some of it might not.  
   * Make sure that your logic accounts for the possibility of an interruption that would cause 
     some of your data not to persist on the server yet.  
   * If the operation was interrupted and you choose to re-insert the whole dataset in a new 
     operation, you can set 
     [SkipOverwriteIfUnchanged](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#section-1) 
     as `true` so the operation will overwrite existing documents only if they changed since 
     the last insertion.  
   * **If you need full transactionality**, using [session](../../client-api/session/what-is-a-session-and-how-does-it-work) 
     may be a better option.  
     Note that if `session` is used all of the data is processed in a single transaction, so the 
     server must have sufficient resources to handle the entire data-set included in the transaction.  
* Bulk insert is **not thread-safe**.  
  A single bulk insert should not be accessed concurrently.  
   * The use of multiple bulk inserts concurrently on the same client is supported.  
   * Usage in an async context is also supported.

### Example

#### Create bulk insert

Here we create a bulk insert operation and insert a million documents of type `Employee`:
{CODE-TABS}
{CODE-TAB:csharp:sync bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.cs /}
{CODE-TAB:csharp:async bulk_inserts_5@ClientApi\BulkInsert\BulkInserts.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: `BulkInsertOptions`}

The following options can be configured for BulkInsert.

#### `CompressionLevel`:

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **Optimal** | `string` | Compression level to be used when compressing static files. |
| **Fastest**<br>(Default)| `string` | Compression level to be used when compressing HTTP responses with `GZip` or `Deflate`. |
| **NoCompression** | `string` | Does not compress. |

{INFO: Default compression level}
For RavenDB versions up to `6.2`, bulk-insert compression is Disabled (`NoCompression`) by default.  
For RavenDB versions from `7.0` on, bulk-insert compression is Enabled (set to `Fastest`) by default.  
{INFO/}

#### `SkipOverwriteIfUnchanged`:

Prevents overriding documents when the inserted document and the existing one are similar.  

Enabling this flag can exempt the server of many operations triggered by document-change, 
like re-indexation and subscription or ETL-tasks updatess.  
There is a slight potential cost in the additional comparison that has to be made between 
the existing documents and the ones that are being inserted. 

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

