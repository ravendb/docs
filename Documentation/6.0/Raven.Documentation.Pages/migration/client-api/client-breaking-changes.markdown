# Migration: Client Breaking Changes
---

{NOTE: }
The features listed on this page were available in former RavenDB versions 
and are unavailable in RavenDB `6.x`, or their behavior is inconsistent with 
their behavior in previous versions.  

* In this page:
   * [Breaking Changes](../../migration/client-api/client-breaking-changes#breaking-changes)  
   * [Breaking Changes in a Sharded Database](../../migration/client-api/client-breaking-changes#breaking-changes-in-a-sharded-database)  
   * [Additional breaking changes](../../migration/client-api/client-breaking-changes#additional-breaking-changes)  

{NOTE/}

{PANEL: Breaking Changes}

* **Include from a Non-tracking session**  
  A non-tracking session will now throw the below exception if an 'Include' operation is 
  registered in it, to indicate that the operation is forbidden from a non-tracking session 
  and warn about its expected results.  
  `"This session does not track any entities, because of that registering includes is forbidden  
  to avoid false expectations when later load operations are performed on those and no requests 
  are being sent to the server. Please avoid any 'Include' operations during non-tracking session 
  actions like load or query."`

* **Type Changes**  
  Many methods related to paging information (Skip, Take, PageSize, TotalResults, etc.) that used the `int` type in former RavenDB versions now use `long`, 
  e.g. `QueryStatistics.TotalResults`

* **Indexing**  
  The default value of the 
  [Indexing.OrderByTicksAutomaticallyWhenDatesAreInvolved](../../server/configuration/indexing-configuration#indexing.orderbyticksautomaticallywhendatesareinvolved) 
  configuration option is now `true`  

* **Facets**  
 `FacetOptions` were removed from `RangeFacet`  

* **Obsolete entities removed**  
  Many obsolete attributes, properties, and methods were removed from the public API 

* A `DisposeCertificate` convention has been added to prevent/allow the disposal of 
  `DocumentStore.Certificate` when the store is disposed of, to help users mitigate the 
  X509Certificate2 leak.  
  More info [here](https://snede.net/the-most-dangerous-constructor-in-net/).  

* **Serialization**  
  Only **Public** fields are serialized/deserialized when doing a projection. 
  Private fields are **not** serialized/deserialized.  

* **`DefaultAsyncHiLoIdGenerator` is replaced with `AsyncHiLoIdGenerator`**  
  {CODE-BLOCK: csharp}
  public AsyncHiLoIdGenerator(string tag, DocumentStore store, string dbName, char identityPartsSeparator)
  {CODE-BLOCK/}


{PANEL/}

{PANEL: Breaking Changes in a Sharded Database}

### Unsupported Features  

RavenDB 6.0 introduces [sharding](../../sharding/overview).  
Features that are currently unavailable under a sharded database 
(but remain available in regular databases) are listed in the 
sharding [unsupported features](../../sharding/unsupported) page.  
Attempting to use these features when the database is sharded will normally 
throw a `NotSupportedInShardingException` exception.  

---

### Casting Smuggler Results  

The result sets returned by Smuggler and features that use it (import, export, Backup 
and Restore) are sharding-specific and should not be cast using a non-sharded type.  

The following code, for example, **will fail** when the database is sharded.  
{CODE-BLOCK: csharp}
var operation = await store.Maintenance.SendAsync(new BackupOperation(config));
var result = await operation.WaitForCompletionAsync(TimeSpan.FromSeconds(60));

// This will fail with a sharded database
var backupResult = (BackupResult)result;
{CODE-BLOCK/}

For the code to succeed, replace the last line with:  
{CODE-BLOCK: csharp}
var backupResult = (ShardedBackupResult)result;
{CODE-BLOCK/}

{INFO: Sharding-specific types}

* For Backup: `ShardedBackupResult`  
* For Restore: `ShardedRestoreResult`  
* For Smuggler, Import, and Export: `ShardedSmugglerResult`  

{INFO/}

---

### Endpoints

* `GET /databases/*/revisions/bin`  
  Pass `start` instead of `etag`  
  We now fetch from the last entry backward rather than from a specific etag.  
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

{PANEL/}

{PANEL: Additional breaking changes}

* [Failed to project when having a backing private field](https://issues.hibernatingrhinos.com/issue/RavenDB-18657)  
* [Unexpected results on projection query on a static index with a dictionary](https://issues.hibernatingrhinos.com/issue/RavenDB-19560)  
* [Check that query relays on PropertyNameConverter to get property name](https://issues.hibernatingrhinos.com/issue/RavenDB-19209)  
* [Overflow of ReduceAttempts](https://issues.hibernatingrhinos.com/issue/RavenDB-19729)  
* [Review all Obsolete() attributes in 6.0](https://issues.hibernatingrhinos.com/issue/RavenDB-19989)  
* [Inconsistency in `BlittableJsonTraverserHelper.TryRead` when trying to filter docs with nested non-existing field](https://issues.hibernatingrhinos.com/issue/RavenDB-19856)  
* [Removed non-typed inheritance from typed incremental time series client API](https://issues.hibernatingrhinos.com/issue/RavenDB-19511)  
* [`CollectionStatistics` properties are now `long`](https://issues.hibernatingrhinos.com/issue/RavenDB-19602)  
* [Removed private fields from the projection](https://issues.hibernatingrhinos.com/issue/RavenDB-18865)  
* [DocumentConventions.UseCompression is not respected by BlittableJsonContent](https://issues.hibernatingrhinos.com/issue/RavenDB-20057)  
* [LINQ projection convention is not applied inside javascript projection when querying static index](https://issues.hibernatingrhinos.com/issue/RavenDB-17708)  
* [RavenBooleanQuery - merging two RavenBooleanQuery requires an additional boosting check](https://issues.hibernatingrhinos.com/issue/RavenDB-20449)  
* [Index deletion is prevented when index lock mode is locked](https://issues.hibernatingrhinos.com/issue/RavenDB-19239)  
* [Wrong query result with CamelCasePropertyNamesContractResolver](https://issues.hibernatingrhinos.com/issue/RavenDB-20634)  
* [Compare Exchange includes do not override compare exchange values that are already tracked in the session](https://issues.hibernatingrhinos.com/issue/RavenDB-21069)  

{PANEL/}

## Related Articles

### Installation
- [Setup Wizard](../../start/installation/setup-wizard)  
- [System Requirements](../../start/installation/system-requirements)  
- [Running in a Container](../../start/containers/overview)  

### Session
- [Introduction](../../client-api/session/what-is-a-session-and-how-does-it-work)  

### Querying
- [Query Overview](../../client-api/session/querying/how-to-query) 
- [What is RQL](../../client-api/session/querying/what-is-rql)  

### Indexes
- [What are Indexes](../../indexes/what-are-indexes)  
- [Indexing Basics](../../indexes/indexing-basics)  

### Sharding
- [Overview](../../sharding/overview)  
- [Migration](../../sharding/migration)  
