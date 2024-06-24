# Indexes: Boosting
---

{NOTE: }

* When querying with some filtering conditions, a basic **score** is 
  calculated by the underlying engine for each document in the results.  

* Providing a **boost value** to selected fields allows prioritization of the resulting documents.  
  The boos value is integrated with the basic score, increasing the document rank.  

* The automatic ordering of results by their score is [configurable](../indexes/boosting#automatic-score-based-ordering).  

* Boosting can be achieved in the following ways:

    * **At query time**:  
      By applying a boost factor to searched terms at query time (see [Boost search results](../client-api/session/querying/text-search/boost-search-results)).  

    * **Via index definition**:  
      By applying a boost factor in the index definition, as described in this article.  
 
* In this page:
    * [Assign a boost factor to an index-field](../indexes/boosting#assign-a-boost-factor-to-an-index-field)
    * [Assign a boost factor to the index-entry](../indexes/boosting#assign-a-boost-factor-to-the-index-entry)
    * [Automatic score-based ordering](../indexes/boosting#automatic-score-based-ordering)
    * [Corax vs Lucene: boosting differences](../indexes/boosting#automatic-score-based-ordering)

{NOTE/}

---

{PANEL: Assign a boost factor to an index-field}

Applying a boost value to an index-field allows prioritization of matching documents based on an index-field.

---


##### The index:

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_1@Indexes\Boosting.cs /}
{CODE-TAB:csharp:JavaScript_index index_1_js@Indexes\Boosting.cs /}
{CODE-TABS/}

##### The query:

{CODE-TABS}
{CODE-TAB:csharp:Query query_1@Indexes\Boosting.cs /}
{CODE-TAB:csharp:Query_async query_2@Indexes\Boosting.cs /}
{CODE-TAB:csharp:DocumentQuery query_3@Indexes\Boosting.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByCountries/BoostByField"
where ShipToCountry == "poland" or CompanyCountry == "portugal"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Assign a boost factor to the index-entry}

Applying a boost value to the whole index-entry allows prioritization of matching documents by content from the document.

---

##### The index:

{CODE-TABS}
{CODE-TAB:csharp:LINQ_index index_2@Indexes\Boosting.cs /}
{CODE-TAB:csharp:JavaScript_index index_2_js@Indexes\Boosting.cs /}
{CODE-TABS/}

##### The query:

{CODE-TABS}
{CODE-TAB:csharp:Query query_4@Indexes\Boosting.cs /}
{CODE-TAB:csharp:Query_async query_5@Indexes\Boosting.cs /}
{CODE-TAB:csharp:DocumentQuery query_6@Indexes\Boosting.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByCountries/BoostByIndexEntry"
where ShipToCountry == "poland" or CompanyCountry == "portugal"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Automatic score-based ordering}

* By default, whenever boosting is applied, either via dynamic querying or when querying an index 
  that has a boosting factor in its definition, the results will be automatically ordered by the score.  

* This behavior can be modified using the [OrderByScoreAutomaticallyWhenBoostingIsInvolved](../server/configuration/indexing-configuration#indexing.orderbyscoreautomaticallywhenboostingisinvolved)  
  configuration key.  

* Refer to the [Get resulting score](../client-api/session/querying/sort-query-results#get-resulting-score) 
  section to learn how to retrieve the calculated score of each result.  

{PANEL/}

{PANEL: Corax vs Lucene: boosting differences}

* **Boosting features available:**  

  * When using **Corax** as the underlying indexing engine, a boost factor can only be assigned 
    to the [index-entry](../indexes/boosting#assign-a-boost-factor-to-the-index-entry).  
    Applying a boost factor to an _index-field_ is Not supported.  
  
  * When using **Lucene**, a boost factor can be assigned to both the index-field and the whole index-entry.  

* **Algorithm used**:  
  Corax ranks search results using the [BM25 algorithm](https://en.wikipedia.org/wiki/Okapi_BM25).  
  Other search engines, e.g. Lucene, may use a different ranking algorithm and return different search results.  

{PANEL/}

## Related Articles

### Querying

- [Full-text search](../client-api/session/querying/text-search/full-text-search)
- [Boost search results](../client-api/session/querying/text-search/boost-search-results)

### Indexes

- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
