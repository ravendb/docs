# Using Linq to query RavenDB indexes

As we have already seen in the previous chapter, querying is made on a collection and using the `Session` object. Unless the user specifies what index to query explicitly, RavenDB will find an appropriate index to query, and create it on the fly if one does not already exist. Regardless of the index being actually queried, querying is being done using Linq expressions. We will discuss static, named indexes later in this chapter.

The built-in Linq provider implements the `IQueryable` interface, and automatically translates any Linq query into a Lucene query understandable by the RavenDB server, handling all the low-level details for you. Therefore, querying RavenDB is easy to anyone who has used Linq before:

{CODE linquerying_0@Consumer\DynamicQueries.cs /}

Assuming we have entities of the following types stored in our database, let's see how we can use Linq to easily query for data in various scenarios:

{CODE company_classes@Common.cs /}

## Basic filtering

Say we wanted to get all entities of type `Company`, how would we go about that? Leaving performance aside for now, we could do this pretty easily:

{CODE linquerying_1@Consumer\DynamicQueries.cs /}

Since the whole idea of using Linq here is to apply some filtering in an efficient manner, we can use Linq's Where clause to do this for us:

{CODE linquerying_2@Consumer\DynamicQueries.cs /}

Or the Any clause if we want to return all entities that have a nested object within them satisfying a condition: