# Indexes: Indexing Counters
---

{NOTE: }

* To index counters, create a [static index](../indexes/creating-and-deploying#static-indexes) 
that inherits from `AbstractCountersIndexCreationTask`.  

* Auto-indexes for counters are not available at this time.  

* In this page:  
  * [Syntax](../../document-extensions/counters/indexing#syntax)  
  * [Querying the Index](../../document-extensions/counters/indexing#querying-the-index)  

{NOTE/}

---

{PANEL:Syntax}

In order to index counter values, create an index that inherits from `AbstractCountersIndexCreationTask`. 
Next, choose one of these two methods which take the index expression:  

{CODE-BLOCK:csharp }
protected void AddMapForAll(Expression<Func<IEnumerable<CounterEntry>, IEnumerable>> map)

protected void AddMap(string counter, Expression<Func<IEnumerable<CounterEntry>, IEnumerable>> map)
{CODE-BLOCK/}

`AddMapForAll` indexes all the counters in the indexed documents. `AddMap` only indexes the counters with 
the specified name.  

Examples of indexes using each method:  

{CODE-TABS}
{CODE-TAB:csharp:AddMap index_1@Indexes\IndexingCounters.cs /}
{CODE-TAB:csharp:AddMapForAll index_2@Indexes\IndexingCounters.cs /}
{CODE-TABS/}  
<br/>

### `CounterNamesFor`

While indexes inheriting from `AbstractIndexCreationTask` cannot index counter _values_, the `CounterNamesFor` 
method is available which returns the names of all counters for a specified document:  

{CODE:csharp syntax@Indexes\IndexingCounters.cs /}

Example of index using `CounterNamesFor`:  

{CODE:csharp index_0@Indexes\IndexingCounters.cs /}

{PANEL/}

{PANEL: Querying the Index}  

{CODE-TABS}
{CODE-TAB:csharp:Sync query1@Indexes\IndexingCounters.cs /}
{CODE-TAB:csharp:Async query2@Indexes\IndexingCounters.cs /}
{CODE-TAB:csharp:DocumentQuery query3@Indexes\IndexingCounters.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles  

### Indexes  
- [Indexing Related Documents](../indexes/indexing-related-documents)  
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)  
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)  

### Document Extensions  
- [Counters: Overview](../document-extensions/counters/overview)  
