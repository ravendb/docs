# Document Archival
---

{NOTE: }

* Documents may be archived for a wide variety of reasons, including obsolete 
  contents, scarce usage, their number breaching a set limit, and so on.  

* A document can be **scheduled for archival** by adding its metadata an 
  `@archive-at` property with the requested archival time (in `UTC`) as a value.  
  RavenDB runs an archiving task that periodically scans the database for documents 
  scheduled this way for archiving. When it's time to archive a document, the task 
  archives it and then replaces its metadata `@archive-at` property with an 
  `@archived: true` property.  

    {INFO: }
    A metadata `@archived: true` property is just an external indication that RavenDB 
    has archived a document. Users **cannot** archive documents manually by adding 
    this property to their metadata. To archive a document, schedule its archival.  
    {INFO/}

* Some **RavenDB features** recognize archived documents and can handle them accordingly.  
  Indexing and Data subscription tasks, for example, **refrain by default** from processing 
  archived documents. The exclusion of archived documents from indexation can improve  
  database performance significantly.  

* **RavenDB clients** can recognize archived documents by their metadata `@archived: true` 
  property and apply whatever specific logic suits them. A user-defined ETL task, for 
  example, can avoid sending its target documents that are marked as archived.  

* On a cluster, the archiving task is running on one node only, which is always the 
  first node in the cluster topology. Archived documents are then propagated to the 
  other nodes by regular replication.  

* Archived documents and their document extensions **can** be updated regularly.  

* The document extensions of an archived document are **not** archived or affected 
  in any way by the archival status of their parent documents. A time series, for 
  example, **will** be indexed even if the document that owns it is archived.  

* Archived documents are **compressed**.  

* In this page:  
  * [Overview](../../server/extensions/archival#overview)  
     * [Why Archive Documents?](../../server/extensions/archival#why-archive-documents)  
     * [Scheduling Document Archival](../../server/extensions/archival#scheduling-document-archival)  
  * [Archiving and Expiration](../../server/extensions/archival#archiving-and-expiration)  
  * [Enabling Archiving and Setting Scan Frequency](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency)  
  * [Configuration](../../server/extensions/archival#configuration)  
  * [Server and DB Configuration Options](../../server/extensions/archival#server-and-db-configuration-options)  
     * [Auto Indexing Archived Docs Handling](../../server/extensions/archival#section-1)  
     * [Static Indexing Archived Docs Handling](../../server/extensions/archival#section-2)  
     * [Data Subscriptions Archived Docs Handling](../../server/extensions/archival#section-3)  
  * [Feature-Level Configuration](../../server/extensions/archival#feature-level-archiving-configuration-options)  
     * [Index Definition Archiving Configuration](../../server/extensions/archival#index-definition-archiving-configuration)  
     * [Data Subscription Task Archiving Configuration](../../server/extensions/archival#data-subscription-task-archiving-configuration)  

{NOTE/}

---

{PANEL: Overview}

#### Why Archive Documents?

Continuous accumulation of documents in a database may, over time, slow 
down some of its functions, including indexation and document distribution.  

* **Indexing** efficiency in particular may sink as a larger number of documents 
  requires more indexing resources, increases the number and size of indexes, and 
  may eventually reduce querying speed.  
  
    Regularly archiving documents, and excluding archived documents from indexation, 
    is therefore an all-round performance enhancer as fewer, more effective, indexes 
    are created for queries that are executed over smaller datasets of higher priority.  
  
    In addition, an index definition can include user-defined code that recognizes 
    archived documents by their `@archived: true` property and handles them accordingly.  

* **Data subscriptions** exclusion of archived documents from data batches reduces 
  workload not only from the server but also from the workers that may now receive 
  a smaller number of more relevant documents.  

* The same attitude can be implemented by **ETL tasks**, that can easily recognize 
  archived documents by their metadata and refrain from extracting them.  

---

#### Scheduling Document Archival

A document can be **scheduled for archival** by adding its metadata an 
`@archive-at` property with the designated archival time (**UTC**) as a value.  
{NOTE: }
Provide the scheduled time in `DateTime` format, in `UTC`.  
E.g., `DateTime.UtcNow`
{NOTE/}


`companies/90-A`:
{CODE-BLOCK:json}
{
    "Name": "Wilman Kala",
    "Phone": "90-224 8858",
    "@metadata": {
        "@archive-at": "2023-09-06T22:45:30.018Z",
        "@collection": "Companies",
     }
}
{CODE-BLOCK/}

RavenDB scans for documents scheduled for archival in a frequency set by 
[DataArchivalConfiguration.ArchiveFrequencyInSec](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency).  

Around the specified time (considering the scan frequency) the document 
will be archived:  

* The document will be compressed.  
  The compression of archived documents, along with their possible absence 
  from any index, makes their retrieval longer than that of live documents.  

* RavenDB will set its internal flags so features like indexing and 
  data subscriptions would be able to recognize the document as archived.  

* The document's `@archive-at` property will be replaced with an 
  `@archived: true` property so clients would be able to recognize 
  the document as archived and handle it however they choose.  

  `companies/90-A`:
  
  {CODE-BLOCK:json}
  {
      "Name": "Wilman Kala",
      "Phone": "90-224 8858",
      "@metadata": {
          "@archived": true,
          "@collection": "Companies",
       }
  }
  {CODE-BLOCK/}

{PANEL/}

{PANEL: Archiving and Expiration}
Archiving can be used alongside other extensions like [expiration](../../server/extensions/expiration).  
A document can, for example, be scheduled for archival in half a year, and 
for expiration in a year. This would keep newer documents alive and within 
immediate reach, archive older documents whose retrieval, should it be required, 
can be slower, and have documents that are no longer needed deleted.  

`companies/90-A`:
{CODE-BLOCK:json}
{
    "Name": "Wilman Kala",
    "Phone": "90-224 8858",
    "@metadata": {
        "@archive-at": "2024-03-06T22:45:30.018Z",
        "@expires": "2024-09-06T22:45:30.018Z",
        "@collection": "Companies",
     }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Enabling Archiving and Setting Scan Frequency}

Archiving is **disabled** by default.  
To Enable the feature on the database, and to set the Frequency by which RavenDB scans 
the database for documents scheduled for archiving, pass the `ConfigureDataArchivalOperation` 
operation a `DataArchivalConfiguration` instance.  

{CODE-BLOCK:csharp}
var configuration = new DataArchivalConfiguration {
                        // Enable archiving
                        Disabled = false, 
                        // Scan for documents scheduled for archiving every 100 seconds 
                        ArchiveFrequencyInSec = 100 
                    };

var result = await store.Maintenance.SendAsync(
                    new ConfigureDataArchivalOperation(configuration));  
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **Disabled** | `bool` | If set to `true`, archival is disabled for the entire database. <br> Default: `true` |
| **ArchiveFrequencyInSec** | `long` | Frequency (in sec) in which the server checks for documents that need to be archived. <br> Default: 60 |

---

{NOTE: }
Read [here](../../studio/database/settings/document-archival) about setting document archival using Studio.  
{NOTE/}

{PANEL/}

{PANEL: Configuration}

RavenDB features that currently include built-in support for archived documents are:  

* Auto indexing  
* Static indexing  
* Data subscription  

You can configure each of these features to **exclude**, **include**, or handle **only** 
archived documents it encounters, using the `ArchivedDataProcessingBehavior` enum.  

#### `ArchivedDataProcessingBehavior`:

{CODE-BLOCK: csharp}
public enum ArchivedDataProcessingBehavior
{
    // Exclude archived documents: avoid indexing them or sending them to workers  
    ExcludeArchived,
    // Include archived documents: index them or send them to workers
    IncludeArchived,
    // Handle ONLY archived documents: index or send to workers only archived documents  
    ArchivedOnly
}
{CODE-BLOCK/}

* [server](../../server/configuration/configuration-options#settings.json) and 
  [database](../../studio/database/settings/database-settings#view-database-settings) 
  configuration options allow you to apply a default behavior toward archived documents 
  across this database or all databases.  
* The same configuration options can be applied to a specific index or data subscription 
  definition, overriding the default behavior.  
* The definition of an index can also read the metadata of a certain document and 
  apply additional logic by its archival status.  

* Below: 
   * [Database-Level Archiving Configuration Options](../../server/extensions/archival#server-and-db-configuration-options)  
   * [Feature-Level Archiving Configuration Options](../../server/extensions/archival#feature-level-archiving-configuration-options)  

{PANEL/}

{PANEL: Server and DB Configuration Options}

The following configuration options determine whether RavenDB 
features would handle archived documents or not.  
{NOTE: }
Note that the definition of a specific index or data subscription 
task can [override](../../server/extensions/archival#feature-level-archiving-configuration-options) 
these settings.  
{NOTE/}

---

#### `Indexing.Auto.ArchivedDataProcessingBehavior`:

Use this option to set the behavior of auto indexes across the database 
when they encounter archived documents.  

- **Type**: [ArchivedDataProcessingBehavior](../../server/extensions/archival#section)  
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

---

#### `Indexing.Static.ArchivedDataProcessingBehavior`:

Use this option to set the behavior of static indexes across the database 
when they encounter archived documents.  

- **Type**: [ArchivedDataProcessingBehavior](../../server/extensions/archival#section)  
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

---

#### `Subscriptions.ArchivedDataProcessingBehavior`:

Use this option to set the behavior of data subscription tasks across the database 
when they encounter archived documents.  

- **Type**: [ArchivedDataProcessingBehavior](../../server/extensions/archival#section)  
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

{PANEL/}

{PANEL: Feature-Level Archiving Configuration Options}

The definition of a certain index or data subscription task can override database-level 
configuration and determine how this index or task would handle archived documents.  

---

#### Index Definition Archiving Configuration:

This index overrides the database-level configuration to determine 
whether to process archived documents.  

{CODE-BLOCK: csharp}
store.Maintenance.Send(new PutIndexesOperation(new[] {
    new IndexDefinition
    {
        Maps = {
                    // maps definition 
               },

        // Process archived documents
        ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.IncludeArchived
    }}));
{CODE-BLOCK/}

see the definition of `ArchivedDataProcessingBehavior` [here](../../server/extensions/archival#section).  

Note that the index can also check documents metadata and base whatever additional logic 
it wishes to apply on the document being archived or not.  

{CODE-BLOCK: csharp}
store.Maintenance.Send(new PutIndexesOperation(new[] {
    new IndexDefinition
    {
        // this will apply only to non-archived documents (whose @archived property is null)
        {"from o in docs where o[\"@metadata\"][\"@archived\"] == null select new" +
                        "{" +
                        "    Name = o.Name" +
                        "}"}
    }}));
{CODE-BLOCK/}

---

#### Data Subscription Task Archiving Configuration:

The following subscription is set so it would process **only** archived documents, 
using the [ArchivedDataProcessingBehavior](../../server/extensions/archival#section) enum.  

{CODE-BLOCK: csharp}
// Set-up the subscription
var subsId = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions
{
    Query = "from Companies", Name = "Created", ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.ArchivedOnly
});
{CODE-BLOCK/}

{PANEL/}


## Related Articles

### Studio
- [Document Archiving](../../studio/database/settings/document-archival)
