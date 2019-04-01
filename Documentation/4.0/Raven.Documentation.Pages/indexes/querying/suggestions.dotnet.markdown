# Querying: Suggestions

RavenDB has an indexing mechanism built upon the Lucene engine which has a great suggestions feature. This capability allows a significant improvement of search functionalities enhancing the overall user experience of the application.

Let's consider an example where the users have the option to look for products by their name. The index and query would appear as follows:

{CODE suggestions_1@Indexes\Querying\Suggestions.cs /}

{CODE suggestions_2@Indexes\Querying\Suggestions.cs /}

If our database has `Northwind` samples deployed then it will not return any results. However, we can ask RavenDB for help:

{CODE-TABS}
{CODE-TAB:csharp:Query suggestions_3@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:DocumentQuery suggestions_4@Indexes\Querying\Suggestions.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByName' 
select suggest('Name', 'chaig')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

It will produce these suggestions:

    Did you mean?
        chang
        chai

{NOTE:Client API}

The `SuggestUsing` method is an extension contained in the `Raven.Client.Documents` namespace. You can read more about it in our [Client API](../../client-api/session/querying/how-to-work-with-suggestions) article. 

{NOTE/}

## Suggest Over Multiple Words

RavenDB allows you to perform a suggestion query over multiple words.

{CODE-TABS}
{CODE-TAB:csharp:Query query_suggestion_over_multiple_words@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:DocumentQuery query_suggestion_over_multiple_words_1@Indexes\Querying\Suggestions.cs /}
{CODE-TABS/}

This will produce the following results:

    Did you mean?
        chai
        chang
        chartreuse
        chef
        tofu

## Remarks

{WARNING: Increased indexing time}

Indexes with turned on suggestions tend to use a lot more CPU power than other indexes. This can impact indexing speed (querying is not impacted).

{WARNING/}

## Related Articles

### Client API

- [How to Work with Suggestions](../../client-api/session/querying/how-to-work-with-suggestions)
