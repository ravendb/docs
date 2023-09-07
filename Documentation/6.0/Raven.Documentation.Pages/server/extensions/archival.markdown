# Data Archival
---

{NOTE: }

* Documents may be archived for a wide variety of reasons: their contents may become 
  obsolete, their usage may become scarce, their number may breach a set limit, and so on.  
  
* In RavenDB, a document is marked as archived when an `@archived: true` property 
  is included in its metadata.  

* A document can also be **scheduled for future archival** by adding its metadata 
  an `@archive-at` property with the designated archival time (**UTC**) as a value.  

* RavenDB features and clients can recognize an archived document by its metadata 
  property and handle it accordingly. Indexing and Data subscription tasks, 
  for example, **refrain by default** from processing archived documents.  
  The exclusion of archived documents from indexation can improve  
  database performance significantly.  

* On a cluster, archiving is handled by a single node only, which 
  is always the first node in the cluster topology.  
  Following archiving, documents are propagated to the other nodes 
  by regular replication.  

{INFO: }
Archived documents are **compressed**.  
{INFO/}

{INFO: }
Document extensions of an archived document are **not** archived.  
{INFO/}

* In this page:  
  * [Overview](../../server/extensions/archival#overview)  
     * [Why Archive Documents?](../../server/extensions/archival#why-archive-documents)  
     * [Scheduling Document Archival](../../server/extensions/archival#scheduling-document-archival)  
  * [Archiving and Expiration](../../server/extensions/archival#archiving-and-expiration)  
  * [Enabling Archiving and Setting Scan Frequency](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency)  
  * [Configuration Options](../../server/extensions/archival#configuration-options)  
     * [Auto Indexing Handling of Archived Documents](../../server/extensions/archival#section)  
     * [Static Indexing Handling of Archived Documents](../../server/extensions/archival#section-1)  
     * [Data Subscriptions Handling of Archived Documents](../../server/extensions/archival#section-2)  

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
  
    Since indexes can recognize archived documents as such during indexation by their 
    metadata, they may also apply their own logic for such encounters in the index 
    definition level.  

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

* The `@archive-at` property will be replaced with an `@archived: true` property.  

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

* **The document will be compressed**.  
  The compression of archived documents, along with their possible absence 
  from any index, makes their retrieval longer than that of live documents.  

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
| **ArchiveFrequencyInSec** | `long` | Determines how often the server checks for documents that need to be archived. |

* To enable archiving via Studio:  
  See [Setting Document Archiving in Studio](../../studio/database/settings/document-archiving).  

{PANEL/}

{PANEL: Configuration Options}

RavenDB features that currently include built-in support for archived documents are:  

* Auto indexing  
* Static indexing  
* Data subscription  

The way these features handle archived documents when they encounter them 
is determined using the following configuration options.

---

#### `Indexing.Auto.ArchivedDataProcessingBehavior`

The behavior of an auto index when it encounters an archived document.  

- **Type**: `enum`
  {CODE-BLOCK: csharp}
  public enum ArchivedDataProcessingBehavior
  {
      // Exclude archived documents: avoid indexing them or sending them to workers  
      ExcludeArchived,
      // Include archived documents: index them or send them to workers
      IncludeArchived,
      // Handle ONLY archived documents: index or send to workers only archived documents)  
      ArchivedOnly
  }
  {CODE-BLOCK/}
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

---

#### `Indexing.Static.ArchivedDataProcessingBehavior`

The behavior of a static index when it encounters an archived document.  

- **Type**: `enum`
  {CODE-BLOCK: csharp}
  public enum ArchivedDataProcessingBehavior
  {
      // Exclude archived documents: avoid indexing them or sending them to workers  
      ExcludeArchived,
      // Include archived documents: index them or send them to workers
      IncludeArchived,
      // Handle ONLY archived documents: index or send to workers only archived documents)  
      ArchivedOnly
  }
  {CODE-BLOCK/}
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

---

#### `Subscriptions.ArchivedDataProcessingBehavior`

The behavior of a data subscription task when it encounters an archived document.  

- **Type**: `enum`
  {CODE-BLOCK: csharp}
  public enum ArchivedDataProcessingBehavior
  {
      // Exclude archived documents: avoid indexing them or sending them to workers  
      ExcludeArchived,
      // Include archived documents: index them or send them to workers
      IncludeArchived,
      // Handle ONLY archived documents: index or send to workers only archived documents)  
      ArchivedOnly
  }
  {CODE-BLOCK/}
- **Default**: `ArchivedDataProcessingBehavior.ExcludeArchived`
- **Scope**: [Server-wide](../../server/configuration/configuration-options#settings.json) 
  or [per database](../../studio/database/settings/database-settings#view-database-settings)

{PANEL/}

## Related Articles

### Studio
- [Document Archiving](../../studio/database/settings/document-archiving)
