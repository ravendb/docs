# Paging Query Results
---

{NOTE: }

* **Paging**:  
  Paging is the process of fetching a subset (a page) of results from a dataset, rather than retrieving the entire results at once.
  This method enables processing query results one page at a time.

* **Default page size**:  
  If the client's query definition does Not explicitly specify the page size, the server will default to `int.MaxValue` (2,147,483,647).
  In such case, all results will be returned in a single server call.

* **Performance**:  
  Using paging is beneficial when handling large result datasets, contributing to improved performance.  
  See [paging and performance](../../indexes/querying/paging#paging-and-performance) here below.

* **Paging policy**:  
  To prevent executing queries that do not specify a page size, you can set the [ThrowIfQueryPageSizeIsNotSet](../../client-api/configuration/querying#throwifquerypagesizeisnotset) convention,
  which can be useful during development or testing phases.

* In this page:

    * [No-paging example](../../indexes/querying/paging#no---paging-example)
    * [Paging examples](../../indexes/querying/paging#paging-examples)
    * [Paging and performance](../../indexes/querying/paging#paging-and-performance)
    * [Paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results)

{NOTE/}

---

{PANEL: No-paging example}

The queries below will return all the results available.

{CODE-TABS}
{CODE-TAB:java:Query paging_0_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_0_4@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByUnitsInStock"
where UnitsInStock > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Paging examples}

#### Basic paging:

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

---

#### Find total results count when paging:

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

{PANEL/}

{PANEL: Paging and performance}

#### Better performance:

It is recommended to explicitly set a page size when making a query that is expected to generate a significant number of results.
This practice has several benefits:

* Optimizes bandwidth usage by reducing data transfer between the server and client.
* Prevents delays in response times caused by sending too much data over the network.
* Avoids high memory consumption when dealing with numerous documents.
* Ensures a more manageable user experience by not overwhelming users with massive datasets at once.

---

#### Performance hints:

* By default, if the number of returned results exceeds **2048**, the server will issue a "Page size too big" notification (visible in the Studio) with information about the query.

* This threshold can be customized by modifying the value of the [PerformanceHints.MaxNumberOfResults](../../server/configuration/performance-hints-configuration#performancehints.maxnumberofresults) configuration key.

* As suggested by the hint, you may consider using [Streaming query results](../../client-api/session/querying/how-to-stream-query-results) instead of paging.

![Figure 1. Performance Hint](images/performance-hint.png "Performance Hint")

{PANEL/}

{PANEL: Paging through tampered results}

* The `QueryStatistics` object contains the `TotalResults` property,  
  which represents the total number of matching documents found in the query results.

* The `QueryStatistics` object also contains the `SkippedResults` property.  
  Whenever this property is greater than **0**, that implies the server has skipped that number of results from the index.

* The server will skip duplicate results internally in the following two scenarios:

    1. When making a [Projection query](../../indexes/querying/projections) with [Distinct](../../indexes/querying/distinct).

    2. When querying a [Fanout index](../../indexes/indexing-nested-data#fanout-index---multiple-index-entries-per-document).

* In those cases:

    * The `SkippedResults` property from the stats object will hold the count of skipped (duplicate) results.

    * The `TotalResults` property will be invalidated -  
      it will Not deduct the number of skipped results from the total number of results.

* In order to do proper paging in those scenarios:  
  include the `SkippedResults` value when specifying the number of documents to skip for each page using:  
  `(currentPage * pageSize) + SkippedResults`.

## Examples

#### A projection query with Distinct:

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

---

#### Querying a Fanout index:

{CODE-TABS}
{CODE-TAB:java:Query paging_6_1@Indexes\Querying\Paging.java /}
{CODE-TAB:java:Index paging_6_0@Indexes\Querying\Paging.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Order/ByOrderLines/ProductName"
limit 0, 50  // First loop will skip 0, take 50, etc.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Basics](../../indexes/querying/query-index)
- [Filtering](../../indexes/querying/filtering)
- [Sorting](../../indexes/querying/sorting)
