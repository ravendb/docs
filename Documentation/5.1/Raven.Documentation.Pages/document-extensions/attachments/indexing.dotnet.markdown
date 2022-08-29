# Indexes: Indexing Attachments
---

{NOTE: }

* Attachments can be referenced and loaded using the `AttachmentsFor` and 
`LoadAttachment`/`LoadAttachments` methods.  

* Auto-indexes for attachments are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/attachments/indexing#syntax)  
  * [Examples](../../document-extensions/attachments/indexing#examples)  
     * [I - Indexes with `AttachmentsFor()`](../../document-extensions/attachments/indexing#i---indexes-with-)  
     * [II - Indexes with `LoadAttachment()`](../../document-extensions/attachments/indexing#ii---indexes-with-)  
     * [III - Indexes with `LoadAttachments()`](../../document-extensions/attachments/indexing#iii---indexes-with-)  
     * [IV - Querying the Index](../../document-extensions/attachments/indexing#iv---querying-the-index)  

{NOTE/}

---

{PANEL: Syntax}

### Using `AttachmentsFor()`

The `AttachmentsFor` method returns information about each attachment that extends 
a specified document, including their names, sizes, and content type.  

{CODE-TABS}
{CODE-TAB:csharp:Method syntax@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Result result@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

The `AttachmentsFor` method is available in `AbstractIndexCreationTask`.

---

### Using `LoadAttachment()`/`LoadAttachments()`

`LoadAttachment()` loads an attachment to the index by document and attachment name.  
`LoadAttachments()` loads all the attachments of a given document.  

{CODE:csharp syntax_2@DocumentExtensions\Attachments\IndexingAttachments.cs /}

| Parameter | Type | Description |
| - | - | - |
| **doc** | A server-side document, an entity | The document whose attachments you want to load |
| **name** | `string` | The name of the attachment you want to load |

---

#### `GetContentAs` Methods

To access the attachment content itself, use `GetContentAsStream()`. To 
convert the content into a `string`, use `GetContentAsString()` with 
the desired character encoding.  

{CODE-BLOCK: csharp}
public Stream GetContentAsStream();

public string GetContentAsString(Encoding encoding);

public string GetContentAsString(); // Default: UTF-8
{CODE-BLOCK/}

---

{PANEL/}

{PANEL: Examples}

{INFO: Applications for Attachment Content: Machine Learning}
Access to the attachment content opens the door to many different 
applications.  

For example, [see how](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb) 
machine learning image recognition can be added to an index using the [additional sources](../../indexes/extending-indexes#including-additional-sources-from-client-code) 
feature. The resulting index allows filtering and querying based on image content.  
{INFO/}

#### I - Indexes with `AttachmentsFor()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax AttFor_index_LINQ@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax AttFor_index_JS@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

#### II - Indexes with `LoadAttachment()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax LoadAtt_index_LINQ@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax LoadAtt_index_JS@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

#### III - Indexes with `LoadAttachments()`

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax LoadAtts_index_LINQ@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JavaScript-syntax LoadAtts_index_JS@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

#### IV - Querying the Index

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Async query2@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query3@DocumentExtensions\Attachments\IndexingAttachments.cs /}
{CODE-TABS/}

{PANEL/}


## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)

### Studio

- [Studio - Attachments](../../studio/database/document-extensions/attachments)

---

### Code Walkthrough

- [Index Attachment Details](https://demo.ravendb.net/demos/csharp/attachments/index-attachment-details)

