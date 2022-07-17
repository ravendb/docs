# Document Extensions Overview
---

{NOTE: }

* Document extensions are data entities associated with documents.  

* Document extensions are stored separately from their parent documents and are referenced in the parent documents' meta-data.
  Separate storage ensures that most modifications of an extension don't change the parent document,
  thus reducing trips to the server and activation of tasks such as ETL that are triggered by document changes.
   * Actions such as creating or deleting an extension do change the parent documents' meta-data.

* In this page:  
   * [The Document Extensions](../document-extensions/overview-extensions#the-document-extensions)  
   * [Studio Document Extension Views](../document-extensions/overview-extensions#studio-document-extension-views)  

{NOTE/}

---

{PANEL: The Document Extensions}

* [Counters](../document-extensions/counters/overview)  
  RavenDB's distributed counters are numeric data variables that can be added to documents and used
  for various counting tasks.

* [Attachments](../document-extensions/attachments/what-are-attachments)  
  Attachments are images, PDF files, videos, and other types of binary streams that are associated with parent documents.

* [Time Series](../document-extensions/timeseries/overview)  
  Time series are vectors of data that collect values over time, store the values consecutively across the cluster,
  and manage the collected data with high efficiency and performance.

* [Revisions](../document-extensions/revisions/overview)  
  Document Revisions are snapshots of documents and their extensions that can be created to give access to a document's history.

{PANEL/}


{PANEL: Studio Document Extension Views}

#### Document Extensions Flags

![Document Extensions in Collections View](images/extensions-collections-view.png "Document Extensions in Collections View")

1. **Documents Tab**  
   Select to view document options.
2. **Collection**  
   Select a documents collection. 
3. **Extensions**  
   View which types of extensions are added to the documents.  
   ![Document Extensions Icons](images/extensions-logos.png "Document Extensions Icons")

---

#### Document Extensions View

Select a specific document to manage the extensions.  

In Studio > click Documents Tab > select specific Collection > select specific DocumentID to navigate to the following view:  

![Managing Document Extensions in Studio](images/extensions-managing-single-doc.png "Managing Document Extensions in Studio")

1. Attachments Settings
2. Counters Settings
3. Time-Series Settings
4. Revisions Settings

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

