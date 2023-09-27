# Breaking Changes
---

{NOTE: }

This page lists RavenDB 6.0 features whose behavior is inconsistent with their behavior 
in previous (5.4 and lower) versions.  

* In this page:
   * [Breaking Features in a Sharded Database](../start/breaking-changes#breaking-features-in-a-sharded-database)  
   * [Breaking Features in a Non-Sharded Database](../start/breaking-changes#breaking-features-in-a-non-sharded-database)  

{NOTE/}

{PANEL: Breaking Features in a Sharded Database}

#### EntireDatabasePendingDeletion()

moved from: `DatabaseTopology.EntireDatabasePendingDeletion`  
moved to: `RawDatabaseRecord.EntireDatabasePendingDeletion`  

---

#### Attachments

[session.Advanced.Attachments.Move](../sharding/unsupported#unsupported-document-extensions-features)  
[session.Advanced.Attachments.Copy](../sharding/unsupported#unsupported-document-extensions-features)  
Trying to use these methods in a sharded database will throw a `NotSupportedInShardingException` exception.  
For a list of features that are not supported under a sharded database [visit this page](../sharding/unsupported).  

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

### Export and Smuggler return a sharded result format

Smuggler and the methods that use it return a sharded-specific results format.  

{CODE-BLOCK: csharp}
public sealed class ShardedSmugglerResult 
{
    public List<ShardNodeSmugglerResult> Results { get; set; }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Breaking Features in a Non-Sharded Database}

* Only **Public** fields are serialized/deserialized.  
  Private fields are **not** serialized/deserialized.  

  E.g.,
  {CODE-BLOCK: JSON}
  public class Item
  {

     // Will be serialized/deserialized
     public Item1(int a)
     {
        content = a;
     }

     // Will Not be serialized/deserialized
     private Item2(int a)
     {
        content = a;
     }

   }
  
  {CODE-BLOCK/}

* `DefaultAsyncHiLoIdGenerator ` is **removed** in RavenDB 6.0.  

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
