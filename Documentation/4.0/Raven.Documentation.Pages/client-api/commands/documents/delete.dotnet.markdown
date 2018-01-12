# Commands : Documents : Delete

**Delete** is used to remove a document from a database.

## Syntax

{CODE delete_interface@ClientApi\Commands\Documents\Delete.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of a document to be deleted |
| **changeVector** | string | Entity Change Vector, used for concurrency checks (`null` to skip check) |

## Example

{CODE delete_sample@ClientApi\Commands\Documents\Delete.cs /}

## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put) 
