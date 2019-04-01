# Commands: Documents: Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE put_interface@ClientApi\Commands\Documents\Put.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | unique ID under which document will be stored |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **document** | BlittableJsonReaderObject | The document to store. You may use `session.Advanced.EntityToBlittable.ConvertEntityToBlittable` to convert your entity to a `BlittableJsonReaderObject` |

## Example

{CODE put_sample@ClientApi\Commands\Documents\Put.cs /}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
