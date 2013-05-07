#### Batch Operations

RavenDB supports batching multiple operations into a single request, reducing the number of remote calls and allowing several operations to share the same transactions.

Request batching in RavenDB is handled by the Client API using the Batch() method of DatabaseCommands, which accepts an array of operations to execute.

Batching PUT and DELETEs:

{CODE batch_1@ClientApi\Advanced\DatabaseCommands\Batch.cs /}

Defer operation: 
In RavenDB when you call SaveChanges() all the changed document are batched into one call to the server and either they all pass or non will pass.  
With the defer command we can add our own commands to this call:

{CODE batch_2@ClientApi\Advanced\DatabaseCommands\Batch.cs /}

Another operation supported by batching is the [PATCH command](../../partial-document-updates).

##### Concurrency

If an etag is specified in the command, that etag is compared to the current etag on the document on the server. If the etags do no match, a 409 Conflict status code will be returned from the server, causing a ConcurrencyException to be thrown. In such a case, the entire operation fails and non of the updates that were tried will succeed.

##### Transactions

All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.