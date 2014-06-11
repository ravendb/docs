# Client API : Attachments : Put

**PutAttachment** is used to insert or update an attachment in a database.

## Syntax

{CODE put_1@ClientApi\Commands\Attachments\Put.cs /}

**Parameters**   

- key - unique key under which attachment will be stored
- etag - current attachment etag, used for concurrency checks (`null` to skip check) 
- data - attachment data   
- metadata - attachment metadata  

## Example

{CODE put_2@ClientApi\Commands\Attachments\Put.cs /}

#### Related articles

- [How to **update** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [GetAttachment](../../../client-api/commands/attachments/get)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  