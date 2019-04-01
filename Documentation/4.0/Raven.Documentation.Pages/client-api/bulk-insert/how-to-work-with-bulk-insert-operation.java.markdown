# Bulk Insert: How to Work With Bulk Insert Operation

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`. This is an optimized time-saving approach with few drawbacks that will be described later.

## Syntax

{CODE:java bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | String | Name of database for which bulk operation should be performed. If `null` then the `Database` from DocumentStore will be used. |

| Return Value | |
| ------------- | ----- |
| BulkInsertOperation| Instance of BulkInsertOperation used for interaction. |

# BulkInsertOperation

### Methods

| Signature | Description |
| ----------| ----- |
| **void abort()** | Abort the operation |
| **void store(Object entity, IMetadataDictionary metadata = null)** | store the entity, identifier will be generated automatically on client-side. Optional, metadata can be provided for the stored entity. |
| **void store(Object entity, String id, IMetadataDictionary metadata = null)** | store the entity, with `id` parameter to explicitly declare the entity identifier. Optional, metadata can be provided for the stored entity.|
| **void close()** | Close an object |

## Limitations

There are a couple limitations to the API:

* The bulk insert operation is broken into batches, each batch is treated in its own transaction so the whole operation isn't treated under a single transaction.
* Bulk insert is not thread safe, a single bulk insert should not be accessed concurrently. The use of multiple bulk inserts, on the same client, concurrently is supported also the use in an async context is supported.

## Example

### Create bulk insert

Here we create a bulk insert operation and inserting a million documents of type Employee

{CODE:java bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.java /}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work)
