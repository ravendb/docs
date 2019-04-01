# Attachments: Loading Attachments

There are a few methods that allow you to download attachments from a database:   

**session.Advanced.Attachments.Get** can be used to download an attachment.   
**session.Advanced.Attachments.GetNames** can be used to download all attachment names that are attached to a document.   
**session.Advanced.Attachments.GetRevision** can be used to download an attachment of a revision document.   
**session.Advanced.Attachments.Exists** can be used to determine if an attachment exists on a document.   

## Syntax

{CODE GetSyntax@ClientApi\Session\Attachments\Attachments.cs /}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync GetAttachment@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAttachmentAsync@ClientApi\Session\Attachments\Attachments.cs /}
{CODE-TABS/}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Storing](../../../client-api/session/attachments/storing)
- [Deleting](../../../client-api/session/attachments/deleting)
