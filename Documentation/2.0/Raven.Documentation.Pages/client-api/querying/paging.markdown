# Paging

Paging, or pagination, is the process of splitting a dataset into pages, reading one page at a time. This is useful for optimizing bandwidth traffic, optimizing hardware usage, or just because no user can handle huge amounts of data at once anyway.

RavenDB makes it very easy to work with paging. In fact, with large data sets, it makes it mandatory (see: Safe By Default).

It is as simple as specifying a page size and passing a starting point. Using Linq from the Client API, it looks like this:

{CODE paging1@ClientApi\Querying\Paging.cs /}

## Finding the total results count when paging

While paging you sometimes need to know the exact number of results returned from the query. The Client API supports this explicitly:

{CODE paging2@ClientApi\Querying\Paging.cs /}

While the query will return with just 10 results, `totalResults` will hold the total number of matching documents.

## Paging through tampered results

For some queries, RavenDB will skip over some results internally, and by that invalidate the `TotalResults` value. For example when executing a Distinct query, `TotalResults` will contain the total count of matching documents found, but will not take into account results that were skipped as a result of the `Distinct` operator.

Whenever `SkippedResults` is greater than 0 it implies that we skipped over some results in the index.
    
In order to do proper paging in those scenarios, you should use the `SkippedResults` when telling RavenDB how many documents to skip. In other words, for each page the starting point should be `.Skip(currentPage * pageSize + SkippedResults)`.

For example, assuming a page size of 10:

{CODE paging3@ClientApi\Querying\Paging.cs /}