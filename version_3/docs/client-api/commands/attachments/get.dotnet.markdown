# Client API : Attachments : Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/commands/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/commands/documents/get#getattachments)   

## GetAttachment

**GetAttachment** can be used to download a single attachment.

### Syntax

{CODE get_1_0@ClientApi\Commands\Attachments\Get.cs /}

**Parameters**   

- key - key of the attachment you want to download   

**Return Value**

{CODE attachment@Common.cs /}

### Example

{CODE get_1_1@ClientApi\Commands\Attachments\Get.cs /}

## GetAttachments

**GetAttachment** can be used to download attachment information for multiple attachments.

### Syntax

{CODE get_2_0@ClientApi\Commands\Attachments\Get.cs /}

**Parameters**   

- startEtag - ETag from which to start    
- batchSize - maximum number of attachment that will be downloaded   

**Return Value**

{CODE attachmentinformation@Common.cs /}

### Example

{CODE get_2_1@ClientApi\Commands\Attachments\Get.cs /}

#### Related articles

- [PutAttachment](../../../client-api/commands/attachments/put)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  