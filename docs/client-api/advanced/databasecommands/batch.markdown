# Batch Operations

RavenDB supports batching multiple operations into a single request, reducing the number of remote calls and allowing several operations to share the same transactions.

Request batching in RavenDB is handled by the Client API using the Batch() method of DatabaseCommands, which accepts an array of operations to execute.

Batching PUT and DELETEs:

// TODO: example for using DeleteCommandData, PutCommandData

Another operation supported by batching is the [PATCH command](../../../partial-document-updates).

## Concurrency

If an etag is specified in the command, that etag is compared to the current etag on the document on the server. If the etags do no match, a 409 Conlict status code will be returned from the server, causing a ConcurrencyException to be thrown. In such a case, the entire operation fails and non of the updates that were tried will succeed.

## Transactions

All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.