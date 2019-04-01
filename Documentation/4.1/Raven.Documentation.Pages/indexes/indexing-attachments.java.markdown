# Indexes: Indexing Attachments

To address the need of indexing (and searching for) the [attachments](../client-api/session/attachments/what-are-attachments) we have introduced the `AttachmentsFor` method that can be used in indexing functions. This method will return a list of attachments in given document with basic information like `Name` or `Size` about every one of them.

## Creating Indexes

The `AttachmentsFor` method returns all of the attachments for a document passed as the first argument.

{CODE-TABS}
{CODE-TAB:java:AttachmentsFor syntax@Indexes\IndexingAttachments.java /}
{CODE-TAB:java:AttachmentName result@Indexes\IndexingAttachments.java /}
{CODE-TABS/}

{CODE:java index@Indexes\IndexingAttachments.java /}

## Example

{CODE:java query1@Indexes\IndexingAttachments.java /}

## Related Articles

### Client API

- [What are Attachments](../client-api/session/attachments/what-are-attachments)
