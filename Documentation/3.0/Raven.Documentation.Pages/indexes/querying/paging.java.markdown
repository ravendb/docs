# Paging

Paging, or pagination, is the process of splitting a dataset into pages, reading one page at a time. This is useful for optimizing bandwidth traffic, optimizing hardware usage, or just because no user can handle huge amounts of data at once anyway.

{SAFE If not specified, page size **on client side** is set to **128**. /}

{SAFE If not specified, maximum page size **on server side** is set to **1024** and can be altered using `Raven/MaxPageSize` setting (more information [here](../../server/configuration/configuration-options)). /}

## Example I - safe by default

All of the bellow queries will return up to **128** results due to the client default page size value.

{CODE-TABS}
{CODE-TAB:java:Query paging_0_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_0_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_0_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TABS/}

All of the bellow queries will return up to **1024** results due to the server default max page size value.

{CODE-TABS}
{CODE-TAB:java:Query paging_1_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_1_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_1_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TABS/}

## Example II - basic paging

Let's assume that our page size is `10` and we want to retrieve 3rd page. To do this we need to issue following query:

{CODE-TABS}
{CODE-TAB:java:Query paging_2_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_2_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_2_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TABS/}

## Finding the total results count when paging

While paging you sometimes need to know the exact number of results returned from the query. The Client API supports this explicitly:

{CODE-TABS}
{CODE-TAB:java:Query paging_3_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_3_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_3_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TABS/}

While the query will return with just 10 results, `totalResults` will hold the total number of matching documents.

## Paging through tampered results


For some queries, server will skip over some results internally, and by that invalidate the `totalResults` value e.g. when executing a Distinct query or index produces multiple index entries per document (a fanout index), then `totalResults` will contain the total count of matching documents found, but will not take into account results that were skipped as a result of the `distinct` operator.

Whenever `skippedResults` is greater than 0 and a query involved some non-stored fields, it implies that we skipped over some results in the index.
    
In order to do proper paging in those scenarios, you should use the `skippedResults` when telling RavenDB how many documents to skip. In other words, for each page the starting point should be `.skip((currentPage * pageSize) + skippedResults)`.

For example, let's page through all the results:

{CODE-TABS}
{CODE-TAB:java:Query paging_4_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_4_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_4_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Query paging_6_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_6_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_6_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_6_0@Indexes\Querying\Paging.java /}
{CODE-TABS/}


The situation would be different if a `Distinct` query and a projection applied to stored fields only. Then to get correct results you shouldn't include `SkippedResults`
into the paging formula. Let's take a look at the example (note the usage of `Store` method in the index definition):

{CODE-TABS}
{CODE-TAB:java:Query paging_7_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:DocumentQuery paging_7_2@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Commands paging_7_3@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_7_0@Indexes\Querying\Paging.java /}
{CODE-TABS/}

## Increasing StartsWith performance

All `startsWith` operations (e.g. [loadStartingWith](../../client-api/session/loading-entities#loadstartingwith) and [stream](../../client-api/session/querying/how-to-stream-query-results) from advanced session operations or [startsWith](../../client-api/commands/documents/get#startswith) and [stream](../../client-api/commands/documents/stream) from low-level commands) contain a `RavenPagingInformation` parameter that can be used to increase the performance of a StartsWith operation when **next page** is requested.

To do this we need to pass same instance of `RavenPagingInformation` to the identical operation. The client will use information contained in this object to increase the performance (only if next page is requested).

{CODE paging_5_1@Indexes\Querying\Paging.cs /}

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
- [Querying : Filtering](../../indexes/querying/filtering)
- [Querying : Sorting](../../indexes/querying/sorting)
