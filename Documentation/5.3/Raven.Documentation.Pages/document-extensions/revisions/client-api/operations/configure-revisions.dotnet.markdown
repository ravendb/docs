# Operations: How to Configure Revisions

---

{NOTE: }

* [Revisions](../../../server/extensions/revisions) are snapshots of documents that are taken automatically each time a 
document is updated or deleted.  

* Revisions can be stored indefinitely, or they can be deleted when certain conditions are met. These conditions can be set 
using the Configure Revisions Operation.  

* In this page:  
  * [Syntax](../../../client-api/operations/revisions/configure-revisions#syntax)  
      * [RevisionsCollectionConfiguration](../../../client-api/operations/revisions/configure-revisions#revisionscollectionconfiguration)  
      * [RevisionsConfiguration](../../../client-api/operations/revisions/configure-revisions#revisionsconfiguration)  
      * [ConfigureRevisionsOperation](../../../client-api/operations/revisions/configure-revisions#configurerevisionsoperation)  
  * [Example](../../../client-api/operations/revisions/configure-revisions#example)  

{NOTE/}

---

{PANEL: Syntax}

The ConfigureRevisionsOperation modifies the revisions settings for a particular [database](../../../studio/database/settings/manage-database-group). 
Within that database, each [collection](../../../client-api/faq/what-is-a-collection) can have its own separate revisions 
settings.  

To configure the revisions settings for a database and/or the collections in that database, follow these steps:  

[1.](../../../client-api/operations/revisions/configure-revisions#revisionscollectionconfiguration) Create a 
`RevisionsCollectionConfiguration` object for each desired collection.  
[2.](../../../client-api/operations/revisions/configure-revisions#revisionsconfiguration) Put those 
`RevisionsCollectionConfiguration` objects in a `RevisionsConfiguration` object.  
[3.](../../../client-api/operations/revisions/configure-revisions#configurerevisionsoperation) Send that 
`RevisionsConfiguration` to the server.  
<br/>
### RevisionsCollectionConfiguration

This object contains the four revisions settings for a particular collection:  

{CODE-BLOCK: csharp}
public class RevisionsCollectionConfiguration
{
    public long? MinimumRevisionsToKeep { get; set; }
    public TimeSpan? MinimumRevisionAgeToKeep { get; set; }
    public bool Disabled { get; set; }
    public bool PurgeOnDelete { get; set; }
    public long? MaximumRevisionsToDeleteUponDocumentUpdate { get; set; }
}
{CODE-BLOCK/}

| Configuration Option | Type | Description | Default |
| - | - | - | - |
| **MinimumRevisionsToKeep** | `long` | The number of revisions to keep per document. <br> E.g. if `MinimumRevisionsToKeep` is set to 5 revisions 6 and on will be purged. <br> set to `null` to keep revisions with no number limit | `null` |
| **MinimumRevisionAgeToKeep** | `TimeSpan` <br> [`TimeSpan` format](https://docs.microsoft.com/en-us/dotnet/api/system.timespan) | The time span for which revisions should be kept. <br> E.g. if `MinimumRevisionAgeToKeep` is set to 1 month, revisions older than a month will be purged. <br> set to `null` to keep revisions with no time span limit | `null` |
| **Disabled** | `bool` | When `true`, the creation of revisions is disabled for documents of this collection | `false` |
| **PurgeOnDelete** | `bool` | When `true`, deleting a document will delete all its revisions as well | `false` |
| **MaximumRevisionsToDeleteUponDocumentUpdate ** | `long` | The maximum number of revisions to delete upon document update. <br> set to `null` for no maximum limit | `null` |

A revision is only deleted if both the `MinimumRevisionsToKeep` for that document is exceeded, **and** the revision is older 
than the `MinimumRevisionAgeToKeep` limit. The oldest revisions are deleted first.  

* By default both these options are set to `null`, meaning that an unlimited number of revisions will be saved 
indefinitely.  

* If only `MinimumRevisionsToKeep` is null, revisions will be deleted only when they are older than 
`MinimumRevisionAgeToKeep`.  

* If only `MinimumRevisionAgeToKeep` is null, revisions will be deleted each time there are more revisions than 
`MinimumRevisionsToKeep`.  

These deletions will only take place _when a new revision is added_ to a document. Until a new revision is added, that 
document's revisions can exceed these limits.  

---

### RevisionsConfiguration

This object contains a `Dictionary` of the revision settings for each collection in the database, plus an optional default 
configuration.  

{CODE-BLOCK: csharp}
public class RevisionsConfiguration
{
        public Dictionary<string, RevisionsCollectionConfiguration> Collections;
        public RevisionsCollectionConfiguration Default;
}
{CODE-BLOCK/}

| Property | Description | Default |
| - | - | - |
| **Collections** | A dictionary in which the keys are collection names, and the values are the corresponding configurations | `null` |
| **Default** | An optional default configuration that applies to any collection not listed in `Collections` | `null` |

Note that when this object is sent to the server, it overrides the configurations for **all** collections, including all existing 
configurations currently on the server. If a collection is not listed in `Collections` and `Default` has not been set, the 
default values listed in the table [above](../../../client-api/operations/revisions/configure-revisions#revisionscollectionconfiguration) 
are applied.  

---

### ConfigureRevisionsOperation

Lastly, the operation itself sends the `RevisionsConfiguration` to the server, overriding **all** existing collection configurations. 
You'll want to store these configurations on the client side so they don't have to be created from scratch each time you want to 
modify them.  

{CODE-BLOCK: csharp}
public ConfigureRevisionsOperation(RevisionsConfiguration configuration);
{CODE-BLOCK/}

| Parameter | Description |
| - | - |
| **configuration** | The new revision settings for a particular database |

{PANEL/}

{PANEL: Example}

The following code sample updates the settings of the Document Store's [default database](../../../client-api/setting-up-default-database) 
- which in this case is a database named "Northwind". To update the configuration of different database, use the 
[`ForDatabase()` method](../../../client-api/operations/how-to/switch-operations-to-a-different-database).

{CODE-TABS}
{CODE-TAB:csharp:Sync operation_sync@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TAB:csharp:Async operation_async@DocumentExtensions\Revisions\ClientAPI\Operations\ConfigureRevisions.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What Are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [What Is a Collection](../../../client-api/faq/what-is-a-collection)
- [What Are Operations](../../../client-api/operations/what-are-operations)
- [Switch Operation Database](../../../client-api/operations/how-to/switch-operations-to-a-different-database)
- [Setting Up a Default Database](../../../client-api/setting-up-default-database)

### Server

- [Revisions](../../../server/extensions/revisions)

### Studio

- [Manage Database Group](../../../studio/database/settings/manage-database-group)
