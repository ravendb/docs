#Search

One of the most common functionality that many real world applications provide is a search feature. Many times it will be enough to apply `Where` closure to create a simple condition,
for example to get all users whose age is greater that 20 use the code:

{CODE linq_extensions_search_where_age@ClientApi\Querying\LinqExtensions\Search.cs /}

where `User` class and `UsersByName` index are defined as follow:
{CODE linq_extensions_search_user_class@ClientApi\Querying\LinqExtensions\Search.cs /}
{CODE linq_extensions_search_index_users_by_name@ClientApi\Querying\LinqExtensions\Search.cs /}

The `Where` statement also is good if you want to perform a really simple text field search, for example let's create a query to retrieve users whose name starts with *Jo*:

{CODE linq_extensions_search_where_name@ClientApi\Querying\LinqExtensions\Search.cs /}

Eventually all queries are always transformed into a Lucene query. The query like above will be translated into <em>Name:Jo*</em>.

{WARNING An attempt to use `string.Contains()` method as condition of `Where` closure, will throw `NotSupportedException`. That is because the search term like \*<em>term</em>* (note wildcards at the beginning and at the end) can cause performance issues. Due to Raven's *safe-by-default* paradigm such a operation is forbidden. If you really want to achieve that more details you will find in this section. /}

{INFO Note that that results of a query might be different depending on [an analyzer](../static-indexes/configuring-index-options) that was applied./}

##Multiple terms

When you need to do a more complex text searching use `Search` extension method. This method allows you to pass a few search terms that will be used in searching process for a particular field. Here is a sample code
that uses `Search` extension to get users with name *John* or *Adam*:
{CODE linq_extensions_search_name@ClientApi\Querying\LinqExtensions\Search.cs /}

Each of search terms (separated by space character) will be checked indepentently. The result documents must match exact one of the passed terms.

The same way you are also able to look for users with any hobby. Create the index:

{CODE linq_extensions_search_index_users_by_hobbies@ClientApi\Querying\LinqExtensions\Search.cs /}

Now you are able to execute the following search:

{CODE linq_extensions_search_hobbies@ClientApi\Querying\LinqExtensions\Search.cs /}

In result you will get users who are interested in *sport*, *books* or *computers*.

##Multiple fields

By using `Search` extension you are also able to perform the search for multiple indexed fields. First let's introduce the index:
 
{CODE linq_extensions_search_index_users_by_name_and_hobbies@ClientApi\Querying\LinqExtensions\Search.cs /}

Now query we are able to do the search by using `Name` and `Hobbies` properties:

{CODE linq_extensions_search_users_by_name_and_hobbies@ClientApi\Querying\LinqExtensions\Search.cs /}

##Boost

You can specify the optional boosting factor parameter that will be relayed into Lucene indexing engine.

//TODO

##Search options

In order to specify the logic of search expression specify the options argument in the `Search` method. This is `SearchOptions` enum with the following values:

* Or,
* And,
* Not,
* Guess (default).

The applied option will influence a next query statement.

By default the query is created by "guessing" //TODO

The query as follow:

{CODE linq_extensions_search_users_by_name_and_hobbies_search_and@ClientApi\Querying\LinqExtensions\Search.cs /}

will result in the following Lucene query: <em>Name:(Adam) AND Hobbies:(sport)</em>

##Query escaping

The code examples presented in this section have hard coded search values. However in a real use case a user types a search term. You are able to control the escaping strategy of the provided query by specifying 
the last argument of the search method which is `EscapeQueryOptions` enum., an enum that can have one of the following values:

* EscapeAll (default),
* AllowPostfixWildcard,
* AllowAllWildcards,
* RawQuery.

By default all special characters contained in the query are escaped (`EscapeAll`). However you can add a bit more of flexibility to your searching mechanism.
`EscapeQueryOptions.AllowPostfixWildcard` enable to search agains a field by using search term that ends with wildcard character:

{CODE linq_extensions_search_where_name_post_wildcard@ClientApi\Querying\LinqExtensions\Search.cs /}

The next option `EscapeQueryOptions.AllowAllWildcards` extends the previous option by allowing the wildcard character to be present at the beginning as well as at the end of the search term.

{CODE linq_extensions_search_where_name_all_wildcard@ClientApi\Querying\LinqExtensions\Search.cs /}

{WARNING RavenDB allows to search by using such queries but you have to be aware that leading wildcards drastically slow down searches. Consider if you really need to find substrings, most cases looking for words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer./}

//TODO