# Put Document Command
---

{NOTE: }

* Use the low-level `PutDocumentCommand` to insert a new document to the database or update an existing document.

* When using `PutDocumentCommand`, you must explicitly **specify the collection** to which the document will belong, 
  otherwise, the document will be placed in the `@empty` collection. See how this is done in the example below.

* To insert a document to the database using a higher-level method, see [storing entities](../../../client-api/session/storing-entities).  
  To update an existing document using a higher-level method, see [update entities](../../../client-api/session/updating-entities).

* In this page:

   * [Example](../../../client-api/commands/documents/put#example)
   * [Syntax](../../../client-api/commands/documents/put#syntax)

{NOTE/}

---

{PANEL: Example}

{CODE-TABS}
{CODE-TAB:csharp:Put_document put_document@ClientApi\Commands\Documents\Put.cs /}
{CODE-TAB:csharp:Put_document_async put_document_async@ClientApi\Commands\Documents\Put.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Commands\Documents\Put.cs /}

| Parameter        | Type                        | Description                                                                                                                                                                                                                                          |
|------------------|-----------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**           | `string`                    | Unique ID under which document will be stored.                                                                                                                                                                                                       |
| **changeVector** | `string`                    | The change-vector of the document you with to update,<br>used for [optimistic concurrency control](../../../server/clustering/replication/change-vector#concurrency-control-&-change-vectors).<br>Pass `null` to skip the check and force the 'put'. |
| **document**     | `BlittableJsonReaderObject` | The document to store. Use:<br>`session.Advanced.JsonConverter.ToBlittable(doc, docInfo);` to convert your entity to a `BlittableJsonReaderObject`.                                                                                                  |

{CODE syntax_2@ClientApi\Commands\Documents\Put.cs /}

{PANEL/}

## Related Articles

### Commands

- [Get document](../../../client-api/commands/documents/get)
- [Delete document](../../../client-api/commands/documents/delete)
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
