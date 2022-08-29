# Attachments
---

{NOTE: }

* Attachments are binary streams that are associated with specified documents but stored separately.  

* Each document can have multiple attachments of various types associated with it.  

* Adding or deleting attachments changes the document metadata, thus triggering any tasks that respond to document changes
  such as revisions, ETL, or indexing.

* Learn more about attachments in the article [What are Attachments](../../../document-extensions/attachments/what-are-attachments).

* In this page:  
   * [Add or Delete Attachments via Studio](../../../studio/database/document-extensions/attachments#add-or-delete-attachments-via-studio)
   * [Index and Query by Attachments via Studio](../../../studio/database/document-extensions/attachments#index-and-query-by-attachments-via-studio)

{NOTE/}

---

{PANEL: Add or Delete Attachments via Studio}

The following view shows how to add or delete attachments in the Studio individual document view.

![Add or Delete Attachments via Studio](images/attachments/add-or-delete-attachments-via-studio.png "Add or Delete Attachments via Studio")

1. **Attachments**  
   Select this document extensions tab to search for, add, or delete attachments.  
   The tab displays the number of attachments referenced by this document.  
2. **Search**  
   Use the search bar to find specific attachments.  
3. **Add Attachment**  
   Click to add as many images, videos, PDFs, and other types of attachments as you wish.  
4. **Details**  
   Displays names and memory used by each attachment.  
   Click the name to download the attachment.  
5. **Delete**  
   Click to delete specific attachments. 


{PANEL/}

{PANEL: Index and Query by Attachments via Studio}

### Index Definition

The following index definition will index various attachment details 
that have been [stored in the document metadata](../../../document-extensions/attachments/what-are-attachments#example-ii---including-attachment-metadata-to-be-able-to-query-attachments).

![Index by Attachment Details](images/attachments/index-attachment-details.png "Index by Attachment Details")

1. **Index Name**  
   This is the name of the index that must be explicitly [called in the query](../../../studio/database/document-extensions/attachments#query-definition) 
   to query this index.  
2. **Map Index Definition**  
   The index is defined by selecting details to make available for the queries.  
   * The attachment metadata fields that are indexed in this example are ` AttachmentNames`, `AttachmentContentTypes`, `AttachmentHashes`, and `AttachmentSizes`.  
   * See our Studio [Map-Index article](../../../studio/database/indexes/create-map-index) 
     to learn more about how to define indexes in Studio.

**Syntax Sample for Indexing in Studio:**  

{CODE-BLOCK:sql}
docs.Employees.Select(employee => new {
    employee = employee,
    attachments = this.AttachmentsFor(employee)
}).Select(this0 => new {
    AttachmentNames = Enumerable.ToArray(this0.attachments.Select(x => x.Name)),
    AttachmentContentTypes = Enumerable.ToArray(this0.attachments.Select(x0 => x0.ContentType)),
    AttachmentHashes = Enumerable.ToArray(this0.attachments.Select(x1 => x1.Hash)),
    AttachmentSizes = Enumerable.ToArray(this0.attachments.Select(x2 => x2.Size))
})
{CODE-BLOCK/}

{NOTE: }
For the API documentation article including LINQ, Java, and NodeJS syntax, see [Indexing Attachments](../../../document-extensions/attachments/indexing).
{NOTE/}

---

### Query Definition

The index that we've built in the previous section can be called in the following query.

![Query by Index Name](images/attachments/query-by-index-name.png "Query by Index Name")

1. **Query Definition**  
   Notice that to use the index defined above, you must call `from index` and the index name.  
   In this example, the syntax to call the index is `from index 'Employees/ByAttachmentDetails'`  
   The `where` keyword enables you to filter the results.  
   Here we are querying an index, so you can filter according to the fields that were indexed above 
   e.g. `AttachmentNames`, `AttachmentContentTypes`, etc.
     {NOTE: }
     [Studio queries use RQL syntax](../../../studio/database/queries/query-view).  
     {NOTE/}
2. **Results**  
   This area shows the index used for the query and lists the documents that match the query definition.  

{PANEL/}

## Related articles

**API - Attachments**:  

* [Storing Attachments](../../../document-extensions/attachments/storing)
* [Loading Attachments](../../../document-extensions/attachments/loading)
* [Deleting Attachments](../../../document-extensions/attachments/deleting)
* [Copying, Moving, & Renaming Attachments](../../../document-extensions/attachments/copying-moving-renaming)
* [Indexing Attachments](../../../document-extensions/attachments/indexing)
* [Bulk Inserting Attachments](../../../document-extensions/attachments/bulk-insert)

**Querying**  

* [Studio Query View](../../../studio/database/queries/query-view)
* [What is RQL](../../../indexes/querying/what-is-rql)
* [Querying Basics](../../../indexes/querying/basics)

---

**Code Walkthrough**

- [Store Attachment](https://demo.ravendb.net/demos/csharp/attachments/store-attachment)
- [Load Attachment](https://demo.ravendb.net/demos/csharp/attachments/load-attachment)
- [Index Attachment Details](https://demo.ravendb.net/demos/csharp/attachments/index-attachment-details)
