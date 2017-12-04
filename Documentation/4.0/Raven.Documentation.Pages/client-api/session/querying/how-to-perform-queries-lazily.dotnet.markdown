# Session : Querying : How to perform queries lazily?

In some situations query execution must be delayed. To cover such scenario `Lazily` and many others query extensions has been introduced.

{PANEL:Lazily and LazilyAsync}

{CODE lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **onEval** | Action<IEnumerable&lt;TResult&gt;> | Action that will be performed on query results. |

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
| Lazy<int> | Lazy query initializer returning count of matched documents. |

### Example

{CODE lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

{PANEL:Suggestions}

{CODE lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Return Value | |
| ------------- | ----- |
| Lazy<Dictionary<string, SuggestionResult>> | Lazy query initializer containing dictionary with suggestions matching executed query |

### Example

{CODE lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

{PANEL:Facets}

{CODE lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

| Return Value | |
| ------------- | ----- |
| Lazy<Dictionary<string, FacetResult>> | Lazy query initializer containing dictionary with facet results matching executed query |

### Example

{CODE lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

{PANEL/}

## Related articles

- [How to perform operations lazily?](../how-to/perform-operations-lazily)
