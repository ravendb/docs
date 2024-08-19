# Migration: Client Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `6.1.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:
   * [Consistency in `null` handling](../../migration/client-api/client-breaking-changes#consistency-in-null-handling)  
   * [`CounterBatchOperation` default increment value](../../migration/client-api/client-breaking-changes#counterbatchoperation-default-increment-value)  
   * [CmpXchg item can only be created with its index set to `0`](../../migration/client-api/client-breaking-changes#cmpxchg-item-can-only-be-created-with-its-index-set-to-0)  
   * [Dynamic Linq query cannot apply `.Any` with `&&`](../../migration/client-api/client-breaking-changes#dynamic-linq-query-cannot-apply-.any-with-&&)  
   * [`LoadDocument` must be provided with a collection name string](../../migration/client-api/client-breaking-changes#loaddocument-must-be-provided-with-a-collection-name-string)  

{NOTE/}

---

{PANEL: Consistency in `null` handling}

`IAsyncDocumentSession.LoadAsync` and `IAsyncLazySessionsOperations.LoadAsync` now 
do not throw an exception when they are passed `null`, becoming consistent with the 
behavior of the majority of RavenDB's methods.  

{CODE-BLOCK:csharp}
// This will work without issues
IAsyncDocumentSession.LoadAsync(null)
{CODE-BLOCK/}

{PANEL/}

{PANEL: `CounterBatchOperation` default increment value}

When [CounterBatchOperation](../../client-api/operations/counters/counter-batch) is 
called without providing it with a `Delta` value, it will increment counters by a default 
`Delta` of `1`.  

{CODE:csharp CounterBatchOperation@migration\BreakingChanges.cs /}

{PANEL/}

{PANEL: CmpXchg item can only be created with its index set to `0`}

Creating a [compare exchange item](../../client-api/operations/compare-exchange/put-compare-exchange-value) 
using `PutCompareExchangeValueOperation` is now possible only if the item's initial index is set to 0.  
(It **is** possible to update an **existing** compare exchange item with an index other than 0.)

{CODE:csharp CmpXchg@migration\BreakingChanges.cs /}  

{PANEL/}

{PANEL: Dynamic Linq query cannot apply `.Any` with `&&`}

RavenDB does not support dynamic Linq queries (that are executed over auto indexes), when 
the query attempts to apply the `.Any` method with a logical AND (`&&`) over multiple parameters.  
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
the creation of a **new** index. Updating an existing index with a collection name 
expression instead of a string will not work, but will not be checked either and 
an exception will not be generated.
{NOTE/}

{PANEL/}

## Related Articles

### Installation
- [Setup Wizard](../../start/installation/setup-wizard)  
- [System Requirements](../../start/installation/system-requirements)  
- [Running in a Docker Container](../../start/installation/running-in-docker-container)  

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
