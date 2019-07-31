# Commands: Documents: Delete

**Delete** is used to remove a document from a database.

## Syntax

{CODE delete_interface@ClientApi\Commands\Documents\Delete.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of a document to be deleted |
| **changeVector** | string | Entity Change Vector, used for concurrency checks (`null` to skip check) |

## Example

{CODE delete_sample@ClientApi\Commands\Documents\Delete.cs /}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
