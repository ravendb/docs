# Attachments : Put

In order to put an attachment in RavenDB you need to create a document and than you can attach an attachment to the document using the `Advanced.Attachments.Store` method.
Note that attachments, same as documents, are a part of the session and will be only saved on the Server when session.SaveChanges is executed in one transaction.

## Syntax

Attachments can be stored using one the following `session.Advanced.Attachments.Store` methods:

{CODE StoreSyntax@ClientApi\Session\Attachments\Attachments.cs /}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync StoreAttachment@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async StoreAttachmentAsync@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TABS/}

## Related Articles

- [GetAttachment](../../../client-api/commands/attachments/get)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)
