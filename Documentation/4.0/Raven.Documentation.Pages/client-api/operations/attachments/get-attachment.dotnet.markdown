# Operations: Attachments: How to Get an Attachment

This operation is used to get an attachment from a document. 

## Syntax

{CODE:csharp get_attachment_syntax@ClientApi\Operations\Attachments.cs /}

{CODE:csharp get_attachment_return_value@ClientApi\Operations\Attachments.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | ID of a document which will contain an attachment |
| **name** | name | Name of an attachment |
| **type** | AttachmentType | **Document** or **Revision** |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

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

{CODE get_1@ClientApi\Operations\Attachments.cs /}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Put Attachment](../../../client-api/operations/attachments/put-attachment) 
- [Delete Attachment](../../../client-api/operations/attachments/delete-attachment)
