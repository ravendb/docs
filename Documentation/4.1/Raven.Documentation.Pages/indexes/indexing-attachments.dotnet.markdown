# Indexes: Indexing Attachments

To address the need of indexing (and searching for) the [attachments](../client-api/session/attachments/what-are-attachments) we have introduced the `AttachmentsFor` method that can be used in indexing functions. This method will return a list of attachments in given document with basic information like `Name` or `Size` about every one of them.

## Creating Indexes

The `AttachmentsFor` method available in `AbstractIndexCreationTask` returns all of the attachments for a document passed as the first argument.

{CODE-TABS}
{CODE-TAB:csharp:AttachmentsFor syntax@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:AttachmentName result@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Index index@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:Async query2@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query3@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

## Related Articles

### Client API

- [What are Attachments](../client-api/session/attachments/what-are-attachments)
