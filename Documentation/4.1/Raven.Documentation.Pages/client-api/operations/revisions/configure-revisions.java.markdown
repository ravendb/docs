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

{CODE-BLOCK: java}
public class RevisionsCollectionConfiguration
{
    private boolean disabled;
    private Duration minimumRevisionAgeToKeep;
    private Long minimumRevisionsToKeep;
    private boolean purgeOnDelete;
}
{CODE-BLOCK/}

| Configuration Option | Description | Default |
| - | - | - |
| **minimumRevisionsToKeep** | The minimum number of revisions to keep per document | `null` - unlimited |
| **minimumRevisionAgeToKeep** | The minimum amount of time to keep each revision. [Format of `Duration`](https://docs.oracle.com/javase/8/docs/api/java/time/Duration.html). | `null` - unlimited |
| **disabled** | Indicates whether to completely disable revisions for documents in this collection | `false` |
| **purgeOnDelete** | When a document is deleted, this indicates whether all of its revisions should be deleted as well | `false` |

A revision is only deleted if both the `minimumRevisionsToKeep` for that document is exceeded, **and** the revision is 
older than the `minimumRevisionAgeToKeep` limit. The oldest revisions are deleted first.  

* By default both these options are set to `null`, meaning that an unlimited number of revisions will be saved 
indefinitely.  

* If only `minimumRevisionsToKeep` is null, revisions will be deleted only when they are older than 
`minimumRevisionAgeToKeep`.  

* If only `minimumRevisionAgeToKeep` is null, revisions will be deleted each time there are more revisions than 
`minimumRevisionsToKeep`.  

These deletions will only take place _when a new revision is added_ to a document. Until a new revision is added, that 
document's revisions can exceed these limits.  

---

### RevisionsConfiguration

This object contains a `Map` of revision configurations for each collection in the database, plus an optional default 
configuration.  

{CODE-BLOCK: java}
public class RevisionsConfiguration
{
     private Map<String, RevisionsCollectionConfiguration> collections;
     private RevisionsCollectionConfiguration defaultConfig;
}
{CODE-BLOCK/}

| Property | Description | Default |
| - | - | - |
| **collections** | A map in which the keys are collection names, and the values are the corresponding configurations | `null` |
| **defaultConfig** | An optional default configuration that applies to any collection not listed in `collections` | `null` |

Note that when this object is sent to the server, it overrides the configurations for **all** collections, including all existing 
configurations currently on the server. If a collection is not listed in `collections` and `defaultConfig` has not been set, the 
default values listed in the table [above](../../../client-api/operations/revisions/configure-revisions#revisionscollectionconfiguration) 
are applied.  

---

### ConfigureRevisionsOperation

Lastly, the operation itself sends the `RevisionsConfiguration` to the server, overriding **all** existing collection configurations. 
You'll want to store these configurations on the client side so they don't have to be created from scratch each time you want to 
modify them.  

{CODE-BLOCK: java}
public ConfigureRevisionsOperation(RevisionsConfiguration configuration);
{CODE-BLOCK/}

| Parameter | Description |
| - | - |
| **configuration** | The new revision settings for a particular database |

{PANEL/}

{PANEL: Example}

The following code sample updates the settings of the Document Store's [default database](../../../client-api/setting-up-default-database) 
- which in this case is a database named "Northwind". To update the configuration of different database, use the 
[`forDatabase()` method](../../../client-api/operations/how-to/switch-operations-to-a-different-database).

{CODE:java operation@ClientApi/Operations/Revisions/ConfigureRevisions.java /}

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
