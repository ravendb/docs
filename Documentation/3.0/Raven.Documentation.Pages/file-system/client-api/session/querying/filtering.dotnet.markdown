#Filtering

In order to retrieve files that you are interested in you need to define a set of conditions that they should match. In order build a query
with filtering criteria you can use a bunch of available methods.

##Where

The first method to create a conditional query is `Where` which takes a string parameter. You need to manually define the whole query according
to [Lucene syntax](http://lucene.apache.org/core/old_versioned_docs/versions/3_0_0/queryparsersyntax.html), the same like
for [`SearchAsync` command](../../commands/files/search/search). The advantage of this approach is that you can use full support of
Lucene search functionality. You can query [built-in RavenFS fields](../../../indexing) as well as custom file metadata.

{CODE filtering_1@FileSystem\ClientApi\Session\Querying\Filtering.cs /}


##WhereEquals

It builds allows to find the exact match on a file property or metadata. The following query will return all files that have `Copyright` metadata and its value is `HR`.

{CODE filtering_5@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereIn

Can be used to check single value against multiple values. For example to get all files that name is either `readme.txt` or `help.doc` you can issue following query:

{CODE filtering_8@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereStartsWith

In order to find files that have metadata value that starts with a specified prefix use the `WhereStartsWith` method. The below code will
return all files located in `/movies/ravenfs` directory and its subdirectories.

{CODE filtering_11@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereEndsWith

Analogously you can build a query that will look for files with a given suffix value. E.g. this can be used to find files of the same type by using file extensions.

{CODE filtering_4@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereLessThan and WhereLessThanOrEqual

In order to query numeric metadata values when methods like `WhereLess` or `WhereBetween` come in handy. You can use them to determine a file size as well as use it to query
numeric custom metadata fields.

{CODE filtering_9@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_10@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereGreaterThan and WhereGreaterThanOrEqual

Note that you can even apply such query on a string field - then values will be compared lexicographically.

{CODE filtering_6@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_7@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereBetween and WhereBetweenOrEqual

Use `WhereBetween` methods to find files that fulfill a range criteria.

{CODE filtering_2@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_3@FileSystem\ClientApi\Session\Querying\Filtering.cs /}


##ContainsAll and ContainsAny

Note that under a single metadata key there can be multiple values stored as an array. For example you can store multiple attributes in metadata:

{CODE filtering_12@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

In order to allow to execute queries against array values you can use `ContainsAll` to determine that all provided values have to be in the array:

{CODE filtering_13@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

`ContainsAny` method checks just for an occurrence of any value from the specified collection:

{CODE filtering_14@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##AndAlso and OrElse

So far we have considered queries that had just a single condition. However you may want to build more complex queries by providing multiple predicates.
Let's take a look at the example:

{CODE filtering_15@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

The following query will be sent to the server then:

`__fileName:readme* Copyright:HR`

Note that between these two conditions there is no logical operator. Then *OR* operator will be used because it is the default Lucene conjunction operator.

However if your indention were to retrieve files that match both conditions then you would need to explicitly indicate that by using `AndAlso` operator.

{CODE filtering_16@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

Then the query sent to the server would look as follow:

`__fileName:readme* AND Copyright:HR`

Sometimes you might need to explicitly indicate that you want to join criteria by using *OR* statement. Then join your predicates by using `OrElse` method:

{CODE filtering_17@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

The actual query will be:

`@in<__fileName>:(help.txt,documentation.doc)  OR __fileName:readme*`

##OnDirectory

This method is used to indicate a directory where the search should be performed. Additionally you can determine whether the search operation
should be run against just this directory or also its subfolders.

{CODE filtering_18@FileSystem\ClientApi\Session\Querying\Filtering.cs /}