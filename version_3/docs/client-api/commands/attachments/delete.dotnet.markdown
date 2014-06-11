# Client API : Attachments : Delete

**DeleteAttachment** is used to remove a attachment from a database.

## Syntax

{CODE delete_1@ClientApi\Commands\Attachments\Delete.cs /}

**Parameters**   

- key - key of an attachment to delete   
- etag - current attachment etag, used for concurrency checks (`null` to skip check)   

## Example

{CODE delete_2@ClientApi\Commands\Attachments\Delete.cs /}

#### Related articles

- [Get](../../../client-api/commands/attachments/get)  
- [Put](../../../client-api/commands/attachments/put)  