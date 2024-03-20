# Delete Attachment Operation

This operation is used to delete an attachment from a document. 

## Syntax

{CODE:java delete_attachment_syntax@ClientApi\Operations\Attachments\Attachments.java /}

| Parameter        |        |                                                                         |
|------------------|--------|-------------------------------------------------------------------------|
| **documentId**   | String | ID of a document containing an attachment                               |
| **name**         | String | Name of an attachment                                                   |
| **changeVector** | String | Entity changeVector, used for concurrency checks (`null` to skip check) |

## Example

{CODE:java delete_1@ClientApi\Operations\Attachments\Attachments.java /}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Get attachment](../../../client-api/operations/attachments/get-attachment)
