# Delete Document Command

{NOTE: }

* Use the `DeleteDocumentCommand` to remove a document from the database.

* In this page:

    * [Example](../../../client-api/commands/documents/delete#example)
    * [Syntax](../../../client-api/commands/documents/delete#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE:nodejs delete@client-api\commands\documents\delete.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\commands\documents\delete.js /}

| Parameter        | Type   | Description                                                             |
|------------------|--------|-------------------------------------------------------------------------|
| __id__           | string | ID of a document to be deleted                                          |
| __changeVector__ | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

{PANEL/}

## Related Articles

### Commands 

- [Get document command](../../../client-api/commands/documents/get)  
- [Put document command](../../../client-api/commands/documents/put)  
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
