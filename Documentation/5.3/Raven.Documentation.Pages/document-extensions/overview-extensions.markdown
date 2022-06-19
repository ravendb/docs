# Document Extensions Overview
---

{NOTE: }

* Document extensions are data or attachments that are associated with documents.  

* A document extension is associated with a specific document but is stored separately.  
  This greatly reduces the cost of using extensions.  
   * Storing extensions separately adds efficiency because you don't need to load the document extensions 
     every time you load a document. e.g. The images and videos attached to a document don't need to load when you 
     want to access a document.
   * You can modify an existing counter or time series without loading the associated document.  
   * On the other hand, an action on an extension that would change the document meta-data, such as creating 
     or deleting an extension, would cause the associated document to load, modify, and save.  
     e.g. Adding images or a new counter would modify the meta-data in the document.

* In this page:  
  * [Counters](../document-extensions/overview-extensions#counters)  
  * [Attachments](../document-extensions/overview-extensions#attachments)  
  * [Time Series](../document-extensions/overview-extensions#time-series)  
  * [Revisions](../document-extensions/overview-extensions#revisions)  
  * [Studio Document Extension Views](../document-extensions/overview-extensions#studio-document-extension-views)  

{NOTE/}

---

{PANEL: Counters}

[Counters](../document-extensions/counters/overview) are integers that can be used for [situations ](../document-extensions/counters/overview#convenient-counting-mechanism) 
that involve incrementing.  

Counters [can be indexed](../document-extensions/counters/indexing) and their values can be calculated in 
[static indexes](../indexes/creating-and-deploying) 
to save query time. 

Their values are distributed between the cluster nodes and are not stored inside the documents. 
This allows any node to rapidly modify it 
without the need to coordinate with other nodes.  
It means fewer trips to the server, fast response times, and low resource usage.  

[Counters can trigger and interact with other features](../document-extensions/counters/counters-and-other-features#counters-and-other-features).

You can have numerous counters associated with one document, but having many counters on a document is a sign of potentially
inefficient [document modeling](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling).

{INFO: }

They can be managed via [Studio](../studio/database/document-extensions/counters#counters)
or with the following API on counters:

* [Creating and Modifying](../document-extensions/counters/create-or-modify)
* [Indexing](../document-extensions/counters/indexing)
* [Deleting](../document-extensions/counters/delete)
* [Getting Values](../document-extensions/counters/retrieve-counter-values)
* [Integrating with Other RavenDB Features](../document-extensions/counters/counters-and-other-features)
* [How Counters Behave in Clusters](../document-extensions/counters/counters-in-clusters)

{INFO/}

{PANEL/}


{PANEL: Attachments}

[Attachments](../document-extensions/attachments/what-are-attachments) 
are binary streams that are associated with specified documents but stored separately.  

Each document can have numerous attachments of various types associated with it.  

The attachments' names (e.g. video.mp4), content type (e.g. image/png), and other info such as hash and size 
are stored in the document metadata.  

Referencing them in the metadata allows you to query attachments like documents by specifying
the document ID and the attachment name.  

Adding or deleting attachments changes the document metadata, thus triggering any tasks that respond to document changes
such as revisions, ETL, or indexing.

{INFO: }

Attachments can be managed via [Studio](../studio/database/documents/document-view#document-view---actions)
or with the following API on attachments:

* [Storing](../document-extensions/attachments/storing)
* [Loading](../document-extensions/attachments/loading)
* [Deleting](../document-extensions/attachments/deleting)
* [Copying, Moving, & Renaming](../document-extensions/attachments/copying-moving-renaming)
* [Indexing](../document-extensions/attachments/indexing)
* [Bulk Insert](../document-extensions/attachments/bulk-insert)

{INFO/}

{PANEL/}


{PANEL: Time Series}

[Time Series](../document-extensions/timeseries/overview) store continuous streams of values over time, 
store them consecutively, and enable you to manage them with high efficiency and performance.  

[Incremental Time Series](../document-extensions/timeseries/incremental-time-series/overview) 
is a special type of time series that allows you to handle time series values as counters.

Time series data can be aggregated and queried for statistical insights over time.  

Each node in a database group can modify a time series without causing conflicts because the data is stored 
separately from the associated documents and each entry is `DateTime` [stamped](../document-extensions/timeseries/overview#timestamps).  
Each entry can hold [multiple](../document-extensions/timeseries/overview#values) 
`double` values and can be [tagged](../document-extensions/timeseries/overview#tags) 
with `strings` for added index or query capabilities.  

Time series data is [compressed](../document-extensions/timeseries/design#compression) 
  and [segmented](../document-extensions/timeseries/overview#time-series-segments) 
  to minimize storage usage and transmission time.  

{INFO: }

Time series can be managed via [Studio](../studio/database/document-extensions/time-series).  

It has a wealth of features that can be managed with [API](../document-extensions/timeseries/client-api/overview) and can be:

* [Queried](../document-extensions/timeseries/querying/overview-and-syntax)
* [Indexed](../document-extensions/timeseries/indexing)
* [Set to Interact with Other Features](../document-extensions/timeseries/time-series-and-other-features)

{INFO/}

{PANEL/}


{PANEL: Revisions}

[Document Revisions](../document-extensions/revisions/overview) 
are snapshots of documents and their extensions that can be: 

* [Created manually](../document-extensions/revisions/overview#force-revision-creation) 
  by forcing a creation 
* [Activated automatically](../document-extensions/revisions/overview#defining-a-revisions-configuration) 
  upon document creation, modification, or deletion

They can be used to track changes to a document over time, revert the current version of a document 
or a whole set of documents to a previous version, restore accidentally deleted documents, and more.

They [must be explicitly activated](../document-extensions/revisions/overview#revisions-configuration) 
to be created automatically. 

In case any document is conflicted and the conflict is not resolved, 
the conflicting document variants are stored as [conflict revisions](../document-extensions/revisions/client-api/operations/conflict-revisions-configuration) 
to their original document while the document itself is removed.  
With [clean document modeling](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling), 
this feature is not likely to be needed to repair conflicts, 
but if needed, revisions can be instrumental in [remediating inconsistency issues](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#transaction-atomicity-and-replication). 

{INFO: }
Revisions can be managed via [Studio](../studio/database/document-extensions/revisions)  
or [Client API](../document-extensions/revisions/client-api/overview).
{INFO/}

{PANEL/}


{PANEL: Studio Document Extension Views}

* [Seeing which types of extensions are added to sets of documents](../document-extensions/overview-extensions#seeing-which-types-of-extensions-are-added-to-sets-of-documents)  
* [Managing extensions in specific documents](../document-extensions/overview-extensions#managing-extensions-in-specific-documents)  

#### Seeing which types of extensions are added to sets of documents

![Document Extensions in Collections View](images/extensions-collections-view.png "Document Extensions in Collections View")

1. **Documents Tab**  
   Select to view document options.
2. **Collection**  
   Select to narrow set of documents to a specified collection. 
3. **Extensions**  
   View which types of extensions are added to the documents.  
   If you want to manage the extensions, see [managing extensions in specific documents](../document-extensions/overview-extensions#managing-extensions-in-specific-documents).  

#### Managing extensions in specific documents

Select a specific document to manage the extensions.  

In Studio > click Documents Tab > select specific Collection > select specific DocumentID to navigate to the following view:  

![Managing Document Extensions in Studio](images/extensions-managing-single-doc.png "Managing Document Extensions in Studio")

1. **Extension Type**  
   Select the type of document extension to view and modify it.
2. **Search**  
   (Optional)  You can search the list of this type of extension in case there are many saved for this document.  
   e.g If there are many images associated with one document.
3. **Add**  
   Click to add a new document extension.  
4. **Details**  
   See overview details about the document extension.  
   The details displayed are varied depending on the extension type.  

   {PANEL/}

## Related articles

**Counters**:  

- [Counters](../document-extensions/counters/overview)
- [Indexing with Counters](../document-extensions/counters/indexing)
- [Counters With Other Features](../document-extensions/counters/counters-and-other-features#counters-and-other-features)

**Attachments**:  

- [Attachments](../document-extensions/attachments/what-are-attachments)

**Time Series**:  

- [Time Series](../document-extensions/timeseries/overview)
- [Incremental Time Series](../document-extensions/timeseries/incremental-time-series/overview)

**Revisions**:  

- [Document Revisions](../document-extensions/revisions/overview)
- [Conflict Revisions](../document-extensions/revisions/client-api/operations/conflict-revisions-configuration)

