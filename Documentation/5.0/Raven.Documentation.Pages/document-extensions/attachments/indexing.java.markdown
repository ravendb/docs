# Indexes: Indexing Attachments

The `AttachmentsFor` method returns a list of [attachments](../client-api/session/attachments/what-are-attachments) in a given document as well as basic information like `Name` or `Size` about each of them.

{CODE-TABS}
{CODE-TAB:java:AttachmentsFor syntax@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TAB:java:AttachmentName result@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

## Creating an index using `AttachmentsFor()`

{CODE:java index@DocumentExtensions\Attachments\IndexingAttachments.java /}

## Querying the index

{CODE:java query1@DocumentExtensions\Attachments\IndexingAttachments.java /}

## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)
