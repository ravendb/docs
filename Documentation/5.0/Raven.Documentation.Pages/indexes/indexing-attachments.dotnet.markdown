# Indexes: Indexing Attachments

* The `AttachmentsFor` method returns a list of 
  [attachments](../document-extensions/attachments/what-are-attachments) 
  in a given document as well as basic information like `Name` or `Size` 
  about each of them.

* For a more thorough explanation including syntax, please read our 
  [Document Extensions article on indexing attachments](../document-extensions/attachments/indexing). 

{CODE-TABS}
{CODE-TAB:csharp:AttachmentsFor syntax@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:AttachmentName result@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

## Creating an index using `AttachmentsFor()`

The `AttachmentsFor` method is available in `AbstractIndexCreationTask`.

{CODE:csharp index@Indexes\IndexingAttachments.cs /}

## Querying the index

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:Async query2@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query3@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

## Related Articles

### Client API

- [What are Attachments](../document-extensions/attachments/what-are-attachments)
