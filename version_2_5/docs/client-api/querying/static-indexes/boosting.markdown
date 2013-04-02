
#### Boosting

Another great feature that Lucene engine provides and RavenDB leverages is called `boosting`. This feature gives user the ability to manually tune the relevance level of matching documents when performing a query. 

From the index perspective we can associate with an index entry a boosting factor and the higher value it has, the more relevant term will be. To do this we must use `Boost` extension method from `Raven.Client.Linq.Indexing` namespace.

To illustrate it better, lets jump straight into the example

	public class User
	{
	    public string FirstName { get; set; }
	 
	    public string LastName { get; set; }
	}

To perform a query that will return users that either `FirstName` or `LastName` is equal to **Bob** and to promote users (move them to the top of the results) that `FirstName` matches the phrase, we must first create an index with boosted entry.

	public class Users_ByName : AbstractIndexCreationTask<User>
	{
	    public Users_ByName()
	    {
	        this.Map = users => from user in users
	                            select new
	                                {
	                                    FirstName = user.FirstName.Boost(10),
	                                    LastName = user.LastName
	                                };
	    }
	}

Next step is to perform a query against that index.

	session.Query<User, Users_ByName>()
	       .Where(x => x.FirstName == "Bob" || x.LastName == "Bob")
	       .ToList();

Boosting is also available when using `Search` method. You can read more about it [here](searching#boosting).