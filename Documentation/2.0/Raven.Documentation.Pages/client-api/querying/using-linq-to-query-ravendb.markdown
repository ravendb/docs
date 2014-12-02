# Using Linq to query RavenDB indexes

As we have already seen in the previous chapter, querying is made on a collection and using the `Session` object. Unless the user specifies what index to query explicitly, RavenDB will find an appropriate index to query, or create it on the fly if one does not already exist. We will see how to create named indexes, also called static, later in this chapter. Regardless of the index being actually queried, querying is usually done using Linq.

The built-in Linq provider implements the `IQueryable` interface in full. Its job is to automatically translates any Linq query into a Lucene query understandable by the RavenDB server, handling all the low-level details for you. Therefore, querying RavenDB is easy to anyone who has used Linq before; if you're not familiar with Linq, the following is a brief tutorial on how to use it to query RavenDB.

Assuming we have entities of the following types stored in our database:

{CODE company_classes@Common.cs /}

Let's see how we can use Linq to easily query for data in various scenarios. We will not cover all Linq's features, but will highlight several which are important to be familiar with.

## Some basics

Say we wanted to get all entities of type `Company` into a List, how would we go about that? Ignoring for now the performance hit of loading a large bulk of data, we could do this pretty easily (note that this query is implicitly using Take(128), because it didn't specify a page size explicitly):

{CODE linquerying_1@ClientApi\Querying\UsingLinqToQueryRavenDb.cs /}

Since the whole idea of using Linq here is to apply some filtering in an efficient manner, we can use Linq's `where` clause to do this for us:

{CODE linquerying_2@ClientApi\Querying\UsingLinqToQueryRavenDb.cs /}

Note, however, Linq is just a syntactic sugar. Under the hood, all queries are transformed into a sequence of method calls and lambda expressions. For example, the above code snippet is re-written by the compiler to look like the following before compiling:

{CODE linquerying_3@ClientApi\Querying\UsingLinqToQueryRavenDb.cs /}

All throughout the documentation we are going to use both flavors interchangeably, and we refer to both as Linq.

One more thing to note before we go any further - all queries shown above, except from the first one, perform no actual querying even though they are well defined. The reason for this is that the Linq provider, by nature, does not perform any querying unless it is forced to. The call to `ToArray()` in the first snippet did just that, and by that triggered the execution of the query.

## More filtering options

Other than the `Where` clause, there are several other useful operators you could use to filter results.

`Any` can be used on collections of objects (or primitive lists) in your entities to return only those who satisfies a condition. RavenDB also supports an `In` operator, to make reverse `Any` comparisons easier:

{CODE linquerying_4@ClientApi\Querying\UsingLinqToQueryRavenDb.cs /}

## Projections

Projections are specific fields projected from documents using the Linq `Select` method, they are not the original object but a new object that is being created on the fly and populated with results gathered by the query.

RavenDB Linq queries support projections, but its important to know that projected entities are not being tracked for changes. That is true even if the projected type is a named type (and not just an anonymous type).

Here is how to use projections:

{CODE linquerying_5@ClientApi\Querying\UsingLinqToQueryRavenDb.cs /}

Projections are useful when only part of the data is needed for your operation. Whenever change tracking isn't required, you're advised to consider using projections to ease bandwidth traffic between you and the server. This isn't a general rule, because caching in the entire application also plays an important role here, and it might make it more efficient to load the cache results of a query than to issue a remote query for a projection.
You can also use the `Distinct` method to only return distinct results from the server. When using projections, that means that on the server side, the database will compare all the projected fields for each object, and send us only unique results. If you aren't using projections, this has no effect but causing the server to do more work.

## Sorting

You can use the `orderby` / `.OrderBy()` / `.OrderByDescending()` clauses to perform sorting. As we will see in a later chapter, more advanced sorting options are supported using static indexes.

## Aggregate operators

RavenDB only supports the `Count` and `Distinct` Linq aggregate operators. For more complex aggregations, you'll need to make use of Map/Reduce indexes.

Similarly, the `SelectMany`, `GroupBy` and `Join` operators are not supported. Such operations should be made through a Map/Reduce index, and not while querying.

The `let` keyword is not supported currently.