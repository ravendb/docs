### Query and LuceneQuery

You might be wondering why does the RavenDB client offer two ways of querying by exposing `Query` as well as `LuceneQuery` methods and what are
differences between them. `LuceneQuery` is the lower level API that we use to query RavenDB but it does not support LINQ - the mandatory data access
solution in .NET. Therefore we have created `Query` that that is the LINQ endpoint for RavenDB. 

The entire LINQ API is a wrapper of `LuceneQuery` and is built on top on that. 
So when you use `Query` it always is translated to `LuceneQuery` object, which then builds a Lucene-syntax query that is sent to the server.
However we still expose `LuceneQuery` in advanced options to allow the users to have the full power of Lucene available to them. 

#### LuceneQuery usage

While in the most cases the usage of `Query` is enough, easier to create and recommended to use you might want to utilize `LuceneQuery` directly.
`LuceneQuery` is mostly designated to be used for dynamic queries and when you want a low level access.

For example dynamic querying as is shown below:

{CODE dynamic_query_1@ClientApi\Querying\QueryAndLuceneQuery.cs /}

will cause that the following [dynamic index](../../http-api/indexes/dynamic-indexes) will be created on a server:

	Map:	from doc in docs.Companies
			from docEmployeesItem in ((IEnumerable<dynamic>)doc.Employees).DefaultIfEmpty()
			select new { Employees_Name = docEmployeesItem.Name }

You can go even futher and create the dynamic query where its result is also `dynamic`:

{CODE dynamic_query_2@ClientApi\Querying\QueryAndLuceneQuery.cs /}

This will create the following map/reduce dynamic index on a server:

	Map:	from doc in docs
			from docTagsItem in ((IEnumerable<dynamic>)doc.Tags).DefaultIfEmpty()
			select new { TagsCount = docTagsItem.Count, Count = 1 }

	Reduce:	from result in results
				group result by result.TagsCount
				into g
				select new
				{
					TagsCount = g.Key,
					Count = g.Sum(x=>x.Count)
				}
#### Immutability

`LuceneQuery` is mutable while `Query` is immutable. It means that you might get different
results if you try to *reuse* a query. The usage of `Query` method like in the following example:

{CODE immutable_query@ClientApi\Querying\QueryAndLuceneQuery.cs /}

will cause that the queries will be translared into following Lucene-syntax queries:

`query - Name:A*`

`ageQuery - (Name:A*) AND (Age_Range:{Ix21 TO NULL})`

`eyeQuery - (Name:A*) AND (EyeColor:blue)`

The similar usage of `LuceneQuery`:

{CODE mutable_lucene_query@ClientApi\Querying\QueryAndLuceneQuery.cs /}

`luceneQuery - Name:A*` (before creating `ageQuery`)

`ageLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL}` (before creating `eyeLuceneQuery`)

`eyeLuceneQuery - Name:A* Age_Range:{Ix21 TO NULL} EyeColor:blue`

In result all created Lucene queries are the same query (actually the same instance). This is important hint that you should be aware if you are going to reuse `LuceneQuery`.

#### Default query operator

The example above shows an another difference between querying methods. Note that the usage of `Where` statement resulted in `AND` operator 
in the final Lucene query when using `Query` method. In case of `LuceneQuery` usage the Lucene query has no operator between query conditions what means
that `OR` will be used. This is the default operator of Lucene engine. You are able to change that by using `UsingDefaultOperator`:

{CODE default_operator@ClientApi\Querying\QueryAndLuceneQuery.cs /}
