# Delete Attachment Operation

This operation is used to delete an attachment from a document. 

## Syntax

{CODE delete_attachment_syntax@ClientApi\Operations\Attachments\Attachments.cs /}

| Parameter        |        |                                                                         |
|------------------|--------|-------------------------------------------------------------------------|
| **documentId**   | string | ID of a document containing an attachment                               |
| **name**         | string | Name of an attachment                                                   |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

## Example

{CODE delete_1@ClientApi\Operations\Attachments\Attachments.cs /}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Get attachment](../../../client-api/operations/attachments/get-attachment)
