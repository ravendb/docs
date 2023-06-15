# Perform a Lazy Query

---

{NOTE: }

* Query execution can be deferred.  
  You can __define a query as lazy__ and only execute it later when the query results are actually needed.

* The lazy query definition is stored in the session and a `Lazy<T>` instance is returned.  
  The query is executed on the server only when you access the value of this instance.  

* You can __define multiple lazy requests__, one after another, and no network activity will be triggered.  
  However, as soon as you access the value of one of those lazy instances,  
  all pending lazy requests held up by the session will be sent to the server as a single unit.  
  This can help reduce the number of remote calls made to the server over the network.  

* Besides queries, other request types can be executed lazily within a session.  
  See [perform requests lazily](../../../client-api/session/how-to/perform-operations-lazily).

* In this page:
  * Lazy query  
  * Lazy count query  
  * Lazy suggestion query  
  * Lazy facets query  
  * Multiple lazy queries  
  * Syntax

{NOTE/}

---

{PANEL:Lazily and LazilyAsync}

{CODE lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **onEval** | Action<IEnumerable&lt;TResult&gt;> | An action that will be performed on the query results. |

| Return Value | |
| ------------- | ----- |
| Lazy<IEnumerable&lt;TResult&gt;> | Lazy query initializer returning query results. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TAB:csharp:Async lazy_3@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Counts}

{CODE lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Return Value | |
| ------------- | ----- |
| Lazy<int> | Lazy query initializer returning a count of matched documents. |

### Example

{CODE lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

{PANEL:Suggestions}

{CODE lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Return Value | |
| ------------- | ----- |
| Lazy<Dictionary<string, SuggestionResult>> | Lazy query initializer containing a dictionary with suggestions for matching executed query |

### Example

{CODE lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

{PANEL:Facets}

{CODE lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Return Value | |
| ------------- | ----- |
| Lazy<Dictionary<string, FacetResult>> | Lazy query initializer containing a dictionary with facet results matching executed query |

### Example

{CODE lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Perform Operations Lazily](../../../client-api/session/how-to/perform-operations-lazily)
