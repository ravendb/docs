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

{CODE:nodejs put@client-api\commands\documents\put.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\commands\documents\put.js /}

| Parameter        | Type     | Description                                                             |
|------------------|----------|-------------------------------------------------------------------------|
| **id**           | `string` | Unique ID under which document will be stored                           |
| **changeVector** | `string` | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **document**     | `object` | The document to store.                                                  |

{CODE:nodejs syntax_2@client-api\commands\documents\put.js /}

{PANEL/}

## Related Articles

### Commands

- [Get document](../../../client-api/commands/documents/get)
- [Delete document](../../../client-api/commands/documents/delete)
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
