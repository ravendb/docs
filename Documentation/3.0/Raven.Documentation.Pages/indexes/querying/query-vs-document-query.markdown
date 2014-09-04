# Query vs DocumentQuery

You might be wondering why does the RavenDB client offer two ways of querying by exposing `Query` as well as `DocumentQuery` methods and what are
differences between them.

`DocumentQuery` is the lower level API that we use to query RavenDB but it does not support LINQ - the mandatory data access solution in .NET. Therefore we have created `Query` that that is the LINQ endpoint for RavenDB. 

The entire LINQ API is a wrapper of `DocumentQuery` and is built on top on that. 
So when you use `Query` it always is translated to `DocumentQuery` object, which then builds a Lucene-syntax query that is sent to the server.
However we still expose `DocumentQuery` in advanced options to allow the users to have the full power of Lucene available to them. 

## Immutability

`DocumentQuery` is mutable while `Query` is immutable. It means that you might get different results if you try to *reuse* a query. The usage of `Query` method like in the following example:

{CODE immutable_query@Indexes\Querying\QueryAndLuceneQuery.cs /}

will cause that the queries will be translated into following Lucene-syntax queries:

`query - Name:A*`

`ageQuery - (Name:A*) AND (Age_Range:{Ix21 TO NULL})`

`eyeQuery - (Name:A*) AND (EyeColor:blue)`

The similar usage of `DocumentQuery`:

{CODE mutable_lucene_query@Indexes\Querying\QueryAndLuceneQuery.cs /}

`documentQuery - Name:A*` (before creating `ageQuery`)

`ageLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL}` (before creating `eyeDocumentQuery`)

`eyeLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL} EyeColor:blue`

In result all created Lucene queries are the same query (actually the same instance). This is important hint that you should be aware if you are going to reuse `DocumentQuery`.

## Default query operator

The example above shows an another difference between querying methods. Note that the usage of `Where` statement resulted in `AND` operator 
in the final Lucene query when using `Query` method. In case of `DocumentQuery` usage the Lucene query has no operator between query conditions what means
that `OR` will be used. This is the default operator of Lucene engine. You are able to change that by using `UsingDefaultOperator`:

{CODE default_operator@Indexes\Querying\QueryAndLuceneQuery.cs /}

## Related articles

TODO