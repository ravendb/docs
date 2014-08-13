# Client API : How to work with bulk insert operation?

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`. This is an optimized time-saving approach with few drawbacks that will be described later.

## Syntax

{CODE bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.cs /}

**Parameters**   

database
:   Type: string   
Name of database for which bulk operation should be performed. If `null` then `DefaultDatabase` from DocumentStore will be used.

options
:   Type: [BulkInsertOptions](../../glossary/client-api/bulk-insert/bulk-insert-options)   
Bulk operations options that should be used.

**Return Value**

Type: [BulkInsertOperation](../../glossary/client-api/bulk-insert/bulk-insert-operation)     
Instance of BulkInsertOperation used for interaction.

## Limitations

There are several limitations to the API:

* Entity **Id** must be provided at the client side. The client by default will use the HiLo generator in order to generate the **Id**.
* Transactions are per batch, not per operation and DTC transactions are not supported.
* Documents inserted using bulk-insert will not raise notifications. More about `Changes API` can be found [here](../changes-api).
* Document Updates and Reference Checking must be explicitly turned on (see `BulkInsertOptions`).
* `AfterCommit` method in `Put Triggers` will be not executed in contrast to `AllowPut`, `AfterPut` and `OnPut`.

## Example

{CODE bulk_inserts_4@ClientApi\BulkInsert\BulkInserts.cs /}

#### Related articles

TODO
