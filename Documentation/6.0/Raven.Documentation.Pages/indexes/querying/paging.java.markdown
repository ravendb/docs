# Querying: Paging

Paging, or pagination, is the process of splitting a dataset into pages, reading one page at a time. This is useful for optimizing bandwidth traffic and hardware usage or simply because no user can handle huge amounts of data at once.

{WARNING:Warning}
Starting from version 4.0, if the page size is not specified **on client side**, the server will assume `Integer.MAX_VALUE` (2,147,483,647) and all the results will be downloaded. It is **recommended to set a page size explicitly** to avoid long response times caused by sending excessive amounts of data over the network or high memory consumption caused by handling large quantities of documents.
{WARNING/}

{INFO:Performance}
By default, if the number of returned results exceeds **2048**, the server will issue a `Performance Hint` notification (visible in the Studio) with information about query details. You can decide if this behavior is desired or not. 
The threshold can be adjusted by changing the `PerformanceHints.MaxNumberOfResults` configuration value.
{INFO/}

{INFO:Limits}
When [Corax](../../indexes/search-engine/corax) is used as the search engine, 
indexes of more than `Integer.MAX_VALUE` (2,147,483,647) documents can be created and used.  
To match this capacity, queries over Corax indexes can [skip](../../indexes/querying/paging#example-ii---basic-paging) 
a number of results that exceeds `Integer.MAX_VALUE` and `take` documents from 
this location.  
{INFO/}

## Example I - No Paging

The queries below will return all the results available.

{CODE-TABS}
{CODE-TAB:java:Query paging_0_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II - Basic Paging

Let's assume that our page size is `10`, and we want to retrieve the 3rd page. To do this, we need to issue following query:

{CODE-TABS}
{CODE-TAB:java:Query paging_2_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 10
limit 20, 10 // skip 20, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Finding the Total Results Count When Paging

While paging, you sometimes need to know the exact number of results returned from the query. The Client API supports this explicitly:

{CODE-TABS}
{CODE-TAB:java:Query paging_3_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 10
limit 20, 10 // skip 20, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

While the query will return with just 10 results, `totalResults` will hold the total number of matching documents.

## Paging Through Tampered Results

For some queries, the server will skip over some results internally and invalidate the `totalResults` value. When executing a `distinct` query or index producing multiple index entries per document (a fanout index), then `totalResults` will contain the total count of matching documents found, but it will not take into account results that were skipped as a result of the `distinct` operator.

Whenever `skippedResults` is greater than 0 and a query involved some non-stored fields, it implies that we skipped over some results in the index.
    
In order to do proper paging in those scenarios, you should use `skippedResults` when telling RavenDB how many documents to skip. For each page, the starting point should be `.skip((currentPage * pageSize) + skippedResults)`.

For example, let's page through all the results:

{CODE-TABS}
{CODE-TAB:java:Query paging_4_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 10
select distinct *
limit 0, 10  // First loop will skip 0, take 10, etc.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Query paging_6_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_6_0@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Order/ByOrderLines/ProductName"
limit 0, 50  // First loop will skip 0, take 50, etc.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Basics](../../indexes/querying/query-index)
- [Filtering](../../indexes/querying/filtering)
- [Sorting](../../indexes/querying/sorting)
