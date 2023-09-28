# Breaking Changes
---

{NOTE: }

This page lists RavenDB 6.0 features whose behavior is inconsistent with their behavior 
in previous (5.4 and lower) versions.  

* In this page:
   * [Breaking Changes in a Sharded Database](../start/breaking-changes#breaking-changes-in-a-sharded-database)  
   * [Breaking Changes in a Non-Sharded Database](../start/breaking-changes#breaking-changes-in-a-non-sharded-database)  

{NOTE/}

{PANEL: Breaking Changes in a Sharded Database}

#### Attachments

The client API does not support [session.Advanced.Attachments.Move](../sharding/unsupported#unsupported-document-extensions-features)  
and [session.Advanced.Attachments.Copy](../sharding/unsupported#unsupported-document-extensions-features) 
when the database is sharded.  
Running these methods will throw a `NotSupportedInShardingException` exception.  
For a full list of features that are not supported under a sharded database [visit this page](../sharding/unsupported).  

---

### When casting a Smuggler Result Set, Use a Sharding-Specific Type

**Smuggler** and the features that use it (**Backup**, **Import**, and **Export**) return result 
sets using sharding-specific types. Casting such a result set with a non-sharded type fill fail.  

* The following sample, for example, will **fail** when the database is sharded.  
  {CODE-BLOCK: csharp}
  var operation = await store.Maintenance.SendAsync(new BackupOperation(config));
  var result = await operation.WaitForCompletionAsync(TimeSpan.FromSeconds(60));

  // This will fail with a sharded database
  var backupResult = (BackupResult)result;
  {CODE-BLOCK/}

* For the code to succeed, replace the last line with:  
  {CODE-BLOCK: csharp}
  var backupResult = (ShardedBackupResult)result;
  {CODE-BLOCK/}

{INFO: Sharding-specific types}

* For Backup: `ShardedBackupResult`  
* For Restore: `ShardedRestoreResult`  
* For Smuggler, Import, and Export: `ShardedSmugglerResult`  

{INFO/}

---

#### Endpoints

* `GET /databases/*/revisions/bin`  
  pass `start` instead of `etag`, we now fetch from last entry backwards rather than from a specific etag.  
  {INFO: }
  `GetRevisionsBinEntryCommand`  
  Previous definition: `GetRevisionsBinEntryCommand(long etag, int? pageSize)`  
  Current definition: `GetRevisionsBinEntryCommand(int start, int? pageSize)`  
  {INFO/}

* `GET /databases/*/indexes/terms`  
  The `Terms` field of the returned results is now `List<String>` instead of `HashSet<String>`
  {CODE-BLOCK: JSON}
  public class TermsQueryResult
  {
     public List<string> Terms { get; set; }
     public long ResultEtag { get; set; }
     public string IndexName { get; set; }
  }
  {CODE-BLOCK/}

* `GET database/*/debug/documents/get-revisions`  
  Operation parameter changed from `etag` to `start`  

---

#### EntireDatabasePendingDeletion()

moved from: `DatabaseTopology.EntireDatabasePendingDeletion`  
moved to: `RawDatabaseRecord.EntireDatabasePendingDeletion`  

{PANEL/}

{PANEL: Breaking Changes in a Non-Sharded Database}

* Only **Public** fields are serialized/deserialized.  
  Private fields are **not** serialized/deserialized.  

* The `DefaultAsyncHiLoIdGenerator` class is **removed** from RavenDB 6.0.  

* Changing `int` to `long` in IndexStorage for fields that are already stored as `long`.  

* Removed obsolete attributes, properties, and methods in the public API.  

{PANEL/}

## Related Articles

### Installation
- [Setup Wizard](../start/installation/setup-wizard)  
- [System Requirements](../start/installation/system-requirements)  
- [Running in a Docker Container](../start/installation/running-in-docker-container)  

### Session
- [Introduction](../client-api/session/what-is-a-session-and-how-does-it-work)  

### Querying
- [Query Overview](../client-api/session/querying/how-to-query) 
- [What is RQL](../client-api/session/querying/what-is-rql)  

### Indexes
- [What are Indexes](../indexes/what-are-indexes)  
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)  
- [Indexing Basics](../indexes/indexing-basics)  
- [Map Indexes](../indexes/map-indexes)  

### Sharding
- [Overview](../sharding/overview)  
- [Migration](../sharding/migration)  
