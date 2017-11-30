# Session : Querying : What is a Document Query?

Low-level querying capabilities can be accessed via `DocumentQuery` method in advanced session operations. `DocumentQuery` gives user more flexibility and control over the process of building a query.

## Syntax

{CODE document_query_1@ClientApi\Session\Querying\DQ\WhatIsDocumentQuery.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform query on (exclusive with **collectionName**)  |
| **collectionName** | string | Name of an collection to perform query on (exclusive with **indexName**) |
| **isMapReduce** | bool | Indicates if queried index is a map/reduce index (modifies how we treat identifier properties) |

| Return Value | |
| ------------- | ----- |
| **IDocumentQuery** | Instance implementing IDocumentQuery interface containing additional query methods and extensions |

## Example I - Basic

{CODE document_query_2@ClientApi\Session\Querying\DQ\WhatIsDocumentQuery.cs /}

{CODE document_query_3@ClientApi\Session\Querying\DQ\WhatIsDocumentQuery.cs /}

## Example II - Querying specified index

{CODE document_query_4@ClientApi\Session\Querying\DQ\WhatIsDocumentQuery.cs /}

or

{CODE document_query_5@ClientApi\Session\Querying\DQ\WhatIsDocumentQuery.cs /}

## Custom methods and extensions

{NOTE Functionality of most of the methods matches functionality of their `Query` counterparts and therefore will not be described again. Please refer to appropriate counterpart documentation articles. Links starting with `[Query]` are marking those articles. /}

Available custom methods and extensions:   

- AddOrder
- AfterQueryExecuted
- AfterStreamExecuted
- [Query] [AggregateBy](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [Query] [AggregateUsing](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- AndAlso
- BeforeQueryExecuted
- Boost
- CloseSubclause
- CmpXchg
- ContainsAll
- ContainsAny
- Count
- CountLazily
- Distinct
- ExplainScores
- First
- FirstOrDefault
- Fuzzy
- [Not](../../../../client-api/session/querying/document-query/how-to-use-not-operator)
- GroupBy
- Include
- Intersect
- InvokeAfterQueryExecuted
- InvokeAfterStreamExecuted
- [Query] [Lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- MoreLikeThis
- NegateNext
- [Query] [NoCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [Query] [NoTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- OfType
- OpenSubclause
- OrderBy
- OrderByDescending
- [Query] [OrderByDistance](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- [Query] [OrderByDistanceDescending](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- OrderByScore
- OrderByScoreDescending
- OrElse
- Proximity
- RandomOrdering
- [Query] [RelatesToShape](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- Search
- SelectFields
- Single
- SingleOrDefault
- Skip
- [Query] [Spatial](../../../../client-api/session/querying/how-to-query-a-spatial-index)
- Statistics
- SuggestUsing
- Take
- UsingDefaultOperator
- [Query] [WaitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [Query] [WaitForNonStaleResultsAsOf](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- Where
- WhereBetween
- WhereEndsWith
- WhereEquals
- WhereExists
- WhereGreaterThan
- WhereGreaterThanOrEqual
- WhereIn
- WhereLessThan
- WhereLessThanOrEqual
- WhereLucene
- WhereNotEquals
- WhereRegex
- WhereStartsWith
- [Query] [WithinRadiusOf](../../../../client-api/session/querying/how-to-query-a-spatial-index)

## Remarks

By default, if `page size` is not specified, all of the matching records will be retrieved from a database.

## Related articles

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
