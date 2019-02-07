#Filtering

To retrieve the files you are interested in, you need to define a set of conditions that they should match. To build a query with filtering criteria, you can use a bunch of available methods.

##Where

The first method to create a conditional query is `Where` which takes a string parameter. You need to manually define the whole query according to the [Lucene syntax](http://lucene.apache.org/core/3_0_3/queryparsersyntax.html), the same as for the [`SearchAsync` command](../../commands/files/search/search). This approach enjoys the full support of the Lucene search functionality. You can query the [built-in RavenFS fields](../../../indexing) as well as the custom files' metadata.

{CODE filtering_1@FileSystem\ClientApi\Session\Querying\Filtering.cs /}


##WhereEquals

It allows you to find the exact match on a file property or metadata. The following query will return all files that have the `Copyright` metadata with `HR` value.

{CODE filtering_5@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereIn

Can be used to check a single value against the multiple values. For example, to get all the files that name is either `readme.txt` or `help.doc`, you can issue the following query:

{CODE filtering_8@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereStartsWith

In order to find the files that have metadata value starting with the specified prefix, use the `WhereStartsWith` method. The below code will return all the files located in the `/movies/ravenfs` directory and its subdirectories.

{CODE filtering_11@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereEndsWith

Analogously, you can build a query that will look for the files with a given suffix value. For example, this can be used to find files of the same type by using file extensions.

{CODE filtering_4@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereLessThan and WhereLessThanOrEqual

While querying numeric metadata values, methods such as `WhereLess` or `WhereBetween` come in handy. You can use them to determine a file size as well as to query the numeric custom metadata fields.

{CODE filtering_9@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_10@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereGreaterThan and WhereGreaterThanOrEqual

Note that you can even apply such query on a string field - then values will be compared lexicographically.

{CODE filtering_6@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_7@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##WhereBetween and WhereBetweenOrEqual

Use the `WhereBetween` method to find files that fulfill a range criteria.

{CODE filtering_2@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

{CODE filtering_3@FileSystem\ClientApi\Session\Querying\Filtering.cs /}


##ContainsAll and ContainsAny

Note that under a single metadata key, there can be multiple values stored as an array. For example, you can store multiple attributes in the metadata:

{CODE filtering_12@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

In order to allow queries execution against array values, you can use `ContainsAll` to determine that all provided values have to be in the array:

{CODE filtering_13@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

The `ContainsAny` method checks just for an occurrence of any value from the specified collection:

{CODE filtering_14@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

##AndAlso and OrElse

So far we have considered queries with a single condition. However, you may want to build a more complex query by providing multiple predicates.
Let's take a look at the example:

{CODE filtering_15@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

It will send the following query to the server:

`__fileName:readme* Copyright:HR`

Note that between these two conditions there is no logical operator. In that case, the *OR* operator will be used, as it is the default Lucene conjunction operator.

However, if your intention is to retrieve files that match both conditions, you should explicitly indicate that by using the `AndAlso` operator.

{CODE filtering_16@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

Then the query sent to the server will look as follows:

`__fileName:readme* AND Copyright:HR`

Sometimes you may need to explicitly indicate that you want to join criteria by using *OR* operator. In that case, join your predicates using the `OrElse` method:

{CODE filtering_17@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

The actual query will be:

`@in<__fileName>:(help.txt,documentation.doc)  OR __fileName:readme*`

##OnDirectory

This method is used to indicate the directory where the search should be performed. Additionally, you can determine whether the search operation should be run only against this directory or also its subfolders.

{CODE filtering_18@FileSystem\ClientApi\Session\Querying\Filtering.cs /}

## Related articles

- [Indexing](../../../indexing)