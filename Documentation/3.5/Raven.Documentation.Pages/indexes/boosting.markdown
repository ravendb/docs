# Boosting

Another great feature that Lucene engine provides and RavenDB leverages is called `boosting`. This feature gives user the ability to manually tune the relevance level of matching documents when performing a query. 

From the index perspective we can associate with an index entry a boosting factor and the higher value it has, the more relevant term will be. To do this we must use `Boost` extension method from `Raven.Client.Linq.Indexing` namespace.

To illustrate it better, let's jump straight into the example. To perform a query that will return employees that either `FirstName` or `LastName` is equal to **Bob** and to promote employees (move them to the top of the results) that `FirstName` matches the phrase, we must first create an index with boosted entry.

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask boosting_2@Indexes\Boosting.cs /}
{CODE-TAB:csharp:Commands boosting_4@Indexes\Boosting.cs /}
{CODE-TABS/}

Next step is to perform a query against that index:

{CODE boosting_3@Indexes\Boosting.cs /}

## Remarks

{INFO Boosting is also available when using `Search` method. You can read more about it [here](../indexes/querying/searching#boosting). /}

## Related articles

- [Querying : Searching](../indexes/querying/searching)
- [Analyzers](../indexes/using-analyzers)
- [Storing data in index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
