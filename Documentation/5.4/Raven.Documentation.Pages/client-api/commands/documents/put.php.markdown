# Commands: Put Document
---

{NOTE: }

* Use `PutDocumentCommand` to insert a document to the database or update an existing document.

* In this page:

   * [Example](../../../client-api/commands/documents/put#example)
   * [Syntax](../../../client-api/commands/documents/put#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE:php put_sample@ClientApi\Commands\Documents\Put.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php put_interface@ClientApi\Commands\Documents\Put.php /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **idOrCopy** | `string` | Unique ID under which document will be stored |
| **changeVector** | `string` (optional) | Entity changeVector, used for concurrency checks (`None` to skip check) |
| **document** | `array` | The document to store |

{PANEL/}

## Related Articles

### Commands

- [Get](../../../client-api/commands/documents/get)
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
