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

{CODE put_sample@ClientApi\Commands\Documents\Put.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE put_interface@ClientApi\Commands\Documents\Put.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | Unique ID under which document will be stored |
| **changeVector** | `string` | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **document** | `BlittableJsonReaderObject` | The document to store. You may use `session.Advanced.JsonConverter.ToBlittable(doc, docInfo);` to convert your entity to a `BlittableJsonReaderObject`. |

{PANEL/}

## Related Articles

### Commands

- [Get](../../../client-api/commands/documents/get)
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
