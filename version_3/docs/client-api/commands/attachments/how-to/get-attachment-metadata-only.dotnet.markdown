# Client API : Attachments : How to get attachment metadata only?

There are few methods that allow you to download attachment metadata from a database:   
- [Head](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#head)   
- [GetAttachmentHeadersStartingWith](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#getattachmentheadersstartingwith)   

## Head

**Head** can be used to download attachment metadata for a single attachment.

### Syntax

{CODE head_1_0@ClientApi\Commands\Attachments\HowTo\Head.cs /}

**Parameters**   

- key - key of the attachment you want to download metadata for   

**Return Value**

{CODE attachment@Common.cs /}

### Example

{CODE head_1_1@ClientApi\Commands\Attachments\HowTo\Head.cs /}

## GetAttachmentHeadersStartingWith

**GetAttachmentHeadersStartingWith** can be used to download attachment metadata for a multiple attachments.

### Syntax

{CODE head_2_0@ClientApi\Commands\Attachments\HowTo\Head.cs /}

**Parameters**   

- idPrefix - prefix for which attachments should be returned   
- start - number of attachments that should be skipped   
- pageSize - maximum number of attachments that will be returned   

**Return Value**

{CODE attachmentinformation@Common.cs /}

### Example

{CODE head_2_1@ClientApi\Commands\Attachments\HowTo\Head.cs /}

## Remarks

**Data** property in `Attachment` will return empty stream for above methods.

#### Related articles

- [How to **update** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [GetAttachment](../../../client-api/commands/attachments/get)  