# Send Multiple Commands in a Batch

To send **multiple commands** in a **single request**, reducing the number of remote calls and allowing several operations to share **same transaction**, `BatchCommand` should be used.

## Syntax

{CODE:java batch_1@ClientApi\Commands\Batches\SendMultipleCommands.java /}

### The following commands can be sent using a batch

* DeleteCommandData
* DeletePrefixedCommandData
* PutCommandData
* PatchCommandData
* DeleteAttachmentCommandData
* PutAttachmentCommandData

### Batch Options

{CODE:java batch_2@ClientApi\Commands\Batches\SendMultipleCommands.java /}


## Example

{CODE:java batch_3@ClientApi\Commands\Batches\SendMultipleCommands.java /}

{NOTE All the commands in the batch will succeed or fail as a **transaction**. Other users will not be able to see any of the changes until the entire batch completes./}

## Related articles

### Transactions

- [Transaction Support](../../../client-api/faq/transaction-support)

### Commands

- [Put document](../../../client-api/commands/documents/put)
- [Delete document](../../../client-api/commands/documents/delete)

### Patching

- [How to Perform Single Document Patch Operations](../../../client-api/operations/patching/single-document)   

### Attachments

- [What are Attachments](../../../document-extensions/attachments/what-are-attachments)
