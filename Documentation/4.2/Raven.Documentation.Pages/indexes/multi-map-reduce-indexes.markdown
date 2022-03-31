# Indexes: Multi-Map-Reduce Indexes
---

{NOTE: }

* A **Multi Map-Reduce** index allows aggregating data from several collections.  

* **The map phase**  
  The index collects data from various collections as defined.  
  It indexes the defined fields in the collections requested and saves the information 
  to reduce trips to the disk every time a query uses the index.  

* **The reduce phase**  
  It then aggregates, or sorts, the data.  

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

In this sample we define the map phase on collections 'Company', 'Supplier' and 'Order'.  
We then define the reduce phase.  

You can see this sample in RavenDB's [Code Walkthrough on Multi-Map-Reduce Index](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-reduce-index#) 
where you can see the code logic step-by-step, play with variables, and run the index in RavenDB's studio.  

{CODE:csharp Multi-Map-Reduce-LINQ@Indexes\MultiMapReduceIndexes.cs /}

After defining the Multi-Map-Reduce index,  
define a session query.

{CODE:csharp multi-map-reduce-index-query@Indexes\MultiMapReduceIndexes.cs /}

{PANEL/}

{PANEL: }

{PANEL/}


## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying

- [Basics](../indexes/querying/basics)

### Studio

- [Create Map-Reduce Index](../studio/database/indexes/create-map-reduce-index)

<br/>

## Code Walkthrough

- [Multi-Map-Reduce-Index](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-reduce-index#)
