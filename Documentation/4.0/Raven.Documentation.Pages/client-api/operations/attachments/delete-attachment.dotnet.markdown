# Operations : Attachments : How to delete an attachment?

This operation is used to delete attachment from document. 

## Syntax

{CODE:csharp delete_attachment_syntax@ClientApi\Operations\Attachments.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | Id of a document containing attachment |
| **name** | name | Name of an attachment |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

## Example

{CODE delete_1@ClientApi\Operations\Attachments.cs /}

## Related Articles

- [How to **create** attachment?](../../../client-api/operations/attachments/put-attachment) 
- [How to **get** attachment?](../../../client-api/operations/attachments/get-attachment)

