# Migration: Client Breaking Changes
---

{NOTE: }
The features listed on this page were available in former RavenDB versions.  
In RavenDB `7.0`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:
   * [Subscription creation overload modification](../../migration/client-api/client-breaking-changes#subscription-creation-overload-modification)  
   * [HTTP-Compression algorithm is now `Zstd` by default](../../migration/client-api/client-breaking-changes#http-compression-algorithm-is-now-zstd-by-default)  
   * [Bulk-insert Compression is now Enabled by default](../../migration/client-api/client-breaking-changes#bulk-insert-compression-is-now-enabled-by-default)  
   * [Removed irrelevant `SingleNodeBatchCommand` parameters](../../migration/client-api/client-breaking-changes#removed-irrelevant-singlenodebatchcommand-parameters)  
   * [Removed obsolete methods](../../migration/client-api/client-breaking-changes#removed-obsolete-methods)  
   * [`FromEtl` is now internal](../../migration/client-api/client-breaking-changes#frometl-is-now-internal)  

{NOTE/}

---

{PANEL: Subscription creation overload modification}

* In RavenDB versions earlier than **7.0**, the `Create<T>` method overload that accepted a predicate also allowed specifying a query through `SubscriptionCreationOptions`, 
  which could cause errors and confusion.
* To eliminate this ambiguity, starting from **7.0**, the `Create<T>` overload for predicate-based subscriptions now accepts `PredicateSubscriptionCreationOptions`,
  which no longer includes a `Query` property.
* Refer to the [Subscription creation API overview](../../client-api/data-subscriptions/creation/api-overview) for the complete list of available `Create` method overloads.

{CODE-TABS}
{CODE-TAB:csharp:7.0_and_on create_1@migration\migration.cs /}
{CODE-TAB:csharp:up_to_6.2 create_2@migration\migration.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: HTTP-Compression algorithm is now `Zstd` by default}
From RavenDB `7.0` on, the default HTTP compression algorithm is `Zstd` (instead of `Gzip`, used in earlier versions).  

{CONTENT-FRAME: To switch the HTTP-compression algorithm:}

Clients can switch to a different HTTP-Compression algorithm using `DocumentStore`'s 
[DocumentConventions.HttpCompressionAlgorithm](../../client-api/configuration/conventions#httpcompressionalgorithm) convention.  

{CODE SwitchCompressionAlgorithm@migration\migration.cs /}

{CONTENT-FRAME/}

{WARNING: }
If you migrate from an earlier RavenDB version to version `7.0` or higher,  
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
[BulkInsertOptions.CompressionLevel](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation#bulkinsertoptions) option.  

{CODE switchBulkInsertState@migration\migration.cs /}

{CONTENT-FRAME/}

{PANEL/}

{PANEL: Removed irrelevant `SingleNodeBatchCommand` parameters}

We removed from [SingleNodeBatchCommand](../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)'s
definition the parameters that are mainly used internally and kept only those relevant to the user.

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

The following methods are no longer used and have been removed from RavenDB `7.0`.  

* `NextPageStart`  
  {CODE-BLOCK:csharp }
   public int NextPageStart { get; set; }
  {CODE-BLOCK/}

* `GenerateEntityIdOnTheClient`  
  {CODE-BLOCK:csharp }
   public GenerateEntityIdOnTheClient(DocumentConventions conventions,
                                      Func<object, string> generateId)
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
`FromEtl` is used internally to get or set a value indicating whether a counters batch originated from an ETL process.
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
