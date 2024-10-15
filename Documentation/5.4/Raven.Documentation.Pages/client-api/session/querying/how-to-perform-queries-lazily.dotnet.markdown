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

{CODE-TABS}
{CODE-TAB:csharp:Lazy_query lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_query_async lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_documentQuery lazy_3@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TABS/}

* Learn more about queries in this [query overview](../../../client-api/session/querying/how-to-query).

{PANEL/}

{PANEL: Lazy count query}

{CODE-TABS}
{CODE-TAB:csharp:Lazy_query lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_query_async lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_documentQuery lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Lazy suggestions query}

{CODE-TABS}
{CODE-TAB:csharp:Lazy_query lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_query_async lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_documentQuery lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TABS/}

* Learn more about suggestions in [query for suggestions](../../../client-api/session/querying/how-to-work-with-suggestions).

{PANEL/}

{PANEL: Lazy facets query}

{CODE-TABS}
{CODE-TAB:csharp:Lazy_query lazy_10@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_query_async lazy_11@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Lazy_documentQuery lazy_12@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Facets_definition the_facets@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Index_definition the_index@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TABS/}

* Learn more about facets in [perform faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE syntax_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE syntax_3@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE syntax_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Parameters | Type                                                                                                                              | Description                                                                          |
|------------|-----------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|
| **onEval** | `Action<IEnumerable<TResult>>` <br> `Action<Dictionary<string, SuggestionResult>>` <br> `Action<Dictionary<string, FacetResult>>` | An action that will be performed on the query results<br>when the query is executed. |

| Return Value                                                                                                                           |                                                                |
|----------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------|
| `Lazy<IEnumerable<TResult>>`<br>`Lazy<int>`<br>`Lazy<Dictionary<string, SuggestionResult>>`<br>`Lazy<Dictionary<string, FacetResult>>` | A lazy instance that will evaluate the query only when needed. |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../client-api/session/querying/how-to-query)
- [Perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily)
