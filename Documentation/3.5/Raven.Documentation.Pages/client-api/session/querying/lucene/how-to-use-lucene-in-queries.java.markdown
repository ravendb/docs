# Session: Querying: How to use Lucene in queries?

Lucene can be used directly by using `documentQuery` method from advanced session operations. It allows us to acquire low-level access to query structure which gives more flexibility.

## Syntax

{CODE:java document_query_1@ClientApi\Session\Querying\Lucene\HowToUseLucene.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to perform query on |
| **isMapReduce** | boolean | Indicates if queried index is a map/reduce index (modifies how we treat identifier properties). |

| Return Value | |
| ------------- | ----- |
| **IDocumentQuery** | Instance implementing IDocumentQuery interface containing additional query methods and extensions. |

## Example I - Basic

{CODE:java document_query_2@ClientApi\Session\Querying\Lucene\HowToUseLucene.java /}

{CODE:java document_query_3@ClientApi\Session\Querying\Lucene\HowToUseLucene.java /}

## Example II - Querying specified index

{CODE:java document_query_4@ClientApi\Session\Querying\Lucene\HowToUseLucene.java /}

or

{CODE:java document_query_5@ClientApi\Session\Querying\Lucene\HowToUseLucene.java /}

## Custom methods and extensions

{NOTE Functionality of most of the methods matches functionality of their `query` counterparts and therefore will not be described again. Please refer to appropriate counterpart documentation articles. Links starting with `[Query]` are marking those articles. /}

Available custom methods and extensions:

- addOrder
- afterQueryExecuted
- andAlso
- beforeQueryExecution
- boost
- closeSubclause
- containsAll
- containsAny
- countLazily
- distinct
- explainScores
- fuzzy
- [query] [getFacets](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- getIndexQuery
- [query] [highlight](../../../../client-api/session/querying/how-to-use-highlighting)
- include
- intersect
- invokeAfterQueryExecuted
- [query] [lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- negateNext
- [query] [noCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [query] [noTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- not
- openSubclause
- orElse
- orderBy
- orderByDescending
- orderByScore
- orderByScoreDescending
- proximity
- randomOrdering
- [query] [RelatesToShape](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- search
- selectFields
- [query] [setAllowMultipleIndexEntriesForSameDocumentToResultTransformer](../../../../client-api/session/querying/how-to-customize-query#setallowmultipleindexentriesforsamedocumenttoresulttransformer)
- [query] [setHighlighterTags](../../../../client-api/session/querying/how-to-use-highlighting)
- [query] [setResultTransformer](../../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [query] [setTransformerParameters](../../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [query] [showTimings](../../../../client-api/session/querying/how-to-customize-query#showtimings)
- [query] [sortByDistance](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- [query] [spatial](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- statistics
- usingDefaultField
- usingDefaultOperator
- [query] [waitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [query] [waitForNonStaleResultsAsOf](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- [query] [waitForNonStaleResultsAsOfLastWrite](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasoflastwrite)
- [query] [waitForNonStaleResultsAsOfNow](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasofnow)
- where
- whereBetween
- whereBetweenOrEqual
- whereEndsWith
- whereEquals
- whereGreaterThan
- whereGreaterThanOrEqual
- whereIn
- whereLessThan
- whereLessThanOrEqual
- whereStartsWith
- [query] [withinRadiusOf](../../../../client-api/session/querying/how-to-query-a-spatial-index)

## Remarks

By default, if `page size` is not specified, the value will be set to `128`. This is part of **Safe-by-Default** approach.

## Related articles

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
