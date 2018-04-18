# Batches : How to send multiple commands using a batch?

To send **multiple commands** in a **single request**, reducing the number of remote calls and allowing several operations to share **same transaction**, `BatchCommand` should be used.

## Syntax

{CODE:java batch_1@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.java /}

### The following commands can be sent using a batch


* DeleteCommandData
* DeletePrefixedCommandData
* PutCommandData

<!--
TODO

* DeleteAttachmentCommandData
* PutAttachmentCommandData
* PatchCommandData
-->

### Batch Options

{CODE:java batch_2@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.java /}


## Example

{CODE:java batch_3@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.java /}

{NOTE All the commands in the batch will succeed or fail as a **transaction**. Other users will not be able to see any of the changes until the entire batch completes./}

## Related articles

- [Put](../../../client-api/commands/documents/put)   
- [Delete](../../../client-api/commands/documents/delete)   
- [How to work with **patch requests**?](../../../client-api/operations/patch/single-doc-patch-operation)   
