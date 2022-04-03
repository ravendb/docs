# Indexes: Multi-Map-Reduce Indexes
---

{NOTE: }

* A **Multi Map-Reduce** index allows aggregating data from several collections.  

* **The multi-map phase**  
  The index collects data from various collections as defined.  
  It indexes the defined fields in the collections requested and saves the information 
  to reduce trips to the disk every time a query uses the index.  

* **The reduce phase**  
  It then aggregates, or sorts, the data. The aggregated value can be queried on with very little cost, 
  as computations are done while indexing, and not at query time. 
  
* **Querying the index**  
  When a query is made, RavenDB returns the results directly from the index.  

* **Updating indexes upon document modifications:**  
  Whenever documents in the indexed collections are modified, the index is updated behind the scenes 
  so that queries remain inexpensive.  

Multi-Map-Reduce indexing combines:  

* [Multi Map Index](../indexes/multi-map-indexes)  
  Multiple Map functions are defined - one per indexed collection.

* [Map-Reduce Index](../indexes/map-reduce-indexes)  
  Data aggregation is done during indexing phase, providing a     computed value for all indexed collections.

In this page: 

* [Syntax](../indexes/multi-map-reduce-indexes#syntax)
* [Multi-Map-Reduce Sample](../indexes/multi-map-reduce-indexes#multi-map-reduce-sample)

{NOTE/}

{PANEL: Syntax}

{PANEL/}

{PANEL: Multi-Map-Reduce Sample}

In this sample we define the map phase on collections 'Employees', 'Companies' and 'Suppliers'.  
We then define the reduce phase.  

You can see this sample described in [Inside RavenDB - Multi-Map-Reduce Indexes](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/11-mapreduce-and-aggregations-in-ravendb#multimapreduce-indexes).

{CODE:csharp multi_map_reduce_LINQ@Indexes\MultiMapReduceIndexes.cs /}

Now define a session query.

{CODE:csharp multi-map-reduce-index-query@Indexes\MultiMapReduceIndexes.cs /}

{PANEL/}

{PANEL: }

{PANEL/}


## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
- [Map Indexes](../indexes/map-indexes)
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)
- [Multi-Map Indexes](../indexes/multi-map-indexes)

### Querying

- [Basics](../indexes/querying/basics)

### Studio

- [Create Map Index](../studio/database/indexes/create-map-index)
- [Create Multi-Map Index](../studio/database/indexes/create-multi-map-index)
- [Create Map-Reduce Index](../studio/database/indexes/create-map-reduce-index)

<br/>

## Code Walkthrough

- [Multi-Map-Reduce-Index](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-reduce-index#)
- [Map Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-index)
- [Map-Reduce Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-reduce-index)
