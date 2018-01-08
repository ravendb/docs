# Suggestions

RavenDB indexing mechanism in built upon Lucene engine that has a great suggestions feature. This capability has been also introduced to RavenDB and allows a significant improvement of search functionalities enhancing the overall user experience of the application.

Let's consider an example where the users have the option to look for products by their name. The index and query would look as follow:

{CODE suggestions_1@Indexes\Querying\Suggestions.cs /}

{CODE suggestions_2@Indexes\Querying\Suggestions.cs /}

If our database have `Northwind` samples deployed then it will not return any results, but we can ask RavenDB for help by using:

{CODE-TABS}
{CODE-TAB:csharp:Query suggestions_3@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:DocumentQuery suggestions_4@Indexes\Querying\Suggestions.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Products/ByName' 
where suggest('Name', 'chaig')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

It will produce the suggestions:

    Did you mean?
        chang
        chai

{NOTE:Client API}

`SuggestUsing` method is an extension contained in `Raven.Client.Documents` namespace. You can read more about it in our [Client API](../../client-api/session/querying/how-to-work-with-suggestions) article. 

{NOTE/}

## Suggest over multiple words

RavenDB allows you to perform a suggestion query over multiple words. In order to use this functionality you have to pass words that you are looking for in *Term* by using special RavenDB syntax (more details [here](../../indexes/querying/full-query-syntax#suggestions-over-multiple-words)):

{CODE-TABS}
{CODE-TAB:csharp:Query query_suggestion_over_multiple_words@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:DocumentQuery query_suggestion_over_multiple_words_1@Indexes\Querying\Suggestions.cs /}
{CODE-TABS/}

This will produce the results:

    Did you mean?
        chai
        chang
        chartreuse
        chef
        tofu

## Remarks

{WARNING:Increased indexing time}

Indexes with turned on suggestions tend to use much more CPU power than other indexes, this can impact indexing speed (querying is not impacted).

{WARNING/}

## Related articles

- [Client API : Session : How to work with suggestions?](../../client-api/session/querying/how-to-work-with-suggestions)
