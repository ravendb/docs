# Session: Querying: How to Perform Queries Lazily

In some situations, query execution must be delayed. To cover such a scenario, `lazily` and many other query extensions have been introduced.

{PANEL:Lazily}

{CODE:java lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **onEval** | Consumer<List&lt;TResult&gt;> | An action that will be performed on the query results. |

| Return Value | |
| ------------- | ----- |
| Lazy<List&lt;TResult&gt;> | Lazy query initializer returning query results. |

### Example

{CODE:java lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:Counts}

{CODE:java lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Return Value | |
| ------------- | ----- |
| Lazy<Integer> | Lazy query initializer returning a count of matched documents. |

### Example

{CODE:java lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:Suggestions}

{CODE:java lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Return Value | |
| ------------- | ----- |
| Lazy<Map<String, SuggestionResult>> | Lazy query initializer containing a map with suggestions for matching executed query |

### Example

{CODE:java lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:Facets}

{CODE:java lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Return Value | |
| ------------- | ----- |
| Lazy<Map<String, FacetResult>> | Lazy query initializer containing a map with facet results matching executed query |

### Example

{CODE:java lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Perform Operations Lazily](../../../client-api/session/how-to/perform-operations-lazily)
