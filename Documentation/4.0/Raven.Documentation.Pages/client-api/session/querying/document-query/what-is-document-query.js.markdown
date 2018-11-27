# Session : Querying : What is a Document Query?

Querying capabilities can be accessed via the `documentQuery()` method in advanced session operations. `DocumentQuery` gives you more flexibility and control over the process of building a query.

## Syntax

{CODE:nodejs document_query_1@clientApi\session\querying\documentQuery\whatIsDocumentQuery.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentType** | class | A class constructor used for reviving the results' entities from which the collection name is determined |
| **options** | object | |
| &nbsp;&nbsp;*indexName* | string | Name of an index to perform a query on (exclusive with *collectionName*)  |
| &nbsp;&nbsp;*collection* | string | Name of a collection to perform a query on (exclusive with *indexName*) |
| &nbsp;&nbsp;*isMapReduce* | bool | Indicates if a queried index is a map/reduce index (modifies how we treat identifier properties) |
| &nbsp;&nbsp;*documentType* | function | A class constructor used for reviving the results' entities |

| Return Value | |
| ------------- | ----- |
| `Promise<IDocumentQuery>` | Promise resolving to query instance implementing IDocumentQuery interface containing additional query methods and extensions |

## Example I - Basic

{CODE:nodejs document_query_2@clientApi\session\querying\documentQuery\whatIsDocumentQuery.js /}

{CODE:nodejs document_query_3@clientApi\session\querying\documentQuery\whatIsDocumentQuery.js /}

## Example II - Querying Specified Index

{CODE:nodejs document_query_4@clientApi\session\querying\documentQuery\whatIsDocumentQuery.js /}

## Custom Methods and Extensions

{NOTE Functionality of most of the methods match the functionality of their `query` counterparts and therefore will not be described again. Please refer to the appropriate counterpart documentation articles. Links starting with `[query]` are marking those articles. /}

Document query object is an event emitter emitting the following events:

- 'afterQueryExecuted`
- 'beforeQueryExecuted`

Available methods:

- addOrder
- addParameter
- [Query] [aggregateBy](../../../../clientApi/session/querying/how-to-perform-a-faceted-search)
- [Query] [aggregateUsing](../../../../clientApi/session/querying/how-to-perform-a-faceted-search)
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
- [groupBy](../../../../clientApi/session/querying/how-to-perform-group-by-query)
- [groupByArrayValues](../../../../clientApi/session/querying/how-to-perform-group-by-query#by-array-values)
- [groupByArrayContent](../../../../clientApi/session/querying/how-to-perform-group-by-query#by-array-content)
- include
- intersect
- invokeAfterQueryExecuted
- invokeAfterStreamExecuted
- [Query] [lazily](../../../../clientApi/session/querying/how-to-perform-queries-lazily)
- moreLikeThis
- negateNext
- [Query] [noCaching](../../../../clientApi/session/querying/how-to-customize-query#nocaching)
- [not](../../../../clientApi/session/querying/document-query/how-to-use-not-operator)
- [Query] [noTracking](../../../../clientApi/session/querying/how-to-customize-query#notracking)
- ofType
- openSubclause
- orderBy
- orderByDescending
- [Query] [orderByDistance](../../../../clientApi/session/querying/how-to-query-a-spatial-index)
- [Query] [orderByDistanceDescending](../../../../clientApi/session/querying/how-to-query-a-spatial-index)
- orderByScore
- orderByScoreDescending
- orElse
- proximity
- randomOrdering
- [Query] [relatesToShape](../../../../clientApi/session/querying/how-to-query-a-spatial-index)
- search
- selectFields
- single
- singleOrDefault
- skip
- [Query] [spatial](../../../../clientApi/session/querying/how-to-query-a-spatial-index)
- statistics
- suggestUsing
- take
- usingDefaultOperator
- [Query] [waitForNonStaleResults](../../../../clientApi/session/querying/how-to-customize-query#waitfornonstaleresults)
- [Query] [waitForNonStaleResultsAsOf](../../../../clientApi/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- where
- whereBetween
- whereEndsWith
- whereEquals
- whereExists
- whereGreaterThan
- whereGreaterThanOrEqual
- whereIn
- whereLessThan
- whereLessThanOrEqual
- [whereLucene](../../../../clientApi/session/querying/document-query/how-to-use-lucene)
- whereNotEquals
- [whereRegex](../../../../clientApi/session/querying/how-to-use-regex)
- whereStartsWith
- [Query] [withinRadiusOf](../../../../clientApi/session/querying/how-to-query-a-spatial-index)


## Remarks

By default, if the `page size` is not specified, all of the matching records will be retrieved from a database.

## Related Articles

### Session

- [How to Query](../../../../clientApi/session/querying/how-to-query)
- [How to Use Lucene](../../../../clientApi/session/querying/document-query/how-to-use-lucene)

### Querying 

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
