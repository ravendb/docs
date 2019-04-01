# Attachments: Loading Attachments

There are a few methods that allow you to download attachments from a database:   

**session.advanced().attachments().get** can be used to download an attachment.   
**session.advanced().attachments().getNames** can be used to download all attachment names that are attached to a document.   
**session.advanced().attachments().getRevision** can be used to download an attachment of a revision document.   
**session.advanced().attachments().exists** can be used to determine if an attachment exists on a document.   

## Syntax

{CODE:java GetSyntax@ClientApi\Session\Attachments\Attachments.java /}

## Example

{CODE:java GetAttachment@ClientApi\Session\Attachments\Attachments.java /}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Storing](../../../client-api/session/attachments/storing)
- [Deleting](../../../client-api/session/attachments/deleting)
