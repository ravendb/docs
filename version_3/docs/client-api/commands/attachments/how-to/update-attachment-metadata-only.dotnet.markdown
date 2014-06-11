# Client API : Attachments : How to update attachment metadata only?

**UpdateAttachmentMetadata** is used to update an attachment metadata in a database.

## Syntax

{CODE update_1@ClientApi\Commands\Attachments\HowTo\Update.cs /}

**Parameters**   

- key - key under which attachment is stored
- etag - current attachment etag, used for concurrency checks (`null` to skip check) 
- metadata - attachment metadata  

## Example

{CODE update_2@ClientApi\Commands\Attachments\HowTo\Update.cs /}

#### Related articles

- [How to **get** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/delete)  