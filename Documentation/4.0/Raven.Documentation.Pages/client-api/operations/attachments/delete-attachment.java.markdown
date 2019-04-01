# Operations: Attachments: How to Delete an Attachment

This operation is used to delete an attachment from a document. 

## Syntax

{CODE:java delete_attachment_syntax@ClientApi\Operations\Attachments.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | String | ID of a document containing an attachment |
| **name** | String | Name of an attachment |
| **changeVector** | String | Entity changeVector, used for concurrency checks (`null` to skip check) |

## Example

{CODE:java delete_1@ClientApi\Operations\Attachments.java /}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Put Attachment](../../../client-api/operations/attachments/put-attachment) 
- [Get Attachment](../../../client-api/operations/attachments/get-attachment)
