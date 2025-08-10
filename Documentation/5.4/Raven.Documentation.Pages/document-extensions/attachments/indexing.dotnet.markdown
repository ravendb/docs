# Index Attachments
---

{NOTE: }

* Indexing attachments allows you to query for documents based on their attachments' details and content.

* **Static indexes**:  
  Both attachments' details and content can be indexed within a static-index definition.

* **Auto-indexes**:  
  Auto-indexing attachments via dynamic queries is not available at this time.

* In this page:
    * [Index attachments details](../../document-extensions/attachments/indexing#index-attachments-details)
    * [Index details & content - by attachment name](../../document-extensions/attachments/indexing#index-details-&-content---by-attachment-name)
    * [Index details & content - all attachments](../../document-extensions/attachments/indexing#index-details-&-content---all-attachments)
    * [Leveraging indexed attachments](../../document-extensions/attachments/indexing#leveraging-indexed-attachments)
    * [Syntax](../../document-extensions/attachments/indexing#syntax)

{NOTE/}

---

{PANEL: Index attachments details}

**The index**:

* To index **attachments' details**, call `AttachmentsFor()` within the index definition.

* `AttachmentsFor()` provides access to the **name**, **size**, **hash**, and **content-type** of each attachment a document has.
  These details can then be used when defining the index-fields.
  Once the index is deployed, you can query the index to find Employee documents based on these attachment properties.

* To index **attachments' content**, see the examples [below](../../document-extensions/attachments/indexing#index-details-&-content---by-attachment-name).

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_1@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JS_index index_1_js@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

**Query the Index**:

You can now query for Employee documents based on their attachments details.

{CODE-TABS}
{CODE-TAB:csharp:Query query_1@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Query_async query_1_async@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_1_documentQuery@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAttachmentDetails"
where AttachmentNames == "photo.jpg" and AttachmentSizes > 20000
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Index details & content - by attachment name}

{CONTENT-FRAME: }

**Sample data**:

* Each Employee document in RavenDB's sample data already includes a _photo.jpg_ attachment.

* For all following examples, let's store a textual attachment (file _notes.txt_) on 3 documents in the 'Employees' collection.

{CODE:csharp store_attachments@documentExtensions\attachments\IndexingAttachments.cs /}

{CONTENT-FRAME/}

---

**The index**:

* To index the **details & content** for a specific attachment, call `LoadAttachment()` within the index definition.

* In addition to accessing the attachment details, `LoadAttachment()` provides access to the attachment's content,
  which can be used when defining the index-fields.

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_2@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JS_index index_2_js@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

**Query the Index**:

You can now query for Employee documents based on their attachment details and/or its content.

{CODE-TABS}
{CODE-TAB:csharp:Query query_2@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Query_async query_2_async@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_2_documentQuery@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAttachment"
where search(AttachmentContent, "Colorado Dallas")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Index details & content - all attachments}

**The index**:

* Use `LoadAttachments()` to be able to index the **details & content** of ALL attachments.

* Note how the index example below is employing the [Fanout index](../../indexes/indexing-nested-data#fanout-index---multiple-index-entries-per-document) pattern.

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_3@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:JS_index index_3_js@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TABS/}

---

**Query the Index**:

{CODE-TABS}
{CODE-TAB:csharp:Query query_3@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:Query_async query_3_async@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB:csharp:DocumentQuery query_3_documentQuery@documentExtensions\attachments\IndexingAttachments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAllAttachments"
where search(AttachmentContent, "Colorado Dallas") or AttachmentSize > 20000 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Leveraging indexed attachments}

* Access to the indexed attachment content opens the door to many different applications,  
  including many that can be integrated directly into RavenDB.

* This [blog post](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb) demonstrates
  how image recognition can be applied to indexed attachments using the [additional sources](../../indexes/extending-indexes) feature.
  The resulting index allows filtering and querying based on image content.

{PANEL/}

{PANEL: Syntax}

#### `AttachmentsFor`

{CODE:csharp syntax_1@documentExtensions\attachments\IndexingAttachments.cs /}

| Parameter    | Type     | Description                                                     |
|--------------|----------|-----------------------------------------------------------------|
| **document** | `object` | The document object whose attachments details you want to load. |

{CODE:csharp attachment_details@documentExtensions\attachments\IndexingAttachments.cs /}

---

#### `LoadAttachment`

{CODE:csharp syntax_2@documentExtensions\attachments\IndexingAttachments.cs /}

| Parameter           | Type      | Description                                     |
|---------------------|-----------|-------------------------------------------------|
| **document**        | `object`  | The document whose attachment you want to load. |
| **attachmentName**  | `string`  | The name of the attachment to load.             |

{CODE:csharp attachment_object@documentExtensions\attachments\IndexingAttachments.cs /}

---

#### `LoadAttachments`

{CODE:csharp syntax_3@documentExtensions\attachments\IndexingAttachments.cs /}

{PANEL/}

## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)
