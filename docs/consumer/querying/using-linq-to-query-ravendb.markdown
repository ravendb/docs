# Using Linq to query RavenDB indexes

As we have already seen in the previous chapter, querying is made on a collection and using the `Session` object. Unless the user specifies what index to query explicitly, RavenDB will find an appropriate index to query, and create it on the fly if one does not already exist. We will see how to create named indexes, also called static, later in this chapter. Regardless of the index being actually queried, querying is being done using Linq expressions.

The built-in Linq provider implements the `IQueryable` interface, and automatically translates any Linq query into a Lucene query understandable by the RavenDB server, handling all the low-level details for you. Therefore, querying RavenDB is easy to anyone who has used Linq before; if you're not familiar with Linq, following is a brief tutorial on how to use it to query RavenDB.

Assuming we have entities of the following types stored in our database:

{CODE company_classes@Common.cs /}

Let's see how we can use Linq to easily query for data in various scenarios:

## Some basics

Say we wanted to get all entities of type `Company` into a List, how would we go about that? Ignoring for now the performance hit of loading a large bulk of data, we could do this pretty easily:

{CODE linquerying_1@Consumer\DynamicQueries.cs /}

Since the whole idea of using Linq here is to apply some filtering in an efficient manner, we can use Linq's `where` clause to do this for us:

{CODE linquerying_2@Consumer\DynamicQueries.cs /}

Note, however, Linq is just a syntactic sugar. Under the hood, all queries are transformed into a sequence of method calls and lambda expressions. For example, the above code snippet is re-written by the compiler to look like the following before compiling:

{CODE linquerying_3@Consumer\DynamicQueries.cs /}

All throughout the documentation we are going to use both flavors interchangeably, and we refer to both as Linq.

One more thing to note before we go any further - all queries shown above, except from the first one, perform no actual querying even though they are well defined. The reason for this is that the Linq provider, by nature, does not perform any querying unless ordered to return an enumerable collection or called `Execute` on. The call to `ToArray()` in the first snippet did just that, and by that triggered the execution of the query.

## More filtering options

Other than the `Where` clause, there are several other useful operators you could use to filter  results.

`Any` and `All` can be used on collections of objects (or primitive lists) in your entities to return only those who satisfies a condition. RavenDB also supports an `In` operator, to make reverse `Any` comparisons easier:

{CODE linquerying_4@Consumer\DynamicQueries.cs /}

To every query you can add a `.Distinct()` call, which will make sure you don't get duplicate entities. This is especially useful whenever using projections, which is what we'll discuss next.

## Projections

Projections are objects that are selected using Linq, which are not the original object but a new object that is being created on the fly and populated with select data gathered by the query.

RavenDB Linq queries support projections, but its important to know that projected entities are not being tracked for changes. That is true also when projecting into an entity (as opposed to an anonymous object).

Here is how to use projections:

{CODE linquerying_5@Consumer\DynamicQueries.cs /}

Projections are useful when only part of the data is needed for your operation. Whenever change tracking isn't required, you're advised to use projections to ease bandwidth traffic between you and the server.