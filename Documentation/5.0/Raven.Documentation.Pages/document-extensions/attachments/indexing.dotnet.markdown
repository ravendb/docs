# Indexes: Indexing Attachments

The `AttachmentsFor` method returns a list of [attachments](../../document-extensions/attachments/what-are-attachments) in a given document as well as basic information like `Name` or `Size` about each of them.

{CODE-TABS}
{CODE-TAB:csharp:AttachmentsFor syntax@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:AttachmentName result@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

## Creating an index using `AttachmentsFor()`

The `AttachmentsFor` method is available in `AbstractIndexCreationTask`.

{CODE:csharp index@DocumentExtensions\Attachments\IndexingAttachments.cs /}

## Querying the index

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Async query2@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query3@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)
