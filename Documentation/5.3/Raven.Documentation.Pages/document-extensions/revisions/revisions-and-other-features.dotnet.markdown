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
   * [Revisions Replication, Export and Import](../../document-extensions/revisions/revisions-and-other-features#revisions-replication,-export-and-import)  
   * [Revisions and Backup](../../document-extensions/revisions/revisions-and-other-features#revisions-and-backup)  
   * [Revisions and Data Subscriptions](../../document-extensions/revisions/revisions-and-other-features#revisions-and-data-subscriptions)  

{NOTE/}

---

{PANEL: Revisions and Counters}

### Revisions Creation

* **Creating** or **removing** a [counter](../../document-extensions/counters/overview) 
  modifies the counter's parent document by adding the counter's name or removing it from 
  the document's metadata, and **will** cause the creation of a new revision.  
* A revision will **not** be created upon modifications to counter values.  

---

### Stored Data
A revision created for a document that contains counters will include in its metadata 
a `@counters-snapshot` property that will hold the document's counter names and values 
at the time of the revision creation.  
The counter's value is stored in its **Accumulated form**: a single sum with no 
specification of the Counter's value on each node.  

---

### Code Sample
Use [GetMetadataFor](../../document-extensions/revisions/client-api/session/loading#getmetadatafor) 
to get a document's revisions metadata, and extract counters' data from the metadata.
{CODE revisions-and-other-features_counters@DocumentExtensions\Revisions\ClientAPI\Session\RevisionsAndOtherFeatures.cs /}

{PANEL/}

{PANEL: Revisions and Time Series}

### Revisions Creation

* **Creating** or **removing** a [time series](../../document-extensions/timeseries/overview) 
  modifies the time series' parent document by adding the time series' name or removing it 
  from the document's metadata, and **will** cause the creation of a new revision.  
* A revision will **not** be created upon modifications to time series values.  

---

### Stored Data
A revision created for a document that contains time series does **not** store 
the time series' data but include in its metadata a `@timeseries-snapshot` property 
with general information regarding the time series at the time of the revision creation.  
  
Read more about Revisions and Time Series [here](../../document-extensions/timeseries/time-series-and-other-features#revisions).  

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

{PANEL/}

{PANEL: Revisions and ETL}

An [ETL](../../server/ongoing-tasks/etl/raven) process will **not** send 
revisions to the destination database.  
However, if revisions are enabled on the destination database, whenever 
the ETL process sends a modified document and the target document is 
overwritten, a new revision will be created for the target document as expected.  

{PANEL/}

{PANEL: Revisions Replication, Export and Import}

* Revisions are [replicated](../../server/clustering/replication/replication) from one database to another.  
* Revisions can be replicated using an [External Replication Task](../../studio/database/tasks/ongoing-tasks/external-replication-task).  
* Revisions can be exported and imported using [Smuggler](../../client-api/smuggler/what-is-smuggler).  
* Revisions can be [exported](../../studio/database/tasks/export-database#export-options) 
  and [imported](../../studio/database/tasks/import-data/import-data-file#import-options) 
  using a `.ravendbdump` file.  
* Revisions can be imported from a [live RavenDB server](../../studio/database/tasks/import-data/import-from-ravendb#step-#4:-set-import-options).  

{PANEL/}

{PANEL: Revisions and Backup}
Revisions are [backed up](../../server/ongoing-tasks/backup-overview#backup-contents).  
{PANEL/}

{PANEL: Revisions and Data Subscriptions}

Learn about revisions and data subscriptions [here](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning).  

{PANEL/}


## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/loading)
- [Loading](../../../client-api/session/revisions/loading)

### Counters

- [Counter Batch Operation](../../../client-api/operations/counters/counter-batch)
- [Get Counters Operation](../../../client-api/operations/counters/get-counters)
