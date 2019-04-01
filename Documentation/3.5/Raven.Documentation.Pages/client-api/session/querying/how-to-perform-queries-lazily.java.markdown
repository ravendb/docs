# Session: Querying: How to perform queries lazily?

In some situations query execution must be delayed. To cover such scenario `lazily` and many others query extensions has been introduced.

{PANEL:Lazily}

{CODE:java lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **onEval** | Action1&lt;List&lt;T&gt;&gt; | Action that will be performed on query results. |

| Return Value | |
| ------------- | ----- |
| Lazy<List&lt;TResult&gt;> | Lazy query initializer returning query results. |

### Example

{CODE:java lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:CountLazily}

{CODE:java lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Return Value | |
| ------------- | ----- |
| Lazy<List&lt;TResult&gt;> | Lazy query initializer returning count of matched documents. |

### Example

{CODE:java lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:SuggestLazy}

{CODE:java lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [SuggestionQuery](../../../glossary/suggestion-query) | A suggestion query definition containing all information required to query a specified index. |

| Return Value | |
| ------------- | ----- |
| Lazy<[SuggestionQueryResult](../../../glossary/suggestion-query-result)> | Lazy query initializer containing array of all suggestions for executed query. |

### Example

{CODE:java lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

{PANEL:ToFacetsLazy}

{CODE:java lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | String | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. |

| Return Value | |
| ------------- | ----- |
| Lazy<[FacetResults](../../../glossary/facet-results)> | Lazy query initializer containing Facet query results with query `Duration` and list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters. |

### Example

{CODE:java lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.java /}

{PANEL/}

## Related articles

- [How to perform operations lazily?](../how-to/perform-operations-lazily)
