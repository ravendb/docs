# Attachments: Storing Attachments

In order to store an attachment in RavenDB you need to create a document. Then you can attach an attachment to the document using the `session.advanced.attachments.store()` method.

Attachments, just like documents, are a part of the session and will be only saved on the Server when `DocumentSession.saveChanges()` is executed (you can read more about saving changes in session [here](../../../client-api/session/saving-changes)).

## Syntax

Attachments can be stored using one of the following `session.advanced.attachments.store()` methods:

{CODE:nodejs StoreSyntax@client-api\session\attachments\attachments.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **documentId** | object or string | instance of the entity or the entity ID |
| **name** | string | attachment name |
| **stream** | `Readable` or `Buffer` | attachment content |
| **contentType** | string | attachment content type |

## Example

{CODE:nodejs StoreAttachment@client-api\session\attachments\attachments.js /}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Loading](../../../client-api/session/attachments/loading)
- [Deleting](../../../client-api/session/attachments/deleting)
