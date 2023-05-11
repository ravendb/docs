# Sharding: Querying
---

{NOTE: }

* Query syntax is similar in sharded and non-sharded databases.  
* A sharded database offers the same set of querying features that 
  a non-sharded database offers, so queries that were written for 
  a non-sharded database can generally be kept as is.  
* Some querying features are yet to be implemented. Others (like 
  [filter](../sharding/querying#filtering-results-in-a-sharded-database)) 
  behave a little differently in a sharded database. These cases 
  are discussed below.  

* In this page:  
  * [Querying in a Sharded Database](../sharding/querying#querying-in-a-sharded-database)  
  * [Querying Map-Reduce Indexes](../sharding/querying#querying-map-reduce-indexes)  
     * [Filtering Results in a Sharded Database](../sharding/querying#filtering-results-in-a-sharded-database)  
     * [Projection](../sharding/querying#projection)  
     * [OrderBy in a Map-Reduce Index](../sharding/querying#orderby-in-a-map-reduce-index)  
     * [`where` vs `filter` Recommendations](../sharding/querying#vs--recommendations)  
  * [Including Items](../sharding/querying#including-items)  
  * [Timing Queries](../sharding/querying#timing-queries)
  * [Unsupported Querying Features](../sharding/querying#unsupported-querying-features)  
  
{NOTE/}

---
{PANEL: Querying in a Sharded Database}

From a user's point of view, querying a sharded RavenDB database 
is similar to querying a non-sharded database: query syntax is the 
same, and the same results can be expected to be returned in the 
same format.  

To allow this comfort, the database performs the following 
steps when we send a query to a sharded database:  

* The query is received by a RavenDB server that was appointed 
  [orchestrator](../sharding/overview#client-server-communication) 
  and now mediates all the communications between the client and 
  the database shards.  
* The orchestrator distributes the query to the shards.  
* Each shard runs the query over its own database, using its own indexes.  
  When the data is retrieved, the shard transfers it to the orchestrator.  
* The orchestrator combines the data it received from all shards into 
  a single dataset, and may perform additional operations over it.  
  E.g., running a [map-reduce query](../sharding/querying#querying-map-reduce-indexes) 
  would retrieve from the shards data that has already been reduced by 
  map-reduce indexes, but once the orchestrator gets all the data it will 
  reduce the full dataset once again.  
* Finally, the orchestrator returns the combined dataset to the client.  
* The client remains unaware that it has just communicated with 
  a sharded database.  
  Note, however, that this process is costly in comparison with 
  the simple data retrieval performed by non-sharded databases.  
  Sharding is therefore [recommended](../sharding/overview#when-should-sharding-be-used) 
  only when the database has grown to substantial size and complexity.  

{PANEL/}

{PANEL: Querying Map-Reduce Indexes}

* [Map-reduce indexes on a sharded database](../sharding/indexing#map-reduce-indexes-on-a-sharded-database) 
  are used to reduce data both over each shard during indexation, and on 
  the orchestrator machine each time a query uses them.  
* Read more below about querying map-reduce indexes in a sharded database.  

## Filtering Results in a Sharded Database

* We can filter data using the keywords [where](../indexes/querying/filtering#where) 
  and [filter](../indexes/querying/exploration-queries#filter) on both non-sharded and 
  sharded databases.  
* There **are**, however, differences in the behavior of these commands on sharded and 
  non-sharded databases. This section explains these differences.  

### `where`
`where` is RavenDB's basic filtering command. It is used by the server to restrict 
data retrieval from the database to only those items that match given conditions.  

* **`where` on a non-sharded database**  
  When a query that applies `where` is executed over a **non-sharded** database, 
  the filtering applies to the entire database.  

     If we query all **products**, for example, and want to find only the 
     most successful products, we can easily run a query such as:  
     {CODE-BLOCK:JSON}
     from index 'Products/Sales'
where TotalSales >= 5000
     {CODE-BLOCK/}
     This will retrieve only the documents of products that were sold at least 5000 times.  

* **`where` on a sharded database**  
  When a query that includes a `where` clause is sent to a **sharded database**, 
  filtering is applied **per-shard**, over each shard's database.  
  
     This presents us with the following problem:  
     The filtering that runs on each shard takes into account only the data present 
     on that shard.  
     If a certain product was sold 4000 times on each shard, the query demonstrated 
     above will filter this product out on each shard, even though its total sales 
     far exceed 5000.  

     To solve this problem, the role of the `filter` command is 
     [altered on sharded databases](../sharding/querying#section-1).  
     {NOTE: }
     Using `where` raises no problem, and is actually [recommended](../sharding/querying#vs--recommendations), 
     when the filtering is done [over a GroupBy field](../sharding/querying#orderby-in-a-map-reduce-index).  
     {NOTE/}

### `filter`
The `filter` command is used when we want to scan data that has already been 
retrieved from the database but is still on the server.  

* When a query that includes a `filter` clause is sent to a 
  **non-sharded database** its main usage is as an 
  [exploration query](../indexes/querying/exploration-queries): 
  an additional layer of filtering that scans the entire retrieved dataset 
  without creating an index that would then have to be maintained.  

     We consider exploration queries one-time operations and use them 
     cautiously because scanning the entire retrieved dataset may take 
     a high toll on resources.  

* When a query that includes a `filter` clause is sent to a 
  **sharded database**:  
   * The `filter` clause is omitted from the query.  
     All data is retrieved from the shards to the orchestrator.  
   * The `filter` clause is executed on the orchestrator machine 
     over the entire downloaded dataset.  
   
  **On the Cons side**, a huge amount of data may be retrieved from 
  the database and then scanned by the filtering condition.  

  **On the Pros side**, this mechanism allows us to filter data using 
  [computational fields](../sharding/querying#orderby-in-a-map-reduce-index) 
  as we do over a non-sharded database.  
  The below query, for example, will indeed return all the products 
  that were sold at least 5000 times, no matter how their sales 
  are divided between the shards.  
     {CODE-BLOCK:JSON}
     from index 'Products/Sales'
filter TotalSales >= 5000
     {CODE-BLOCK/}
  {NOTE: }
  The results volume retrieved from the shards can be decreased 
  (when it makes sense as part of the query) by applying `where` 
  [over a GroupBy field](../sharding/querying#orderby-in-a-map-reduce-index) 
  before calling `filter`.  
  {NOTE/}

## Projection

[Loading a document within a map-reduce projection](../indexes/querying/projections#example-viii---projection-using-a-loaded-document) 
is **not supported** in a Sharded Database.  
  
Database response to an attempt to load a document from a map-reduce projection:  
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

## OrderBy in a Map-Reduce Index

Similarly to its behavior under a non-sharded database, 
[OrderBy](../indexes/querying/sorting) is used in an index or a query to 
sort the retrieved dataset by a given order.  

But under a sharded database, when `OrderBy` is used in a map-reduce 
index and [limit](../indexes/querying/paging#example-ii---basic-paging) 
is applied to restrict the number of retrieved results, there are scenarios 
in which **all** the results will still be retrieved from all shards.  
To understand how this can happen, let's run a few queries over this 
map-reduce index:  
{CODE-BLOCK:csharp}
Reduce = results => from result in results
                    group result by result.Name
                    into g
                    select new Result
                    {
                        // Group-by field (reduce key)
                        Name = g.Key,
                        // Computation field
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
  
* The second query sorts the results by one of the `GroupBy` fields, 
  `Name`, and sets a limit to restrict the retrieved dataset to 3 results.  
  This **will** restrict the retrieved dataset to the set limit.  
  {CODE-BLOCK:csharp}
                    var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Name)
                        .Take(3) // this limit will apply while retrieving the items
                        .ToList();
  {CODE-BLOCK/}
  
* The third query sorts the results **not** by a `GroupBy` field 
  but by a field that computes a sum from retrieved values.  
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

## `where` vs `filter` Recommendations

As using `filter` may (unless `where` is also used) cause the retrieval 
and scanning of a substantial amount of data, it is recommended to use 
`filter` cautiously and restrict its operation wherever needed.  

* Prefer `where` over `filter` when the query is executed over 
  a [GroupBy](../sharding/querying#orderby-in-a-map-reduce-index) field.  
* Prefer `filter` over `where` when the query is executed over 
  a conditional query field like [Total or Sum](../sharding/querying#orderby-in-a-map-reduce-index) field.  
* When using `filter`, set a [limit](../indexes/querying/exploration-queries#usage) if possible.  
* When `filter` is needed, use `where` first to minimize the dataset that 
  needs to be transferred from the shards to the orchestrator and scanned 
  by `filter` over the orchestrator machine. E.g. -  
  {CODE-BLOCK:JSON}
  from index 'Products/Sales'
where Category = 'categories/7-A'
filter TotalSales >= 5000
  {CODE-BLOCK/}
{PANEL/}

{PANEL: Including Items}

**Including** items by a query or an index **will** work even if an included 
item resides on another shard.  
If a requested item is not found on this shard, the orchestrator will connect 
the shard it is stored on, load the item and provide it.  

Note that this process will cost the additional travel to the shard that the 
requested item resides on.  

{PANEL/}

{PANEL: Timing Queries}
The duration of queries and query parts (e.g. optimization 
or execution time) can be measured using API or Studio.  

Timing is **disabled** by default, to avoid the measuring overhead. 
It can be enabled per query by adding `include timings()` to an RQL 
query or calling `.Timings` in a [Linq](../client-api/session/querying/how-to-query#session.query) 
query, as explained [here](../client-api/session/querying/debugging/query-timings).  

To time queries in a sharded database using Studio open the 
[Query View](../studio/database/queries/query-view), enable timings 
as explained above, run the query, and open the **Timing** tab.  

!["Timing Shards Querying"](images/querying_timing.png "Timing Shards Querying")

1. **Textual view** of query parts and their duration.  
   Point the mouse cursor at captions to display timing properties in the graphical view on the right.  
2. **Per-shard Timings**  
3. **Graphical View**  
   Point the mouse cursor at graph sections to display query parts duration:  
    **A**. Shard #0 overall query time  
    **B**. Shard #0 optimization period  
    **C**. Shard #0 query period  
    **D**. Shard #0 staleness period  

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

### Indexing
- [Map-Reduce Indexes](../indexes/map-reduce-indexes)  
- [Indexing Basics](../indexes/indexing-basics)  
- [Rolling index deployment](../indexes/rolling-index-deployment)  
- [Map-Reduce Output Documents](../indexes/map-reduce-indexes#map-reduce-output-documents)  

### Querying
- [Filtering: Where](../indexes/querying/filtering#where)  
- [Exploration Queries: Filter](../indexes/querying/exploration-queries#filter)  
- [Stream Query Results](../client-api/session/querying/how-to-stream-query-results#stream-an-index-query)  

### Sharding
- [Share a Bucket: $ Syntax](../sharding/overview#forcing-documents-to-share-a-bucket)  
- [Shards Indexing](../sharding/indexing)  

