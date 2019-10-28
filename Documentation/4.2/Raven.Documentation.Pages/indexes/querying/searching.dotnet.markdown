# Querying: Searching

Use the `Search()` extension method to perform a full-text search on a particular field. `Search()` accepts a string containing the 
desired search terms separated by spaces. These search terms are matched with the terms in the index being queried.  

An index's terms are derived from the values of the documents' textual fields. These values were converted into one or more terms 
depending on which [Lucene analyzer](../../../indexes/using-analyzers) the index used.  

Here is a code sample that uses the `Search` extension to get users with the name *John* or *Steve*:  

{CODE-TABS}
{CODE-TAB:csharp:Query search_3_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_3_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'John Steve')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Each of the search terms (separated by space character) will be checked independently. The result documents must match one or more of the passed terms.

In the same way, you can also look for users that have some hobby:

{CODE-TABS}
{CODE-TAB:csharp:Query search_4_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_4_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'looking for someone who likes sport books computers')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The results will include users that are interested in *sport*, *books* or *computers*.

You can also pass the desired terms as an array or other `IEnumerable`:

{CODE-BLOCK:csharp}
IList<User> users = session
    .Query<User>()
    .Search(x => x.Name, new[] { "John", "Steve" })
    .ToList();
{CODE-BLOCK/}

## Multiple Fields

By using the `Search` extension, you are also able to look for multiple indexed fields. In order to search using both `Name` and `Hobbies` properties, you need to issue the following query:

{CODE-TABS}
{CODE-TAB:csharp:Query search_5_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_5_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'Steve') or search(Hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Boosting

Indexing in RavenDB is built upon the Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. 

For example:

{CODE-TABS}
{CODE-TAB:csharp:Query search_6_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_6_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This search will promote users who do sports before book readers and they will be placed at the top of the results list.

## Search Options

In order to specify the logic of a search expression, specify the `options` argument of the `Search` method. It is a `SearchOptions` enum with the following values:

* `Or`
* `And`
* `Not`
* `Guess` (default)

By default, RavenDB attempts to guess and match up the semantics between terms. If there are consecutive searches, they will be OR together, otherwise the AND semantic will be used.

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

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follows:

{CODE-TABS}
{CODE-TAB:csharp:Query search_8_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_8_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

will result in the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Steve') and search(Hobbies, 'sport')
{CODE-BLOCK/}

If you want to negate the term use `SearchOptions.Not`:

{CODE-TABS}
{CODE-TAB:csharp:Query search_9_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_9_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

According to RQL syntax it will be transformed into the query:

{CODE-BLOCK:csharp}
from Users
where exists(Name) and not search(Name, 'Steve')
{CODE-BLOCK/}

You can treat `SearchOptions` values as bit flags and create any combination of the defined enum values,

{CODE-TABS}
{CODE-TAB:csharp:Query search_10_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_10_1@Indexes\Querying\Searching.cs /}
{CODE-TABS/}

It will produce the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Steve') and (exists(Hobbies) and not search(Hobbies, 'sport'))
{CODE-BLOCK/}

## Search Operator

The argument `@operator` determines the operator between different terms of the same search, and can be set to:

* `SearchOperator.Or` (the default value)
* `SearchOperator.And`

{CODE-TABS}
{CODE-TAB:csharp:Query search_22_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_22_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'John Steve', or)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This query retrieves documents with a field `Name` that contains 'John' _or_ 'Steve' - or both.

{CODE-TABS}
{CODE-TAB:csharp:Query search_22_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_22_3@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'John Steve', and)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This query only retrieves documents with a field `Name` that contains both 'John' _and_ 'Steve'.

## Using Wildcards

When the beginning or ending of a search term is unknown, wildcards can be used to add additional power to the searching feature. RavenDB supports both suffix and postfix wildcards.

### Example I - Using Postfix Wildcards

{CODE-TABS}
{CODE-TAB:csharp:Query search_11_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_11_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'Jo* Ad*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Using Suffix and Postfix Wildcards

{CODE-TABS}
{CODE-TAB:csharp:Query search_12_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_12_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, '*oh* *da*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Warning}
RavenDB allows you to search by using such queries, but you have to be aware that **leading wildcards drastically slow down searches**.

Consider if you really need to find substrings. In most cases, looking for whole words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer.
{WARNING/}

## Static Indexes

All of the previous examples demonstrated searching capabilities by executing dynamic queries and were using auto indexes underneath. The same set of queries can be done when static indexes are used, and also those capabilities can be customized by changing the [analyzer](../using-analyzers) or setting up full text search on multiple fields.

### Example I - Basics

To be able to search you need to set `Indexing` to `Search` on a desired field.

{CODE:csharp search_20_2@Indexes\Querying\Searching.cs/}

{CODE-TABS}
{CODE-TAB:csharp:Query search_20_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_20_1@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByName'
where search(Name, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - FullTextSearch

{CODE-TABS}
{CODE-TAB:csharp:Query search_21_0@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_21_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Index search_21_2@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/Search'
where search(Query, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Indexes

- [Analyzers](../../../indexes/using-analyzers)
- [Boosting](../../indexes/boosting)

### Querying

- [Boosting](../../indexes/querying/boosting)

### Client API

- [How to Use Search](../../client-api/session/querying/how-to-use-search)
