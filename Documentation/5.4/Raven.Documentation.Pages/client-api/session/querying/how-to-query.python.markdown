# Query Overview

---

{NOTE: }

* Queries in RavenDB can be written with either of the following:
   * Using a rich API via session's `query` method  
   * Using the [document_query](../../../client-api/session/querying/document-query/query-vs-document-query) method  
   * Using **RQL** -  
      - when querying via session's `raw_query` method  
      - when querying through Studio's [Query view](../../../studio/database/queries/query-view)  

* Queries defined with `query` or `document_query` are translated by the RavenDB client to [RQL](../../../client-api/session/querying/what-is-rql)  
  when sent to the server.

---

* All queries in RavenDB use an **index** to provide results, even when you don't specify one.  
  Learn more [below](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* Queries that do Not specify which index to use are called **Dynamic Queries**.  
  This article displays examples of dynamic queries only.  
  For examples showing how to query an index see [querying an index](../../../indexes/querying/query-index).

---

* The entities returned by the query are 'loaded' and **tracked** by the [Session](../../../client-api/session/what-is-a-session-and-how-does-it-work).  
  Entities will Not be tracked when:  
    * Query returns a [projection](../../../client-api/session/querying/how-to-project-query-results)  
    * Tracking is [disabled](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)  

* Query results are **cached** by default. To disable query caching see [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching).

* Queries are timed out after a configurable time period.  See [query timeout](../../../server/configuration/database-configuration#databases.querytimeoutinsec).

---

* In this page:
  * [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index)  
  * [session.query](../../../client-api/session/querying/how-to-query#session.query)  
  * [session.advanced.document_query](../../../client-api/session/querying/how-to-query#session.advanced.document_query)  
  * [session.advanced.raw_query](../../../client-api/session/querying/how-to-query#session.advanced.raw_query)
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

* Example RQL: &nbsp; `from Employees where FirstName == "Laura"`  
  See more examples [below](../../../client-api/session/querying/how-to-query#session.query).

---

* Note: Counters and Time series are an exception to this flow.  
  Dynamic queries on counters and time series values don't create auto-indexes.  
  However, a static-index can be defined on [Time series](../../../document-extensions/timeseries/indexing) and [Counters](../../../document-extensions/counters/indexing).

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

* The simplest way to issue a query is using session's `query` method.  

* The following examples show **dynamic queries** that do not specify which index to use.  
  Please refer to [querying an index](../../../indexes/querying/query-index) for other examples.

* Querying can be enhanced using these [extension methods](../../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq).
 
{NOTE: }

**Query collection - no filtering** 

{CODE-TABS}
{CODE-TAB:python:Method-syntax query_1_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB:python:Query-syntax query_1_3@ClientApi\Session\Querying\HowToQuery.py /}
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
{CODE-TAB:python:Method-syntax query_2_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB:python:Query-syntax query_2_3@ClientApi\Session\Querying\HowToQuery.py /}
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
{CODE-TAB:python:Method-syntax query_3_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB:python:Query-syntax query_3_3@ClientApi\Session\Querying\HowToQuery.py /}
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
{CODE-TAB:python:Method-syntax query_4_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB:python:Query-syntax query_4_3@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - page results
// No auto-index is created since no filtering is applied

from "Products" limit 5, 10 // skip 5, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* By default, if the page size is not specified, all matching records will be retrieved from the database.

{NOTE/}

{PANEL/}

{PANEL: session.advanced.document_query}

* Below is a simple `document_query` usage sample and its RQL equivalent.  
  For a full description and more examples see:  
   * [What is a document query](../../../client-api/session/querying/document-query/what-is-document-query)
   * [query -vs- document_query](../../../client-api/session/querying/document-query/query-vs-document-query)

**Example**:

{CODE-TABS}
{CODE-TAB:python:document_query query_5_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - filter by document field

// An auto-index will be created if there isn't already an existing auto-index
// that indexes the requested field

from "Employees" where FirstName = "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: session.advanced.raw_query}

* Queries defined with [query](../../../client-api/session/querying/how-to-query#session.query) or [document_query](../../../client-api/session/querying/how-to-query#session.advanced.document_query) are translated by the RavenDB client to [RQL](../../../client-api/session/querying/what-is-rql)  
  when sent to the server.

* The session also gives you a way to express the query directly in RQL using the `raw_query` method.

**Example**:

{CODE-TABS}
{CODE-TAB:python:RawQuery query_6_1@ClientApi\Session\Querying\HowToQuery.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Custom methods}

Available custom methods for session's [query](../../../client-api/session/querying/how-to-query#session.query) method:

- [aggregate_by](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [count](../../../client-api/session/querying/how-to-count-query-results)
- [count_lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [customize](../../../client-api/session/querying/how-to-customize-query)
- [highlight](../../../client-api/session/querying/text-search/highlight-query-results)
- [include](../../../client-api/how-to/handle-document-relationships)
- [intersect](../../../client-api/session/querying/how-to-use-intersect)
- [lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [long_count](../../../client-api/session/querying/how-to-count-query-results)
- [more_like_this](../../../client-api/session/querying/how-to-use-morelikethis)
- [of_type](../../../client-api/session/querying/how-to-project-query-results#oftype-(as)---simple-projection)
- [order_by_distance](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
- [order_by_distance_descending](../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
- [order_by_score](../../../client-api/session/querying/sort-query-results#order-by-score)
- [order_by_score_descending](../../../client-api/session/querying/sort-query-results#order-by-score)
- [project_into](../../../client-api/session/querying/how-to-project-query-results)
- [search](../../../client-api/session/querying/text-search/full-text-search)
- [spatial](../../../client-api/session/querying/how-to-make-a-spatial-query)
- [statistics](../../../client-api/session/querying/how-to-get-query-statistics)
- [suggest_using](../../../client-api/session/querying/how-to-work-with-suggestions)

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax@ClientApi\Session\Querying\HowToQuery.py /}


| Parameter | Type | Description |
|--------------------|--------|------------------------------------------|
| **object_type** | `Type[_T]` | Queried entities type |
| **collection_name** | `str` | Queried collection name |
| **query** | `str` | RQL query string |
| **index_name** | `str` | Queried index name |
| **index_type** | `Type[_TIndex]` | Queried index type |
| **is_map_reduce** | `bool` | Is a map-reduce index queried |

| Return Value | |
| - | - |
| `DocumentQuery[_T]`<br>`RawDocumentQuery[_T]` | Instances exposing additional query methods and [extensions](../../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq) |
 
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
