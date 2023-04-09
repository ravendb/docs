# Query Overview

---

{NOTE: }

* Queries in RavenDB can be written with:
    * The session's `query` method - rich API is provided
    * The session's  `rawQuery` method - using RQL
    * The [Query view](../../../studio/database/queries/query-view) in Studio - using RQL

* Queries defined with the `query` method are translated by the RavenDB client to [RQL](../../../client-api/session/querying/what-is-rql)  
  when sent to the server.

---

* All queries in RavenDB use an __index__ to provide results, even when you don't specify one.  
  Learn more [below](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* Queries that do Not specify which index to use are called __Dynamic Queries__.  
  Examples in this article show dynamic queries.  
  See [querying an index](../../../indexes/querying/basics) for other examples.

---

* The entities returned by the query are 'loaded' and __tracked__ by the [Session](../../../client-api/session/what-is-a-session-and-how-does-it-work).  
  Entities will Not be tracked when:  
    * Query returns a [projection](../../../client-api/session/querying/how-to-project-query-results)  
    * Tracking is [disabled](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)  

* Queries results are __cached__ by default. To disable query caching see [noCaching](../../../client-api/session/querying/how-to-customize-query#nocaching).  

* Queries are timed out after a configurable time period.  See [query timeout](../../../server/configuration/database-configuration#databases.querytimeoutinsec).

---

* In this page:
  * [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index)  
  * [session.query](../../../client-api/session/querying/how-to-query#session.query)  
  * [session.advanced.rawQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery)
  * [query API](../../../client-api/session/querying/how-to-query#query-api)  

{NOTE/}

---

{PANEL: Queries always provide results using an index} 

* Queries always use an index to provide fast results regardless of the size of your data.  
  Indexes allow to provide query results without scanning the entire dataset each and every time.  
  Learn more about indexes in [indexes overview](../../../studio/database/indexes/indexes-overview). 

* We differentiate between the following __3 query scenarios__.  
  For each scenario, a different index type will be used.

{NOTE: }

__Query an existing index__:

* You can specify which __static index__ the query will use.

* Static indexes are defined by the user, as opposed to auto-indexes that are created by the server  
  when querying a collection with some filtering applied.

{NOTE/}

{NOTE: }

__Query a collection - with filtering (Dynamic Query)__:

* When querying a collection without specifying an index and with some filtering condition,   
  the query-optimizer will analyze the query to see if an __auto-index__ that can answer the query already exists,  
  i.e. an auto-index on the collection queried with index-fields that match those queried.  

* If such auto-index (Not a static one...) is found, it will be used to fetch the results.  
  Else, the query-optimizer will create a new auto-index with matching fields.

* Note: if there exists an auto-index that is defined on the collection queried  
  but is indexing a different field than the one queried on,  
  then the query-optimizer will create a new auto-index that merges both the  
  fields from the existing auto-index and the new fields queried.

* Once the newly created auto-index is done indexing the data,  
  the old auto-index is removed in favor of the new one.

{NOTE/}

{NOTE: }

__Query a collection - no filtering__:

* Full collection query:

  * When querying a collection without specifying an index and with no filtering condition,  
    then all documents from the specified collection are returned.

  * RavenDB uses the raw collection documents in its __internal storage indexes__ as the source for this query.  
    No auto-index is created.

* Query by document ID:
 
  * When querying a collection only by document ID or IDs,  
    then similar to the full collection query, no auto-index is created.  
    RavenDB uses the raw collection documents as the source for this query.  

{NOTE/}

{PANEL/}

{PANEL: session.query}

* The simplest way to issue a query is by using the session's `query` method.    
  Customize your query with these [API methods](../../../client-api/session/querying/how-to-query#query-method-api).

* The following examples show __dynamic queries__ that do not specify which index to use.  
  Please refer to [querying an index](../../../indexes/querying/basics) for other examples.

{NOTE: }

__Query collection - no filtering__ 

{CODE-TABS}
{CODE-TAB:nodejs:Query query_1@ClientApi\Session\Querying\howToQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// This RQL is a Full Collection Query
// No auto-index is created since no filtering is applied

from "employees"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection by ID__

{CODE-TABS}
{CODE-TAB:nodejs:Query query_2@ClientApi\Session\Querying\howToQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// This RQL queries the 'Employees' collection by ID
// No auto-index is created when querying only by ID

from "employees" where id() = "employees/1-A"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection - with filtering__ 

{CODE-TABS}
{CODE-TAB:nodejs:Query query_3@ClientApi\Session\Querying\howToQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - filter by document field

// An auto-index will be created if there isn't already an existing auto-index
// that indexes the requested field

from "employees" where firstName = "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection with paging__ 

{CODE-TABS}
{CODE-TAB:nodejs:Query query_4@ClientApi\Session\Querying\howToQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Query collection - page results
// No auto-index is created since no filtering is applied

from "products" limit 5, 10 // skip 5, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

---

__Syntax__:

{CODE:nodejs syntax@ClientApi\Session\Querying\howToQuery.js /}

| Parameter | Type | Description |
| - | - | - |
| __opts__ | `DocumentQueryOptions` object | Query options |

| `DocumentQueryOptions` | | | 
| - | - | - |
|  __collection__ | string | <ul><li>Collection name queried</li></ul> |
|  __indexName__ | string | <ul><li>Index name queried</li></ul> |
|  __index__ | object | <ul><li>Index object queried</li><li>Note:<br>`indexName` & `index` are mutually exclusive with `collection`.<br>See examples in [querying an index](../../../indexes/querying/basics).</li></ul> |

| Return Value | |
| - | - |
| `object` | Instance implementing `IDocumentQuery` exposing the additional [query methods](../../../client-api/session/querying/how-to-query#query-method-api). |

* Note:  
  Use `await` when executing the query, e.g. when calling `.all`, `.single`, `.first`, `.count`, etc.  

{PANEL/}

{PANEL: session.advanced.rawQuery}

* Queries defined with [query](../../../client-api/session/querying/how-to-query#session.query) are translated by the RavenDB client to [RQL](../../../client-api/session/querying/what-is-rql) when sent to the server.  

* The session also gives you a way to express the query directly in RQL using the `rawQuery` method.

__Example__:

{CODE:nodejs query_5@ClientApi\Session\Querying\howToQuery.js /}

{PANEL/}

{PANEL: query API}

Available methods for the session's [query](../../../client-api/session/querying/how-to-query#session.query) method:

- addOrder
- addParameter
- aggregateBy
- aggregateUsing
- andAlso
- any
- boost
- closeSubclause
- containsAll
- containsAny
- count
- countLazily
- distinct
- first
- firstOrNull
- fuzzy
- getQueryResult
- [groupBy](../../../client-api/session/querying/how-to-perform-group-by-query)
- highlight
- include
- [includeExplanations](../../../client-api/session/querying/debugging/include-explanations)
- getIndexQuery
- [intersect](../../../client-api/session/querying/how-to-use-intersect)
- lazily
- longCount
- [moreLikeThis](../../../client-api/session/querying/how-to-use-morelikethis)
- negateNext
- noCaching
- noTracking
- not
- [ofType](../../../client-api/session/querying/how-to-project-query-results#oftype)
- openSubclause
- orderBy
- orderByDescending
- orderByDistance
- orderByDistanceDescending
- orderByScore
- orderByScoreDescending
- orElse
- proximity
- randomOrdering
- relatesToShape
- search
- selectFields
- selectTimeSeries
- single
- singleOrNull
- skip
- spatial
- [statistics](../../../client-api/session/querying/how-to-get-query-statistics)
- suggestUsing
- take
- timings
- usingDefaultOperator
- waitForNonStaleResults
- whereBetween
- whereEndsWith
- whereEquals
- [whereExists](../../../client-api/session/querying/how-to-filter-by-field)
- whereGreaterThan
- whereGreaterThanOrEqual
- whereIn
- whereLessThan
- whereLessThanOrEqual
- [whereLucene](../../../client-api/session/querying/document-query/how-to-use-lucene)
- whereNotEquals
- [whereRegex](../../../client-api/session/querying/how-to-use-regex)
- whereStartsWith
- withinRadiusOf

{PANEL/}

## Related Articles

### Session

- [How to Project Query Results](../../../client-api/session/querying/how-to-project-query-results)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Querying

- [Basics](../../../indexes/querying/basics)
- [Filtering](../../../indexes/querying/filtering)
- [Paging](../../../indexes/querying/paging)
- [Sorting](../../../indexes/querying/sorting)
- [Projections](../../../indexes/querying/projections)

### Indexes

- [What are Indexes](../../../indexes/what-are-indexes)  
- [Indexing Basics](../../../indexes/indexing-basics)
- [Map Indexes](../../../indexes/map-indexes)
