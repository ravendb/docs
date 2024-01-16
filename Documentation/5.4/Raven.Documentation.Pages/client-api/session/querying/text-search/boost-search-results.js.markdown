# Boost Search Results

---

{NOTE: }

* When querying with some filtering conditions,  
  a basic score is calculated for each document in the results by the underlying engine.

* Providing a boost value to some fields allows you to prioritize the resulting documents.  
  The boost value is integrated with the basic score, making the document rank higher.

* Boosting can be achieved in the following ways:

    * __At query time__:  
      Apply a boost factor to searched terms at query time - as described in this article.

    * __Via index definition__:  
      Apply a boost factor in your index definition - see this [boosting](../../../../indexes/boosting) article in under indexes.

* The automatic ordering of the results by the score is now configurable.  
  Learn more in section [automatic score-based ordering](../../../../indexes/boosting#automatic-score-based-ordering).

* In this page:

  * [Boost results - when making a full-text search](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-making-a-full-text-search)
  * [Boost results - when querying with where clause](../../../../client-api/session/querying/text-search/boost-search-results#boost-results---when-querying-with-where-clause)  
  * [Get resulting score](../../../../client-api/session/querying/text-search/boost-search-results#get-resulting-score)

{NOTE/}

---

{PANEL: Boost results - when making a full-text search}

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query boost_1@ClientApi\Session\Querying\TextSearch\boostResults.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where
search(Notes, "English") or boost(search(Notes, "Italian"), 10)
{"p0":"English","p1":"Italian"}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Boost results - when querying with where clause}

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query boost_2@ClientApi\Session\Querying\TextSearch\boostResults.js /}
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

{CODE:nodejs boost_3@ClientApi\Session\Querying\TextSearch\boostResults.js /}

{NOTE/}

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Full-text search with index](../../../../indexes/querying/searching)
