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

{NOTE: }

**The index**:

---

* To index attachments' details, call `attachmentsFor()` within the index definition.  

* `attachmentsFor()` provides access to the **name**, **size**, **hash**, and **content-type** of each attachment a document has.
  These details can then be used when defining the index-fields.

* To index attachments' content, see the examples below. 

{CODE:nodejs index_1@documentExtensions\attachments\indexAttachments.js /}

{NOTE/}

{NOTE: }

**Query the Index**:

---

You can now query for Employee documents based on their attachments details.

{CODE-TABS}
{CODE-TAB:nodejs:Query query_1@documentExtensions\attachments\indexAttachments.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAttachmentDetails"
where attachmentNames == "photo.jpg" and attachmentSizes > 20000
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: Index details & content - by attachment name}

{NOTE: }

**Sample data**:

---

* Each Employee document in the Northwind sample data already includes a _photo.jpg_ attachment.

* For all following examples, let's store a textual attachment (file _notes.txt_) on 3 documents  
  in the 'Employees' collection.

{CODE:nodejs store_attachments@documentExtensions\attachments\indexAttachments.js /}

{NOTE/}

{NOTE: }

**The index**:

---

* To index the **details & content** for a specific attachment, call `loadAttachment()` within the index definition.  
 
* In addition to accessing the attachment details, `loadAttachment()` provides access to the attachment's content, 
  which can be used when defining the index-fields.

{CODE:nodejs index_2@documentExtensions\attachments\indexAttachments.js /}

{NOTE/}

{NOTE: }

**Query the Index**:

---

You can now query for Employee documents based on their attachment details and/or its content.

{CODE-TABS}
{CODE-TAB:nodejs:Query query_2@documentExtensions\attachments\indexAttachments.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAttachment"
where search(attachmentContent, "Colorado Dallas")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Index details & content - all attachments}

{NOTE: }

**The index**:

---

* Use `loadAttachments()` to be able to index the **details & content** of ALL attachments.

* Note how the index example below is employing the [Fanout index](../../indexes/indexing-nested-data#fanout-index---multiple-index-entries-per-document) pattern.

{CODE:nodejs index_3@documentExtensions\attachments\indexAttachments.js /}

{NOTE/}

{NOTE: }

**Query the Index**:

---

{CODE-TABS}
{CODE-TAB:nodejs:Query query_3@documentExtensions\attachments\indexAttachments.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByAllAttachments"
where attachmentSize > 20000 or search(attachmentContent, "Colorado Dallas")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: Leveraging indexed attachments}

* Access to the indexed attachment content opens the door to many different applications,  
  including many that can be integrated directly into RavenDB.

* This [blog post](https://ayende.com/blog/192001-B/using-machine-learning-with-ravendb) demonstrates 
  how image recognition can be applied to indexed attachments using the [additional sources](../../indexes/extending-indexes) feature. 
  The resulting index allows filtering and querying based on image content.

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@documentExtensions\attachments\indexAttachments.js /}

| Parameter    | Type     | Description                                             |
|--------------|----------|---------------------------------------------------------|
| **document** | `object` | The document whose attachments details you want to load |

{CODE:nodejs syntax_2@documentExtensions\attachments\indexAttachments.js /}

---

{CODE:nodejs syntax_3@documentExtensions\attachments\indexAttachments.js /}

| Parameter           | Type      | Description                                    |
|---------------------|-----------|------------------------------------------------|
| **document**        | `object`  | The document whose attachment you want to load |
| **attachmentName**  | `string`  | The name of the attachment to load             |

{CODE:nodejs syntax_4@documentExtensions\attachments\indexAttachments.js /}

---

{CODE:nodejs syntax_5@documentExtensions\attachments\indexAttachments.js /}

{PANEL/}

## Related Articles

### Document Extensions

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  

### Indexes

- [What are Indexes](../../indexes/what-are-indexes)
