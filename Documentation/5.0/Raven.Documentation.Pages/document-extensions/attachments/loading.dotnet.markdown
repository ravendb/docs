# Attachments: Loading Attachments

There are a few methods that allow you to load attachments from a database:   

**session.Advanced.Attachments.Get** can be used to load an attachment or multiple attachments.  
**session.Advanced.Attachments.GetNames** can be used to load all attachment names that are attached to a document.  
**session.Advanced.Attachments.GetRevision** can be used to load an attachment of a revision document.  
**session.Advanced.Attachments.Exists** can be used to determine if an attachment exists on a document.  

## Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync GetSyntax@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetSyntaxAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Example I - Load attachments using the names of the attachments

{CODE-TABS}
{CODE-TAB:csharp:Sync GetAttachment@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAttachmentAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Example II - Load attachments using the references in their parent document
Here, we load multiple string attachments we previously created for a document. We then 
go through them, and decode each attachment to its original text.  
{CODE-TABS}
{CODE-TAB:csharp:Sync GetAllAttachments@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAllAttachmentsAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}


## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  
- [Storing](../../document-extensions/attachments/storing)  
- [Deleting](../../document-extensions/attachments/deleting)  
