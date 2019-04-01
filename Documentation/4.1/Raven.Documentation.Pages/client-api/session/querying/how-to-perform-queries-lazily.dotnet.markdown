# Session: Querying: How to Perform Queries Lazily

In some situations, query execution must be delayed. To cover such a scenario, `Lazily` and many other query extensions have been introduced.

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
