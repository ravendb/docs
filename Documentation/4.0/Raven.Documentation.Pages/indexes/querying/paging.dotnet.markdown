# Paging

Paging, or pagination, is the process of splitting a dataset into pages, reading one page at a time. This is useful for optimizing bandwidth traffic, optimizing hardware usage, or just because no user can handle huge amounts of data at once anyway.

{WARNING:Warning}
Starting from version 4.0 if page size is not specified **on client side** server will assume **int.MaxValue** (2,147,483,647) and all results will be downloaded. It is **recommended to set a page size explicitly** to avoid long response times caused by sending excessive amount of data over the network or high memory consumption caused by the need of handling large quantities of documents.
{WARNING/}

## Example I - no paging

All of the bellow queries will return all the results available.

{CODE-TABS}
{CODE-TAB:csharp:Query paging_0_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_0_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

## Example II - basic paging

Let's assume that our page size is `10` and we want to retrieve 3rd page. To do this we need to issue following query:

{CODE-TABS}
{CODE-TAB:csharp:Query paging_2_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_2_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

## Finding the total results count when paging

While paging you sometimes need to know the exact number of results returned from the query. The Client API supports this explicitly:

{CODE-TABS}
{CODE-TAB:csharp:Query paging_3_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_3_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

While the query will return with just 10 results, `totalResults` will hold the total number of matching documents.

## Paging through tampered results

For some queries, server will skip over some results internally, and by that invalidate the `TotalResults` value e.g. when executing a `Distinct` query or index produces multiple index entries per document (a fanout index), then `TotalResults` will contain the total count of matching documents found, but will not take into account results that were skipped as a result of the `Distinct` operator.

Whenever `SkippedResults` is greater than 0 and a query involved some non-stored fields, it implies that we skipped over some results in the index.
    
In order to do proper paging in those scenarios, you should use the `SkippedResults` when telling RavenDB how many documents to skip. In other words, for each page the starting point should be `.Skip((currentPage * pageSize) + SkippedResults)`.

For example, let's page through all the results:

{CODE-TABS}
{CODE-TAB:csharp:Query paging_4_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_4_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Query paging_6_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_6_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_6_0@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

The situation would be different if a `Distinct` query and a projection applied to stored fields only. Then to get correct results you shouldn't include `SkippedResults`
into the paging formula. Let's take a look at the example (note the usage of `Store` method in the index definition):

{CODE-TABS}
{CODE-TAB:csharp:Query paging_7_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_7_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_7_0@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
- [Querying : Filtering](../../indexes/querying/filtering)
- [Querying : Sorting](../../indexes/querying/sorting)
