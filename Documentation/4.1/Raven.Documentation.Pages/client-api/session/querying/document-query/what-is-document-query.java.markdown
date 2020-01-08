# Session: Querying: What is a Document Query?

Querying capabilities can be accessed via the `documentQuery` method in advanced session operations. `DocumentQuery` gives you more flexibility and control over the process of building a query.

## Syntax

{CODE:java document_query_1@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to perform a query on (exclusive with **collectionName**)  |
| **collectionName** | String | Name of a collection to perform a query on (exclusive with **indexName**) |
| **isMapReduce** | bool | Indicates if a queried index is a map/reduce index (modifies how we treat identifier properties) |

| Return Value | |
| ------------- | ----- |
| **IDocumentQuery** | Instance implementing IDocumentQuery interface containing additional query methods and extensions |

## Example I - Basic

{CODE:java document_query_2@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.java /}

{CODE:java document_query_3@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.java /}

## Example II - Querying Specified Index

{CODE:java document_query_4@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.java /}

or

{CODE:java document_query_5@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.java /}

## Custom Methods and Extensions

{NOTE Functionality of most of the methods match the functionality of their `query` counterparts and therefore will not be described again. Please refer to the appropriate counterpart documentation articles. Links starting with `[query]` are marking those articles. /}

Available methods:

- addAfterQueryExecutedListener
- addBeforeQueryExecutedListener
- addOrder
- addParameter
- [Query] [aggregateBy](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [Query] [aggregateUsing](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- andAlso
- boost
- closeSubclause
- containsAll
- containsAny
- count
- countLazily
- distinct
- first
- firstOrDefault
- fuzzy
- [groupBy](../../../../client-api/session/querying/how-to-perform-group-by-query)
- [groupByArrayValues](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)
- [groupByArrayContent](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- include
- intersect
- invokeAfterQueryExecuted
- invokeAfterStreamExecuted
- [Query] [lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- moreLikeThis
- negateNext
- [Query] [noCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [not](../../../../client-api/session/querying/document-query/how-to-use-not-operator)
- [Query] [noTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- ofType
- openSubclause
- orderBy
- orderByDescending
- [Query] [orderByDistance](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- [Query] [orderByDistanceDescending](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- orderByScore
- orderByScoreDescending
- orElse
- proximity
- randomOrdering
- [Query] [relatesToShape](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- search
- selectFields
- single
- singleOrDefault
- skip
- [Query] [spatial](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- statistics
- suggestUsing
- take
- usingDefaultOperator
- [Query] [waitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [Query] [waitForNonStaleResultsAsOf](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- where
- whereBetween
- whereEndsWith
- whereEquals
- [whereExists](../../../../client-api/session/querying/how-to-filter-by-field)
- whereGreaterThan
- whereGreaterThanOrEqual
- whereIn
- whereLessThan
- whereLessThanOrEqual
- [whereLucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
- whereNotEquals
- [whereRegex](../../../../client-api/session/querying/how-to-use-regex)
- whereStartsWith
- [Query] [withinRadiusOf](../../../../client-api/session/querying/how-to-query-a-spatial-index)


## Remarks

By default, if the `page size` is not specified, all of the matching records will be retrieved from a database.

## Related Articles

### Session

- [How to Query](../../../../client-api/session/querying/how-to-query)
- [How to Use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Querying 

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
- [Projections](../../../../indexes/querying/projections)
