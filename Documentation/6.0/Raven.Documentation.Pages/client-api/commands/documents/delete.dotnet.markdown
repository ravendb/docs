# Delete Document Command

{NOTE: }

* Use the low-level `DeleteDocumentCommand` to remove a document from the database.

* To delete a document using a higher-level method, see [deleting entities](../../../client-api/session/deleting-entities).  

* In this page:

    * [Examples](../../../client-api/commands/documents/delete#examples)
    * [Syntax](../../../client-api/commands/documents/delete#syntax)

{NOTE/}

---

{PANEL: Examples}

###### Delete a document:

{CODE-TABS}
{CODE-TAB:csharp:Delete_document delete_document_1@ClientApi\Commands\Documents\Delete.cs /}
{CODE-TAB:csharp:Delete_document_async delete_document_1_async@ClientApi\Commands\Documents\Delete.cs /}
{CODE-TABS/}

###### Delete a document with concurrency check:

{CODE-TABS}
{CODE-TAB:csharp:Delete_document delete_document_2@ClientApi\Commands\Documents\Delete.cs /}
{CODE-TAB:csharp:Delete_document_async delete_document_2_async@ClientApi\Commands\Documents\Delete.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Commands\Documents\Delete.cs /}

| Parameter        | Type     | Description                                                                                                                                                                                                                                             |
|------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**           | `string` | The ID of the document to delete.                                                                                                                                                                                                                       |
| **changeVector** | `string` | The change-vector of the document you wish to delete,<br>used for [optimistic concurrency control](../../../server/clustering/replication/change-vector#concurrency-control-&-change-vectors).<br>Pass `null` to skip the check and force the deletion. |

{PANEL/}

## Related Articles

### Commands 

- [Get document](../../../client-api/commands/documents/get)
- [Put document](../../../client-api/commands/documents/put)  
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
