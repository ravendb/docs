#Suggestions

RavenDB indexing mechanism in built upon Lucene engine that has a great suggestions feature. This capability has been also introduced to RavenDB and will allow a significant improvement of search functionalities enhancing the overall user experience of the application.
Let's consider an example where the users have the option to look for other users by full name. The index and query would look as follow:

{CODE user_class@ClientApi\Querying\StaticIndexes\Suggestions.cs /}
{CODE index_def@ClientApi\Querying\StaticIndexes\Suggestions.cs /}
{CODE query@ClientApi\Querying\StaticIndexes\Suggestions.cs /}

If in the database there is the following collection of users:

{CODE-START:json /}
// users/1
{
	"Name": "John Smith"
}
// users/2
{
	"Name": "Jack Johnson"
}
// users/3
{
	"Name": "Robery Jones"
}
// users/4
{
	"Name": "David Jones"
}
{CODE-END/}
then our sample query will not find any user. Then we can ask RavenDB for help by using:

{CODE query_suggestion@ClientApi\Querying\StaticIndexes\Suggestions.cs /}

It will produce the suggestions:

	Did you mean?
			john
			jones
			johnson

The `Suggest` method is an extension contained in `Raven.Client` namespace. It also has an overload that takes one parameter - `SuggestionQuery` that allows
you to specify the suggesion query options:

* *Field* - the name of the field that you want to find suggestions in,
* *Term* - the provided by user search term,
* *MaxSuggestions* - the number of suggestions to return (default: 15),
* *Distance* - the enum that indicates what string distance algorithm should be used: [JaroWinkler](https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance), [Levenshtein](https://en.wikipedia.org/wiki/Levenshtein_distance) (default) or [NGram](http://webdocs.cs.ualberta.ca/~kondrak/papers/spire05.pdf),
* *Accuracy* - the minimal accuracy required from a string distance for a suggestion match (default: 0.0),
* *Popularity* - determines whether the returned terms should be in order of popularity (default: false).

{CODE query_suggestion_with_options@ClientApi\Querying\StaticIndexes\Suggestions.cs /}

The suggestion mechanism is also accessible from the document store:

{CODE document_store_suggestion@ClientApi\Querying\StaticIndexes\Suggestions.cs /}

##Suggest over multiple words

RavenDB allows you to perform a suggestion query over multiple words. In order to use this functionalify you have to pass words that you are looking for
in *Term* by using special RavenDB syntax (more details [here](../../advanced/full-query-syntax#suggestions-over-multiple-words)):

{CODE query_suggestion_over_multiple_words@ClientApi\Querying\StaticIndexes\Suggestions.cs /}

This will produce the results:

	Did you mean?
        john
        jones
        johnson
        david
        jack

{NOTE Suggestions does not take advantage  of [the encryption bundle](../../../server/extending/bundles/encryption). You should never use this feature on information that should be encrypted, because then you have a risk of storing sensitive data on a disk in unsecured manner. /}
