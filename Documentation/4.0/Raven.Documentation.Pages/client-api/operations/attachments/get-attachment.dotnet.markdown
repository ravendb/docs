# Operations : Attachments : How to get an attachment?

This operation is used to get attachment from document. 

## Syntax

{CODE:csharp get_attachment_syntax@ClientApi\Operations\Attachments.cs /}

{CODE:csharp get_attachment_return_value@ClientApi\Operations\Attachments.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | Id of a document which will contain attachment |
| **name** | name | Name of an attachment |
| **type** | AttachmentType | **Document** or **Revision** |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

| Return Value | |
| ------------- | ----- |
| **Stream** | Stream containing attachment |
| **ChangeVector** | Change vector of attachment |
| **DocumentId** | Id of document |
| **Name** | Name of attachment |
| **Hash** | Hash of attachment |
| **ContentType** | MIME content type of attachment |
| **Size** | Size of attachment |

## Example

{CODE get_1@ClientApi\Operations\Attachments.cs /}


## Related Articles

- [How to **create** attachment?](../../../client-api/operations/attachments/put-attachment) 
- [How to **delete** attachment?](../../../client-api/operations/attachments/delete-attachment)

