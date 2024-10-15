# Boost Search Results

---

{NOTE: }

* When querying with some filtering conditions,  
  a basic score is calculated for each document in the results by the underlying engine.

* Providing a boost value to some fields allows you to prioritize the resulting documents.  
  The boost value is integrated with the basic score, making the document rank higher.  

* Boosting can be achieved in the following ways:  

    * **At query time**:  
      Apply a boost factor to searched terms at query time - as described in this article.

    * **Via index definition**:  
      Apply a boost factor in your index definition - see this [boosting](../../../../indexes/boosting) indexing article.

* The automatic ordering of the results by the score is configurable.  
  Learn more here: [automatic score-based ordering](../../../../indexes/boosting#automatic-score-based-ordering)  

* The calculated score details of the results can be retrieved if needed.  
  Learn more here: [get resulting score](../../../../client-api/session/querying/sort-query-results#get-resulting-score)  

* In this page:

  * [Boost results - when making a full-text search](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-making-a-full-text-search)
  * [Boost results - when querying with where clause](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-querying-with-where-clause)  

{NOTE/}

---

{PANEL: Boost results - when making a full-text search}

To apply boosting while running a full-text search, use the 
`boost()` method to prioritize the preceding `search()` results.  

{CODE-TABS}
{CODE-TAB:php:query boost_1@ClientApi\Session\Querying\TextSearch\BoostResults.php /}
{CODE-TAB:csharp:documentQuery boost_3@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where
search(Notes, "English") or boost(search(Notes, "Italian"), 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Boost results - when querying with where clause}

`boost()` can be used to give different priorities to the results 
returned by different `where` clauses.  

{CODE-TABS}
{CODE-TAB:php:query boost_4@ClientApi\Session\Querying\TextSearch\BoostResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" where
boost(startsWith(Name, "O"), 10) or
boost(startsWith(Name, "P"), 50) or
boost(endsWith(Name, "OP"), 90)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Full-text search with index](../../../../indexes/querying/searching)
