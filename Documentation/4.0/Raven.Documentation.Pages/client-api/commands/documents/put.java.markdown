# Commands : Documents : Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE:java put_interface@ClientApi\Commands\Documents\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | unique ID under which document will be stored |
| **changeVector** | String | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **document** | ObjectNode | The document to store. You may use `session.advanced().getEntityToJson().convertEntityToJson` to convert your entity to a `ObjectNode` |

## Example

{CODE:java put_sample@ClientApi\Commands\Documents\Put.java /}

## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)  
