# Attachments: Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/commands/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/commands/attachments/get#getattachments)   

{PANEL:**GetAttachment**}

**GetAttachment** can be used to download a single attachment.

### Syntax

{CODE:java get_1_0@ClientApi\Commands\Attachments\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key of the attachment you want to download |

| Return Value | |
| ------------- | ----- |
| [Attachment](../../../glossary/attachment) | Object that represents attachment. |

### Example

{CODE:java get_1_1@ClientApi\Commands\Attachments\Get.java /}

{PANEL/}
{PANEL:GetAttachments}

**GetAttachments** can be used to download attachment information for multiple attachments.

### Syntax

{CODE:java get_2_0@ClientApi\Commands\Attachments\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | Indicates how many attachments should be skipped |
| **startEtag** | Etag | ETag from which to start |
| **batchSize** | int | maximum number of attachments that will be downloaded |

| Return Value | |
| ------------- | ----- |
| [AttachmentInformation](../../../glossary/attachment-information) | Object that represents attachment metadata information. |

### Example

{CODE:java get_2_1@ClientApi\Commands\Attachments\Get.java /}

{PANEL/}

## Related articles

- [How to **get** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  
