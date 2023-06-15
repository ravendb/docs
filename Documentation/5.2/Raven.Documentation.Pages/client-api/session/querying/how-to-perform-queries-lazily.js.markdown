# Perform a Lazy Query

In some situations, query execution must be delayed. To cover such a scenario, `lazily` and many other query extensions have been introduced.

{PANEL:Lazily}

{CODE:nodejs lazy_1@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

| Return Value | |
| ------------- | ----- |
| `Lazy<object[]>` | Lazy query initializer returning query results. |

### Example

{CODE:nodejs lazy_2@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

{PANEL:Counts}

{CODE:nodejs lazy_4@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

| Return Value | |
| ------------- | ----- |
| `Lazy<number>` | Lazy query initializer returning a count of matched documents. |

### Example

{CODE:nodejs lazy_5@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

{PANEL:Suggestions}

{CODE:nodejs lazy_6@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

| Return Value | |
| ------------- | ----- |
| `Lazy<{ [key]: SuggestionResult }>` | Lazy query initializer containing a map with suggestions for matching executed query |

### Example

{CODE:nodejs lazy_7@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

{PANEL:Facets}

{CODE:nodejs lazy_8@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

| Return Value | |
| ------------- | ----- |
| `Lazy<{ [key]: FacetResult }>` | Lazy query initializer containing a map with facet results matching executed query |

### Example

{CODE:nodejs lazy_9@ClientApi\Session\Querying\howToPerformQueriesLazily.js /}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Perform Operations Lazily](../../../client-api/session/how-to/perform-operations-lazily)
