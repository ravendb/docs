# Client API : Batches: How to send multiple commands using a batch?

To send **multiple operations** in a **single request**, reducing the number of remote calls and allowing several operations to share **same transaction**, `Batch` should be used.

## Syntax

{CODE batch_1@ClientApi\Commands\Batches\Batch.cs /}

**Parameters**

An array of following commands:

- PutCommandData

{CODE putcommanddata@Common.cs /}

- DeleteCommandData

{CODE deletecommanddata@Common.cs /}

- PatchCommandData

{CODE patchcommanddata@Common.cs /}

- ScriptedPatchCommandData

{CODE scriptedpatchcommanddata@Common.cs /}

**Return Value**

An array of batch results matching **exactly** the order of commands send.

{CODE batchresult@Common.cs /}

## Example

{CODE batch_2@ClientApi\Commands\Batches\Batch.cs /}

## Concurrency

If an ETag is specified in the command, that ETag is compared to the current ETag on the document on the server. If the ETag does not match, a 409 Conflict status code will be returned from the server, causing a **ConcurrencyException** to be thrown. In such a case, the entire operation fails and non of the updates that were tried will succeed.

## Transactions

All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.

#### Related articles

- [Put](../../../client-api/commands/documents/put)   
- [Delete](../../../client-api/commands/documents/delete)   
- [How to work with **patch requests**?](../../../client-api/commands/patches/how-to-work-with-patch-requests)   
- [How to use **JavaScript** to **patch** your documents?](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents)  