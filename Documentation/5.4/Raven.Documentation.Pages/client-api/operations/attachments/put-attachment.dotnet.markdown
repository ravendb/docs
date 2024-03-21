# Put Attachment Operation

This operation is used to put an attachment to a document. 

## Syntax

{CODE:csharp put_attachment_syntax@ClientApi\Operations\Attachments\Attachments.cs /}

{CODE:csharp put_attachment_return_value@ClientApi\Operations\Attachments\Attachments.cs /}

| Parameter        |        |                                                                         |
|------------------|--------|-------------------------------------------------------------------------|
| **documentId**   | string | ID of a document which will contain an attachment                       |
| **name**         | string | Name of an attachment                                                   |
| **stream**       | Stream | Stream contains attachment raw bytes                                    |
| **contentType**  | string | MIME type of attachment                                                 |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

| Return Value     |                                     |
|------------------|-------------------------------------|
| **ChangeVector** | Change vector of created attachment |
| **DocumentId**   | ID of document                      |
| **Name**         | Name of created attachment          |
| **Hash**         | Hash of created attachment          |
| **ContentType**  | MIME content type of attachment     |
| **Size**         | Size of attachment                  |

## Example

{CODE put_1@ClientApi\Operations\Attachments\Attachments.cs /}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Get attachment](../../../client-api/operations/attachments/get-attachment)
- [Delete attachment](../../../client-api/operations/attachments/delete-attachment)
