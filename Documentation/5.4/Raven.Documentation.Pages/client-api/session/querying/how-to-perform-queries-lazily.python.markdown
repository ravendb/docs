# Perform a Lazy Query

---

{NOTE: }

* Query execution can be deferred: the query can be defined as **Lazy**, and executed 
  at a later time, when its results are actually needed.  

* This article contains examples for lazy queries. Prior to reading it, please refer 
  to [perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily) 
  for general knowledge about RavenDB's lazy behavior and other request types that can be 
  executed lazily within a session.

* In this page:
  * [Lazy query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-query)  
  * [Lazy count query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-count-query)  
  * [Lazy suggestions query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-suggestions-query)  
  * [Lazy facets query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-facets-query)  
  * [Syntax](../../../client-api/session/querying/how-to-perform-queries-lazily#syntax)

{NOTE/}

---

{PANEL: Lazy query}

To run a [regular query](../../../client-api/session/querying/how-to-query) 
(not a [suggestions](../../../client-api/session/querying/how-to-work-with-suggestions) 
query or a [faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search)) 
lazily, use `.lazily()` as follows.  

{CODE:python lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}

{PANEL/}

{PANEL: Lazy count query}

To count query results lazily, use `count_lazily()` as follows.  

{CODE:python lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}

{PANEL/}

{PANEL: Lazy suggestions query}

To run a suggestions search lazily, use [suggest_using](../../../client-api/session/querying/how-to-work-with-suggestions) 
along with `.execute_lazy()` as shown below.

{CODE:python lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}

{PANEL/}

{PANEL: Lazy facets query}

To run a faceted query lazily:  

* Search a predefined static index (see _Index_definition_ below)
* Aggregate the results using a facets definition (see _Facets_definition_ below)
* Apply `.execute_lazy()` (see _Query_ below)

{CODE-TABS}
{CODE-TAB:python:Query lazy_10@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE-TAB:python:Facets_definition the_facets@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE-TAB:python:Index_definition the_index@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE-TABS/}

{NOTE: }
Learn more about facets in [perform faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).
{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE:python syntax_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE:python syntax_3@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}
{CODE:python syntax_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.py /}

| Parameters | Type                                                                                                                              | Description                                                                          |
|------------|-----------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|
| **on_eval** | `Callable[[List[_T]], None]`<br>`Callable[[Dict[str, SuggestionResult]], None]` (optional)<br>`Callable[[Dict[str, FacetResult]], None]` (optional)| An action that will be performed on the query results when the query is executed. |

| Return Value                                                                                                                           |                                                                |
|----------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------|
| `Lazy[List[_T]]`<br>`Lazy[Dict[str, SuggestionResult]]`<br>`Lazy[Dict[str, FacetResult]]` | A lazy instance that will evaluate the query only when needed. |
| `Lazy[int]` | The results of a lazy count query |
{PANEL/}

## Related Articles

### Session

- [Query overview](../../../client-api/session/querying/how-to-query)
- [Perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)
- [query for suggestions](../../../client-api/session/querying/how-to-work-with-suggestions)
