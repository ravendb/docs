# Querying: Searching

When you need to do a more complex text searching, use the `search` method. This method allows you to pass a few search terms that will be used in the searching process for a particular field. Here is a sample code
that uses the `search` method to get users with the name *John* or *Adam*:

{CODE-TABS}
{CODE-TAB:java:Query search_3_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'John Adam')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Each of the search terms (separated by space character) will be checked independently. The result documents must match exactly one of the passed terms.

In the same way, you can also look for users that have some hobby:

{CODE-TABS}
{CODE-TAB:java:Query search_4_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'looking for someone who likes sport books computers')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The results will return users that are interested in *sport*, *books* or *computers*.

## Multiple Fields

By using the `search` method, you are also able to look for multiple indexed fields. In order to search using both `name` and `hobbies` properties, you need to issue the following query:

{CODE-TABS}
{CODE-TAB:java:Query search_5_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'Adam') or search(Hobbies, 'sport')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Boosting

Indexing in RavenDB is built upon the Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. 

For example:

{CODE-TABS}
{CODE-TAB:java:Query search_6_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where boost(search(Hobbies, 'I love sport'), 10) or boost(search(Hobbies, 'but also like reading books'), 5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This search will promote users who do sports before book readers and they will be placed at the top of the results list.

## Search Options

You can specify the logic of a search expression. It can be either:

* or,
* andAlso,
* not.

The following query:

{CODE:java search_7_0@Indexes\Querying\Searching.java /}

will be translated into

{CODE-BLOCK:csharp}
from Users
where search(Hobbies, 'computers') or search(Name, 'James') and Age = 20
{CODE-BLOCK/}

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follows:

{CODE:java search_8_0@Indexes\Querying\Searching.java /}

will result in the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Adam') and search(Hobbies, 'sport')
{CODE-BLOCK/}

If you want to negate the term use `not`:

{CODE:java search_9_0@Indexes\Querying\Searching.java /}

According to RQL syntax it will be transformed into the query:

{CODE-BLOCK:csharp}
from Users
where exists(Name) and not search(James, 'Adam')
{CODE-BLOCK/}

You can also combine search options:

{CODE:java search_10_1@Indexes\Querying\Searching.java /}

It will produce the following RQL query:

{CODE-BLOCK:csharp}
from Users
where search(Name, 'Adam') and (exists(Hobbies) and not search(Hobbies, 'sport'))
{CODE-BLOCK/}

## Using Wildcards

When the beginning or ending of a search term is unknown, wildcards can be used to add additional power to the searching feature. RavenDB supports both suffix and postfix wildcards.

### Example I - Using Postfix Wildcards

{CODE-TABS}
{CODE-TAB:java:Query search_11_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Users
where search(Name, 'Jo* Ad*')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Using Suffix and Postfix Wildcards

{CODE-TABS}
{CODE-TAB:java:Query search_12_0@Indexes\Querying\Searching.java /}
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

{CODE:java search_20_2@Indexes\Querying\Searching.java/}

{CODE-TABS}
{CODE-TAB:java:Query search_20_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/ByName'
where search(Name, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - FullTextSearch

{CODE-TABS}
{CODE-TAB:java:Query search_21_0@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Users/Search'
where search(Query, 'John')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Indexing all fields for FTS

This indexing method is supported only when using **Lucene** as the indexing engine.

{CODE:java index_all_fields@Indexes\Querying\Searching.java /}

{CODE-TABS}
{CODE-TAB:java:Query search_22@Indexes\Querying\Searching.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByAllValues"
where search(allValues, "tofu")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Indexes

- [Boosting](../../indexes/boosting)
