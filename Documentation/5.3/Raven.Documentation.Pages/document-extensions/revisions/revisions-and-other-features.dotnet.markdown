# Revisions: Revisions and Other Features

---

{NOTE: }

* This page describes relationships between Revisions and other RavenDB features, including -  
   * How revisions are supported by other features  
   * How revisions creation is triggered by other features  

* In this page:  
   * [Revisions and Counters](../../document-extensions/revisions/revisions-and-other-features#revisions-and-counters)  
   * [Revisions and Time Series](../../document-extensions/revisions/revisions-and-other-features#revisions-and-time-series)  
   * [Revisions and Attachments](../../document-extensions/revisions/revisions-and-other-features#revisions-and-attachments)  
   * [Revisions and ETL](../../document-extensions/revisions/revisions-and-other-features#revisions-and-etl)  
   * [Revisions and Replication](../../document-extensions/revisions/revisions-and-other-features#revisions-and-replication)  
   * [Revisions Import and Export](../../document-extensions/revisions/revisions-and-other-features#revisions-import-and-export)  
   * [Revisions and Backup](../../document-extensions/revisions/revisions-and-other-features#revisions-and-backup)  
   * [Revisions and Data Subscriptions](../../document-extensions/revisions/revisions-and-other-features#revisions-and-data-subscriptions)  

{NOTE/}

---

{PANEL: Revisions and Counters}

### Revisions Creation

* A revision will **not** be created upon modifications to existing 
  [counter](../../document-extensions/counters/overview) values.  
* **Creating** or **removing** a counter modifies the counter's 
  parent document by adding the counter's name or removing it from 
  the document's metadata, and **will** cause the creation of a new revision.  

---

### Stored Data
A revision created for a document that contains counters will include in its metadata 
a `@counters-snapshot` property that will hold the document's counter names and values 
at the time of the revision creation.  
The counter's value is stored in its **Accumulated form**: a single sum with no 
specification of the Counter's value on each node.  

---

### Reverted Data
When a document is reverted to a revision that owns counters, the counters are restored 
to functionality along with their values.  

---

### Code Sample
Use [GetMetadataFor](../../document-extensions/revisions/client-api/session/loading#getmetadatafor) 
to get a document's revisions metadata, and extract counters' data from the metadata.
{CODE revisions-and-other-features_counters@DocumentExtensions\Revisions\ClientAPI\Session\RevisionsAndOtherFeatures.cs /}

{PANEL/}

{PANEL: Revisions and Time Series}

### Revisions Creation

* A revision will **not** be created upon modifications to existing 
  [time series](../../document-extensions/timeseries/overview) values.  
* **Creating** or **removing** a time series modifies the time series' 
  parent document by adding the time series' name or removing it 
  from the document's metadata, and **will** cause the creation of 
  a new revision.  

---

### Stored Data
A revision created for a document that contains time series does **not** store 
the time series' data but include in its metadata a `@timeseries-snapshot` property 
with general information regarding the time series at the time of the revision creation.  
  
Read more about Revisions and Time Series [here](../../document-extensions/timeseries/time-series-and-other-features#revisions).  

---

### Reverted Data

When a document is reverted to a point in time in which the revision had a time series: 

* The reverted document will **Not** contain the time series in the following cases:
   * The time series was deleted from the current document.  
   * The document itself was deleted,  

* The reverted document **Will** contain the time series if:  
   * The time series is held by the current document (i.e. neither the time series nor the document were deleted).  
     {NOTE: }
      Note: the time series **data** will **not** be reverted, but remain as it was before the document was reverted.  
     {NOTE/}

{PANEL/}

{PANEL: Revisions and Attachments}

### Revisions Creation
**Adding** or **removing** an [attachment](../../document-extensions/attachments/what-are-attachments) 
modifies the attachment's parent document by adding a reference to the attachment or removing it from the 
document's metadata, and **will** cause the creation of a new revision.  

---

### Stored Data
Documents and their revisions do not store attachments, but contain 
references to them in an `@attachments` array in their metadata.  
Attachments are not replicated when new revisions are created.  
An attachment will be removed from RavenDB's storage only when 
there is no live document or document revision that refers to it.  

---

### Reverted Data
When a document is reverted to a revision that owns attachments, the attachments 
are restored to their state when the revision was created.  

{PANEL/}

{PANEL: Revisions and ETL}

An [ETL](../../server/ongoing-tasks/etl/raven) process will **not** send 
revisions to the destination database.  
However, if revisions are enabled on the destination database, whenever 
the ETL process sends a modified document and the target document is 
overwritten, a new revision will be created for the target document as expected.  

{PANEL/}

{PANEL: Revisions and Replication}

* Revisions **are** transferred during [replication](../../server/clustering/replication/replication) between nodes 
  of the same database group.  
* Revisions **can be** sent by an [External Replication Task](../../studio/database/tasks/ongoing-tasks/external-replication-task#general-information-about-external-replication-task).  

{INFO: }
Revisions can [help in keeping the consistency of replicated data](../../server/clustering/replication/replication#replication-consistency-can-be-achieved-by--).  
{INFO/}

{PANEL/}

{PANEL: Revisions Import and Export}

* Revisions can be Imported and exported with a `.ravendbdump` file -  
   * Using [the API](../../client-api/smuggler/what-is-smuggler).  
   * Using the Studio [import](../../studio/database/tasks/import-data/import-data-file#import-options) 
  and [export](../../studio/database/tasks/export-database#export-options) views.  
* Revisions can be imported from a [live RavenDB server](../../studio/database/tasks/import-data/import-from-ravendb#step-#4:-set-import-options).  
  ![Import from Live Server](images\import-from-live-server.png "Import from Live Server")

{PANEL/}

{PANEL: Revisions and Backup}
Revisions are [backed up](../../server/ongoing-tasks/backup-overview#backup-contents).  
{PANEL/}

{PANEL: Revisions and Data Subscriptions}

Learn about revisions and data subscriptions [here](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning).  

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../document-extensions/revisions/overview)  
* [Revert Revisions](../../document-extensions/revisions/revert-revisions)  
* [Counters: Overview](../../document-extensions/counters/overview)
* [Time Series: Overview](../../document-extensions/timeseries/overview)
* [Attachments: What are Attachments](../../document-extensions/attachments/what-are-attachments)

### Client API

* [Revisions: API Overview](../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  

### Studio

* [Settings: Document Revisions](../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../studio/database/document-extensions/revisions)  

### Data Subscriptions

* [What Are Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)  
* [Revisions and Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
