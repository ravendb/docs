# Indexes: Boosting

A feature that RavenDB leverages from Lucene is called Boosting. This feature gives you the ability to manually tune the relevance level of matching documents when performing a query. 

From the index perspective we can associate to an index entry a boosting factor. The higher value it has, the more relevant term will be. To do this, we must use the `Boost` extension method from the `Raven.Client.Documents.Linq.Indexing` namespace.

Let's jump straight into the example. To perform the query that will return employees where either `FirstName` or `LastName` is equal to _Bob_, and to promote employees (move them to the top of the results) where `FirstName` matches the phrase, we must first create an index with boosted entry.

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask boosting_2@Indexes\Boosting.cs /}
{CODE-TAB:csharp:Operation boosting_4@Indexes\Boosting.cs /}
{CODE-TABS/}

The next step is to perform a query against that index:

{CODE boosting_3@Indexes\Boosting.cs /}

## Remarks

{INFO Boosting is also available at the query level. You can read more about it [here](../client-api/session/querying/text-search/boost-search-results). /}

{NOTE: }
When using [Corax](../indexes/search-engine/corax) as the search engine, 
[indexing-time boosting is available](../indexes/search-engine/corax#supported-features) 
for documents, but not for document fields.  
{NOTE/}

## Related Articles

### Querying

- [Full-text search](../client-api/session/querying/text-search/full-text-search)
- [Boost search results](../client-api/session/querying/text-search/boost-search-results)

### Indexes

- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
