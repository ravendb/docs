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

{CODE:nodejs lazy_1@client-api\session\Querying\howToPerformQueriesLazily.js /}

* Learn more about queries in this [query overview](../../../client-api/session/querying/how-to-query).

{PANEL/}

{PANEL: Lazy count query}

{CODE:nodejs lazy_2@client-api\session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

{PANEL: Lazy suggestions query}

{CODE:nodejs lazy_3@client-api\session\Querying\howToPerformQueriesLazily.js /}

* Learn more about suggestions in [query for suggestions](../../../client-api/session/querying/how-to-work-with-suggestions).

{PANEL/}

{PANEL: Lazy facets query}

{CODE-TABS}
{CODE-TAB:nodejs:Lazy_query lazy_4@client-api\session\Querying\howToPerformQueriesLazily.js /}
{CODE-TAB:nodejs:Index_definition the_index@client-api\session\Querying\howToPerformQueriesLazily.js /}
{CODE-TABS/}

* Learn more about facets in [perform faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\Querying\howToPerformQueriesLazily.js /}

| Parameters | Type                 | Description                                                                          |
|------------|----------------------|--------------------------------------------------------------------------------------|
| **onEval** | `(object[]) => void` | An action that will be performed on the query results<br>when the query is executed. |

| Return Value |                                                                  |
|--------------|------------------------------------------------------------------|
| **object**   | A `Lazy` instance that will evaluate the query only when needed. |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../client-api/session/querying/how-to-query)
- [Perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)
