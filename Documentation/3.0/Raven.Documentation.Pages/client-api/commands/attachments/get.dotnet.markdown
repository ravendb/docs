# Attachments: Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/commands/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/commands/attachments/get#getattachments)   

{PANEL:**GetAttachment**}

**GetAttachment** can be used to download a single attachment.

### Syntax

{CODE get_1_0@ClientApi\Commands\Attachments\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | key of the attachment you want to download |

| Return Value | |
| ------------- | ----- |
| [Attachment](../../../glossary/attachment) | Object that represents attachment. |

### Example

{CODE get_1_1@ClientApi\Commands\Attachments\Get.cs /}

{PANEL/}
{PANEL:GetAttachments}

**GetAttachments** can be used to download attachment information for multiple attachments.

### Syntax

{CODE get_2_0@ClientApi\Commands\Attachments\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | Indicates how many attachments should be skipped |
| **startEtag** | Etag | ETag from which to start |
| **batchSize** | int | maximum number of attachments that will be downloaded |

| Return Value | |
| ------------- | ----- |
| [AttachmentInformation](../../../glossary/attachment-information) | Object that represents attachment metadata information. |

### Example

{CODE get_2_1@ClientApi\Commands\Attachments\Get.cs /}

{PANEL/}

## Related articles

- [How to **get** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  
