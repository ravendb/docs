# Indexes: Indexing Attachments

The `AttachmentsFor` method returns a list of [attachments](../client-api/session/attachments/what-are-attachments) in a given document as well as basic information like `Name` or `Size` about each of them.

{CODE-TABS}
{CODE-TAB:java:AttachmentsFor syntax@Indexes\IndexingAttachments.java /}
{CODE-TAB:java:AttachmentName result@Indexes\IndexingAttachments.java /}
{CODE-TABS/}

## Creating an index using `AttachmentsFor()`

{CODE:java index@Indexes\IndexingAttachments.java /}

## Querying the index

{CODE:java query1@Indexes\IndexingAttachments.java /}

## Related Articles

### Client API

- [What are Attachments](../client-api/session/attachments/what-are-attachments)
