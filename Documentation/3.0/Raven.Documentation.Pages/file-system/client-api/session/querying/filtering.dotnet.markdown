#Filtering

In order to retrieve files that you are interested in you need to define a set of conditions that they should match. In order build a query
with filtering criteria you can use a bunch of available methods.

##Where

The first method to create a conditional query is `Where` which takes a string parameter. You need to manually define the whole query according
to [Lucene syntax](http://lucene.apache.org/core/old_versioned_docs/versions/3_0_0/queryparsersyntax.html), the same like
for [`SearchAsync` command](../../commands/files/search/search). The advantage of this approach is that you can use full support of
Lucene search functionality. You can query [built-in RavenFS fields](../../../indexing) as well as file metadata.

{CODE filtering_1@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereBetween and WhereBetweenOrEqual

{CODE filtering_2@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_3@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereEndsWith

{CODE filtering_3@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereEquals

{CODE filtering_5@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereGreaterThan and WhereGreaterThanOrEqual

{CODE filtering_6@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_7@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereIn

{CODE filtering_8@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereLessThan and WhereLessThanOrEqual

{CODE filtering_9@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_10@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereStartsWith

{CODE filtering_11@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

