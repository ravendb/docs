# Attachments: Loading Attachments
---

{NOTE: }
Learn in this page how to load a part of an attachment, an entire attachment, 
or multiple attachments.  

* In this page:  
   * [Load attachments](../../document-extensions/attachments/loading#load-attachments)  
   * [Load a part of an attachment](../../document-extensions/attachments/loading#load-a-part-of-an-attachment)  

{NOTE/}

---

{PANEL: Load attachments}

* Use these methods to load attachments from the database.  
   * **session.Advanced.Attachments.Get**  
     Can be used to download an attachment or multiple attachments.  
   * **session.Advanced.Attachments.GetNames**  
     Can be used to download all attachment names that are attached to a document.  
   * **session.Advanced.Attachments.GetRevision**  
     Can be used to download an attachment of a revision document.  

* Use this method to verify that an attachment exists.  
   * **session.Advanced.Attachments.Exists**  

## Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync GetSyntax@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetSyntaxAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync GetAttachment@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAttachmentAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

## Example II
Here, we load multiple string attachments we previously created for a document. We then 
go through them, and decode each attachment to its original text.  
{CODE-TABS}
{CODE-TAB:csharp:Sync GetAllAttachments@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TAB:csharp:Async GetAllAttachmentsAsync@DocumentExtensions\Attachments\Attachments.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Load a part of an attachment}

Use `GetRange` to load a part of an attachment by document ID and the attachment name.  

## Syntax
{CODE:csharp GetRngSyntax@DocumentExtensions\Attachments\Attachments.cs /}  

## Sample
{CODE-TABS}
{CODE-TAB:csharp:Sync GetRange@DocumentExtensions\Attachments\Attachments.cs /}  
{CODE-TAB:csharp:Async GetRangeAsync@DocumentExtensions\Attachments\Attachments.cs /}  
{CODE-TABS/}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  
- [Storing](../../document-extensions/attachments/storing)  
- [Deleting](../../document-extensions/attachments/deleting)  
