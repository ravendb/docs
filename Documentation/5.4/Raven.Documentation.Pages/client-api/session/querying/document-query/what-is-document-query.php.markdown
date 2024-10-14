# What is a Document Query?

---

{NOTE: }

* RavenDB queries can be executed via `query`, `documentQuery` or directly using `RQL`.  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* See [Query -vs- documentQuery](../../../../client-api/session/querying/document-query/query-vs-document-query) 
  for additional details.

* In this page:
  * [documentQuery examples](../../../../client-api/session/querying/document-query/what-is-document-query#documentquery-examples)
  * [Custom methods](../../../../client-api/session/querying/document-query/what-is-document-query#custom-methods)
  * [Syntax](../../../../client-api/session/querying/document-query/what-is-document-query#syntax)

{NOTE/}

---

{PANEL: documentQuery examples}

#### Query collection - no filtering

{CODE-TABS}
{CODE-TAB:php:documentQuery documentQuery_2@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.php /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query collection - with filtering

{CODE-TABS}
{CODE-TAB:php:documentQuery documentQuery_3@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.php /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query an index

* Using a Path string
  {CODE:php documentQuery_4@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.php /})

* Using an index Class
  {CODE:php documentQuery_5@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.php /})

{NOTE: }
Please refer to [Querying an index](../../../../indexes/querying/query-index#session.advanced.documentquery) for examples of querying an index using a documentQuery.
{NOTE/}

{PANEL/}

{PANEL: Custom Methods}

{NOTE: }

Several methods share the same functionality as their `query` counterparts.  
Refer to the corresponding documentation articles, marked with links starting with "[Query]" in the list below.

{NOTE/}

Available custom methods:   

- AddOrder
- [query] [afterQueryExecuted](../../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- [query] [afterStreamExecuted](../../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [query] [aggregateBy](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [query] [aggregateUsing](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- andAlso
- [query] [beforeQueryExecuted](../../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- [boost](../../../../client-api/session/querying/text-search/boost-search-results)
- closeSubclause
- cmpXchg
- containsAll
- containsAny
- [count](../../../../client-api/session/querying/how-to-count-query-results)
- [countLazily](../../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-count-query)
- distinct
- explainScores
- first
- firstOrDefault
- fuzzy
- getIndexQuery
- getQueryResult
- [groupBy](../../../../client-api/session/querying/how-to-perform-group-by-query)
- [groupByArrayValues](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)
- [groupByArrayContent](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- [query] [Highlight](../../../../client-api/session/querying/text-search/highlight-query-results)
- include
- includeExplanations
- intersect
- invokeAfterQueryExecuted
- invokeAfterStreamExecuted
- [query] [lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- [longCount](../../../../client-api/session/querying/how-to-count-query-results)
- moreLikeThis
- negateNext
- [not](../../../../client-api/session/querying/document-query/how-to-use-not-operator)
- [query] [noCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [query] [noTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- ofType
- openSubclause
- [orderBy](../../../../client-api/session/querying/sort-query-results)
- [orderByDescending](../../../../client-api/session/querying/sort-query-results)
- [query] [orderByDistance](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
- [query] [orderByDistanceDescending](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
- [orderByScore](../../../../client-api/session/querying/sort-query-results#order-by-score)
- [orderByScoreDescending](../../../../client-api/session/querying/sort-query-results#order-by-score)
- orElse
- [query] [projection](../../../../client-api/session/querying/how-to-customize-query#projection)
- proximity
- [query] [randomOrdering](../../../../client-api/session/querying/how-to-customize-query#randomordering)
- [query] [relatesToShape](../../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape)
- [search](../../../../client-api/session/querying/text-search/full-text-search)
- selectFields
- selectTimeSeries
- single
- singleOrDefault
- skip
- [query] [spatial](../../../../client-api/session/querying/how-to-make-a-spatial-query)
- statistics
- [suggestUsing](../../../../client-api/session/querying/how-to-work-with-suggestions)
- take
- [query] [timings](../../../../client-api/session/querying/how-to-customize-query#timings)
- usingDefaultOperator
- [query] [waitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- where
- whereBetween
- [whereEndsWith](../../../../client-api/session/querying/text-search/ends-with-query)
- whereEquals
- [whereExists](../../../../client-api/session/querying/how-to-filter-by-field)
- whereGreaterThan
- whereGreaterThanOrEqual
- whereIn
- whereLessThan
- whereLessThanOrEqual
- [whereLucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
- whereNotEquals
- [whereRegex](../../../../client-api/session/querying/text-search/using-regex)
- [whereStartsWith](../../../../client-api/session/querying/text-search/starts-with-query)
- withinRadiusOf

{PANEL/}

{PANEL: Syntax}

The definition for `documentQuery` is listed in the [Syntax section](../../../../client-api/session/querying/how-to-query#syntax) 
of the [Query Overview](../../../../client-api/session/querying/how-to-query).

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Querying 

- [Query vs documentQuery](../../../../client-api/session/querying/document-query/query-vs-document-query)
- [Projections](../../../../indexes/querying/projections)
