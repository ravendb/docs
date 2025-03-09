# Attachments: Storing Attachments

To store an attachment in RavenDB, you need to first create a document.  
Then, you can attach an attachment to the document using the `session.advanced.attachments.store` method.  

Just like documents, sttachments are a part of the session and will only be saved on the server 
when `save_changes` is executed (read more about saving changes in session [here](../../client-api/session/saving-changes)).

## Syntax

Attachments can be stored using `session.advanced.attachments.store`:

{CODE:python StoreSyntax@DocumentExtensions\Attachments\Attachments.py /}

## Example

{CODE:python StoreAttachment@DocumentExtensions\Attachments\Attachments.py /}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
