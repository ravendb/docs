# Client API : Attachments : How to get attachment metadata only?

There are few methods that allow you to download attachment metadata from a database:   
- [Head](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#head)   
- [GetAttachmentHeadersStartingWith](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#getattachmentheadersstartingwith)   

## Head

**Head** can be used to download attachment metadata for a single attachment.

### Syntax

{CODE head_1_0@ClientApi\Commands\Attachments\HowTo\Head.cs /}

**Parameters**   

key
:   Type: string   
key of the attachment you want to download metadata for

**Return Value**

Type: [Attachment](../../../glossary/json/attachment)   
Object that represents attachment.

### Example

{CODE head_1_1@ClientApi\Commands\Attachments\HowTo\Head.cs /}

## GetAttachmentHeadersStartingWith

**GetAttachmentHeadersStartingWith** can be used to download attachment metadata for a multiple attachments.

### Syntax

{CODE head_2_0@ClientApi\Commands\Attachments\HowTo\Head.cs /}

**Parameters**   

idPrefix
:   Type: string   
prefix for which attachments should be returned

start
:   Type: int   
number of attachments that should be skipped

pageSize
:   Type: int   
maximum number of attachments that will be returned

**Return Value**

Type: [AttachmentInformation](../../../glossary/json/attachment-information)   
Object that represents attachment metadata information.

### Example

{CODE head_2_1@ClientApi\Commands\Attachments\HowTo\Head.cs /}

## Remarks

**Data** property in `Attachment` will return empty stream for above methods.

#### Related articles

- [How to **update** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [GetAttachment](../../../client-api/commands/attachments/get)  