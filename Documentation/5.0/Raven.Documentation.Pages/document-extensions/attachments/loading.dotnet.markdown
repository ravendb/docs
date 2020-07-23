# Attachments: Loading Attachments

There are a few methods that allow you to download attachments from a database:   

**session.Advanced.Attachments.Get** can be used to download an attachment.   
**session.Advanced.Attachments.GetNames** can be used to download all attachment names that are attached to a document.   
**session.Advanced.Attachments.GetRevision** can be used to download an attachment of a revision document.   
**session.Advanced.Attachments.Exists** can be used to determine if an attachment exists on a document.   

## Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync GetSyntax@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetSyntaxAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync GetAttachment@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAttachmentAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Storing](../../document-extensions/attachments/storing)
- [Deleting](../../document-extensions/attachments/deleting)
