# Indexes: Boosting

A that RavenDB leverages from Lucene is called Boosting. This feature gives you the ability to manually tune the relevance level of matching documents when performing a query. 

From the index perspective we can associate to an index entry a boosting factor. The higher value it has, the more relevant term will be. To do this, we must use the `Boost` method.

Let's jump straight into the example. To perform the query that will return employees where either `FirstName` or `LastName` is equal to _Bob_, and to promote employees (move them to the top of the results) where `FirstName` matches the phrase, we must first create an index with boosted entry.

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask boosting_2@Indexes\Boosting.java /}
{CODE-TAB:java:Operation boosting_4@Indexes\Boosting.java /}
{CODE-TABS/}

The next step is to perform a query against that index:

{CODE:java boosting_3@Indexes\Boosting.java /}

## Remarks

{INFO Boosting is also available at the query level. You can read more about it [here](../indexes/querying/boosting). /}

## Related Articles

### Querying

- [Searching](../indexes/querying/searching)
- [Boosting](../indexes/querying/boosting)

### Indexes

- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
