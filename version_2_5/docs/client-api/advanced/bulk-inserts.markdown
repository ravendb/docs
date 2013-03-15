# Bulk inserts

One of the features that is particularly useful when inserting large amount of data is `bulk inserting`. This is an optimized time-saving approach with few drawbacks that will be described later.

From the developer side, the API can be used as follows:

{CODE bulk_inserts_1@ClientApi\Advanced\BulkInserts.cs /}

The `BulkInsert` method is a part of `IDocumentStore` and is used to start a `BulkInsertOperation`.

{CODE bulk_inserts_2@ClientApi\Advanced\BulkInserts.cs /}

{CODE bulk_inserts_3@ClientApi\Advanced\BulkInserts.cs /}

The available `BulkInsertOptions` are:   

* **CheckForUpdates** - enables document updates (default: false)    
* **CheckReferencesInIndexes** - enables reference checking (default: false)     
* **BatchSize** - used batch size (default: 512)   

{CODE bulk_inserts_4@ClientApi\Advanced\BulkInserts.cs /}

The `BulkInsertOperation` consist of following the methods and events:

* **Store** - `method` used to store the entity, with optional `id` parameter to explicitly declare the entity identifier (will be generated automatically on client-side when overload without `id` will be used)       
* **Report** - `event` that will be raised every time a batch has finished processing and after the whole operation      
* **OnBeforeEntityInsert** - `event` that will be raised before entity will be processed    

##Limitations

There are several limitations to the API:

* Entity **Id** must be provided at the client side    
* Transactions are per batch, not per operation and DTC transactions are not supported   
* Documents inserted using bulk-insert will not raise notifications. More about `Changes API` can be found [here](../changes-api).
* Document Updates and Reference Checking must be explicitly turned on (see `BulkInsertOptions`)
* `AfterCommit` method in `Put Triggers` will be not executed in contrast to `AllowPut`, `AfterPut` and `OnPut`   
