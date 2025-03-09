# Attachments: Storing Attachments

To store an attachment in RavenDB, you need to first create a document.  
Then, you can attach an attachment to the document using `session.advanced.attachments.store` 
or `session.advanced.attachments.storeFile`.  

Just like documents, sttachments are a part of the session and will only be saved on the server 
when `saveChanges` is executed (read more about saving changes in session [here](../../client-api/session/saving-changes)).

## Syntax

Attachments can be stored using `session.advanced.attachments.store` 
or `session.advanced.attachments.storeFile`:

{CODE:php StoreSyntax@DocumentExtensions\Attachments\Attachments.php /}

## Example

{CODE:php StoreAttachment@DocumentExtensions\Attachments\Attachments.php /}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
