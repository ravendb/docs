# Query Overview

---

{NOTE: }

* PHP Queries can be written -  
    * Using the session `query` method  
    * Using the session  `documentQuery` method  
    * Using **RQL**:  
        * via the session `rawQuery` method  
        * via Studio's [Query view](../../../studio/database/queries/query-view)  

* Queries defined using `query` or `documentQuery` are translated by the RavenDB client 
  to [RQL](../../../client-api/session/querying/what-is-rql) when sent to the server.  

---

* All RavenDB queries use an **index** to provide results, even when an index is not explicitly defined.  
  Learn more [below](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* Queries that do Not specify which index to use are called **Dynamic Queries**.  
  This article shows examples of dynamic queries only.  
  For index query examples see [querying an index](../../../indexes/querying/query-index).

---

* The entities returned by the query are 'loaded' and **tracked** by the [session](../../../client-api/session/what-is-a-session-and-how-does-it-work).  
  Entities will Not be tracked when:  
    * Query returns a [projection](../../../client-api/session/querying/how-to-project-query-results)  
    * Tracking is [disabled](../../../client-api/session/querying/how-to-customize-query#notracking)  

* Query results are **cached** by default.  
  To disable query caching see [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching).

* Queries are timed out after a configurable time period.  
  See [query timeout](../../../server/configuration/database-configuration#databases.querytimeoutinsec).

---

* In this page:
  * [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index)  
  * [session.query](../../../client-api/session/querying/how-to-query#session.query)  
  * [session.advanced.documentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery)  
  * [session.advanced.rawQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery)
  * [Custom methods](../../../client-api/session/querying/how-to-query#custom-methods)  
  * [Syntax](../../../client-api/session/querying/how-to-query#syntax)  

{NOTE/}

---

{PANEL: Queries always provide results using an index} 

* Queries always use an index to provide fast results regardless of the size of your data.  

* When a query reaches a RavenDB instance, the instance calls its **query optimizer** to analyze the query  
  and determine which index should be used to retrieve the requested data.
 
* Indexes allow to provide query results without scanning the entire dataset each and every time.  
  Learn more about indexes in [indexes overview](../../../studio/database/indexes/indexes-overview). 

{INFO: }

We differentiate between the following **3 query scenarios**:  

  * Index query  
  * Dynamic query  
  * Full collection query  

For each scenario, a different index type will be used.

{INFO/}

{NOTE: }

<a id="indexQuery" /> 
**1. Query an existing index**:

*  **Query type**: Index query  
   **Index used**: Static-index  

* You can specify which **STATIC-index** the query will use.

* Static indexes are defined by the user, as opposed to auto-indexes that are created by the server  
  when querying a collection with some filtering applied. See [Static-index vs Auto-index](../../../studio/database/indexes/indexes-overview#auto-indexes--vs--static-indexes).  
  

* Example RQL: &nbsp; `from index "Employees/ByFirstName" where FirstName == "Laura"`  
  See more examples in [querying an index](../../../indexes/querying/query-index).

{NOTE/}

{NOTE: }

<a id="dynamicQuery" /> 
**2. Query a collection - with filtering**:  

*  **Query type**: Dynamic Query  
   **Index used**: Auto-index  

* When querying a collection without specifying an index and with some filtering condition  
  (other than just the document ID) the query-optimizer will analyze the query to see if an **AUTO-index**  
  that can answer the query already exists, i.e. an auto-index on the collection queried with index-fields that match those queried.  

* If such auto-index (Not a static one...) is found, it will be used to fetch the results.  

* Else, if no relevant auto-index is found,  
  the query-optimizer will create a new auto-index with fields that match the query criteria.  
  At this time, and only at this time, the query will wait for the auto-indexing process to complete.  
  Subsequent queries that target this auto-index will be served immediately.  

* Note: if there exists an auto-index that is defined on the collection queried  
  but is indexing a different field than the one queried on,  
  then the query-optimizer will create a new auto-index that merges both the  
  fields from the existing auto-index and the new fields queried.

* Once the newly created auto-index is done indexing the data,  
  the old auto-index is removed in favor of the new one.

* Over time, an optimal set of indexes is generated by the query optimizer to answer your queries.

* RQL Example: &nbsp; `from Employees where FirstName == "Laura"`  
  See more examples [below](../../../client-api/session/querying/how-to-query#session.query).

---

* Note: Counters and Time series are an exception to this flow.  
  Dynamic queries on counters and time series values don't create auto-indexes.  
  However, a static-index can be defined on [Time series](../../../document-extensions/timeseries/indexing) 
  and [Counters](../../../document-extensions/counters/indexing).

{NOTE/}

{NOTE: }

<a id="collectionQuery" /> 
**3. Query a collection - query full collection | query by ID**:  
 
* **Query type**: Full collection Query  
  **Index used**: The raw collection (internal storage indexes)  

* Full collection query:

  * When querying a collection without specifying an index and with no filtering condition,  
    then all documents from the specified collection are returned.

  * RavenDB uses the raw collection documents in its **internal storage indexes** as the source for this query.  
    No auto-index is created.
   
  * Example RQL: &nbsp; `from Employees`

* Query by document ID:
 
  * When querying a collection only by document ID or IDs,  
    then similar to the full collection query, no auto-index is created.  
  
  * RavenDB uses the raw collection documents as the source for this query.  

  * Example RQL: &nbsp; `from Employees where id() == "employees/1-A"`  
    See more examples [below](../../../client-api/session/querying/how-to-query#session.query).

{NOTE/}

{PANEL/}

{PANEL: session.query}

* The simplest way to issue a query is by using the session's `query` method which supports LINQ.  
  Both the LINQ method syntax and the LINQ query syntax are supported.  

* The following examples show **dynamic queries** that do not specify which index to use.  
  Please refer to [querying an index](../../../indexes/querying/query-index) for other examples.

* Querying can be enhanced using these [extension methods](../../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq).
 
{NOTE: }

**Query collection - no filtering** 

{CODE-TABS}
{CODE-TAB:php query_1_1@ClientApi\Session\Querying\HowToQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
// This RQL is a Full Collection Query
// No auto-index is created since no filtering is applied

from "Employees"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query collection - by ID**

{CODE-TABS}
{CODE-TAB:php query_2_1@ClientApi\Session\Querying\HowToQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
// This RQL queries the 'Employees' collection by ID
// No auto-index is created when querying only by ID

from "Employees" where id() == "employees/1-A"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query collection - with filtering** 

{CODE-TABS}
{CODE-TAB:php query_3_1@ClientApi\Session\Querying\HowToQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - filter by document field

// An auto-index will be created if there isn't already an existing auto-index
// that indexes the requested field

from "Employees" where FirstName == "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Query collection - with paging** 

{CODE-TABS}
{CODE-TAB:php:Method-syntax query_4_1@ClientApi\Session\Querying\HowToQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - page results
// No auto-index is created since no filtering is applied

from "Products" limit 5, 10 // skip 5, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* By default, if the page size is not specified, all matching records will be retrieved from the database.

{NOTE/}

{PANEL/}

{PANEL: session.advanced.documentQuery}

* `documentQuery` provides a full spectrum of low-level querying capabilities,  
   giving you more flexibility and control when making complex queries.

* Below is a simple _DocumentQuery_ usage.  
  For a full description and more examples see:  
    * [What is a document query](../../../client-api/session/querying/document-query/what-is-document-query)
    * [query -vs- documentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)

**Example**:

{CODE-TABS}
{CODE-TAB:php query_5_1@ClientApi\Session\Querying\HowToQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - filter by document field

// An auto-index will be created if there isn't already an existing auto-index
// that indexes the requested field

from "Employees" where FirstName = "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: session.advanced.rawQuery}

* Queries defined with [query](../../../client-api/session/querying/how-to-query#session.query) 
  or [documentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) 
  are translated by the RavenDB client to [RQL](../../../client-api/session/querying/what-is-rql)  
  when sent to the server.

* The session also gives you a way to express the query directly in RQL using the `rawQuery` method.

**Example**:

{CODE:php query_6_1@ClientApi\Session\Querying\HowToQuery.php /}

{PANEL/}

{PANEL: Custom methods}

Available custom methods for session's [query](../../../client-api/session/querying/how-to-query#session.query) method:

- [aggregateBy](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [count](../../../client-api/session/querying/how-to-count-query-results)
- [countLazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [customize](../../../client-api/session/querying/how-to-customize-query)
- [highlight](../../../client-api/session/querying/text-search/highlight-query-results)
- [include](../../../client-api/how-to/handle-document-relationships)
- [intersect](../../../client-api/session/querying/how-to-use-intersect)
- [lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [longCount](../../../client-api/session/querying/how-to-count-query-results)
- [moreLikeThis](../../../client-api/session/querying/how-to-use-morelikethis)
- [ofType](../../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection)
- [orderByDistance](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
- [orderByDistanceDescending](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
- [orderByScore](../../../client-api/session/querying/sort-query-results#order-by-score)
- [orderByScoreDescending](../../../client-api/session/querying/sort-query-results#order-by-score)
- [projectInto](../../../client-api/session/querying/how-to-project-query-results)
- [search](../../../client-api/session/querying/text-search/full-text-search)
- [spatial](../../../client-api/session/querying/how-to-make-a-spatial-query)
- [statistics](../../../client-api/session/querying/how-to-get-query-statistics)
- [suggestUsing](../../../client-api/session/querying/how-to-work-with-suggestions)

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\Querying\HowToQuery.php /}

| Parameter          | Type   | Description                                                                                                                                                                                                                                                                                                                                                                                                              |
|--------------------|--------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **$className** | `string` | The name of the class to query |
| **$collectionOrIndexName** | `?string` | The name of the collection or index to query |
| **$indexNameOrClass** | `?string` | The name or class of the index to query |
| **$collectionName** | `?string` | The name of the collection to query |
| **$isMapReduce** | `bool` | Whether querying a map-reduce index |
| **$query** |`string` | The RQL query string |

| Return Value | |
| - | - |
| `DocumentQueryInterface`<br>`RawDocumentQueryInterface` | Interfaces exposing additional query methods and [extensions](../../../client-api/session/querying/how-to-query#custom-methods) |
 
{PANEL/}

## Related Articles

### Session

- [How to Project Query Results](../../../client-api/session/querying/how-to-project-query-results)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Querying

- [Querying an Index](../../../indexes/querying/query-index)
- [Filtering](../../../indexes/querying/filtering)
- [Paging](../../../indexes/querying/paging)
- [Sorting](../../../indexes/querying/sorting)
- [Projections](../../../indexes/querying/projections)

### Indexes

- [What are Indexes](../../../indexes/what-are-indexes)  
- [Indexing Basics](../../../indexes/indexing-basics)
- [Map Indexes](../../../indexes/map-indexes)
