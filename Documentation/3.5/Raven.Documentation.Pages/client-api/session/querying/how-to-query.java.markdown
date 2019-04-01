# Session: Querying: How to query?

Database can be queried using QueryDSL-enabled `query` method with few custom extension that will be described later.

## Syntax

{CODE:java query_1_0@ClientApi\Session\Querying\HowToQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to perform query on |
| **isMapReduce** | boolean | Indicates if queried index is a map/reduce index (modifies how we treat identifier properties). |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods. |

{SAFE The default value of a page size for a query is `128` results. In order to retrieve a different number of results in a single query use `.Take(pageSize)` method. /}

## Example I - Basic

{CODE:java query_1_1@ClientApi\Session\Querying\HowToQuery.java /}

Notice that by specifying `Employee` as a class parameter, we are not only defining a result type, but also marking name of collection that will be queried.

## Example II - Syntax

Queries can be performed using strongly typed syntax.

{CODE:java query_1_2@ClientApi\Session\Querying\HowToQuery.java /}

## Example III - Querying specified index

{CODE:java query_1_4@ClientApi\Session\Querying\HowToQuery.java /}

or 

{CODE:java query_1_5@ClientApi\Session\Querying\HowToQuery.java /}

## Custom methods and extensions

Available custom methods and extensions:

- [AddTransformerParameter](../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [AggregateBy](../../../client-api/session/querying/how-to-perform-dynamic-aggregation)
- [As](../../../client-api/session/querying/how-to-perform-projection)
- [AsProjection](../../../client-api/session/querying/how-to-perform-projection)
- [CountLazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [Customize](../../../client-api/session/querying/how-to-customize-query)
- [Include](../../../indexes/querying/handling-document-relationships)
- [Intersect](../../../client-api/session/querying/how-to-use-intersect)
- [Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- OrderByScore
- [ProjectFromIndexFieldsInto](../../../client-api/session/querying/how-to-perform-projection)
- [Search](../../../client-api/session/querying/how-to-use-search)
- [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index)
- [Statistics](../../../client-api/session/querying/how-to-get-query-statistics)
- [Suggest](../../../client-api/session/querying/how-to-work-with-suggestions)
- [SuggestLazy](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [ToFacetQuery](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [ToFacets](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [ToFacetsLazy](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [TransformWith](../../../client-api/session/querying/how-to-use-transformers-in-queries)

## Related articles

- [What are indexes?](../../../indexes/what-are-indexes)   
- [Indexes : Querying: Basics](../../../indexes/querying/basics)  
