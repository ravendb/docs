# Attachments: Storing Attachments

In order to store an attachment in RavenDB you need to create a document. Then you can attach an attachment to the document using the `session.advanced.attachments.store()` method.

Attachments, just like documents, are a part of the session and will be only saved on the Server when `DocumentSession.saveChanges()` is executed.  

## Syntax

Attachments can be stored using one of the following `session.advanced.attachments.store()` methods:

{CODE:nodejs StoreSyntax@DocumentExtensions\Attachments\attachments.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **documentId** | object or string | instance of the entity or the entity ID |
| **name** | string | attachment name |
| **stream** | `Readable` or `Buffer` | attachment content |
| **contentType** | string | attachment content type |

## Example

{CODE:nodejs StoreAttachment@DocumentExtensions\Attachments\attachments.js /}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
