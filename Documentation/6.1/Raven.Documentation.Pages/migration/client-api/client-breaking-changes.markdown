# Migration: Client Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `6.1.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:
   * [`CounterBatchOperation` default increment Delta is 1](../../migration/client-api/client-breaking-changes#counterbatchoperation-default-increment-delta-is-1)  
   * [CmpXchg item can only be created with an index of 0](../../migration/client-api/client-breaking-changes#cmpxchg-item-can-only-be-created-with-an-index-of-0)  
   * [Dynamic Linq query cannot apply `.Any` with logical AND (`&&`)](../../migration/client-api/client-breaking-changes#dynamic-linq-query-cannot-apply-.any-with-logical-and-(&&))  
   * [`LoadDocument` must be provided with a collection name string](../../migration/client-api/client-breaking-changes#loaddocument-must-be-provided-with-a-collection-name-string)  
   * [Consistency in `null` handling](../../migration/client-api/client-breaking-changes#consistency-in-null-handling)  

{NOTE/}

---

{PANEL: `CounterBatchOperation` default increment Delta is 1}

When [CounterBatchOperation](../../client-api/operations/counters/counter-batch) is 
called to `Increment` a batch of counters, and `Delta` is not specified to indicate 
what value should be added to the counters, the operation will increment the counters 
by a default `Delta` of `1`.  

(The default `Delta` was `0` in previous RavenDB versions, we changed it in this 
version to make it consistent with the rest of the API.)

{CODE:csharp CounterBatchOperation@migration\BreakingChanges.cs /}

{PANEL/}

{PANEL: CmpXchg item can only be created with an index of 0}

Creating a [compare exchange item](../../client-api/operations/compare-exchange/put-compare-exchange-value) 
using `PutCompareExchangeValueOperation` is now possible only if the `index` parameter 
passed to the method is `0`.  

{CODE:csharp CmpXchg@migration\BreakingChanges.cs /}  

{PANEL/}

{PANEL: Dynamic Linq query cannot apply `.Any` with logical AND (`&&`)}

RavenDB does not support dynamic Linq queries (i.e. queries executed over auto indexes) when 
they attempt to apply multiple conditions using the `.Any` method with a logical AND (`&&`).  
The below query, for example, is *not supported*.  

{CODE-BLOCK:sql}
using (var store = new DocumentStore())
{
    var guestsCarryingSmallSilverSuitcases = session.Query<Guest>()
        // Applying .Any with && is not supported
        .Where(x => x.Suitcase.Any(p => p.Size == "Small" && p.Color == "Silver"))
        .ToList();
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: `LoadDocument` must be provided with a collection name String}

When [LoadDocument](../../indexes/indexing-related-documents) is used in an index query 
to load related documents, it is passed a **document ID** and a **collection name**.  

The collection name **must** be passed to `LoadDocument` as a regular string; attempting 
to pass it a dynamic expression instead of a string will fail with an `IndexCompilationException` 
exception and this message: `LoadDocument method has to be called with constant value as collection name`

The following query, for example, will fail because it attempts to build the collection name dynamically.  

{CODE-BLOCK:sql}
from doc in docs.Orders
select new {
    // This attempt to construct a collection name will fail with an exception
    ComapnyName = LoadDocument(doc.Company, doc.Company.Split('/')[0]).Name
}
{CODE-BLOCK/}

{NOTE: }
Note that the validation of the collection name string will be forced only during 
the creation of a **new** index. 
An attempt to update an existing index with a collection name expression rather 
than a string will fail but the failure will remain unnoticed and an exception 
will not be thrown.
{NOTE/}

{PANEL/}

{PANEL: Consistency in `null` handling}

`IAsyncDocumentSession.LoadAsync` and `IAsyncLazySessionsOperations.LoadAsync` now 
do not throw an exception when they are passed `null`, becoming consistent with the 
behavior of the majority of RavenDB's methods.  

{CODE-BLOCK:csharp}
// This will work without issues
IAsyncDocumentSession.LoadAsync(null)
{CODE-BLOCK/}

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
