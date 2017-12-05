# Bulk Insert : How to work with bulk insert operation?

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`. This is an optimized time-saving approach with few drawbacks that will be described later.

## Syntax

{CODE bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of database for which bulk operation should be performed. If `null` then `DefaultDatabase` from DocumentStore will be used. |

| Return Value | |
| ------------- | ----- |
| [BulkInsertOperation](../../glossary/bulk-insert-operation) | Instance of BulkInsertOperation used for interaction. |

## Limitations

There are several limitations to the API:

* The bulk insert operation is broken into batches, each batch is treated in its own transaction so the whole operation isn't treated under a single transaction.
* Bulk insert is not thread safe, a single bulk insert should not be accessed concurrently. The use of multiple bulk inserts, on the same client, concurrently is supported.

## Example

{CODE bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.cs /}

## Related articles

- [How to subscribe to bulk insert operation changes?](../changes/how-to-subscribe-to-bulk-insert-operation-changes)
