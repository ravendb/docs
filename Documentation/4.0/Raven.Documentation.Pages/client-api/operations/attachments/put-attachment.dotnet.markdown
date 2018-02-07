# Operations : Attachments : How to put an attachment?

This operation is used to put attachment to document. 

## Syntax

{CODE:csharp put_attachment_syntax@ClientApi\Operations\Attachments.cs /}

{CODE:csharp put_attachment_return_value@ClientApi\Operations\Attachments.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | Id of a document which will contain attachment |
| **name** | name | Name of an attachment |
| **stream** | Stream | Stream contains attachment raw bytes |
| **contentType** | string | MIME type of attachment |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |

| Return Value | |
| ------------- | ----- |
| **ChangeVector** | Change vector of created attachment |
| **DocumentId** | Id of document |
| **Name** | Name of created attachment |
| **Hash** | Hash of created attachment |
| **ContentType** | MIME content type of attachment |
| **Size** | Size of attachment |

## Example

{CODE put_1@ClientApi\Operations\Attachments.cs /}

## Related Articles

- [How to **delete** attachment?](../../../client-api/operations/attachments/delete-attachment)
- [How to **get** attachment?](../../../client-api/operations/attachments/get-attachment)

