# Operations: Attachments: How to Put an Attachment

This operation is used to put an attachment to a document. 

## Syntax

{CODE:java put_attachment_syntax@ClientApi\Operations\Attachments.java /}

{CODE:java put_attachment_return_value@ClientApi\Operations\Attachments.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | String | ID of a document which will contain an attachment |
| **name** | String | Name of an attachment |
| **stream** | InputStream | Stream contains attachment raw bytes |
| **contentType** | String | MIME type of attachment |
| **changeVector** | String | Entity changeVector, used for concurrency checks (`null` to skip check) |

| Return Value | |
| ------------- | ----- |
| **ChangeVector** | Change vector of created attachment |
| **DocumentId** | ID of document |
| **Name** | Name of created attachment |
| **Hash** | Hash of created attachment |
| **ContentType** | MIME content type of attachment |
| **Size** | Size of attachment |

## Example

{CODE:java put_1@ClientApi\Operations\Attachments.java /}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Get Attachment](../../../client-api/operations/attachments/get-attachment)
- [Delete Attachment](../../../client-api/operations/attachments/delete-attachment)
