# Commands: Delete Document

{NOTE: }

* Use `DeleteDocumentCommand` to remove a document from the database.

* In this page:

    * [Example](../../../client-api/commands/documents/delete#example)
    * [Syntax](../../../client-api/commands/documents/delete#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE delete_sample@ClientApi\Commands\Documents\Delete.cs /}

{PANEL/}


{PANEL: Syntax}

{CODE delete_interface@ClientApi\Commands\Documents\Delete.cs /}

| Parameters | Type | Description |
|------------|------|-------------|
| **id**           | `string` | ID of a document to be deleted |
| **changeVector** | `string` | Entity Change Vector, used for concurrency checks (`null` to skip check) |

{PANEL/}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
