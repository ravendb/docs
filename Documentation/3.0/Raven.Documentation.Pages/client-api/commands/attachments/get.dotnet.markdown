# Client API : Attachments : Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/commands/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/commands/attachments/get#getattachments)   

## GetAttachment

**GetAttachment** can be used to download a single attachment.

### Syntax

{CODE get_1_0@ClientApi\Commands\Attachments\Get.cs /}

**Parameters**   

key
:   Type: string   
key of the attachment you want to download

**Return Value**

Type: [Attachment](../../../glossary/json/attachment)   
Object that represents attachment.

### Example

{CODE get_1_1@ClientApi\Commands\Attachments\Get.cs /}

## GetAttachments

**GetAttachment** can be used to download attachment information for multiple attachments.

### Syntax

{CODE get_2_0@ClientApi\Commands\Attachments\Get.cs /}

**Parameters**   

start
:   Type: int   
Indicates how many attachments should be skipped.

startEtag
:   Type: Etag   
ETag from which to start

batchSize
:   Type: int   
maximum number of attachment that will be downloaded   

**Return Value**

Type: [AttachmentInformation](../../../glossary/json/attachment-information)   
Object that represents attachment metadata information.

### Example

{CODE get_2_1@ClientApi\Commands\Attachments\Get.cs /}

#### Related articles

- [How to **get** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  