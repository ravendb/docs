# Attachments: Delete

**DeleteAttachment** is used to remove an attachment from a database.

## Syntax

{CODE delete_1@ClientApi\Commands\Attachments\Delete.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | key of an attachment to delete |
| **etag** | Etag | current attachment etag, used for concurrency checks (`null` to skip check) |

## Example

{CODE delete_2@ClientApi\Commands\Attachments\Delete.cs /}

## Related articles

- [GetAttachment](../../../client-api/commands/attachments/get)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
