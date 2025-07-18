# Archived Documents and Other Features
---

{NOTE: }

* Once you have archived documents in your database (see how to [enable](../data-archival/enable-data-archiving) and [schedule](../data-archival/schedule-document-archiving) document archiving),  
  RavenDB features can detect these documents and handle them in different ways.

* Some features, like indexes and data subscriptions, provide native support for configuring whether to:
  * **Exclude** archived documents from processing, reducing index size and improving query relevance.
  * **Include** only archived documents, for tasks that target archived data specifically.
  * **Process both** archived and non-archived documents when needed.

* Other features can manage archived documents differently based on their purpose. For example:
  * ETL tasks can skip or selectively process archived documents.
  * Archived documents can be included or excluded when exporting or importing data.

* Limiting processing to either archived or non-archived documents may improve performance by reducing workload and transfer volume.

* Learn more below about how various RavenDB features interact with archived documents.

---

* In this article:  
  * [Archived documents and indexing](../data-archival/archived-documents-and-other-features#archived-documents-and-indexing)
  * [Archived documents and querying](../data-archival/archived-documents-and-other-features#archived-documents-and-querying)
  * [Archived documents and data subscriptions](../data-archival/archived-documents-and-other-features#archived-documents-and-subscriptions)
  * [Archived documents and document extensions](../data-archival/archived-documents-and-other-features#archived-documents-and-document-extensions)
  * [Archived documents and smuggler (export/import)](../data-archival/archived-documents-and-other-features#archived-documents-and-smuggler-(export/import))
  * [Archived documents and expiration](../data-archival/archived-documents-and-other-features#archived-documents-and-expiration)
  * [Archived documents and ETL](../data-archival/archived-documents-and-other-features#archived-documents-and-etl)
  * [Archived documents and backup](../data-archival/archived-documents-and-other-features#archived-documents-and-backup)
  * [Archived documents and replication](../data-archival/archived-documents-and-other-features#archived-documents-and-replication)
  * [Archived documents and patching](../data-archival/archived-documents-and-other-features#archived-documents-and-patching)

{NOTE/}

---

{PANEL: Archived documents and indexing}
     
* Indexing performance may decline as the database grows, since a larger number of documents increases indexing load, expands index size, and can eventually reduce query speed.
* Archiving documents and excluding them from indexing can be an effective way to maintain performance.  
  By removing low-priority or infrequently accessed documents from the indexing process, RavenDB can create smaller, faster indexes focused on current or high-value data.
  This also improves the relevance and responsiveness of queries, as they execute over a smaller and more meaningful dataset.

---

* **Configuring indexing behavior - Static indexes**:
    * **At the database level or server-wide**:  
      To control whether static indexes process archived documents from the source collection,  
      set the [Indexing.Static.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior)
      configuration key at either the database level or server-wide (default: `ExcludeArchived`).
    * Note that this setting applies only to static-indexes that are using _Documents_ as their data source.
      This global configuration does Not apply to static-indexes based on _Time Series_ or _Counters_, which default to `IncludeArchived`.
    * **Per index**:  
      You can override this global behavior per-index directly in the index definition, using the Client API from the Studio   
      (see the examples below).

* **Configuring indexing behavior - Auto indexes:**
    * **At the database level or server-wide**:  
      To control whether auto-indexes process archived documents at the database level or server-wide,  
      set the [Indexing.Auto.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.auto.archiveddataprocessingbehavior) configuration key (default `ExcludeArchived`).
    * **Per index**:  
      Unlike static indexes, you cannot configure this behavior per auto-index,  
      because dynamic queries (which trigger auto-index creation) do not provide a way to control this setting.

---

* The available configuration options are:  
  * `ExcludeArchived`: only non-archived documents are processed by the index.
  * `IncludeArchived`: both archived and non-archived documents are processed by the index.
  * `ArchivedOnly`: only archived documents are processed by the index.

---

##### Configuring archived document processing for a static index - from the Client API

You can configure how a static index handles archived documents when creating the index using the Client API.
This setting will **override** the global configuration defined by the [Indexing.Static.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior) configuration key.

{CONTENT-FRAME: }

Example:

  {CODE-TABS}
  {CODE-TAB:csharp:LINQ_index index_1@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TAB:csharp:JS_index index_2@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TAB:csharp:IndexDefinitionBuilder index_3@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TABS/}   

{CONTENT-FRAME/}
{CONTENT-FRAME: }

When a static-index is configured to include **both** archived and non-archived documents in its processing,  
you can also apply custom logic based on the presence of the `@archived` metadata property. 

For example:  

  {CODE-TABS}
  {CODE-TAB:csharp:LINQ_index index_4@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TAB:csharp:JS_index index_5@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TAB:csharp:IndexDefinition index_6@DataArchival\ArchivedDocsIntegration.cs /}
  {CODE-TABS/}

{CONTENT-FRAME/}

---

##### Configuring archived document processing for a static index - from the Studio

You can configure how a static index handles archived documents directly from the Studio.  
This setting will **override** the global configuration defined by the [Indexing.Static.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior) configuration key.

![Configure index](images/configure-static-index.png)

1. Open the [Indexes list view](../studio/database/indexes/indexes-list-view) and select the index you want to configure,
   or create a new index.
2. Scroll down and open the **Archived Data** tab.
3. Click to select how this index should process archived documents:
   * **Default**: The index will use the behavior set by the global configuration.
   * **Exclude Archived**: Index only non-archived documents.
   * **Include Archived**: Index both archived and non-archived documents.
   * **Archived Only**: Index only archived documents.

![Processing options](images/processing-options.png "Configure the static index")

{PANEL/}

{PANEL: Archived documents and querying}

* **Full collection queries**:  
  * Queries that scan an entire collection without any filtering condition (e.g. `from Orders`) will include archived documents.  
  * These queries are not influenced by indexing configuration related to archived documents because they do not use indexes.
  * Learn more about full collection queries in [Full collection query](../client-api/session/querying/how-to-query#collectionQuery).  

* **Dynamic queries (auto-indexes)**:
  * When making a dynamic query, RavenDB creates an auto-index to serve it.
    Whether that index processes archived documents depends on the value of the [Indexing.Auto.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.auto.archiveddataprocessingbehavior) configuration key at the time the query is made.
  * Once created, the auto-index retains that behavior.
    Query results will continue to reflect the configuration that was in effect when the index was first built - even if the setting is changed later.
  * Learn more about dynamic queries in [Query a collection - with filtering](../client-api/session/querying/how-to-query#dynamicQuery).

* **Querying static-indexes**:  
  * When querying a static-index, the results will include, exclude, or consist solely of archived documents depending on how the static-index was configured.
    The index behavior is determined by:  
      * the value of the [Indexing.Static.ArchivedDataProcessingBehavior](../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior)  configuration key at the time the static-index was created, or -
      * the explicit setting in the index definition, which overrides the global configuration key.
  * The index's archived data processing behavior can be modified after its creation using the Studio or the Client API.

{PANEL/}
  
{PANEL: Archived documents and subscriptions}

* Processing large volumes of documents in data subscriptions increases the workload on both the server and subscription workers.
* You can reduce this load by defining the subscription query to exclude archived documents, include only archived documents, or process both archived and non-archived data.  
  This gives you control over which documents are sent to workers - helping you focus on the most relevant data and reduce unnecessary processing.

---

* **Configuring the subscription task behavior**:
  * **At the database level or server-wide**:  
    To control whether queries in data subscription tasks process archived documents,  
    set the [Subscriptions.ArchivedDataProcessingBehavior](../todo..) configuration key at either the database level or server-wide   
    (default: `ExcludeArchived`).
  * **Per task**:  
    You can override this global behavior per data subscription task directly in the task definition,  
    using the Client API or from the Studio (see the examples below).

---

* The available configuration options are:
    * `ExcludeArchived`: only non-archived documents are processed by the subscription query.
    * `IncludeArchived`: both archived and non-archived documents are processed by the subscription query.
    * `ArchivedOnly`: only archived documents are processed by the subscription query.

---

##### Configuring archived document processing for a data subscription task - from the Client API

You can configure how a subscription task handles archived documents when creating the subscription using the Client API.  
This setting will **override** the global configuration defined by the [Subscriptions.ArchivedDataProcessingBehavior](../server/configuration/subscription-configuration#subscriptions.archiveddataprocessingbehavior) configuration key.

{CONTENT-FRAME: }

Example:

{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax subscription_task_generic_syntax@DataArchival\ArchivedDocsIntegration.cs /}
{CODE-TAB:csharp:RQL-syntax subscription_task_rql_syntax@DataArchival\ArchivedDocsIntegration.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}

---

##### Configuring archived document processing for a data subscription task - from the Studio

You can configure how a subscription task handles archived documents directly from the Studio.  
This setting will **override** the global configuration defined by the [Subscriptions.ArchivedDataProcessingBehavior](../server/configuration/subscription-configuration#subscriptions.archiveddataprocessingbehavior) configuration key.

![Configure subscription](images/configure-subscription.png)

1. Open the [Ongoing tasks list view](../studio/database/tasks/ongoing-tasks/general-info) and select the subscription task you want to configure,  
   or create a new subscription.
2. Click to select how the subscription query should process archived documents:
   * **Default**: The subscription will use the behavior set by the global configuration.
   * **Exclude Archived**: Process only non-archived documents.
   * **Include Archived**: Process both archived and non-archived documents.
   * **Archived Only**: Process only archived documents.

{PANEL/}

{PANEL: Archived documents and document extensions}

* **Attachments**:  
  * Attachments are Not archived (compressed), even if the document they belong to is archived.
 
* **Counters**:  
  * Counters are Not archived (compressed), even if the document they belong to is archived.  
  * Unlike indexes whose source data is _Documents_ - which default to `ExcludeArchived` -  
    indexes whose source data is _Counters_ do process archived documents by default (`IncludeArchived`).  
    This behavior can be modified in the index definition.

* **Time series**:  
  * Time series are Not archived (compressed), even if the document they belong to is archived.  
  * Unlike indexes whose source data is _Documents_ - which default to `ExcludeArchived` -  
    indexes whose source data is _Time series_ do process archived documents by default (`IncludeArchived`).  
    This behavior can be modified in the index definition.
 
* **Revisions**:  
  * No revision is created at the time the server archives a document, even if the Revisions feature is enabled.
  * However, if you modify an archived document (when Revisions are enabled), a revision is created for that document - and that revision is archived as well.

{PANEL/}

{PANEL: Archived documents and smuggler (export/import)}

You can control whether archived documents are included when exporting or importing a database.

---

##### Export/Import archived documents - from the Client API

[Smuggler](../client-api/smuggler/what-is-smuggler), RavenDB’s tool for database export and import, can be configured to include or exclude archived documents.
By default, archived documents are **included** in the operation.

{CONTENT-FRAME: }

In this example, exported data **excludes** archived documents:

{CODE export@DataArchival\ArchivedDocsIntegration.cs /}

{CONTENT-FRAME/}

{CONTENT-FRAME: }

In this example, imported data **includes** archived documents:

{CODE import@DataArchival\ArchivedDocsIntegration.cs /}

{CONTENT-FRAME/}

---

##### Export archived documents - from the Studio

![Export archived documents](images/export-archived-documents.png "Export archived documents")

1. Go to **Tasks > Export Database**.
2. Toggle the **Include archived documents** option to control whether archived documents are included in the database export.

---

##### Import archived documents - from the Studio

![Import archived documents](images/import-archived-documents.png "Import archived documents")

1. Go to **Tasks > Import Data**.
2. Toggle the **Include archived documents** option to control whether archived documents are included in the import.

{PANEL/}

{PANEL: Archived documents and expiration}

* Archiving can be used alongside other features, such as [Document expiration](../server/extensions/expiration).  
 
* For example, a document can be scheduled to be archived after six months and expired after one year.  
  This allows you to keep recent documents active and quickly accessible, move older documents to archival storage where slower access is acceptable,
  and eventually remove documents that are no longer needed.

* In the following example, both the `@archive-at` and the `@expires` metadata properties have been added to document `companies/90-A`
  to schedule it for archiving and expiration, respectively:
 
     {CODE-BLOCK:json}
{
    "Name": "Wilman Kala",
    "Phone": "90-224 8858",
    ...
    "@metadata": {
        "@archive-at": "2026-01-06T22:45:30.018Z",
        "@expires": "2026-07-06T22:45:30.018Z",
        "@collection": "Companies",
        ...  
    }
}
     {CODE-BLOCK/}

{PANEL/}

{PANEL: Archived documents and ETL}

* An ETL transformation script can examine each source document’s [metadata](../server/ongoing-tasks/etl/raven#accessing-metadata)
  for the existence of the `@archived: true` property, which indicates that the document is archived.
  Based on this check, the script can decide how to handle the document - for example, skip it entirely or send only selected fields.

* With [RavenDB ETL](../../../server/ongoing-tasks/etl/raven), documents that are archived in the source database and sent to the target
  are not archived in the destination database.

* In the following example, the ETL script checks whether the document is archived, and skips it if it is:

     {CODE-BLOCK: JavaScript}
var isArchived = this['@metadata']['@archived'];

if (isArchived === true) {
    return; // Do not process archived documents
}

// Transfer only non-archived documents to the target
loadToOrders(this);
     {CODE-BLOCK/}

{PANEL/}

{PANEL: Archived documents and backup}

* Archived documents are included in database backups (both _logical backups_ and _snapshots_),  
  no special configuration is required.

* When restoring a database from a backup, archived documents are restored as well,  
  and their archived status is preserved.

{PANEL/}

{PANEL: Archived documents and replication}

Archived documents are included in [Internal](../server/clustering/replication/replication#internal-replication) replication, 
[External](../server/clustering/replication/replication#external-replication) replication, and [Hub/Sink](../server/clustering/replication/replication#hubsink-replication) replication -   
no special configuration is required.

{PANEL/}

{PANEL: Archived documents and patching}

* Patching can be used to **schedule** multiple documents for archiving. See the dedicated sections:   
  [Schedule multiple documents for archiving - from the Studio](../data-archival/schedule-document-archiving#schedule-multiple-documents-for-archiving---from-the-studio).  
  [Schedule multiple documents for archiving - from the Client API](../data-archival/schedule-document-archiving#schedule-multiple-documents-for-archiving---from-the-client-api).  

* Patching is used to **unarchive** documents.  
  See the dedicated article [Unarchiving documents](../data-archival/unarchiving-documents).   

* When **cloning** an archived document using the `put` method within a patching script  
  (see method details in this [Document operations](../server/kb/javascript-engine#document-operations) section) the cloned document will Not be archived,  
  and the `@archived: true` property will be removed from the cloned document.

{PANEL/}

## Related Articles

### Document Archival
- [Overview](../data-archival/overview)
- [Enable data archiving](../data-archival/enable-data-archiving)
- [Schedule document archiving](../data-archival/schedule-document-archiving)
- [Unarchiving documents](../data-archival/unarchiving-documents)

### Configuration
- [Overview](../server/configuration/configuration-options#settings.json)  
- [Database Settings](../studio/database/settings/database-settings#view-database-settings)  

### Tasks
- [Smuggler (Import/Export)](../client-api/smuggler/what-is-smuggler) 
- [ETL Basics](../server/ongoing-tasks/etl/basics)  
- [Replication](../../server/clustering/replication/replication)  

### Extensions
- [Document Expiration](../../server/extensions/expiration)  

### Patching
- [Patch By Query](../client-api/rest-api/queries/patch-by-query)  
