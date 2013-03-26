
#### Suggestions

RavenDB indexing mechanism in built upon Lucene engine that has a great suggestions feature. This capability has been also introduced to RavenDB and will allow a significant improvement of search functionalities enhancing the overall user experience of the application.
Let's consider an example where the users have the option to look for other users by full name. The index and query would look as follow:

	public class User
	{
	    public string Id { get; set; }
	    public string FullName { get; set; }
	}

	public class Users_ByFullName : AbstractIndexCreationTask<User>
	{
	    public Users_ByFullName()
	    {
	        Map = users => from user in users
	                       select new { user.FullName };
	 
	        Indexes.Add(x => x.FullName, FieldIndexing.Analyzed);
	    }
	}

	var query = session.Query<User, Users_ByFullName>().Where(x => x.FullName == "johne");
	 
	var user = query.FirstOrDefault();

If in the database there is the following collection of users:

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

then our sample query will not find any user. Then we can ask RavenDB for help by using:

	if (user == null)
	{
	    SuggestionQueryResult suggestionResult = query.Suggest();
	 
	    Console.WriteLine("Did you mean?");
	 
	    foreach (var suggestion in suggestionResult.Suggestions)
	    {
	        Console.WriteLine("\t{0}", suggestion);
	    }
	}

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
* *Distance* - the enum that indicates what string distance algorithm should be used: [JaroWinkler](http://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance), [Levenshtein](http://en.wikipedia.org/wiki/Levenshtein_distance) (default) or [NGram](http://webdocs.cs.ualberta.ca/~kondrak/papers/spire05.pdf),
* *Accuracy* - the minimal accuracy required from a string distance for a suggestion match (default: 0.0),
* *Popularity* - determines whether the returned terms should be in order of popularity (default: false).

	session.Query<User, Users_ByFullName>()
	       .Suggest(new SuggestionQuery()
	                    {
	                        Field = "FullName",
	                        Term = "johne",
	                        Accuracy = 0.4f,
	                        MaxSuggestions = 5,
	                        Distance = StringDistanceTypes.JaroWinkler,
	                        Popularity = true,
	                    });

The suggestion mechanism is also accessible from the document store:

	store.DatabaseCommands.Suggest("Users/ByFullName", new SuggestionQuery()
	                                                   {
	                                                       Field = "FullName",
	                                                       Term = "johne"
	                                                   });

##### Suggest over multiple words

RavenDB allows you to perform a suggestion query over multiple words. In order to use this functionalify you have to pass words that you are looking for
in *Term* by using special RavenDB syntax (more details [here](../../advanced/full-query-syntax#suggestions-over-multiple-words)):

	SuggestionQueryResult resultsByMultipleWords = session.Query<User, Users_ByFullName>()
	       .Suggest(new SuggestionQuery()
	       {
	           Field = "FullName",
	           Term = "<<johne davi>>",
	           Accuracy = 0.4f,
	           MaxSuggestions = 5,
	           Distance = StringDistanceTypes.JaroWinkler,
	           Popularity = true,
	       });
	 
	Console.WriteLine("Did you mean?");
	 
	foreach (var suggestion in resultsByMultipleWords.Suggestions)
	{
	    Console.WriteLine("\t{0}", suggestion);

This will produce the results:

	Did you mean?
        john
        jones
        johnson
        david
        jack


