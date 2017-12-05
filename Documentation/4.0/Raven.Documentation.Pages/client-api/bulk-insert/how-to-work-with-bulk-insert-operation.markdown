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

* Entity **Id** must be provided by the client. The client by default will use the HiLo generator in order to generate the **Id**.
* Transactions are per batch, not per operation.
* Documents inserted using bulk-insert will not raise notifications. More about `Changes API` can be found [here](../changes/what-is-changes-api).
* Bulk insert is not thread safe, it should not be accessed concurrently.

## Example

{CODE bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.cs /}

## Related articles

- [How to subscribe to bulk insert operation changes?](../changes/how-to-subscribe-to-bulk-insert-operation-changes)
