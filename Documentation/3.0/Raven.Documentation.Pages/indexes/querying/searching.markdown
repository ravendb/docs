# Searching

One of the most common functionalities that many real world applications provide is a search feature. Many times it will be enough to apply `Where` closure to create a simple condition, for example to get all users whose name equals `John Doe` use the code:

{CODE-TABS}
{CODE-TAB:csharp:Query search_1_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_1_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_1_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

where `User` class is defined as follows:

{CODE linq_extensions_search_user_class@Indexes\Querying\Searching.cs /}

The `Where` statement also is good if you want to perform a really simple text field search, for example let's create a query to retrieve users whose name starts with *Jo*:

{CODE-TABS}
{CODE-TAB:csharp:Query search_2_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_2_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_2_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

Eventually all queries are always transformed into a Lucene query. The query like above will be translated into <em>Name:Jo*</em>.

{SAFE An attempt to use `string.Contains()` method as condition of `Where` closure, will throw `NotSupportedException`. That is because the search term like \*<em>term</em>* (note wildcards at the beginning and at the end) can cause performance issues. Due to Raven's *safe-by-default* paradigm such operation is forbidden. If you really want to achieve this case, you will find more details in one of the next section below. /}

{INFO Note that that results of a query might be different depending on [an analyzer](../static-indexes/configuring-index-options) that was applied./}

<hr />

## Multiple terms

When you need to do a more complex text searching use `Search` extension method (in `Raven.Client` namespace). This method allows you to pass a few search terms that will be used in searching process for a particular field. Here is a sample code
that uses `Search` extension to get users with name *John* or *Adam*:

{CODE-TABS}
{CODE-TAB:csharp:Query search_3_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_3_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_3_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

Each of search terms (separated by space character) will be checked independently. The result documents must match exact one of the passed terms.

The same way you are also able to look for users that have some hobby:

{CODE-TABS}
{CODE-TAB:csharp:Query search_4_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_4_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_4_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_4_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

In result you will get users that are interested in *sport*, *books* or *computers*.

<hr />

## Multiple fields

By using `Search` extension you are also able to look for by multiple indexed fields. First let's introduce the index:
 
{CODE search_5_3@Indexes\Querying\Searching.cs /}

Now we are able to search by using `Name` and `Hobbies` properties:

{CODE-TABS}
{CODE-TAB:csharp:Query search_5_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_5_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_5_2@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

<hr />

## Boosting

Indexing in RavenDB is built upon Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. Let's see the example:

{CODE-TABS}
{CODE-TAB:csharp:Query search_6_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_6_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_6_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_4_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

The search above will promote users who do sports before book readers and they will be placed at the top of the result list.

<hr />

## Search options

In order to specify the logic of search expression specify the options argument of the `Search` method. It is `SearchOptions` enum with the following values:

* Or,
* And,
* Not,
* Guess (default).

By default RavenDB attempts to guess and match up the semantics between terms. If there are consecutive searches, they will be OR together, otherwise AND semantic will be used by default.

The following query:

{CODE-TABS}
{CODE-TAB:csharp:Query search_7_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_7_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_7_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_7_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

will be translated into `(Hobbies:(computers) Name:(James)) AND (Age:20)` (if there is no boolean operator then OR is used).

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follow:

{CODE-TABS}
{CODE-TAB:csharp:Query search_8_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_8_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_8_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_5_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

will result in the following Lucene query: `Name:(Adam) AND Hobbies:(sport)`

If you want to negate the term use `SearchOptions.Not`:

{CODE-TABS}
{CODE-TAB:csharp:Query search_9_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_9_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_9_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

According to Lucene syntax it will be transformed to the query: `-Name:(James)`.

You can treat `SearchOptions` values as bit flags and create any combination of the defined enum values, e.g:

{CODE-TABS}
{CODE-TAB:csharp:Query search_10_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_10_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_10_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_5_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

It will produce the following Lucene query: `Name:(Adam) AND -Hobbies:(sport)`.

<hr />

## Query escaping

The code examples presented in this section have hard coded searching terms. However in a real use case the user will specify the term. You are able to control the escaping strategy of the provided query by specifying 
the `EscapeQueryOptions` parameter. It's the enum that can have one of the following values:

* EscapeAll (default),
* AllowPostfixWildcard,
* AllowAllWildcards,
* RawQuery.

By default all special characters contained in the query will be escaped (`EscapeAll`) when [Query]() from session is used. However you can add a bit more of flexibility to your searching mechanism.
`EscapeQueryOptions.AllowPostfixWildcard` enables searching against a field by using search term that ends with wildcard character:

{CODE-TABS}
{CODE-TAB:csharp:Query search_11_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_11_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_11_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

The next option `EscapeQueryOptions.AllowAllWildcards` extends the previous one by allowing the wildcard character to be present at the beginning as well as at the end of the search term.

{CODE-TABS}
{CODE-TAB:csharp:Query search_12_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_12_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_12_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

{WARNING:Warning}
RavenDB allows to search by using such queries but you have to be aware that **leading wildcards drastically slow down searches**.

Consider if you really need to find substrings, most cases looking for words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer.
{WARNING/}

The last option makes that the query will not be escaped and the raw term will be relayed to Lucene:

{CODE-TABS}
{CODE-TAB:csharp:Query search_13_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_13_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Commands search_13_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_1_3@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

{INFO:EscapeQueryOptions}
Default `EscapeQueryOptions` value for **Query** is `EscapeQueryOptions.EscapeAll`.

Default `EscapeQueryOptions` value for **DocumentQuery** is `EscapeQueryOptions.RawQuery`.
{INFO/}

## Related articles

TODO