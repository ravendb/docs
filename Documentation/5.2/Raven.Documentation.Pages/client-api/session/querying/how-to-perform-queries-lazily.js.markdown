# Perform a Lazy Query

---

{NOTE: }

* Query execution can be deferred.  
  You can __define a query as lazy__ and execute it later when query results are actually needed.

* This article contains lazy queries examples.  
  __Prior to this article__, please refer to [perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)  for general knowledge about  
  RavenDB's lazy behavior, and other request types that can be executed lazily within a session.

* In this page:
    * [Lazy query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-query)
    * [Lazy count query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-count-query)
    * [Lazy suggestions query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-suggestions-query)
    * [Lazy facets query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-facets-query)
    * [Syntax](../../../client-api/session/querying/how-to-perform-queries-lazily#syntax)

{NOTE/}

---

{PANEL: Lazy query}

{CODE:nodejs lazy_1@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

* Learn more about queries in this [query overview](../../../client-api/session/querying/how-to-query).

{PANEL/}

{PANEL: Lazy count query}

{CODE:nodejs lazy_2@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

{PANEL: Lazy suggestions query}

{CODE:nodejs lazy_3@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

* Learn more about suggestions in [query for suggestions](../../../client-api/session/querying/how-to-work-with-suggestions).

{PANEL/}

{PANEL: Lazy facets query}

{CODE-TABS}
{CODE-TAB:nodejs:Lazy_query lazy_4@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}
{CODE-TAB:nodejs:Index_definition the_index@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}
{CODE-TABS/}

* Learn more about facets in [perform faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

| Parameters | Type                 | Description                                                                          |
|------------|----------------------|--------------------------------------------------------------------------------------|
| __onEval__ | `(object[]) => void` | An action that will be performed on the query results<br>when the query is executed. |

| Return Value |                                                                  |
|--------------|------------------------------------------------------------------|
| __object__   | A `Lazy` instance that will evaluate the query only when needed. |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../client-api/session/querying/how-to-query)
- [Perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)
