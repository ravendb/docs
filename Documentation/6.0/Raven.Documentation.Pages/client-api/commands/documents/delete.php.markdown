# Commands: Delete Document

{NOTE: }

* Use `DeleteDocumentCommand` to remove a document from the database.

* In this page:

    * [Example](../../../client-api/commands/documents/delete#example)
    * [Syntax](../../../client-api/commands/documents/delete#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE:php delete_sample@ClientApi\Commands\Documents\Delete.php /}

{PANEL/}


{PANEL: Syntax}

{CODE:php delete_interface@ClientApi\Commands\Documents\Delete.php /}

| Parameters | Type | Description |
|------------|------|-------------|
| **idOrCopy** | `string` | ID of a document to be deleted |
| **changeVector** | `string` (optional) | Entity Change Vector, used for concurrency checks (`None` to skip check) |

{PANEL/}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
