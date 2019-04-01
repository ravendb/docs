# Batches: How to Send Multiple Commands Using a Batch

To send **multiple commands** in a **single request**, reducing the number of remote calls and allowing several operations to share **same transaction**, `BatchCommand` should be used.

## Syntax

{CODE batch_1@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.cs /}

### The following commands can be sent using a batch

* DeleteAttachmentCommandData
* DeleteCommandData
* DeletePrefixedCommandData
* PatchCommandData
* PutAttachmentCommandData
* PutCommandData

### Batch Options

{CODE batch_2@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.cs /}


## Example

{PANEL}
{CODE-TABS}
{CODE-TAB:csharp:Sync batch_3@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.cs /}
{CODE-TAB:csharp:Async batch_3_async@ClientApi\Commands\Batches\HowToSendMultipleCommandsUsingBatch.cs/}
{CODE-TABS/}
{PANEL/}

{NOTE All the commands in the batch will succeed or fail as a **transaction**. Other users will not be able to see any of the changes until the entire batch completes./}

## Related articles

### Transactions

- [Transaction Support](../../../client-api/faq/transaction-support)

### Commands

- [Put](../../../client-api/commands/documents/put)   
- [Delete](../../../client-api/commands/documents/delete)
- [How to Get Document Metadata Only](../../../client-api/commands/documents/how-to/get-document-metadata-only)

### Patching

- [How to Perform Single Document Patch Operations](../../../client-api/operations/patching/single-document)   

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
