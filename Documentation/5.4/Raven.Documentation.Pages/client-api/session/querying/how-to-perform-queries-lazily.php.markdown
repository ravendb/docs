# Perform a Lazy Query

---

{NOTE: }

* Query execution can be deferred: the query can be defined as **Lazy**, and executed 
  at a later time, when its results are actually needed.  

* This article contains examples for lazy queries. Prior to reading it, please refer 
  to [perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily) 
  for general knowledge about RavenDB's lazy behavior and other request types that can be 
  executed lazily within a session.

* Learn more about queries in this [query overview](../../../client-api/session/querying/how-to-query).

* In this page:
  * [Lazy query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-query)  
  * [Lazy count query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-count-query)  
  * [Lazy suggestions query](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-suggestions-query)  
  * [Lazy Aggregation](../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-aggregation)  
  * [Syntax](../../../client-api/session/querying/how-to-perform-queries-lazily#syntax)

{NOTE/}

---

{PANEL: Lazy query}

{CODE:php lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}

{PANEL/}

{PANEL: Lazy count query}

{CODE:php lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}

{PANEL/}

{PANEL: Lazy suggestions query}

{CODE:php lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}

* Learn more about suggestions in [query for suggestions](../../../client-api/session/querying/how-to-work-with-suggestions).

{PANEL/}

{PANEL: Lazy aggregation}

{CODE:php lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}
{CODE:php lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}
{CODE:php lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.php /}

| Parameters | Type       | Description                                                           |
|------------|------------------------------------------------------------------------------------|
| **$onEval** | `?Closure` | An action to perform on the query results when the query is executed |

| Return Value                                                                                                                           |                                                                |
|------------------------------------------------------------------------|
| `Lazy` | A lazy instance that will evaluate the query only when needed |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../client-api/session/querying/how-to-query)
- [Perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)
