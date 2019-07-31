# Commands: Documents: Delete

**Delete** is used to remove a document from a database.

## Syntax

{CODE:java delete_interface@ClientApi\Commands\Documents\Delete.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | ID of a document to be deleted |
| **changeVector** | String | Entity Change Vector, used for concurrency checks (`null` to skip check) |

## Example

{CODE:java delete_sample@ClientApi\Commands\Documents\Delete.java /}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
