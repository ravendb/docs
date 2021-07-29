# Indexes: Indexing Counters

---

{NOTE: }

* To index counters, create a [static index](../../indexes/creating-and-deploying#static-indexes) 
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

{CODE-BLOCK:java }
protected void addMap(String map)
{CODE-BLOCK/}

`addMap` only indexes the counters with 
the specified name.  

Examples of indexes using the method:  

{CODE-TABS}
{CODE-TAB:java:AddMap index_1@Indexes\IndexingCounters.java /}
{CODE-TABS/}  
<br/>

### `CounterNamesFor`

While indexes inheriting from `AbstractIndexCreationTask` cannot index counter _values_, the `counterNamesFor()` 
method is available which returns the names of all counters for a specified document:  

{CODE-TABS}
{CODE-TAB:java:CounterNamesFor syntax@Indexes\IndexingCounters.java /}
{CODE-TABS/}

Example of index using `counterNamesFor()`:  

{CODE-TABS}
{CODE-TAB:java:Index index@Indexes\IndexingCounters.java /}
{CODE-TABS/}
{PANEL/}


{PANEL: Querying the Index}  

{CODE-TABS}
{CODE-TAB:java:Querying query1@Indexes\IndexingCounters.java /}

{CODE-TABS/}

{PANEL/}

## Related Articles  

### Indexes  
- [Indexing Related Documents](../../indexes/indexing-related-documents)  
- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)  

### Document Extensions  
- [Counters: Overview](../../document-extensions/counters/overview)  
