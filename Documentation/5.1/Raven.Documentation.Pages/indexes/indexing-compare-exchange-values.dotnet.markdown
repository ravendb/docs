# Indexes: Indexing Compare Exchange Values

---

{NOTE: }

* Compare exchange values can be loaded within an index using the value's key.  

* The index will update when the compare exchange value is updated, as well 
as when documents are modified in the indexed collection(s).  

* In this page:  
  * [Syntax](../indexes/indexing-compare-exchange-values#syntax)  
  * [Examples](../indexes/indexing-compare-exchange-values#examples)  
  * [Querying the Index](../indexes/indexing-compare-exchange-values#querying-the-index)  

{NOTE/}

---

{PANEL: Syntax}

When creating an index using `AbstractIndexCreationTask`, use the method 
`LoadCompareExchangeValue()` to load a compare exchange value by its key.  

{CODE:csharp methods@Indexes/IndexingCompareExchange.cs /}

For javascript indexes, use the method `cmpxchg(<key>)`.

| Parameter | Type | Description |
| - | - | - |
| **key** | `string` | The key of a particular compare exchange value. |
| **keys** | `IEnumerable<string>` | The keys of multiple compare exchange values. |
<br/>
### Examples

These indexes map the rooms in a hotel, as well as compare exchange values 
representing the guests in those rooms.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ-syntax index_0@Indexes/IndexingCompareExchange.cs /}
{CODE-TAB:csharp:JavaScript-syntax index_1@Indexes/IndexingCompareExchange.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Querying the Index}

{CODE-TABS}
{CODE-TAB:csharp:Sync query_0@Indexes/IndexingCompareExchange.cs /}
{CODE-TAB:csharp:Async query_1@Indexes/IndexingCompareExchange.cs /}
{CODE-TAB:csharp:RawQuery query_2@Indexes/IndexingCompareExchange.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client-API

- [Compare Exchange Overview](../client-api/operations/compare-exchange/overview)

### Indexes

- [What Are Indexes](../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
