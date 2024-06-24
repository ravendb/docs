# Indexes: Boosting

When querying with some filtering conditions,  
a basic score is calculated for each document in the results by the underlying engine.

Providing a **boost value** to some fields allows you to prioritize the resulting documents.  
The boost value is integrated with the basic score, making the document rank higher.  
Automatic ordering of the results by the score is [configurable](../indexes/boosting#automatic-score-based-ordering).

From the index perspective we can associate to an index entry a boosting factor.  
The higher value it has, the more relevant term will be. To do this, we must use the `Boost` method. 

Let's jump straight into the example.  
To perform the query that will return employees where either `FirstName` or `LastName` is equal to _Bob_,  
and to promote employees (move them to the top of the results) where `FirstName` matches the phrase, we must first create an index with boosted entry.

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask boosting_2@Indexes\Boosting.java /}
{CODE-TAB:java:Operation boosting_4@Indexes\Boosting.java /}
{CODE-TABS/}

The next step is to perform a query against that index:

{CODE:java boosting_3@Indexes\Boosting.java /}

## Remarks

{INFO Boosting is also available at the query level. /}

{NOTE: }

#### Automatic score-based ordering

* By default, whenever boosting is involved, either via a dynamic query or when querying an index that has a boosting factor in its definition,
  the results will be automatically ordered by the score.

* This behavior can be modified using the [OrderByScoreAutomaticallyWhenBoostingIsInvolved](../server/configuration/indexing-configuration#indexing.orderbyscoreautomaticallywhenboostingisinvolved)    
  configuration key.

{NOTE/}

{NOTE: }

When using [Corax](../indexes/search-engine/corax) as the search engine:  

* [indexing-time boosting](../indexes/search-engine/corax#supported-features) 
  is available for documents, but not for document fields.  
* Corax ranks search results using the [BM25 algorithm](https://en.wikipedia.org/wiki/Okapi_BM25).  
  Other search engines, e.g. Lucene, may use a different ranking algorithm and return different search results.  

{NOTE/}

## Related Articles

### Indexes

- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
