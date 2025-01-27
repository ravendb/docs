# Migration: Client Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `7.0`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:
   * [Creating a subscription with a predicate is done with dedicated class and functions](../../migration/client-api/client-breaking-changes#creating-a-subscription-with-a-predicate-is-done-with-dedicated-class-and-functions)  
   * [HTTP-Compression algorithm is now `zstd` by default](../../migration/client-api/client-breaking-changes#http-compression-algorithm-is-now-zstd-by-default)  
   * [Bulk-insert Compression is now Enabled by default](../../migration/client-api/client-breaking-changes#bulk-insert-compression-is-now-enabled-by-default)  
   * [Removed irrelevant `SingleNodeBatchCommand` parameters](../../migration/client-api/client-breaking-changes#removed-irrelevant-singlenodebatchcommand-parameters)  
   * [Removed obsolete methods](../../migration/client-api/client-breaking-changes#removed-obsolete-methods)  
   * [`FromEtl` is now internal](../../migration/client-api/client-breaking-changes#frometl-is-now-internal)  

{NOTE/}

---

{PANEL: Creating a subscription with a predicate is done with dedicated class and functions}

RavenDB versions earlier than `7.0` allow the creation of a data subscription using the 
`create` method with both a query and a predicate.  
To prevent errors and confusions that this duplicity may cause, RavenDB now requires the 
usage of dedicated class and methods for the creation of a subscription with a predicate.  

{CONTENT-FRAME: `SingleNodeBatchCommand` signature:}
{CODE-TABS}
{CODE-TAB-BLOCK:plain:Sync}
public string Create<T>
    (Expression<Func<T, bool>> predicate,
     PredicateSubscriptionCreationOptions options = null,
     string database = null){CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:plain:Async}
public Task<string> CreateAsync<T>
    (Expression<Func<T, bool>> predicate,
     PredicateSubscriptionCreationOptions options = null,
     string database = null,
     CancellationToken token = default)
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:plain:Class}
public sealed class PredicateSubscriptionCreationOptions : ISubscriptionCreationOptions
{
    public string Name { get; set; }
    public string ChangeVector { get; set; }
    public string MentorNode { get; set; }
    public bool Disabled { get; set; }
    public bool PinToMentorNode { get; set; }
    public ArchivedDataProcessingBehavior? ArchivedDataProcessingBehavior { get; set; }

    internal SubscriptionCreationOptions ToSubscriptionCreationOptions()
    {
        return new SubscriptionCreationOptions
        {
            Name = Name,
            ChangeVector = ChangeVector,
            MentorNode = MentorNode,
            PinToMentorNode = PinToMentorNode,
            Disabled = Disabled,
            ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior
        };
    }
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{CONTENT-FRAME/}

{PANEL/}

{PANEL: HTTP-Compression algorithm is now `zstd` by default}
From RavenDB `7.0` on, the default HTTP compression algorithm is `zstd` 
(instead of `Gzip`, used in earlier versions).  

{CONTENT-FRAME: To switch the HTTP-compression algorithm:}
Clients can switch to a different HTTP-Compression algorithm using `DocumentStore`'s 
[DocumentConventions.HttpCompressionAlgorithm](../../client-api/configuration/conventions#httpcompressionalgorithm) 
convention.  
{CODE SwitchCompressionAlgorithm@migration\migration.cs /}
{CONTENT-FRAME/}

{WARNING: }
If you migrate from an earlier RavenDB version to vertion `7.0` or higher, 
please note the [potential significance of this change](../../migration/client-api/client-migration#client-migration-to-ravendb-7.x).  
{WARNING/}

{PANEL/}

{PANEL: Bulk-insert Compression is now Enabled by default}

Compression is now [Enabled by default for bulk-insert operations](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#section).  

{CODE-BLOCK:csharp }
 CompressionLevel DefaultCompressionLevel = CompressionLevel.Fastest;
{CODE-BLOCK/}

{CONTENT-FRAME: To switch the bulk-insert state:}
Clients can switch to a different bulk-insert compression state using `Store`'s 
[BulkInsertOptions.CompressionLevel](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) 
option.  
{CODE switchBulkInsertState@migration\migration.cs /}
{CONTENT-FRAME/}

{PANEL/}

{PANEL: Removed irrelevant `SingleNodeBatchCommand` parameters}
We removed from 
[SingleNodeBatchCommand](../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)'s 
definition parameters that are used mainly internally and left only ones that are relevant to the user.  

{CONTENT-FRAME: `SingleNodeBatchCommand` signature:}
{CODE-TABS}
{CODE-TAB-BLOCK:plain:7.0_and_on}
public SingleNodeBatchCommand
    (DocumentConventions conventions, 
     IList<ICommandData> commands, 
     BatchOptions options = null)
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:plain:up_to_6.2}
public SingleNodeBatchCommand
    (DocumentConventions conventions, 
     JsonOperationContext context, 
     IList<ICommandData> commands, 
     BatchOptions options = null, 
     TransactionMode mode = TransactionMode.SingleNode)
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{CONTENT-FRAME/}

{PANEL/}

{PANEL: Removed obsolete methods}
The following methods are no longer in use and have been removed from RavenDB `7.0`.  

* `NextPageStart`  
  {CODE-BLOCK:csharp }
   public int NextPageStart { get; set; }
  {CODE-BLOCK/}

* `GenerateEntityIdOnTheClient`  
  {CODE-BLOCK:csharp }
   public GenerateEntityIdOnTheClient(DocumentConventions conventions, Func<object, string> generateId)
  {CODE-BLOCK/}

* `InMemoryDocumentSessionOperations.GenerateId`  
  {CODE-BLOCK:csharp }
   protected override string GenerateId(object entity)
  {CODE-BLOCK/}

* `InMemoryDocumentSessionOperations.GetOrGenerateDocumentIdAsync`  
  {CODE-BLOCK:csharp }
   protected async Task<string> GetOrGenerateDocumentIdAsync(object entity)
  {CODE-BLOCK/}

{PANEL/}

{PANEL: `FromEtl` is now internal}
The `CounterBatch` class `FromEtl` property is now **internal**.  
`FromEtl` is used internally to get or set a value indicating whether a counters batch 
originated from an ETL process.
{PANEL/}

## Related Articles

### Counters
- [Counters](../../document-extensions/counters/overview)  
- [CounterBatchOperation](../../client-api/operations/counters/counter-batch)  

### Compare Exchange
- [Compare Exchange](../../client-api/operations/compare-exchange/overview)  
- [Put compare exchange op](../../client-api/operations/compare-exchange/put-compare-exchange-value)  

### Querying
- [Query Overview](../../client-api/session/querying/how-to-query) 
- [What is RQL](../../client-api/session/querying/what-is-rql)  

### Indexes
- [What are Indexes](../../indexes/what-are-indexes)  
- [Indexing Basics](../../indexes/indexing-basics)  
- [LoadDocument](../../indexes/indexing-related-documents)  
