# Indexes: Boosting
---

{NOTE: }

* When querying with some filtering conditions,  
  a basic score is calculated for each document in the results by the underlying engine.

* Providing a __boost value__ to some fields allows you to prioritize the resulting documents.  
  The boost value is integrated with the basic score, making the document rank higher.  
  Automatic ordering of the results by the score is [configurable](../indexes/boosting#automatic-score-based-ordering). 

* Boosting can be achieved in the following ways:

    * __At query time__:  
      Apply a boost factor to searched terms at query time - see article [Boost search results](../client-api/session/querying/text-search/boost-search-results).

    * __Via index definition__:  
      Apply a boost factor in your index definition - as described in this article.
 
* In this page:
    * [Assign a boost factor to an index-field](../indexes/boosting#assign-a-boost-factor-to-an-index-field)
    * [Assign a boost factor to the index-entry](../indexes/boosting#assign-a-boost-factor-to-the-index-entry)
    * [Automatic score-based ordering](../indexes/boosting#automatic-score-based-ordering)

{NOTE/}

---

{PANEL: Assign a boost factor to an index-field}

Applying a boost value to an index-field allows you to prioritize matching documents based on an index-field.

---


##### The index:

{CODE-TABS}
{CODE-TAB:csharp:LINQ-index index_1@Indexes\Boosting.cs /}
{CODE-TAB:csharp:JavaScript-index index_1_js@Indexes\Boosting.cs /}
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

Applying a boost value to the whole index-entry allows you to prioritize matching documents by content from the document.

---

##### The index:

{CODE-TABS}
{CODE-TAB:csharp:LINQ-index index_2@Indexes\Boosting.cs /}
{CODE-TAB:csharp:JavaScript-index index_2_js@Indexes\Boosting.cs /}
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

* By default, whenever boosting is involved, either via a dynamic query or when querying an index that has a boosting factor in its definition,
  the results will be automatically ordered by the score.

* This behavior can be modified using the [OrderByScoreAutomaticallyWhenBoostingIsInvolved](../server/configuration/indexing-configuration#indexing.orderbyscoreautomaticallywhenboostingisinvolved)    
  configuration key.

* Refer to section [Get resulting score](../client-api/session/querying/sort-query-results#get-resulting-score) to learn how to retrieve the calculated score of each result.

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
