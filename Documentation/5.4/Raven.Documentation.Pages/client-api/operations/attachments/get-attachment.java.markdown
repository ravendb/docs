# Get Attachment Operation

This operation is used to get an attachment from a document. 

## Syntax

{CODE:java get_attachment_syntax@ClientApi\Operations\Attachments\Attachments.java /}

{CODE:java get_attachment_return_value@ClientApi\Operations\Attachments\Attachments.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | String | ID of a document which will contain an attachment |
| **name** | String | Name of an attachment |
| **type** | AttachmentType | Specify whether getting an attachment from a document or from a revision.<br>(`DOCUMENT` or `REVISION`). |
| **changeVector** | String | The ChangeVector of the document or the revision to which the attachment belongs.<br>Mandatory when getting an attachment from a revision.<br>Used for concurrency checks (use `null` to skip the check). |

| Return Value | |
| ------------- | ----- |
| **Stream** | InputStream containing an attachment |
| **ChangeVector** | Change vector of document |
| **DocumentId** | ID of document |
| **Name** | Name of attachment |
| **Hash** | Hash of attachment |
| **ContentType** | MIME content type of an attachment |
| **Size** | Size of attachment |

## Example

{CODE:java get_1@ClientApi\Operations\Attachments\Attachments.java /}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Delete attachment](../../../client-api/operations/attachments/delete-attachment)
