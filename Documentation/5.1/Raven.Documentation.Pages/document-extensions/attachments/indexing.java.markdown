# Indexes: Indexing Attachments

The `AttachmentsFor` method returns a list of 
[attachments](../document-extensions/attachments/what-are-attachments) 
in a given document as well as basic information like `name` or `size` 
about each of them.

{CODE-TABS}
{CODE-TAB:java:AttachmentsFor syntax@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TAB:java:AttachmentName result@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

## Creating an index using `AttachmentsFor()`

{CODE:java index@DocumentExtensions\Attachments\IndexingAttachments.java /}

## Querying the index

{CODE:java query1@DocumentExtensions\Attachments\IndexingAttachments.java /}

## Related Articles

### Client API

- [What are Attachments](../document-extensions/attachments/what-are-attachments)
