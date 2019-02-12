# Suggestions

RavenDB indexing mechanism in built upon Lucene engine that has a great suggestions feature. This capability has been also introduced to RavenDB and allows a significant improvement of search functionalities enhancing the overall user experience of the application.

Let's consider an example where the users have the option to look for products by their name. The index and query would look as follow:

{CODE suggestions_1@Indexes\Querying\Suggestions.cs /}

{CODE suggestions_2@Indexes\Querying\Suggestions.cs /}

If our database have `Northwind` samples deployed then it will not return any results, but we can ask RavenDB for help by using:

{CODE suggestions_3@Indexes\Querying\Suggestions.cs /}

It will produce the suggestions:

    Did you mean?
        chang
        chai

The `Suggest` method is an extension contained in `Raven.Client` namespace. It also has an overload that takes one parameter - `SuggestionQuery` that allows
you to specify the suggestion query options:

* *Field* - the name of the field that you want to find suggestions in,
* *Term* - the provided by user search term,
* *MaxSuggestions* - the number of suggestions to return (default: `15`),
* *Distance* - the enum that indicates what string distance algorithm should be used: [JaroWinkler](https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance), [Levenshtein](https://en.wikipedia.org/wiki/Levenshtein_distance) (default) or [NGram](http://webdocs.cs.ualberta.ca/~kondrak/papers/spire05.pdf),
* *Accuracy* - the minimal accuracy required from a string distance for a suggestion match (default: 0.0),
* *Popularity* - determines whether the returned terms should be in order of popularity (default: false).

{CODE-TABS}
{CODE-TAB:csharp:Query query_suggestion_with_options@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:Commands document_store_suggestion@Indexes\Querying\Suggestions.cs /}
{CODE-TAB:csharp:Index suggestions_1@Indexes\Querying\Suggestions.cs /}
{CODE-TABS/}

## Suggest over multiple words

RavenDB allows you to perform a suggestion query over multiple words. In order to use this functionalify you have to pass words that you are looking for in *Term* by using special RavenDB syntax (more details [here](../../indexes/querying/full-query-syntax#suggestions-over-multiple-words)):

{CODE query_suggestion_over_multiple_words@Indexes\Querying\Suggestions.cs /}

This will produce the results:

    Did you mean?
        chai
        chang
        chartreuse
        chef
        tofu

## Remarks

{WARNING Suggestions does not take advantage of the [encryption bundle](../../server/bundles/encryption). You should never use this feature on information that should be encrypted, because then you have a risk of storing sensitive data on a disk in unsecured manner. /}

{WARNING:Increased indexing time}

Indexes with turned on suggestions tend to use much more CPU power than other indexes, this can impact indexing speed (querying is not impacted).

{WARNING/}

## Related articles

- [Client API : Session : How to work with suggestions?](../../client-api/session/querying/how-to-work-with-suggestions)