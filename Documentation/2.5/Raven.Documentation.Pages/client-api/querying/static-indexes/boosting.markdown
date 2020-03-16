#Boosting

Another great feature that Lucene engine provides and RavenDB leverages is called `boosting`. This feature gives user the ability to manually tune the relevance level of matching documents when performing a query. 

From the index perspective we can associate with an index entry a boosting factor and the higher value it has, the more relevant term will be. To do this we must use `Boost` extension method from `Raven.Client.Linq.Indexing` namespace.

To illustrate it better, let's jump straight into the example

{CODE boosting_1@ClientApi\Querying\StaticIndexes\Boosting.cs /}

To perform a query that will return users that either `FirstName` or `LastName` is equal to **Bob** and to promote users (move them to the top of the results) that `FirstName` matches the phrase, we must first create an index with boosted entry.

{CODE boosting_2@ClientApi\Querying\StaticIndexes\Boosting.cs /}

Next step is to perform a query against that index.

{CODE boosting_3@ClientApi\Querying\StaticIndexes\Boosting.cs /}

Boosting is also available when using `Search` method. You can read more about it [here](searching#boosting).