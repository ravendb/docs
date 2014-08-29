# Paging

Paging, or pagination, is the process of splitting a dataset into pages, reading one page at a time. This is useful for optimizing bandwidth traffic, optimizing hardware usage, or just because no user can handle huge amounts of data at once anyway.

{SAFE If not specified, page size **on client side** is set to **128**. /}

{SAFE If not specified, maximum page size **on server side** is set to **1024** and can be altered using `Raven/MaxPageSize` setting (more information [here]()). /}

## Example I - safe by default

All of the bellow queries will return up to **128** results due to the client default page size value.

{CODE-TABS}
{CODE-TAB:csharp:Query paging_0_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_0_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Commands paging_0_3@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

All of the bellow queries will return up to **1024** results due to the server default max page size value.

{CODE-TABS}
{CODE-TAB:csharp:Query paging_1_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_1_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Commands paging_1_3@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

## Example II - basic paging

Let's assume that our page size is `10` and we want to retrieve 3rd page. To do this we need to issue following query:

{CODE-TABS}
{CODE-TAB:csharp:Query paging_2_1@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:DocumentQuery paging_2_2@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Commands paging_2_3@Indexes\Querying\Paging.cs /}
{CODE-TAB:csharp:Index paging_0_4@Indexes\Querying\Paging.cs /}
{CODE-TABS/}

## Finding the total results count when paging

While paging you sometimes need to know the exact number of results returned from the query. The Client API supports this explicitly:

{CODE paging_2@Indexes\Querying\Paging.cs /}

While the query will return with just 10 results, `totalResults` will hold the total number of matching documents.

## Paging through tampered results

For some queries, RavenDB will skip over some results internally, and by that invalidate the `TotalResults` value. For example when executing a Distinct query, `TotalResults` will contain the total count of matching documents found, but will not take into account results that were skipped as a result of the `Distinct` operator.

Whenever `SkippedResults` is greater than 0 it implies that we skipped over some results in the index.
    
In order to do proper paging in those scenarios, you should use the `SkippedResults` when telling RavenDB how many documents to skip. In other words, for each page the starting point should be `.Skip(currentPage * pageSize + SkippedResults)`.

For example, assuming a page size of 10:

{CODE paging_3@Indexes\Querying\Paging.cs /}

#### Related articles

TODO