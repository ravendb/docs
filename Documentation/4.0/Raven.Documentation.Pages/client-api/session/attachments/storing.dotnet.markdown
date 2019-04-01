# Attachments: Storing Attachments

In order to store an attachment in RavenDB you need to create a document. Then you can attach an attachment to the document using the `session.Advanced.Attachments.Store` method.

Attachments, just like documents, are a part of the session and will be only saved on the Server when `DocumentSession.SaveChanges` is executed (you can read more about saving changes in session [here](../../../client-api/session/saving-changes)).

## Syntax

Attachments can be stored using one of the following `session.Advanced.Attachments.Store` methods:

{CODE StoreSyntax@ClientApi\Session\Attachments\Attachments.cs /}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync StoreAttachment@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async StoreAttachmentAsync@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TABS/}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Loading](../../../client-api/session/attachments/loading)
- [Deleting](../../../client-api/session/attachments/deleting)
