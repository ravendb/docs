# Attachments: How to get attachment metadata only?

There are few methods that allow you to download attachment metadata from a database:   
- [HeadAttachment](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#head)   
- [GetAttachmentHeadersStartingWith](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#getattachmentheadersstartingwith)   

{PANEL:**HeadAttachment**}

**HeadAttachment** can be used to download attachment metadata for a single attachment.

### Syntax

{CODE:java head_1_0@ClientApi\Commands\Attachments\HowTo\Head.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key of the attachment you want to download metadata for |

| Return Value | |
| ------------- | ----- |
| [Attachment](../../../../glossary/attachment) | Object that represents attachment. |

### Example

{CODE:java head_1_1@ClientApi\Commands\Attachments\HowTo\Head.java /}

{PANEL/}

{PANEL:**GetAttachmentHeadersStartingWith**}

**GetAttachmentHeadersStartingWith** can be used to download attachment metadata for a multiple attachments.

### Syntax

{CODE:java head_2_0@ClientApi\Commands\Attachments\HowTo\Head.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **idPrefix** | String | prefix for which attachments should be returned |
| **start** | int | number of attachments that should be skipped |
| **pageSize** | int | maximum number of attachments that will be returned |

| Return Value | |
| ------------- | ----- |
| List&lt;[AttachmentInformation](../../../../glossary/attachment-information)&gt; | Object that represents attachment metadata information. |

### Example

{CODE:java head_2_1@ClientApi\Commands\Attachments\HowTo\Head.java /}

{PANEL/}

## Remarks

**getData()**  in `Attachment` will throw for the above methods.

## Related articles

- [How to **update** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  
- [GetAttachment](../../../../client-api/commands/attachments/get)  
