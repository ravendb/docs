# Sharding: Querying
---

{NOTE: }

* Query syntax is similar in sharded and non-sharded databases.  

* A sharded database offers the same set of querying features that a non-sharded database offers,  
  so queries that were written for a non-sharded database can generally be kept as is.  

* Some querying features are yet to be implemented.   
  Others (like [filter](../sharding/querying#filtering-results-in-a-sharded-database)) behave a little differently in a sharded database. 
  These cases are discussed below.  

* In this page:  
  * [Querying a sharded database](../sharding/querying#querying-a-sharded-database)
  * [Querying a selected shard](../sharding/querying#querying-a-selected-shard)
  * [Including items](../sharding/querying#including-items)
  * [Paging results](../sharding/querying#paging-results)
  * [Filtering results](../sharding/querying#filtering-results)
      * [`where`](../sharding/querying#section)
      * [`filter`](../sharding/querying#section-1)
      * [`where` vs `filter` recommendations](../sharding/querying#vsrecommendations)
  * [Querying Map-Reduce indexes](../sharding/querying#querying-map-reduce-indexes)
     * [Loading document within a projection](../sharding/querying#loading-document-within-a-projection)  
     * [OrderBy in a Map-Reduce index query](../sharding/querying#orderby-in-a-map-reduce-index-query)     
  * [Timing queries](../sharding/querying#timing-queries)
  * [Unsupported querying features](../sharding/querying#unsupported-querying-features)  
  
{NOTE/}

---

{PANEL: Querying a sharded database}

From a user's point of view, querying a sharded RavenDB database is similar to querying a non-sharded database:  
query syntax is the same, and the same results can be expected to be returned in the same format.  

To allow this comfort, the database performs the following steps when a client sends a query to a sharded database:  

* The query is received by a RavenDB server that was appointed as an [orchestrator](../sharding/overview#client-server-communication).  
  The orchestrator mediates all the communications between the client and the database shards.  
* The orchestrator distributes the query to the shards.  
* Each shard runs the query over its own database, using its own indexes.  
  When the data is retrieved, the shard transfers it to the orchestrator.  
* The orchestrator combines the data it received from all shards into a single dataset, and may perform additional operations over it.  
  E.g., querying a [map-reduce index](../sharding/indexing#map-reduce-indexes-on-a-sharded-database) would retrieve from the shards data that has already been reduced by map-reduce indexes.  
  Once the orchestrator gets all the data it will reduce the full dataset once again.  
* Finally, the orchestrator returns the combined dataset to the client.  
* The client remains unaware that it has just communicated with a sharded database.  
  Note, however, that this process is costly in comparison with the simple data retrieval performed by non-sharded databases.  
  Sharding is therefore [recommended](../sharding/overview#when-should-sharding-be-used) only when the database has grown to substantial size and complexity.  

{PANEL/}

{PANEL: Querying a selected shard}

* A query is normally executed over all shards. However, it is also possible to query only selected shards.

* Query a selected shard when you know in advance that the documents you need to query reside on that shard,  
  to avoid trips to other shards.

* This approach is useful when documents are intentionally stored on the same shard,  
  either by using [Anchoring documents](../sharding/administration/anchoring-documents) or by configuring [Prefixed sharding](../sharding-by-prefix#prefixed-sharding-vs-anchoring-documents).

---

* Use the `ShardContext` method in your query to specify which shard/s to query.  

* Use the `ByDocumentId` or `ByDocumentIds` methods to identify the shard(s) where documents are stored.  
  Pass .... todo... Aviv ?

{NOTE: }

**Examples**:  

  Query only the shard containing document `users/1`:

  {CODE-TABS}
  {CODE-TAB:csharp:Query query_selected_shard_1@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:Query_async query_selected_shard_1_async@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:DocumentQuery query_selected_shard_2@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:DocumentQuery_async query_selected_shard_2_async@Sharding\ShardingQuerying.cs /}
  {CODE-TAB-BLOCK:sql:RQL}
  from "Users
  where Name == Joe"
  { "__shardContext": "users/1" }
  {CODE-TAB-BLOCK/}
  {CODE-TABS/}

  Query only the shard/s containing documents `users/2` and `users/3`:  

  {CODE-TABS}
  {CODE-TAB:csharp:Query query_selected_shard_3@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:Query_async query_selected_shard_3_async@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:DocumentQuery query_selected_shard_4@Sharding\ShardingQuerying.cs /}
  {CODE-TAB:csharp:DocumentQuery_async query_selected_shard_4_async@Sharding\ShardingQuerying.cs /}
  {CODE-TAB-BLOCK:sql:RQL}
  from "Users
  where Name == Joe"
  { "__shardContext" : ["users/2", "users/3"] }
  {CODE-TAB-BLOCK/}
  {CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Including items}

* **Including** items by a query or an index **will** work even if the included item resides on another shard.  
  If the requested item is not located on this shard, the orchestrator will fetch it from the shard where it is located.  

* Note that this process will cost an extra travel to the shard that hosts the requested item.

{PANEL/}

{PANEL: Paging results}

From the client's point of view, [paging](../indexes/querying/paging) is conducted similarly in sharded and non-sharded databases,  
and the same API is used to define page size and retrieve selected pages.

Under the hood, however, performing paging in a sharded database entails some overhead since the orchestrator is required to load
the requested data **from each shard** and sort the retrieved results before handing the selected page to the client.

For example, let's compare what happens when we load the 8th page (with a page size of 100) from a non-sharded and a sharded database:  

{CODE-TABS}
{CODE-TAB:csharp:Query Query_basic-paging@Sharding\ShardingQuerying.cs /}
{CODE-TAB:csharp:DocumentQuery DocumentQuery_basic-paging@Sharding\ShardingQuerying.cs /}
{CODE-TAB:csharp:Index index-for-paging-sample@Sharding\ShardingQuerying.cs /}
{CODE-TABS/}

* When the database is **Not sharded** the server would:  
  * Skip 7 pages.  
  * Hand page 8 to the client (results 701 to 800).

* When the database is **Sharded** the orchestrator would:  
  * Load 8 pages (sorted by modification order) from each shard.  
  * Sort the retrieved results (in a 3-shard database, for example, the orchestrator would sort 2400 results).  
  * Skip 7 pages (of 24).  
  * Hand page 8 to the client (results 701 to 800).

{NOTE: }
The shards sort the data by modification order before sending it to the orchestrator.  
For example, if a shard is required to send 800 results to the orchestrator,
the first result will be the most recently modified document, while the last result will be the document modified first.
{NOTE/}

{PANEL/}

{PANEL: Filtering results}

* Data can be filtered using the [where](../indexes/querying/filtering#where) 
  and [filter](../indexes/querying/exploration-queries#filter) keywords on both non-sharded and sharded databases.  

* There **are**, however, differences in the behavior of these commands on sharded and non-sharded databases.  
  This section explains these differences.  

---

### `where`

`where` is RavenDB's basic filtering command.  
It is used by the server to restrict data retrieval from the database to only those items that match given conditions.  

* **On a non-sharded database**  
  When a query that applies `where` is executed over a non-sharded database,  
  the filtering applies to the **entire** database.

    If want to find only the most successful products, we can easily run a query such as:  
    {CODE-BLOCK:JSON}
from index 'Products/Sales'
where TotalSales >= 5000
    {CODE-BLOCK/}
    
    This will retrieve only the documents of products that were sold at least 5000 times.

* **On a sharded database**:  
  When a query that includes a `where` clause is sent to a sharded database,  
  filtering is applied **per-shard**, over each shard's database.

    This presents us with the following problem:  
    The filtering that runs on each shard takes into account only the data present on that shard.  
    If a certain product was sold 4000 times on each shard, the query demonstrated
    above will filter this product out on each shard, even though its total sales far exceed 5000.

    To solve this problem, the role of the `filter` command is [altered on sharded databases](../sharding/querying#section-1).  

    {NOTE: }
    Using `where` raises no problem and is actually [recommended](../sharding/querying#vs--recommendations)
    when the filtering is done [over a GroupBy field](../sharding/querying#orderby-in-a-map-reduce-index).  
    {NOTE/}

---

### `filter`

The `filter` command is used when we want to scan data that has already been retrieved from the database but is still on the server.  

* **On a non-sharded database**  
  When a query that includes a `filter` clause is sent to a non-sharded database its main usage is as an [exploration query](../indexes/querying/exploration-queries): 
  an additional layer of filtering that scans the entire retrieved dataset without creating an index that would then have to be maintained.  

     We consider exploration queries one-time operations and use them cautiously because scanning the entire retrieved dataset may take a high toll on resources.  

* **On a sharded database**:  
  When a query that includes a `filter` clause is sent to a sharded database:  
   * The `filter` clause is omitted from the query.  
     All data is retrieved from the shards to the orchestrator.  
   * The `filter` clause is executed on the orchestrator machine over the entire retrieved dataset.  
   
     **On the Cons side**,  
     a huge amount of data may be retrieved from the database and then scanned by the filtering condition.  

     **On the Pros side**,  
     this mechanism allows us to filter data using [computational fields](../sharding/querying#orderby-in-a-map-reduce-index) as we do over a non-sharded database.  
     The below query, for example, will indeed return all the products that were sold at least 5000 times,  
     no matter how their sales are divided between the shards.  
     {CODE-BLOCK:JSON}
from index 'Products/Sales'
filter TotalSales >= 5000
     {CODE-BLOCK/}

    {NOTE: }
    The results volume retrieved from the shards can be decreased (when it makes sense as part of the query)  
    by applying `where` [over a GroupBy field](../sharding/querying#orderby-in-a-map-reduce-index) before calling `filter`.  
    {NOTE/}

---

### `where`&nbsp;vs&nbsp;`filter`&nbsp;recommendations

As using `filter` may (unless `where` is also used) cause the retrieval and scanning of a substantial amount of data,  
it is recommended to use`filter` cautiously and restrict its operation wherever needed.

* Prefer `where` over `filter` when the query is executed over a [GroupBy](../sharding/querying#orderby-in-a-map-reduce-index) field.
* Prefer `filter` over `where` when the query is executed over  a conditional query field like [Total or Sum](../sharding/querying#orderby-in-a-map-reduce-index) field.
* When using `filter`, set a [limit](../indexes/querying/exploration-queries#usage) if possible.
* When `filter` is needed, use `where` first to minimize the dataset that needs to be transferred from the shards to the orchestrator and scanned by `filter` over the orchestrator machine. 
  E.g. -  
  {CODE-BLOCK:JSON}
from index 'Products/Sales'
where Category = 'categories/7-A'
filter TotalSales >= 5000
  {CODE-BLOCK/}

{PANEL/}

{PANEL: Querying Map-Reduce indexes}

### Loading document within a projection

[Loading a document within a Map-Reduce projection](../indexes/querying/projections#example-viii---projection-using-a-loaded-document) 
is **not supported** in a sharded database.  

When attempting to load a document from a Map-Reduce projection, the database will respond with a `NotSupportedInShardingException`, 
specifying that "Loading a document inside a projection from a Map-Reduce index isn't supported."

Unlike Map-Reduce index projections, projections of queries that use no index and projections of Map indexes can load a document, 
[provided that the document is on this shard](../sharding/querying#unsupported-querying-features).  

| Projection                  | Can load Document   | Condition                     |
|-----------------------------|---------------------|-------------------------------|
| Query projection            | Yes                 | The document is on this shard |
| Map index projection        | Yes                 | The document is on this shard |
| Map-Reduce index projection | No                  |                               |

### OrderBy in a Map-Reduce index query

Similar to its behavior under a non-sharded database, [OrderBy](../indexes/querying/sorting) is used in an index query or a dynamic query to sort the retrieved dataset by a given order.  

But under a sharded database, when `OrderBy` is used in a Map-Reduce index and [limit](../indexes/querying/paging#example-ii---basic-paging) 
is applied to restrict the number of retrieved results, there are scenarios in which **all** the results will still be retrieved from all shards.  
To understand how this can happen, let's run a few queries over this Map-Reduce index:  

{CODE map-reduce-index@Sharding\ShardingQuerying.cs /}

* The first query sorts the results using `OrderBy` without setting any limit.  
  This will load **all** matching results from all shards (just like this query would load all matching results from a non-sharded database).  
  {CODE OrderBy_with-no-limit@Sharding\ShardingQuerying.cs /}
  
* The second query sorts the results by one of the `GroupBy` fields, `Name`, and sets a limit to restrict the retrieved dataset to 3 results.  
  This **will** restrict the retrieved dataset to the set limit.  
  {CODE OrderBy_with-limit@Sharding\ShardingQuerying.cs /}
  
* The third query sorts the results **not** by a `GroupBy` field but by a field that computes a sum from retrieved values. 
  This will retrieve **all** the results from all shards regardless of the set limit, perform the computation over them all, 
  and only then sort them and provide us with just the number of results we requested.  
  {CODE compute-sum-by-retrieved-results@Sharding\ShardingQuerying.cs /}
    
  {NOTE: }
  Note that retrieving all the results from all shards, either by setting no limit or by setting a limit based on a computation as demonstrated above, 
  may cause the retrieval of a large amount of data and extend memory, CPU, and bandwidth usage.  
  {NOTE/}

{PANEL/}

{PANEL: Timing queries}

* The duration of queries and query parts (e.g. optimization or execution time) can be measured using API or Studio.

* In a sharded database, the timings for each part will be provided __per shard__.

* Timing is disabled by default, to avoid the measuring overhead.  
  It can be enabled per query by adding `include timings()` to an RQL query or calling [`Timings()`](../client-api/session/querying/debugging/query-timings#syntax) 
  in your query code, as explained in [include query timings](../client-api/session/querying/debugging/query-timings).

* To view the query timings in the Studio, open the [Query View](../studio/database/queries/query-view),  
  run an RQL query with `include timings()`, and open the __Timings tab__.

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

{PANEL: Unsupported querying features}

Querying features that are not supported or not yet implemented on sharded databases include:  

* **Loading a document that resides on another shard**  
  An [index](../sharding/indexing#unsupported-indexing-features) or a query can only load a document if it resides on the same shard.  
  Loading a document that resides on a different shard will return _null_ instead of the loaded document.

* **Loading a document within a map-reduce projection**  
  Read more about this topic [above](../sharding/querying#projection).  

* **Streaming Map-Reduce results**  
  [Streaming](../client-api/session/querying/how-to-stream-query-results#stream-an-index-query) 
  map-reduce results is not supported in a sharded database.  

* **Querying with a limit is not supported in patch/delete by query operations**  
  Attempting to set a [limit](../client-api/session/querying/what-is-rql#limit) when executing 
  [PatchByQueryOperation](../client-api/operations/patching/set-based#sending-a-patch-request) or [DeleteByQueryOperation](../client-api/operations/common/delete-by-query)  
  will throw a `NotSupportedInShardingException` exception.  

* **Querying for similar documents with _MoreLikeThis_**  
  Method [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) is not supported in a sharded database.  

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

