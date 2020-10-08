# Indexes: Indexing Attachments
---

{NOTE: }

* Attachments can be referenced and loaded using the `AttachmentsFor` and 
`LoadAttachment`/`LoadAttachments` methods.  

* Auto-indexes for attachments are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/counters/indexing#syntax)  
  * [Examples](../../document-extensions/counters/indexing#examples)  

{NOTE/}

---

{PANEL: Syntax}

#### Using AttachmentsFor()

The `AttachmentsFor` method returns information about each attachment that extends 
a specified document, including their names, sizes, and content type.  

{CODE-TABS}
{CODE-TAB:csharp:Method syntax@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:Result result@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

The `AttachmentsFor` method is available in `AbstractIndexCreationTask`.

#### Using LoadAttachment()/LoadAttachments()

`LoadAttachment()` loads an attachment to the index by document and attachment name.  
`LoadAttachments()` loads all the attachments of a given document.  

{CODE:csharp syntax_2@Indexes\IndexingAttachments.cs /}

| Parameter | Type | Description |
| - | - | - |
| **doc** | A server-side document, an entity | The document whose attachments you want to load |
| **name** | `string` | The name of the attachment you want to load |

{PANEL/}

{PANEL: Examples}

#### Indexes with `AttachmentsFor()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax AttFor_index_LINQ@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax AttFor_index_JS@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

#### Indexes with `LoadAttachment()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax LoadAtt_index_LINQ@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax LoadAtt_index_JS@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

#### Indexes with `LoadAttachments()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax LoadAtts_index_LINQ@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax LoadAtts_index_JS@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

#### Querying the Index

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:Async query2@Indexes\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query3@Indexes\IndexingAttachments.cs /}
{CODE-TABS/}

{PANEL/}


## Related Articles

### Client API

- [What are Attachments](../client-api/session/attachments/what-are-attachments)
