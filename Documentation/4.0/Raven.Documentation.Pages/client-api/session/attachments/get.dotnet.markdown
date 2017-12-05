# Attachments : Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/session/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/session/attachments/get#getattachments)   

**session.Advanced.Attachments.Get** can be used to download an attachment.
**session.Advanced.Attachments.GetNames** can be used to all attachment names that attached to a document.
**session.Advanced.Attachments.GetRevision** can be used to download an attachment of a reivions document.
**session.Advanced.Attachments.Exists** can be used to determine if an attachment is exists on a document.

### Syntax

{CODE GetSyntax@ClientApi\Session\Attachments\Attachments.cs /}

### Example

{CODE GetAttachment@ClientApi\Attachments\Attachments\Attachments.cs /}

{CODE GetAttachmentAsync@ClientApi\Attachments\Attachments\Attachments.cs /}

## Related articles

- [PutAttachment](../../../client-api/session/attachments/put)  
- [DeleteAttachment](../../../client-api/session/attachments/delete)  
