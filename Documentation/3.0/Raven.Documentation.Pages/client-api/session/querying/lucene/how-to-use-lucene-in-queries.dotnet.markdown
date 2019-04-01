# Session: Querying: How to use Lucene in queries?

Lucene can be used directly by using `DocumentQuery` method from advanced session operations. It allows us to acquire low-level access to query structure which gives more flexibility.

## Syntax

{CODE document_query_1@ClientApi\Session\Querying\Lucene\HowToUseLucene.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform query on |
| **isMapReduce** | bool | Indicates if queried index is a map/reduce index (modifies how we treat identifier properties). |

| Return Value | |
| ------------- | ----- |
| **IDocumentQuery** | Instance implementing IDocumentQuery interface containing additional query methods and extensions. |

## Example I - Basic

{CODE document_query_2@ClientApi\Session\Querying\Lucene\HowToUseLucene.cs /}

{CODE document_query_3@ClientApi\Session\Querying\Lucene\HowToUseLucene.cs /}

## Example II - Querying specified index

{CODE document_query_4@ClientApi\Session\Querying\Lucene\HowToUseLucene.cs /}

or

{CODE document_query_5@ClientApi\Session\Querying\Lucene\HowToUseLucene.cs /}

## Custom methods and extensions

{NOTE Functionality of most of the methods matches functionality of their `Query` counterparts and therefore will not be described again. Please refer to appropriate counterpart documentation articles. Links starting with `[Query]` are marking those articles. /}

Available custom methods and extensions:   

- AddOrder
- AfterQueryExecuted
- AndAlso
- BeforeQueryExecution
- Boost
- CloseSubclause
- ContainsAll
- ContainsAny
- CountLazily
- Distinct
- ExplainScores
- Fuzzy
- [Query] [GetFacets](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- GetIndexQuery
- [Query] [Highlight](../../../../client-api/session/querying/how-to-use-highlighting)
- Include
- Intersect
- InvokeAfterQueryExecuted
- [Query] [Lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- NegateNext
- [Query] [NoCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [Query] [NoTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- [Not](../../../../client-api/session/querying/lucene/how-to-use-not-operator)
- OpenSubclause
- OrElse
- OrderBy
- OrderByDescending
- OrderByScore
- OrderByScoreDescending
- Proximity
- RandomOrdering
- [Query] [RelatesToShape](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- Search
- SelectFields
- [Query] [SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer](../../../../client-api/session/querying/how-to-customize-query#setallowmultipleindexentriesforsamedocumenttoresulttransformer)
- [Query] [SetHighlighterTags](../../../../client-api/session/querying/how-to-use-highlighting)
- [Query] [SetResultTransformer](../../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [Query] [SetTransformerParameters](../../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [Query] [ShowTimings](../../../../client-api/session/querying/how-to-customize-query#showtimings)
- [Query] [SortByDistance](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- [Query] [Spatial](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- Statistics
- UsingDefaultField
- UsingDefaultOperator
- [Query] [WaitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [Query] [WaitForNonStaleResultsAsOf](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- [Query] [WaitForNonStaleResultsAsOfLastWrite](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasoflastwrite)
- [Query] [WaitForNonStaleResultsAsOfNow](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasofnow)
- Where
- WhereBetween
- WhereBetweenOrEqual
- WhereEndsWith
- WhereEquals
- WhereGreaterThan
- WhereGreaterThanOrEqual
- WhereIn
- WhereLessThan
- WhereLessThanOrEqual
- WhereStartsWith
- [Query] [WithinRadiusOf](../../../../client-api/session/querying/how-to-query-a-spatial-index)

## Remarks

By default, if `page size` is not specified, the value will be set to `128`. This is part of **Safe-by-Default** approach.

## Related articles

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
