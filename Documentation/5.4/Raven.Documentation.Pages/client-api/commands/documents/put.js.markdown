# Put Document Command
---

{NOTE: }

* Use the `PutDocumentCommand` to insert or update a document in the database.

* In this page:

   * [Example](../../../client-api/commands/documents/put#example)
   * [Syntax](../../../client-api/commands/documents/put#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE:nodejs put@client-api\commands\documents\put.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\commands\documents\put.js /}

| Parameter        | Type   | Description                                                             |
|------------------|--------|-------------------------------------------------------------------------|
| __id__           | string | Unique ID under which document will be stored                           |
| __changeVector__ | string | Entity changeVector, used for concurrency checks (`null` to skip check) |
| __document__     | object | The document to store.                                                  |

{CODE:nodejs syntax_2@client-api\commands\documents\put.js /}

{PANEL/}

## Related Articles

### Commands

- [Get document](../../../client-api/commands/documents/get)
- [Delete document](../../../client-api/commands/documents/delete)
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
