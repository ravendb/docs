# Attachments: Loading Attachments

There are a few methods that allow you to download attachments from a database:   

**session.advanced().attachments().get** can be used to download an attachment.   
**session.advanced().attachments().getNames** can be used to download all attachment names that are attached to a document.   
**session.advanced().attachments().getRevision** can be used to download an attachment of a revision document.   
**session.advanced().attachments().exists** can be used to determine if an attachment exists on a document.   

## Syntax

{CODE:java GetSyntax@DocumentExtensions\Attachments\Attachments.java /}

## Example

{CODE:java GetAttachment@DocumentExtensions\Attachments\Attachments.java /}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  
- [Storing](../../document-extensions/attachments/storing)  
- [Deleting](../../document-extensions/attachments/deleting)  
