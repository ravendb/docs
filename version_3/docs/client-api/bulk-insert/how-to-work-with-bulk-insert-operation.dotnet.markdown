# Client API : How to work with bulk insert operation?

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`. This is an optimized time-saving approach with few drawbacks that will be described later.

## Syntax

{CODE bulk_inserts_1@ClientApi\BulkInsert\BulkInserts.cs /}

**Parameters**   

database
:   Type: string   
Name of database for which bulk operation should be performed. If `null` then `DefaultDatabase` from DocumentStore will be used.

options
:   Type: [BulkInsertOptions]()   
Bulk operations options that should be used.

**Return Value**

Type: [BulkInsertOperation]()   
Instance of BulkInsertOperation used for interaction.

## Options

{CODE bulk_inserts_2@ClientApi\BulkInsert\BulkInserts.cs /}

OverwriteExisting
:   Type: bool   
Indicates if existing documents should be overwritten. If not, exception will be thrown. Default: `false`.

CheckReferencesInIndexes
:   Type: bool   
Enables reference checking. Default: `false`.

BatchSize
:   Type: bool   
Used batch size. Default: `512`.

WriteTimeoutMilliseconds
:   Type: int   
Maximum 'quiet period' in milliseconds. If there will be no writes during that period operation will end with `TimeoutException`. Default: `15000`.

## Operation

The `BulkInsertOperation` consist of following the methods, events and properties:

{CODE bulk_inserts_3@ClientApi\BulkInsert\BulkInserts.cs /}

OnBeforeEntityInsert
:   Type: event   
`event` that will be raised before entity will be processed.

IsAborted
:   Type: property   
`property` indicates if operation has aborted.

Abort
:   Type: method   
`method` used to abort the operation.

OperationId
:   Type: property   
Unique operation Id.

Report
:   Type: event   
`event` that will be raised every time a batch has finished processing and after the whole operation.

Store
:   Type: method   
`method` used to store the entity, with optional `id` parameter to explicitly declare the entity identifier (will be generated automatically on client-side when overload without `id` will be used).

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
