# Boost Search Results

---

{NOTE: }

* When querying with some filtering conditions,  
  a basic score is calculated for each document in the results by the underlying engine.

* In order to prioritize results,  
  you can provide a __boost value__ to the searched terms which will be integrated with the basic score.

* Usually, the higher the boost value, the more relevant the term will be,  
  resulting in a higher ranking of its matching document in the results.

* In this page:

  * [Boost results - when making a full-text search](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-making-a-full-text-search)
  * [Boost results - when querying with where clause](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-querying-with-where-clause)  
  * [Get resulting score](../../../../client-api/session/querying/text-search/boost-search-results#get-resulting-score)

{NOTE/}

---

{PANEL: Boost results - when making a full-text search}

* When making a full-text search with the `Search()` method then boosting can be applied  
  to both `Query` and `DocumentQuery`.

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query boost_1@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB:csharp:Query_async boost_2@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB:csharp:DocumentQuery boost_3@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where
search(Notes, "English") or boost(search(Notes, "Italian"), 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Boost results - when querying with where clause}

* When querying with `Where` clauses (using an OR condition in between) then boosting can be applied  
  only with `DocuemtQuery`.

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery boost_4@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB:csharp:DocumentQuery_async boost_5@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" where
boost(startsWith(Name, "O"), 10) or
boost(startsWith(Name, "P"), 50) or
boost(endsWith(Name, "OP"), 90)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Get resulting score}

* The score can be retrieved by either:

   * Request to __include explanations__ when making the query.  
     See [include query explanations](../../../../client-api/session/querying/debugging/include-explanations).

   * __Get the metadata__ of the resulting entities that were loaded to the session.  
     See example below.  

{NOTE: }

{CODE:csharp boost_6@ClientApi\Session\Querying\TextSearch\BoostResults.cs /}

{NOTE/}

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Full-text search with index](../../../../indexes/querying/searching)
