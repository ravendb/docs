# Attachments: Storing Attachments

In order to store an attachment in RavenDB you need to create a document. Then you can attach an attachment to the document using the `session.Advanced.Attachments.Store` method.

Attachments, just like documents, are a part of the session and will only be saved on the Server when `DocumentSession.SaveChanges` is executed (you can read more about saving changes in session [here](../../client-api/session/saving-changes)).

## Syntax

Attachments can be stored using one of the following `session.Advanced.Attachments.Store` methods:

{CODE StoreSyntax@DocumentExtensions\Attachments\Attachments.cs /}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync StoreAttachment@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async StoreAttachmentAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
