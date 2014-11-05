# Query vs DocumentQuery

You might be wondering why does the RavenDB client offer two ways of querying by exposing `query` as well as `documentQuery` methods and what are
differences between them.

`documentQuery` is the lower level API that we use to query RavenDB. Therefore we have created `query`.

`query` is just a wrapper of `documentQuery`, so when you use `query` it always is translated to `documentQuery` object, which then builds a Lucene-syntax query that is sent to the server.
However we still expose `documentQuery` in advanced options to allow the users to have the full power of Lucene available to them. 

## Immutability

`documentQuery` is mutable while `query` is immutable. It means that you might get different results if you try to *reuse* a query. The usage of `query` method like in the following example:

{CODE:java immutable_query@Indexes\Querying\QueryAndLuceneQuery.java /}

will cause that the queries will be translated into following Lucene-syntax queries:

`query - Name:A*`

`ageQuery - (Name:A*) AND (Age_Range:{Ix21 TO NULL})`

`eyeQuery - (Name:A*) AND (EyeColor:blue)`

The similar usage of `documentQuery`:

{CODE:java mutable_lucene_query@Indexes\Querying\QueryAndLuceneQuery.java /}

`documentQuery - Name:A*` (before creating `ageQuery`)

`ageLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL}` (before creating `eyeDocumentQuery`)

`eyeLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL} EyeColor:blue`

In result all created Lucene queries are the same query (actually the same instance). This is important hint that you should be aware if you are going to reuse `documentQuery`.

## Default query operator

The example above shows an another difference between querying methods. Note that the usage of `where` statement resulted in `AND` operator 
in the final Lucene query when using `query` method. In case of `documentQuery` usage the Lucene query has no operator between query conditions what means
that `OR` will be used. This is the default operator of Lucene engine. You are able to change that by using `usingDefaultOperator`:

{CODE:java default_operator@Indexes\Querying\QueryAndLuceneQuery.java /}

## Related articles

- [Querying : Basics](../../indexes/querying/basics)
- [Client API : Session : How to query?](../../client-api/session/querying/how-to-query)
- [Client API : Session : How to use lucene in queries?](../../client-api/session/querying/lucene/how-to-use-lucene-in-queries)
