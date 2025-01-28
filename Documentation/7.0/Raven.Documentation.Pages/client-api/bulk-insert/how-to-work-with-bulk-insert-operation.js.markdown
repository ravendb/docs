# Bulk Insert: How to Work With Bulk Insert Operation

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`.  
This is an optimized time-saving approach with few drawbacks that will be described later.

## Syntax

{CODE:nodejs bulk_inserts_1@client-api\BulkInsert\bulkInserts.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | `string` | The name of the database to perform the bulk operation on.<br>If `null`, the DocumentStore `Database` will be used. |

| Return Value | |
| ------------- | ----- |
| `BulkInsertOperation` | Instance of `BulkInsertOperation` used for interaction. |

# `BulkInsertOperation`

### Methods

| Signature | Description |
| ----------| ----- |
| **async abort()** | Aborts the bulk insert operation. Returns a `Promise`. |
| **async store(entity, [metadata])** | store the entity, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. Returns a `Promise`. |
| **async store(entity, id, [metadata])** | store the entity, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity. Returns a `Promise`. |
| **async finish()** | Finish bulk insert and flush everything to the server. Returns a `Promise`. |

## Limitations

There are a couple limitations to the API:

* The bulk insert operation is broken into batches, each batch is treated in its own transaction 
  so the whole operation isn't treated under a single transaction.

## Example

### Create bulk insert

Here we create a bulk insert operation and insert a million documents of type Employee

{CODE:nodejs bulk_inserts_4@client-api\BulkInsert\bulkInserts.js /}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work)
