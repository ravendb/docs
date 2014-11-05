# Searching

One of the most common functionalities that many real world applications provide is a search feature. Many times it will be enough to apply `where` closure to create a simple condition, for example to get all users whose name equals `John Doe` use the code:

{CODE-TABS}
{CODE-TAB:java:Query search_1_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_1_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_1_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

where `User` class is defined as follows:

{CODE:java linq_extensions_search_user_class@Indexes\Querying\Searching.java /}

The `where` statement also is good if you want to perform a really simple text field search, for example let's create a query to retrieve users whose name starts with *Jo*:

{CODE-TABS}
{CODE-TAB:java:Query search_2_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_2_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_2_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

Eventually all queries are always transformed into a Lucene query. The query like above will be translated into <em>Name:Jo*</em>.

{SAFE An attempt to use `contains()` method as condition of `where` closure, will throw `IllegalStateException`. That is because the search term like \*<em>term</em>* (note wildcards at the beginning and at the end) can cause performance issues. Due to Raven's *safe-by-default* paradigm such operation is forbidden. If you really want to achieve this case, you will find more details in one of the next section below. /}

{INFO Note that that results of a query might be different depending on [an analyzer](../../indexes/using-analyzers) that was applied./}

<hr />

## Multiple terms

When you need to do a more complex text searching use `search` method. This method allows you to pass a few search terms that will be used in searching process for a particular field. Here is a sample code
that uses `search` to get users with name *John* or *Adam*:

{CODE-TABS}
{CODE-TAB:java:Query search_3_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_3_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_3_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

Each of search terms (separated by space character) will be checked independently. The result documents must match exact one of the passed terms.

The same way you are also able to look for users that have some hobby:

{CODE-TABS}
{CODE-TAB:java:Query search_4_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_4_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_4_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_4_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

In result you will get users that are interested in *sport*, *books* or *computers*.

<hr />

## Multiple fields

By using `search` you are also able to look for by multiple indexed fields. First let's introduce the index:
 
{CODE:java search_5_3@Indexes\Querying\Searching.java /}

Now we are able to search by using `Name` and `Hobbies` fields:

{CODE-TABS}
{CODE-TAB:java:Query search_5_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_5_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_5_2@Indexes\Querying\Searching.java /}
{CODE-TABS/}

<hr />

## Boosting

Indexing in RavenDB is built upon Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. Let's see the example:

{CODE-TABS}
{CODE-TAB:java:Query search_6_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_6_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_6_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_4_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

The search above will promote users who do sports before book readers and they will be placed at the top of the result list.

<hr />

## Search options

In order to specify the logic of search expression specify the options argument of the `search` method. It is `SearchOptions` enum with the following values:

* OR,
* AND,
* NOT,
* GUESS (default).

By default RavenDB attempts to guess and match up the semantics between terms. If there are consecutive searches, they will be OR together, otherwise AND semantic will be used by default.

The following query:

{CODE-TABS}
{CODE-TAB:java:Query search_7_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_7_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_7_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_7_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

will be translated into `(Hobbies:(computers) Name:(James)) AND (Age:20)` (if there is no boolean operator then OR is used).

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follow:

{CODE-TABS}
{CODE-TAB:java:Query search_8_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_8_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_8_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_5_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

will result in the following Lucene query: `Name:(Adam) AND Hobbies:(sport)`

If you want to negate the term use `SearchOptions.NOT`:

{CODE-TABS}
{CODE-TAB:java:Query search_9_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_9_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_9_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

According to Lucene syntax it will be transformed to the query: `-Name:(James)`.

You can create combination of `SearchOptions` values, e.g.: 

{CODE-TABS}
{CODE-TAB:java:Query search_10_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_10_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_10_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_5_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

It will produce the following Lucene query: `Name:(Adam) AND -Hobbies:(sport)`.

<hr />

## Query escaping

The code examples presented in this section have hard coded searching terms. However in a real use case the user will specify the term. You are able to control the escaping strategy of the provided query by specifying 
the `EscapeQueryOptions` parameter. It's the enum that can have one of the following values:

* ESCAPE_ALL (default),
* ALLOW_POSTFIX_WILDCARD,
* ALLOW_ALL_WILDCARDS,
* RAW_QUERY.

By default all special characters contained in the query will be escaped (`ESCAPE_ALL`) when [Query](../../client-api/session/querying/how-to-query) from session is used. However you can add a bit more of flexibility to your searching mechanism.
`EscapeQueryOptions.ALLOW_POSTFIX_WILDCARD` enables searching against a field by using search term that ends with wildcard character:

{CODE-TABS}
{CODE-TAB:java:Query search_11_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_11_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_11_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

The next option `EscapeQueryOptions.ALLOW_ALL_WILDCARDS` extends the previous one by allowing the wildcard character to be present at the beginning as well as at the end of the search term.

{CODE-TABS}
{CODE-TAB:java:Query search_12_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_12_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_12_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

{WARNING:Warning}
RavenDB allows to search by using such queries but you have to be aware that **leading wildcards drastically slow down searches**.

Consider if you really need to find substrings, most cases looking for words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer.
{WARNING/}

The last option makes that the query will not be escaped and the raw term will be relayed to Lucene:

{CODE-TABS}
{CODE-TAB:java:Query search_13_0@Indexes\Querying\Searching.java /}
{CODE-TAB:java:DocumentQuery search_13_1@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Commands search_13_2@Indexes\Querying\Searching.java /}
{CODE-TAB:java:Index search_1_3@Indexes\Querying\Searching.java /}
{CODE-TABS/}

{INFO:EscapeQueryOptions}
Default `EscapeQueryOptions` value for **Query** is `EscapeQueryOptions.ESCAPE_ALL`.

Default `EscapeQueryOptions` value for **DocumentQuery** is `EscapeQueryOptions.RAW_QUERY`.
{INFO/}

## Related articles

- [Indexing : Boosting](../../indexes/boosting)
- [Client API : Session : How to use search?](../../client-api/session/querying/how-to-use-search)
