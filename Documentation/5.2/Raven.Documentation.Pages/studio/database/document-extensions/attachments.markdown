# Attachments
---

{NOTE: }

* [Attachments](../../../document-extensions/attachments/what-are-attachments) 
  are binary streams that are associated with specified documents but stored separately.  

* Each document can have numerous attachments of various types associated with it.  

* Adding or deleting attachments changes the document metadata, thus triggering any tasks that respond to document changes
  such as revisions, ETL, or indexing.

* The attachments' names (e.g. video.mp4), content type (e.g. image/png), and other info such as DateTime, hash, and size 
  [can be stored in the document metadata](../../../document-extensions/attachments/what-are-attachments#example-ii) via [API](../../../studio/database/document-extensions/attachments#api---attachments).  
   * Referencing them in the metadata allows you to query attachments like you would query documents by specifying
     the document ID and the attachment name.  

* In this page:  
   * [Add or Delete Attachments via Studio](../../../studio/database/document-extensions/attachments#add-or-delete-attachments-via-studio)
   * [Index and Query by Attachments via Studio](../../../studio/database/document-extensions/attachments#index-and-query-by-attachments-via-studio)
   * [API - Attachments](../../../studio/database/document-extensions/attachments#api---attachments)

{NOTE/}

---

{PANEL: Add or Delete Attachments via Studio}

![Add or Delete Attachments via Studio](images/attachments/add-or-delete-attachments-via-studio.png "Add or Delete Attachments via Studio")

1. **"HasAttachments"**  
   @metadata flag in the .json document is a reference to the attachments that are stored separately.  
2. **Attachments**  
   Select this document extensions tab to search for, add, or delete attachments.  
   The tab displays the number of attachments referenced by this document.  
3. **Search**  
   Use the search bar to find specific attachments.  
4. **Add Attachment**  
   Click to add as many images, videos, PDFs, and other types of attachments as you wish.  
5. **Details**  
   Displays names and memory used by each attachment.  
   Click the name to download the attachment.  
6. **Delete**  
   Click to delete specific attachments. 


{PANEL/}

{PANEL: Index and Query by Attachments via Studio}

For the API documentation article, see [Indexing Attachments](../../../document-extensions/attachments/indexing).

#### Index Definition

![Index by Attachment Details](images/attachments/index-attachment-details.png "Index by Attachment Details")

1. **Index Name**  
   This is the name of the index that must be explicitly called in the query to query this index.
2. **Map Index Definition**  
   Define the index by specifying details to make available for the queries.  
   * See the [Code Walkthrough](https://demo.ravendb.net/demos/csharp/attachments/index-attachment-details#) 
     to copy and play with the code sample used in this example.  
   * See our Studio [Map-Index article](../../../studio/database/indexes/create-map-index) 
     to learn more about how to define indexes in Studio.

---

#### Query Definition

![Query by Index Name](images/attachments/query-by-index-name.png "Query by Index Name")

1. **Query Definition**  
   Notice that to use the index defined above, you must call `from index` and the index name.  
   The `where` keyword enables you to filter the results. If you always want to filter with the same conditions, 
   you can also use `where` in the index definition to narrow the index terms that the query must scan. This can save 
   query-time but narrows the terms available to query.
   * Note that [RQL is used in Studio](../../../indexes/querying/what-is-rql).  
     For more information about querying see the article [Querying Basics](../../../indexes/querying/basics).  
2. **Results**  
   This area shows the index used for the query and lists the documents that match the query definition.  
   Learn more about [querying in Studio](../../../studio/database/queries/query-view).

{PANEL/}

{PANEL: API - Attachments}

Attachments can be managed via [Studio](../../../studio/database/documents/document-view#document-view---actions)
or with the following API on attachments:

* [Storing](../../../document-extensions/attachments/storing)
* [Loading](../../../document-extensions/attachments/loading)
* [Deleting](../../../document-extensions/attachments/deleting)
* [Copying, Moving, & Renaming](../../../document-extensions/attachments/copying-moving-renaming)
* [Indexing](../../../document-extensions/attachments/indexing)
* [Bulk Insert](../../../document-extensions/attachments/bulk-insert)

{PANEL/}

## Related articles

**API - Attachments**:  

* [Storing](../../../document-extensions/attachments/storing)
* [Loading](../../../document-extensions/attachments/loading)
* [Deleting](../../../document-extensions/attachments/deleting)
* [Copying, Moving, & Renaming](../../../document-extensions/attachments/copying-moving-renaming)
* [Indexing](../../../document-extensions/attachments/indexing)
* [Bulk Insert](../../../document-extensions/attachments/bulk-insert)

**Querying**  

* [What is RQL](../../../indexes/querying/what-is-rql)

---

**Code Walkthrough**

- [Store Attachment](https://demo.ravendb.net/demos/csharp/attachments/store-attachment)
- [Load Attachment](https://demo.ravendb.net/demos/csharp/attachments/load-attachment)
- [Index Attachment Details](https://demo.ravendb.net/demos/csharp/attachments/index-attachment-details)
