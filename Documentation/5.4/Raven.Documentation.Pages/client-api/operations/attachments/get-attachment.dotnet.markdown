# Get Attachment Operation

This operation is used to get an attachment from a document. 

## Syntax

{CODE:csharp get_attachment_syntax@ClientApi\Operations\Attachments\Attachments.cs /}

{CODE:csharp get_attachment_return_value@ClientApi\Operations\Attachments\Attachments.cs /}

| Parameter        |                | |
|------------------|----------------| ----- |
| **documentId**   | string         | ID of a document which will contain an attachment |
| **name**         | string     | Name of an attachment |
| **type**         | AttachmentType | **Document** or **Revision** |
| **changeVector** | string         | Entity changeVector, used for concurrency checks (`null` to skip check) |

| Return Value | |
| ------------- | ----- |
| **Stream** | Stream containing an attachment |
| **ChangeVector** | Change vector of an attachment |
| **DocumentId** | ID of document |
| **Name** | Name of attachment |
| **Hash** | Hash of attachment |
| **ContentType** | MIME content type of an attachment |
| **Size** | Size of attachment |

## Example

{CODE get_1@ClientApi\Operations\Attachments\Attachments.cs /}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Delete attachment](../../../client-api/operations/attachments/delete-attachment)
