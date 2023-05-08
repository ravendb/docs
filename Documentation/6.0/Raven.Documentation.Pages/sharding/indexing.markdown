﻿# Sharding: Indexing
---

{NOTE: }

* Indexing a sharded database is performed locally, per shard.  
  There is no multi-shard indexing process.  
* Indexes use the same syntax in sharded and non-sharded databases.  
* Most indexing features supported by non-sharded databases 
  are also supported by sharded databases. Unsupported features are listed below.  

* In this page:  
  * [Indexing](../sharding/indexing#indexing)  
  * [Map-Reduce Indexes on a Sharded Database](../sharding/indexing#map-reduce-indexes-on-a-sharded-database)  
  * [Unsupported Indexing Features](../sharding/indexing#unsupported-indexing-features)  

{NOTE/}

---

{PANEL: Indexing}

Indexing each database shard is basically similar to indexing a non-sharded database.  
As each shard holds and manages a unique dataset, indexing is performed 
per-shard and indexes are stored only on the shard that created and uses them.  

## Map-Reduce Indexes on a Sharded Database

Map-reduce indexes on a sharded database are used to reduce data both over each 
shard during indexation, and on the orchestrator machine each time a query uses them.  

1. **Reduction by each shard during indexation**  
   Similarly to non-sharded databases, when shards index their data they reduce 
   the results by map-reduce indexes.  
2. **Reduction by the orchestrator during queries**  
   When a query is executed over map-reduce indexes the orchestrator 
   distributes the query to the shards, collects and combines the results, 
   and then reduces them again.  
  
{NOTE: }
Learn about **querying map-reduce indexes** in a sharded database [here](../sharding/querying#orderby-in-a-map-reduce-index).  
{NOTE/}

## Unsupported Indexing Features

Unsupported or yet-unimplemented indexing features include: 

* **Rolling index deploymeny**  
  [Rolling index deploymeny](../indexes/rolling-index-deployment) 
  is not supported in a Sharded Database.  
* **Loading documents from other shards**  
  Loading a document during indexing is possible only if the document 
  resides on the shard.  
  Consider the below index, for example, that attempts to load a document.  
  If the requested document is stored on a different shard, the load operation 
  will be ignored.  
  {CODE-BLOCK:csharp}
  Map = products => from product in products
                          select new Result
                          {
                              CategoryName = LoadDocument<Category>(product.Category).Name
                          };
  {CODE-BLOCK/}
  {NOTE: }
  You can make sure that documents share a bucket and a shard, and can 
  therefore locate and load each other, using the 
  [$ syntax](../sharding/overview#pairing-documents).  
  {NOTE/}
* **Map-Reduce Output Documents**  
  Using [OutputReduceToCollection](../indexes/map-reduce-indexes#map-reduce-output-documents) 
  to output the results of a map-reduce index to a collection 
  is not supported in a Sharded Database.  
* [Custom Sorters](../indexes/querying/sorting#creating-a-custom-sorter) 
  are not supported in a Sharded Database.  




{PANEL/}

## Related articles

### Indexing
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)  
- [Indexing Basics](../indexes/indexing-basics)  
- [Rolling index deployment](../indexes/rolling-index-deployment)  
- [Map-Reduce Output Documents](../indexes/map-reduce-indexes#map-reduce-output-documents)  

### Sharding
- [Pairing Documents](../sharding/overview#pairing-documents)  
- [Querying](../sharding/querying)  

