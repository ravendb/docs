# Indexes: Indexing Attachments
---

{NOTE: }

* Attachments can be referenced and loaded using the `AttachmentsFor` and 
`LoadAttachment`/`LoadAttachments` methods.  

* Auto-indexes for attachments are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/attachments/indexing#syntax)  
  * [Examples](../../document-extensions/attachments/indexing#examples)  

{NOTE/}

---

{PANEL: Syntax}

### Using AttachmentsFor()

The `AttachmentsFor` method returns information about each attachment that extends 
a specified document, including their names, sizes, and content type. write index definition as string.

{CODE-TABS}
{CODE-TAB:java:Method syntax@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TAB:java:AttachmentName result@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

The `AttachmentsFor` method is available in `AbstractIndexCreationTask`.

### Using LoadAttachment()/LoadAttachments()

`LoadAttachment()` loads an attachment to the index by document and attachment name.  
`LoadAttachments()` loads all the attachments of a given document.  

{CODE:java syntax_2@DocumentExtensions\Attachments\IndexingAttachments.java /}

| Parameter | Type | Description |
| - | - | - |
| **doc** | A server-side document, an entity | The document whose attachments you want to load |
| **name** | `String` | The name of the attachment you want to load |

#### GetContentAs Methods

To access the attachment content itself, use `GetContentAsStream()`. To 
convert the content into a `string`, use `GetContentAsString()` with 
the desired character encoding.  

{CODE-BLOCK: csharp}
public Stream GetContentAsStream();

public string GetContentAsString(Encoding encoding);

public string GetContentAsString(); // Default: UTF-8
{CODE-BLOCK/}

{INFO: Applications for Attachment Content: Machine Learning}
Access to the attachment content opens the door to many different 
applications, including many that can be integrated directly into 
RavenDB.  

In this [blog post](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb), 
Oren Eini demonstrates how machine learning image recognition can be 
added to an index using the [additional sources](../../indexes/extending-indexes) 
feature. The resulting index allows filtering and querying based on 
image content.  
{INFO/}

{PANEL/}

{PANEL: Examples}

#### Indexes with `AttachmentsFor()`

{CODE-TABS}
{CODE-TAB:java:JavaScript-syntax AttFor_index_JS@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

#### Indexes with `LoadAttachment()`

{CODE-TABS}
{CODE-TAB:java:JavaScript-syntax LoadAtt_index_JS@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

#### Indexes with `LoadAttachments()`

{CODE-TABS}
{CODE-TAB:java:JavaScript-syntax LoadAtts_index_JS@DocumentExtensions\Attachments\IndexingAttachments.java /}
{CODE-TABS/}

#### Querying the Index


{CODE:java query1@DocumentExtensions\Attachments\IndexingAttachments.java /}

{PANEL/}


## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)
