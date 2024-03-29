﻿# Data Archival
---

{NOTE: }

* RavenDB (6.0 and on) offers the ability to archive selected documents.  
  Archived documents are **compressed** and can be handled differently by 
  RavenDB functions (e.g. Indexing can exclude archived documents from indexes 
  and Data Subscriptions can avoid sending archived docs to workers), helping 
  to keep the database smaller and quicker and its contents more relevant.  

* Documents are not archived directly by users, but **scheduled for archival** 
  and then archived by a periodical RavenDB archiving task.  
  Archived documents are marked by a boolean `@archived: true` property in their metadata.  

* Archived documents and their document extensions **remain accessible** and 
  can be updated regularly.  
  Modifying an archived document or its extensions does not change its archival status.  

* Document extensions are **not** archived.  

* In this page:  
  * [Overview](../../server/extensions/archival#overview)  
     * [Licensing](../../server/extensions/archival#licensing)
  * [Scheduling Document Archival](../../server/extensions/archival#scheduling-document-archival)  
  * [Archival and Other Features](../../server/extensions/archival#archival-and-other-features)  
     * [Archiving and Indexing](../../server/extensions/archival#archiving-and-indexing)  
     * [Archiving and Data Subscriptions](../../server/extensions/archival#archiving-and-data-subscriptions)  
     * [Archiving and Document Extensions](../../server/extensions/archival#archiving-and-document-extensions)  
     * [Archiving and Smuggler (Import/Export)](../../server/extensions/archival#archiving-and-smuggler-importexport)  
     * [Archiving and Expiration](../../server/extensions/archival#archiving-and-expiration)  
     * [Archiving and ETL](../../server/extensions/archival#archiving-and-etl)  
     * [Archiving and Backup](../../server/extensions/archival#archiving-and-backup)  
     * [Archiving and Querying](../../server/extensions/archival#archiving-and-querying)  
     * [Archiving and Replication](../../server/extensions/archival#archiving-and-replication)  
  * [Archiving and Patching](../../server/extensions/archival#archiving-and-patching)  
     * [Archive by Patch](../../server/extensions/archival#archive-by-patch)  
     * [Unarchive by Patch](../../server/extensions/archival#unarchive-by-patch)  
  * [Enabling Archiving and Setting Scan Frequency](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency)  
  * [Default (Server/Database) Configuration Options](../../server/extensions/archival#default-(server/database)-configuration-options)  
     * [Auto Indexing Archived Docs Handling](../../server/extensions/archival#section-1)  
     * [Static Indexing Archived Docs Handling](../../server/extensions/archival#section-2)  
     * [Data Subscriptions Archived Docs Handling](../../server/extensions/archival#section-3)  

{NOTE/}

---

{PANEL: Overview}

* As a database grows very big basic functions like indexation and document 
  distribution may slow down. Archiving documents when they become obsolete, 
  when their usage becomes scarce, or for various other reasons, can exempt 
  RavenDB features from processing these documents and significantly improve 
  performance.  

* A document can be **scheduled for archival** by adding its metadata an 
  `@archive-at` property with the requested archival time (in `UTC`) as a value.  
  When the archival feature is [enabled](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency), 
  RavenDB runs an archiving task that periodically scans the database for 
  documents scheduled for archival.  

    {INFO: }
    On a cluster, the archiving task is running on one node only, which is 
    always the first node in the cluster topology. Archived documents are then 
    propagated to the other nodes by regular replication.  
    {INFO/}

* When it's time to archive a document the archiving task archives it and then 
  replaces its metadata `@archive-at` property with an `@archived: true` property.  

    {INFO: }
    A metadata `@archived: true` property is just an external indication that 
    RavenDB has archived a document. Users **cannot** archive documents manually 
    by adding this property to their metadata. To archive a document, schedule 
    its archival.  
    {INFO/}

* Features like indexing and data subscriptions recognize archived documents 
  by internal RavenDB flags, and can react to them based either on a default 
  high-level policy (set by server or database configuration options) or on 
  a local [index](../../server/extensions/archival#archiving-and-indexing) or 
  [task](../../server/extensions/archival#archiving-and-data-subscriptions) definition.  

* Other features, and RavenDB clients, can recognize archived documents by their 
  metadata `@archived: true` property and apply whatever specific logic suits them.  
  A user-defined ETL task, for example, can avoid this way from sending its target 
  archived documents.  

---

### Licensing

{INFO: }
Archival is Available on an **Enterprise** license.  
{INFO/}

Learn more about licensing [here](../../start/licensing/licensing-overview).  

{PANEL/}

{PANEL: Scheduling Document Archival}

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
        "@archive-at": "2024-01-01T12:00:00.000Z",
        "@collection": "Companies",
     }
}
{CODE-BLOCK/}

RavenDB scans for documents scheduled for archival in a frequency set by 
[DataArchivalConfiguration.ArchiveFrequencyInSec](../../server/extensions/archival#enabling-archiving-and-setting-scan-frequency).  

Around the specified time (considering the scan frequency) the document 
will be archived:  

* The document will be compressed.  
  {INFO: }
  Compressing archived documents saves disk space.  
  Note, though, that archived docs' compression, as well as their 
  possible absence from any index, makes their retrieval longer and 
  more CPU/memory consuming than that of non-archived documents.  
  {INFO/}

* RavenDB will set its internal flags so features like indexing and 
  data subscriptions can recognize the document as archived.  

* The document's `@archive-at` property will be replaced with an 
  `@archived: true` property so clients can  recognize the document 
  as archived and handle it however they choose.  

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

{PANEL: Archival and Other Features}

#### Archiving and Indexing

Indexing efficiency in particular may drop when a database becomes very big, 
as a larger number of documents requires more indexing resources, increases the 
number and size of indexes, and may eventually reduce querying speed.  
  
Routinely archiving documents and excluding the archived documents from indexation 
is, therefore, an all-round performance enhancer, as fewer and more effective indexes 
are created for queries that are executed over smaller datasets of higher priority.  
  
An index may inherit the way it handles archived documents from the 
[default](../../server/extensions/archival#default-(server/database)-configuration-options) 
server or database configuration, or have this behavior defined in the index 
definition, overriding higher-level configuration.  

* An index definition can override the default database/server configuration 
  to determine how the index would process archived documents.  
  {CODE overrideDefaultIndexConfiguration@Server\Archival.cs /}

    see the definition of `ArchivedDataProcessingBehavior` [here](../../server/extensions/archival#section).  

* An index definition can also check whether the document metadata includes 
  `@archived: true`, and if so freely apply any archived-document logic.  
  {CODE applyAdditionalLogic@Server\Archival.cs /}

* `ArchivedDataProcessingBehavior` can be used with additional static index creation methods.  

   * When the index is created using [IndexDefinitionBuilder](../../indexes/creating-and-deploying#indexdefinitionbuilder):  
     {CODE useIndexDefinitionBuilder@Server\Archival.cs /}
     
   * When the index is created using [AbstractIndexCreationTask](../../indexes/creating-and-deploying#using-abstractindexcreationtask):  
     {CODE useAbstractIndexCreationTask@Server\Archival.cs /}

---

#### Archiving and Data Subscriptions

Data subscriptions exclusion of archived documents from data batches reduces 
workload for both the server and the workers which may now receive fewer and more 
relevant documents.  

As with indexes, a data subscription task may inherit a default high-level policy 
toward archived documents or override it locally, in the task definition.  

* The below data subscription task overrides default server/database settings 
  and uses its `ArchivedDataProcessingBehavior` property with the 
  [ArchivedDataProcessingBehavior](../../server/extensions/archival#section) 
  enum to process **only** archived documents.  
  {CODE dataSubscriptionDefinition@Server\Archival.cs /}

---

#### Archiving and Document Extensions

The document extensions of an archived document are **not** archived or affected 
in any way by the archival status of their parent documents. A time series, for 
example, **will** be indexed even if the document that owns it is archived.  

---

#### Archiving and Smuggler (Import/Export)

[Smuggler](../../client-api/smuggler/what-is-smuggler), used by RavenDB to import and 
export data, checks documents' archival status and can be set to skip archived docs.  

Determine whether archived documents would be transferred or not by setting to `true` 
or `false` the boolean `IncludeArchived` property in the `DatabaseSmugglerExportOptions` 
instance you pass Smuggler.  

In the following example, the exported data **includes** archived documents.  
{CODE smugglerOption@Server\Archival.cs /}

{INFO: }
By default, archived documents are **Included** when importing/exporting data.  
{INFO/}

---

#### Archiving and Expiration

Archiving can be used alongside other extensions like [expiration](../../server/extensions/expiration).  
A document can, for example, be scheduled for archival in half a year, and 
for expiration in a year. This would keep newer documents alive and within 
immediate reach, archive older documents whose retrieval, should it be required, 
is allowed to be slower, and have documents that are no longer needed deleted.  

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

---

#### Archiving and ETL

An ETL transform script can check each extracted document's 
[metadata](../../server/ongoing-tasks/etl/raven#accessing-metadata) 
for an `@archived: true` property, whose existence indicates 
the document is archived, and handle the document by the result.  
Archived documents can be skipped, for example, or only relevant 
parts of them can be sent to the target.  

{CODE-BLOCK: JavaScript}
var isArchived = this['@metadata']['@archived'];
if (isArchived === true) {
    return; // do not load archived documents
}}
{CODE-BLOCK/}

---

#### Archiving and Backup

Archived documents **Are** included in backups.  

---

#### Archiving and Querying

Collection queries **will** retrieve archived documents (since they do not run over 
indexes that exclude archived docs from the results).  

Auto indexes will **not** retrieve archived documents, if the indexes were created 
when the default configuration excluded archived documents.  

Static indexes will **not** retrieve archived documents, if the indexes were created 
when the default configuration excluded archived documents or the index definition was 
edited to exclude them locally.  

---

#### Archiving and Replication

Archived documents **Are** included in [Regular](../../server/clustering/replication/replication) 
replication, [External](../../server/ongoing-tasks/external-replication) replication, 
and [Hub/Sink](../../server/ongoing-tasks/hub-sink-replication) replication.  

{PANEL/}

{PANEL: Archiving and Patching}

## Archive by Patch

To archive documents using [patching](../../client-api/rest-api/queries/patch-by-query), 
**schedule their archival** using the patch API `archived.archiveAt` method.
{CODE-BLOCK: JavaScript}
// Pass the method the document object and a string with the designated archival (`UTC`) time.
archived.archiveAt (doc, utcDateTimeString)
{CODE-BLOCK/}

{CODE archiveByPatch@Server\Archival.cs /}

## Unarchive by Patch

Since unarchiving documents is most often a mass operation, unarchiving 
is done using the patch API `archived.unarchive` method. To unarchive 
a document, pass the method the document object.  

{CODE unarchiveByPatch@Server\Archival.cs /}

{INFO: }
**Be aware** that a patch query may run over an index, and if 
the index **excludes archived documents** the query will not find
any archived documents to unarchive.  

The following patch query, for example, runs over an index.  
If this is an auto index that inherits the [default configuration](../../server/extensions/archival#section-1), 
archived documents will be excluded and the patch will find 
and unarchive **no documents**.  
{CODE unarchiveUsingAutoIndex@Server\Archival.cs /}

Two possible workarounds are:  

* Configure the index that the patch you're running uses to 
  **include** archived documents, [as explained here](../../server/extensions/archival#archiving-and-indexing).  
* Run a simple collection query, that creates and uses no 
  index, and then apply your own logic to find the documents 
  you want to unarchive. E.g. -  
  {CODE unarchiveCollectionQuery@Server\Archival.cs /}
{INFO/}

{PANEL/}


{PANEL: Enabling Archiving and Setting Scan Frequency}

Archiving is **disabled** by default.  
To Enable the feature on the database, and to set the Frequency by which RavenDB scans 
the database for documents scheduled for archiving, pass the `ConfigureDataArchivalOperation` 
operation a `DataArchivalConfiguration` instance.  

{CODE enableArchivingAndSetFrequency@Server\Archival.cs /}

| Parameter | Type | Description |
| - | - | - |
| **Disabled** | `bool` | If set to `true`, archival is disabled for the entire database. <br> Default: `true` |
| **ArchiveFrequencyInSec** | `long` | Frequency (in sec) in which the server checks for documents that need to be archived. <br> Default: 60 |

---

{NOTE: }
Read [here](../../studio/database/settings/document-archival) about setting document archival using Studio.  
{NOTE/}

{PANEL/}

{PANEL: Default (Server/Database) Configuration Options}

RavenDB features that currently include built-in support for archived documents are:  

* Auto indexing  
* Static indexing  
* Data subscription  

These features can be configured to **exclude**, **include**, or handle **only** 
archived documents they encounter, using the `ArchivedDataProcessingBehavior` enum 
(see below).  
{WARNING: }
The archiving policy is applied to an index or a data subscription task 
**when the index or task are created**, i.e. changing the default configuration 
will **not** change the behavior of an existing index or data subscription task.  
{WARNING/}

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

---

The following configuration options determine how RavenDB features handle archived documents.  
{NOTE: }
Note that configuring the behavior of a specific [index](../../server/extensions/archival#archiving-and-indexing) 
or [data subscription task](../../server/extensions/archival#archiving-and-data-subscriptions) 
when they encounter archived documents will override the default settings presented here.  
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

## Related Articles

### Studio
- [Document Archiving](../../studio/database/settings/document-archival)

### Configuration
- [Overview](../../server/configuration/configuration-options#settings.json)  
- [Database Settings](../../studio/database/settings/database-settings#view-database-settings)  

### Tasks
- [Smuggler (Import/Export)](../../client-api/smuggler/what-is-smuggler) 
- [ETL Basics](../../server/ongoing-tasks/etl/basics)  
- [Regular Replication](../../server/clustering/replication/replication)  
- [External Replication](../../server/ongoing-tasks/external-replication)  
- [Hub/Sink Replication](../../server/ongoing-tasks/hub-sink-replication)  

### Extensions
- [Document Expiration](../../server/extensions/expiration)  

### Patching
- [Patch By Query](../../client-api/rest-api/queries/patch-by-query)  
