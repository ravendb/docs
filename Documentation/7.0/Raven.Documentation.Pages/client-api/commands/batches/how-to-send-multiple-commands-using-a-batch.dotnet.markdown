# Send Multiple Commands in a Batch
---

{NOTE: }

* Use the low-level `SingleNodeBatchCommand` to send **multiple commands** in a **single request** to the server.  
  This reduces the number of remote calls and allows several operations to share the same transaction.

* All the commands sent in the batch are executed as a **single transaction** on the node the client communicated with.
  If any command fails, the entire batch is rolled back, ensuring data integrity.  

* The commands are replicated to other nodes in the cluster only AFTER the transaction is successfully completed on that node.

* In this page:
    * [Examples](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch#examples)
    * [Available batch commands](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch#available-batch-commands)
    * [Syntax](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch#syntax)

{NOTE/}

---

{PANEL: Examples}

{CONTENT-FRAME: }

#### Send multiple commands - using the Store's request executor:

---

{CODE-TABS}
{CODE-TAB:csharp:Sync batch_1@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE-TAB:csharp:Async batch_1_async@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Send multiple commands - using the Session's request executor:

---

* `SingleNodeBatchCommand` can also be executed using the session's request executor.

* Note that the transaction created for the HTTP request when executing `SingleNodeBatchCommand`
  is separate from the transaction initiated by the session's [SaveChanges](../../../client-api/session/saving-changes) method, even if both are called within the same code block.  
  Learn more about transactions in RavenDB in [Transaction support](../../../client-api/faq/transaction-support).

{CODE-TABS}
{CODE-TAB:csharp:Sync batch_2@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE-TAB:csharp:Async batch_2_async@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Available batch commands}

**The following commands can be sent in a batch via `SingleNodeBatchCommand`**:  
(These commands implement the `ICommandData` interface).
 
   * BatchPatchCommandData
   * CopyAttachmentCommandData
   * CountersBatchCommandData
   * DeleteAttachmentCommandData
   * DeleteCommandData
   * DeleteCompareExchangeCommandData
   * DeletePrefixedCommandData
   * ForceRevisionCommandData
   * IncrementalTimeSeriesBatchCommandData
   * JsonPatchCommandData
   * MoveAttachmentCommandData
   * PatchCommandData
   * PutAttachmentCommandData
   * PutCommandData
   * PutCompareExchangeCommandData
   * TimeSeriesBatchCommandData

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE syntax_2@ClientApi\Commands\Batches\SendMultipleCommands.cs /}
{CODE syntax_3@ClientApi\Commands\Batches\SendMultipleCommands.cs /}

{PANEL/}

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
