# Sharding: Querying in a Sharded Database
---

{NOTE: }

* Queries use the same syntax in sharded and non-sharded databases.  
* Sharded databases support most querying features that non-sharded 
  database support, and retain similar behavior.  
  Please read below about unsupported or altered features.  

* In this page:  
  * [Querying Map Reduce Indexes](../sharding/querying#querying-map-reduce-indexes)  
     * [Filtering Results in a Sharded Database](../sharding/querying#filtering-results-in-a-sharded-database)  
     * [Projection](../sharding/querying#projection)  
     * [OrderBy in a Map Reduce Index](../sharding/querying#orderby-in-a-map-reduce-index)  
  * [Include](../sharding/querying#include)  
  * [Unsupported Querying Features](../sharding/querying#unsupported-querying-features)  
  
{NOTE/}

---

{PANEL: Querying Map Reduce Indexes}

* When [map reduce queries are executed over a sharded database](../sharding/indexing#map-reduce-indexes-in-a-sharded-database) 
  the results are reduced both by each shard during indexation **and** by 
  the orchestrator over the entire dataset after the results are collected from 
  all shards.  
* Read more below about querying map reduce indexes in a sharded database.  

## Filtering Results in a Sharded Database

Query results can be filtered either on each shard, or on the orchestrator 
machine after the results are collected by it from all shards.  

* Use [where](../indexes/querying/filtering#where) 
  to filter the results **on each shard**.  
  {CODE-BLOCK:csharp}
  var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                  .Where(x => x.Sum >= 30)
                  .ToList();
  {CODE-BLOCK/}
  In the sample above:  
    * Only documents whose `Sum` is greater than 30 are retrieved from the shards.  

* Use [Filter](../indexes/querying/exploration-queries#filter) 
  to filter the results **on the orchestrator** machine after it retrieves 
  the results from all shards.  
  {CODE-BLOCK:csharp}
  var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                  .Filter(x => x.Sum >= 30)
                  .ToList();
  {CODE-BLOCK/}
  In the sample above:
    * Documents with any `Sum` are retrieved from all shards  
    * When the documents arrive at the orchestrator machine those with values not 
      greater than 30 are filtered out.  

## Projection

[Loading a document within a map-reduce projection](../indexes/querying/projections#example-viii---projection-using-a-loaded-document) 
is **not supported** in a Sharded Database.  
  
Database response to an attempt to load a document from a map reduce projection:  
A `NotSupportedInShardingException` exception will be thrown, specifying 
"Loading a document inside a projection from a map-reduce index isn't supported".  

Unlike map-reduce index projections, projections of queries that use no index 
and projections of map indexes **can** load a document, 
[providing that the document is on this shard](../sharding/querying#unsupported-querying-features).  

| Projection | Can load Document | Conditions |
| ---------- | ----------------- | ---------- |
| Query Projection | Yes | The document is on this shard |
| Map Index Projection | Yes | The document is on this shard |
| Map-Reduce Index Projection | No |  |

## OrderBy in a Map Reduce Index

Simlarly to its behavior under a non-sharded database, 
[OrderBy](../indexes/querying/sorting) is used in an index or a query to 
sort the retrieved dataset by a given order.  

But under a sharded database, when `OrderBy` is used in a map reduce 
index and [limit](../indexes/querying/paging#example-ii---basic-paging) 
is applied to restrict the number of retrieved results, there are scenarios 
in which **all** the results will still be retrieved from all shards.  
To understand how this can happen, let's run a few queries over this 
map reduce index:  
{CODE-BLOCK:csharp}
Reduce = results => from result in results
                    group result by result.Name
                    into g
                    select new Result
                    {
                        // Reduce key
                        Name = g.Key,
                        // Calculation
                        Sum = g.Sum(x => x.Sum)
                    };
{CODE-BLOCK/}

* The first query sorts the results using `OrderBy` without setting any limit.  
  This will load **all** matching results from all shards (just like this query 
  would load all matching results from a non-sharded database).  
  {CODE-BLOCK:csharp}
                      var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Name)
                        .ToList();
  {CODE-BLOCK/}
  
* The second query sorts the results by one of the reduce keys, `Name`, 
  and sets a limit to restrict the retrieved dataset to 3 results.  
  This **will** restrict the retrieved dataset to the set limit.  
  {CODE-BLOCK:csharp}
                    var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Name)
                        .Take(3) // this limit will apply while retrieving the items
                        .ToList();
  {CODE-BLOCK/}
  
* The third query sorts the results **not** by a reduce key but by 
  a field that computes a sum from retrieved values.  
  This will retrieve **all** the results from all shards regardless of 
  the set limit, perform the computation over them all, and only then 
  sort them and provide us with just the number of results we requested.  
  {CODE-BLOCK:csharp}
                    var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Sum)
                        .Take(3) // this limit will only apply after retrieving all items
                        .ToList();
  {CODE-BLOCK/}
    
  {NOTE: }
  Note that retrieving all the results from all shards, either 
  by setting no limit or by setting a limit based on a computation 
  as demonstrated above, may cause the retrieval of a large amount 
  of data and extend memory, CPU, and bandwidth usage.  
  {NOTE/}

{PANEL/}

{PANEL: Include}

**Including** documents by a query or an index **will** work even 
if the included entity resides on another shard: if the requested 
document is not found on the current shard it will be searched on 
other shards and if found loaded from the shard that holds it.  

Note that this process will cost the extra travel to the shard 
that the requested document is on.  

{PANEL/}

{PANEL: Unsupported Querying Features}

Querying features that are unsupported or yet unimplemented on sharded 
databases include:  

* **Loading a document that resides on another shard**  
  An [index](../sharding/indexing#unsupported-indexing-features) 
  or a query can only load a document if it resides on the same shard.  
  If the requested document is stored on a different shard:  
   * The result will be `null`  
   * If the document resides on another shard RavenDB will **not** 
     load it.  
* **Loading a document within a map-reduce projection**  
  Read more about this topic [above](../sharding/querying#projection).  
* **Streaming Map-Reduce results**  
  [Streaming](../client-api/session/querying/how-to-stream-query-results#stream-an-index-query) 
  map-reduce results is not supported in a Sharded Database.  
* **Using `limit` with `PatchByQueryOperation` or `DeleteByQueryOperation`**  
  Attempting to set a `limit` when executing 
  [PatchByQueryOperation](../client-api/operations/patching/set-based#sending-a-patch-request) 
  or [DeleteByQueryOperation](../client-api/operations/delete-by-query) 
  will throw a `NotSupportedInShardingException` exception, 
  specifying "Query with limit is not supported in patch / delete by query operation".  
* [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis)  
  This method is not supported in a Sharded Database.  

{PANEL/}

## Related articles

**Indexing**  
[Map-Reduce Indexes](../indexes/map-reduce-indexes)  
[Indexing Basics](../indexes/indexing-basics)  
[Rolling index deploymeny](../indexes/rolling-index-deployment)  
[Map-Reduce Output Documents](../indexes/map-reduce-indexes#map-reduce-output-documents)  

**Querying**  
[Filtering: Where](../indexes/querying/filtering#where)  
[Exploration Queries: Filter](../indexes/querying/exploration-queries#filter)  
[Stream Query Results](../client-api/session/querying/how-to-stream-query-results#stream-an-index-query)  

**Sharding**  
[Share a Bucket: $ Syntax](../sharding/overview#forcing-documents-to-share-a-bucket)  
[Shards Indexing](../sharding/indexing)  

