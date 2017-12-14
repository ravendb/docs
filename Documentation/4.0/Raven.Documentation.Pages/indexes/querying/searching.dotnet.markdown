# Searching

When you need to do a more complex text searching use `Search` extension method (in `Raven.Client.Documents` namespace). This method allows you to pass a few search terms that will be used in searching process for a particular field. Here is a sample code
that uses `Search` extension to get users with name *John* or *Adam*:

{CODE-TABS}
{CODE-TAB:csharp:Query search_3_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_3_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, 'John Adam')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Each of search terms (separated by space character) will be checked independently. The result documents must match exact one of the passed terms.

The same way you are also able to look for users that have some hobby:

{CODE-TABS}
{CODE-TAB:csharp:Query search_4_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_4_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, 'looking for someone who likes sport books computers')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

In result you will get users that are interested in *sport*, *books* or *computers*.

<hr />

## Multiple fields

By using `Search` extension you are also able to look for by multiple indexed fields. In order to do search by using `Name` and `Hobbies` properties you need to issue following query:

{CODE-TABS}
{CODE-TAB:csharp:Query search_5_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_5_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, 'Adam') or search(Hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

<hr />

## Boosting

Indexing in RavenDB is built upon Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. Let's see the example:

{CODE-TABS}
{CODE-TAB:csharp:Query search_6_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_6_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
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
{CODE-TABS/}

will be translated into

{CODE-BLOCK:csharp}
from Users
where search(Hobbies, 'computers') or search(Name, 'James') and Age = 20
{CODE-BLOCK/}

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follow:

{CODE-TABS}
{CODE-TAB:csharp:Query search_8_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_8_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

will result in the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Adam') and search(Hobbies, 'sport')
{CODE-BLOCK/}

If you want to negate the term use `SearchOptions.Not`:

{CODE-TABS}
{CODE-TAB:csharp:Query search_9_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_9_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

According to RQL syntax it will be transformed to the query:

{CODE-BLOCK:csharp}
from Users
where exists(Name) and not search(James, 'Adam')
{CODE-BLOCK/}

You can treat `SearchOptions` values as bit flags and create any combination of the defined enum values, e.g:

{CODE-TABS}
{CODE-TAB:csharp:Query search_10_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_10_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

It will produce the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Adam') and (exists(Hobbies) and not search(Hobbies, 'sport'))
{CODE-BLOCK/}

<hr />

## Using Wildcards

When the beginning or ending of a search term is unknown, wildcards can be used to add additional power to the searching feature. RavenDB supports both, postfix and suffix wildcards.

### Example I - using postfix wildcards

{CODE-TABS}
{CODE-TAB:csharp:Query search_11_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_11_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, 'Jo* Ad*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - using suffix and postfix wildcards

{CODE-TABS}
{CODE-TAB:csharp:Query search_12_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_12_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, '*oh* *da*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Warning}
RavenDB allows to search by using such queries but you have to be aware that **leading wildcards drastically slow down searches**.

Consider if you really need to find substrings, in most cases looking for whole words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer.
{WARNING/}

## Static Indexes

All of the previous examples were demonstrating searching capabilities by executing dynamic queries and were using auto indexes underneath. Same set of queries can be done when static indexes are used, but also those capabilities can be customized e.g. by changing the [analyzer](../using-analyzers) or setting up full text search on multiple fields.

### Example I - Basics

To be able to search you need to set `Indexing` to `Search` on a desired field.

{CODE:csharp search_20_2@Indexes\Querying\Searching.cs/}

{CODE-TABS}
{CODE-TAB:csharp:Query search_20_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_20_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Name, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - FullTextSearch

{CODE-TABS}
{CODE-TAB:csharp:Query search_21_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_21_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_21_2@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users
where search(Query, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related articles

- [Indexing : Boosting](../../indexes/boosting)
- [Client API : Session : How to use search?](../../client-api/session/querying/how-to-use-search)
