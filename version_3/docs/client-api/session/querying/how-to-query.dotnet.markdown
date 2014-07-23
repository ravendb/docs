# Session : Querying : How to query?

Database can be queried using LINQ-enabled `Query` method with few custom extension that will be described later.

## Syntax

{CODE query_1_0@ClientApi\Session\Querying\HowToQuery.cs /}

**Parameters**

indexName
:   Type: string   
Name of an index to perform query on

isMapReduce
:   Type: bool   
Indicates if queried index is a map/reduce index (modifies how we treat identifier properties).

## Example I - Basic

{CODE query_1_1@ClientApi\Session\Querying\HowToQuery.cs /}

Notice that by specifying `Person` as a type parameter, we are not only defining a result type, but also marking name of collection that will be queried.

## Example II - Syntax

Queries can be performed using both LINQ syntaxes.

{CODE query_1_2@ClientApi\Session\Querying\HowToQuery.cs /}

{CODE query_1_3@ClientApi\Session\Querying\HowToQuery.cs /}

## Example III - Querying specified index

{CODE query_1_4@ClientApi\Session\Querying\HowToQuery.cs /}

or 

{CODE query_1_5@ClientApi\Session\Querying\HowToQuery.cs /}

## Custom methods and extensions

Available custom methods and extensions:

- [AddTransformerParameter](../../../client-api/session/querying/how-to-use-transformers-in-queries)
- [AggregateBy](../../../client-api/session/querying/how-to-perform-dynamic-aggregation)
- AnyAsync
- [As](../../../client-api/session/querying/how-to-perform-projection)
- [AsProjection](../../../client-api/session/querying/how-to-perform-projection)
- CountAsync
- [CountLazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [Customize](../../../client-api/session/querying/how-to-customize-query)
- FirstAsync
- FirstOrDefaultAsync
- [Include]()
- [Intersect](../../../client-api/session/querying/how-to-intersect-queries)
- [Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [LazilyAsync](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [OrderByScore]()
- [ProjectFromIndexFieldsInto](../../../client-api/session/querying/how-to-perform-projection)
- [Search](../../../client-api/session/querying/how-to-use-search)
- SingleAsync
- SingleOrDefaultAsync
- [Spatial](../../../client-api/session/querying/how-to-query-a-spatial-index)
- [Statistics](../../../client-api/session/querying/how-to-get-query-statistics)
- [Suggest](../../../client-api/session/querying/how-to-work-with-suggestions)
- [SuggestAsync](../../../client-api/session/querying/how-to-work-with-suggestions)
- [SuggestLazy](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [ToFacetQuery](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [ToFacets](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [ToFacetsAsync](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [ToFacetsLazy](../../../client-api/session/querying/how-to-perform-a-faceted-search)
- ToListAsync
- [TransformWith](../../../client-api/session/querying/how-to-use-transformers-in-queries)

## Remarks

By default, if `page size` is not specified, the value will be set to `128`. This is part of **Safe-by-Default** approach.
