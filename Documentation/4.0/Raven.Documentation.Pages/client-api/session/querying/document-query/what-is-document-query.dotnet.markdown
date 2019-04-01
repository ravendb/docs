# Session: Querying: What is a Document Query?

Low-level querying capabilities can be accessed via the `DocumentQuery` method in advanced session operations. `DocumentQuery` gives you more flexibility and control over the process of building a query.

## Syntax

{CODE document_query_1@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform a query on (exclusive with **collectionName**)  |
| **collectionName** | string | Name of a collection to perform a query on (exclusive with **indexName**) |
| **isMapReduce** | bool | Indicates if a queried index is a map/reduce index (modifies how we treat identifier properties) |

| Return Value | |
| ------------- | ----- |
| **IDocumentQuery** | Instance implementing IDocumentQuery interface containing additional query methods and extensions |

## Example I - Basic

{CODE document_query_2@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /}

{CODE document_query_3@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /}

## Example II - Querying Specified Index

{CODE document_query_4@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /}

or

{CODE document_query_5@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /}

## Custom Methods and Extensions

{NOTE Functionality of most of the methods match the functionality of their `Query` counterparts and therefore will not be described again. Please refer to the appropriate counterpart documentation articles. Links starting with `[Query]` are marking those articles. /}

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
- [GroupBy](../../../../client-api/session/querying/how-to-perform-group-by-query)
- [GroupByArrayValues](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)
- [GroupByArrayContent](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
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
- [WhereLucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
- WhereNotEquals
- [WhereRegex](../../../../client-api/session/querying/how-to-use-regex)
- WhereStartsWith
- [Query] [WithinRadiusOf](../../../../client-api/session/querying/how-to-query-a-spatial-index)

## Remarks

By default, if the `page size` is not specified, all of the matching records will be retrieved from a database.

## Related Articles

### Session

- [How to Query](../../../../client-api/session/querying/how-to-query)
- [How to Use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Querying 

- [Query vs DocumentQuery](../../../../indexes/querying/query-vs-document-query)
