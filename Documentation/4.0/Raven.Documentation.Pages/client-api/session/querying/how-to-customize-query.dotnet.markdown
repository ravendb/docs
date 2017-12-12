# Session : Querying : How to Customize a Query

The following query customization options are available in the `IDocumentQueryCustomization` interface:

- [BeforeQueryExecuted](../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- [AfterQueryExecuted](../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- [AfterStreamExecuted](../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching)
- [NoTracking](../../../client-api/session/querying/how-to-customize-query#notracking)
- [RandomOrdering](../../../client-api/session/querying/how-to-customize-query#randomordering)
- [WaitForNonStaleResultsAsOf](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- [WaitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)

{PANEL:BeforeQueryExecuted}

Allows you to modify the index query just before it is executed.

{CODE customize_1_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | Action<[IndexQuery](../../../glossary/index-query)> | Action that will modify IndexQuery. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_1_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:AfterQueryExecuted}

Allows you to retrieve a raw query result after it's executed.

{CODE customize_1_0_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | Action<[QueryResult](../../../glossary/query-result)> | Action that has the query result. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_1_1_0@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:AfterStreamExecuted}

Allows you to retrieve a raw (blittable) result of the streaming query.

{CODE customize_1_0_1@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | Action<[BlittableJsonReaderObject](../../../glossary/blittable-json-reader-object)> | Action that has the single query result. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_1_1_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:NoCaching}

By default, queries are cached. To disable query caching use the `NoCaching` customization.

{CODE customize_2_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_2_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:NoTracking}

To disable entity tracking by `Session` use `NoTracking`. Usage of this option will prevent holding the query results in memory.

{CODE customize_3_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_3_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:RandomOrdering}

To order results randomly, use `RandomOrdering` method.

{CODE customize_4_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **seed** | string | Seed used for ordering. Useful when repeatable random queries are needed. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_4_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOf}

Queries can be 'instructed' to wait for non-stale results as of a cutoff Etag for a specified amount of time using the `WaitForNonStaleResultsAsOf` method. 
If the query won't be able to return non-stale results within the specified (or default) timeout, then a `TimeoutException` is thrown.

{CODE customize_9_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **cutOffEtag** | long | Minimum Etag of last indexed document. If last indexed document Etag is greater than this value, the results are considered non-stale. |
| **waitTimeout** | TimeSpan | Time to wait for an index to return non-stale results. The default is 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

{PANEL/}

{PANEL:WaitForNonStaleResults}

{NOTE This method should be used only for testing purposes and are considered **EXPERT ONLY** /}

Queries can be 'instructed' to wait for non-stale results for a specified amount of time using the `WaitForNonStaleResults` method. It is not advised to use this method on a live production
database since the high load might cause the index never to become non-stale. The preferred usage is `WaitForNonStaleResultsAsOf` where the cutoff is specified.

{CODE customize_8_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan | Time to wait for an index to return non-stale results. The default is 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_8_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}
