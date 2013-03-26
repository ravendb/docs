
#### Searching

One of the most common functionality that many real world applications provide is a search feature. Many times it will be enough to apply `Where` closure to create a simple condition,
for example to get all users whose age is greater that 20 use the code:

	users = session.Query<User>("UsersByAge").Where(x => x.Age > 20).ToList();

where `User` class and `UsersByName` index are defined as follow:

	public class User
	{
	    public string Name { get; set; }
	 
	    public byte Age { get; set; }
	 
	    public ICollection<string> Hobbies { get; set; } 
	}

	documentStore.DatabaseCommands.PutIndex("UsersByName", new IndexDefinition
	{
	    Map = "from user in docs.Users select new { user.Name }",
	    Indexes = { { "Name", FieldIndexing.Analyzed } }
	});

The `Where` statement also is good if you want to perform a really simple text field search, for example let's create a query to retrieve users whose name starts with *Jo*:

	users = session.Query<User>("UsersByName").Where(x => x.Name.StartsWith("Jo")).ToList();

Eventually all queries are always transformed into a Lucene query. The query like above will be translated into <em>Name:Jo*</em>.

{WARNING An attempt to use `string.Contains()` method as condition of `Where` closure, will throw `NotSupportedException`. That is because the search term like \*<em>term</em>* (note wildcards at the beginning and at the end) can cause performance issues. Due to Raven's *safe-by-default* paradigm such operation is forbidden. If you really want to achieve this case, you will find more details in one of the next section below. /}

{INFO Note that that results of a query might be different depending on [an analyzer](../static-indexes/configuring-index-options) that was applied./}

##### Multiple terms

When you need to do a more complex text searching use `Search` extension method (in `Raven.Client` namespace). This method allows you to pass a few search terms that will be used in searching process for a particular field. Here is a sample code
that uses `Search` extension to get users with name *John* or *Adam*:

	users = session.Query<User>("UsersByName").Search(x => x.Name, "John Adam").ToList();

Each of search terms (separated by space character) will be checked independently. The result documents must match exact one of the passed terms.

The same way you are also able to look for users that have some hobby. Create the index:

	documentStore.DatabaseCommands.PutIndex("UsersByHobbies", new IndexDefinition
	{
	    Map = "from user in docs.Users select new { user.Hobbies }",
	    Indexes = { { "Hobbies", FieldIndexing.Analyzed } }
	});

Now you are able to execute the following search:

	users = session.Query<User>("UsersByHobbies")
	    .Search(x => x.Hobbies, "looking for someone who likes sport books computers").ToList();

In result you will get users that are interested in *sport*, *books* or *computers*.

##### Multiple fields

By using `Search` extension you are also able to look for by multiple indexed fields. First let's introduce the index:
 
	documentStore.DatabaseCommands.PutIndex("UsersByNameAndHobbies", new IndexDefinition
	{
	    Map = "from user in docs.Users select new { user.Name, user.Hobbies }",
	    Indexes = { { "Name", FieldIndexing.Analyzed }, { "Hobbies", FieldIndexing.Analyzed } }
	});

Now we are able to search by using `Name` and `Hobbies` properties:

	users = session.Query<User>("UsersByNameAndHobbies")
	               .Search(x => x.Name, "Adam")
	               .Search(x => x.Hobbies, "sport").ToList();

##### Boosting

Indexing in RavenDB is built upon Lucene engine that provides a boosting term mechanism. This feature introduces the relevance level of matching documents based on the terms found. 
Each search term can be associated with a boost factor that influences the final search results. The higher the boost factor, the more relevant the term will be. 
RavenDB also supports that, in order to improve your searching mechanism and provide the users with much more accurate results you can specify the boost argument. Let's see the example:

	users = session.Query<User>("UsersByHobbies")
	               .Search(x => x.Hobbies, "I love sport", boost:10)
	               .Search(x => x.Hobbies, "but also like reading books", boost:5).ToList();

The search above will promote users who do sports before book readers and they will be placed at the top of the result list.

##### Search options

In order to specify the logic of search expression specify the options argument of the `Search` method. It is `SearchOptions` enum with the following values:

* Or,
* And,
* Not,
* Guess (default).

By default RavenDB attempts to guess and match up the semantics between terms. If there are consecutive searches, they will be OR together, otherwise AND semantic will be used by default.

The following query:

	users = session.Query<User>("UsersByNameAndHobbiesAndAge")
	               .Search(x => x.Hobbies, "computers")
	               .Search(x => x.Name, "James")
	               .Where(x => x.Age == 20).ToList();

will be translated into <em>( Hobbies:(computers) Name:(James)) AND (Age:20)</em> (if there is no boolean operator then OR is used).

You can also specify what exactly the query logic should be. The applied option will influence a query term where it was used. The query as follow:

	users = session.Query<User>("UsersByNameAndHobbies")
	               .Search(x => x.Name, "Adam")
	               .Search(x => x.Hobbies, "sport", options: SearchOptions.And).ToList();

will result in the following Lucene query: <em>Name:(Adam) AND Hobbies:(sport)</em>

If you want to negate the term use `SearchOptions.Not`:

	users = session.Query<User>("UsersByName")
	        .Search(x => x.Name, "James", options: SearchOptions.Not).ToList();

According to Lucene syntax it will be transformed to the query: <em>-Name:(James)</em>.

You can treat `SearchOptions` values as bit flags and create any combination of the defined enum values, e.g:

	users = session.Query<User>("UsersByNameAndHobbies")
	        .Search(x => x.Name, "Adam")
	        .Search(x => x.Hobbies, "sport", options: SearchOptions.Not | SearchOptions.And)
	        .ToList();

It will produce the following Lucene query: <em>Name:(Adam) AND -Hobbies:(sport)</em>.

##### Query escaping

The code examples presented in this section have hard coded searching terms. However in a real use case the user will specify the term. You are able to control the escaping strategy of the provided query by specifying 
the `EscapeQueryOptions` parameter. It's the enum that can have one of the following values:

* EscapeAll (default),
* AllowPostfixWildcard,
* AllowAllWildcards,
* RawQuery.

By default all special characters contained in the query will be escaped (`EscapeAll`). However you can add a bit more of flexibility to your searching mechanism.
`EscapeQueryOptions.AllowPostfixWildcard` enables searching against a field by using search term that ends with wildcard character:

	users = session.Query<User>("UsersByName")
	    .Search(x => x.Name, "Jo* Ad*", 
	            escapeQueryOptions:EscapeQueryOptions.AllowPostfixWildcard).ToList();

The next option `EscapeQueryOptions.AllowAllWildcards` extends the previous one by allowing the wildcard character to be present at the beginning as well as at the end of the search term.

	users = session.Query<User>("UsersByName")
	    .Search(x => x.Name, "*oh* *da*", 
	            escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards).ToList();

{WARNING RavenDB allows to search by using such queries but you have to be aware that leading wildcards drastically slow down searches. Consider if you really need to find substrings, most cases looking for words is enough. There are also other alternatives for searching without expensive wildcard matches, e.g. indexing a reversed version of text field or creating a custom analyzer./}

The last option makes that the query will not be escaped and the raw term will be relayed to Lucene:

	users = session.Query<User>("UsersByName")
	    .Search(x => x.Name, "*J?n*",
	            escapeQueryOptions: EscapeQueryOptions.RawQuery).ToList();