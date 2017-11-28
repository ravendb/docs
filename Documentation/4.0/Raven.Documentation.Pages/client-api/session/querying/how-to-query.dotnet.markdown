# Session : Querying : How to query?

The session exposes the following methods to query a database:

* `session.Query`
* `session.Advanced.DocumentQuery`
* `session.Advanced.RawQuery`

## Session.Query

The most straightforward way to issue a query is using `Query` method which allows to define queries using LINQ. In order to take advantage of querying capabilities specific for RavenDB
the querying API provides extension methods that will be described later.

### Syntax

{CODE query_1_0@ClientApi\Session\Querying\HowToQuery.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform a query on (mutually exclusive with collectionName) |
| **collectionName** | string | Name of a collection  (mutually exclusive with indexName) |
| **isMapReduce** | bool | Indicates if queried index is a map/reduce index (modifies how we treat identifier properties) |

| Return Value | | 
| ------------- | ----- |
| IRavenQueryable | Instance implementing `IRavenQueryable` interface containing additional query methods and extensions |


###Example I - Basic dynamic query

{CODE query_1_1@ClientApi\Session\Querying\HowToQuery.cs /}

The above is an example of a dynamic query which doesn't require to specify an index name. RaveDB will create an auto index automatically if that will be necessary.

The provided `Employee` type as the generic type parameter does not only define the type of returned
results but it also indicates that the queried collection will be `Employees`. There is no need to specify it as the parameter.

### Example II - Linq syntax support

Both LINQ syntaxes are supported:

- method syntax:

{CODE query_1_2@ClientApi\Session\Querying\HowToQuery.cs /}

- query syntax:

{CODE query_1_3@ClientApi\Session\Querying\HowToQuery.cs /}

### Example III - Using specific index

{CODE query_1_4@ClientApi\Session\Querying\HowToQuery.cs /}

or 

{CODE query_1_5@ClientApi\Session\Querying\HowToQuery.cs /}

## Session.Advanced.DocumentQuery

The advanced querying methods accessible by `session.Advanced.DocumentQuery` is the low-level API used to query RavenDB. The entire LINQ API is the wrapper of `DocumentQuery` API and
each query created using LINQ is built on top of it. Since it offers the full spectrum of querying capabilities you might find it handy when doing very complex queries, difficult
to shape using Linq.

### Example IV

{CODE query_1_6@ClientApi\Session\Querying\HowToQuery.cs /}

## Session.Advanced.RawQuery

Queries in RavenDB are using a SQL-like language called RavenDB Query Language (RQL). All of the above queries generates RQL sent to the server. The session also gives you the way to express the query directly in RQL.

### Example IV

{CODE query_1_7@ClientApi\Session\Querying\HowToQuery.cs /}

<!--
### Custom methods and extensions for LINQ

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
- [Include](../../../indexes/querying/handling-document-relationships)
- [Intersect](../../../client-api/session/querying/how-to-use-intersect)
- [Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
- [LazilyAsync](../../../client-api/session/querying/how-to-perform-queries-lazily)
- OrderByScore
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
-->

### Related articles

- [What are indexes?](../../../indexes/what-are-indexes)   
- [Indexes : Querying: Basics](../../../indexes/querying/basics)  
