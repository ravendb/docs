# Put Document Command
---

{NOTE: }

* Use `PutDocumentCommand` to insert a document to the database or update an existing document.

* In this page:

   * [Example](../../../client-api/commands/documents/put#example)
   * [Syntax](../../../client-api/commands/documents/put#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE:python put_sample@ClientApi\Commands\Documents\Put.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python put_interface@ClientApi\Commands\Documents\Put.py /}

| Parameter         | Type             | Description                                                             |
|-------------------|------------------|-------------------------------------------------------------------------|
| **key**           | `str`            | Unique ID under which document will be stored                           |
| **change_vector** | `str` (optional) | Entity changeVector, used for concurrency checks (`None` to skip check) |
| **document**      | `dict`           | The document to store                                                   |

{PANEL/}

## Related Articles

### Commands

- [Get](../../../client-api/commands/documents/get)
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
