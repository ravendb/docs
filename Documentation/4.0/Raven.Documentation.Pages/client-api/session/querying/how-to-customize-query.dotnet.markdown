# Session: Querying: How to Customize a Query

The following query customization options are available in the `IDocumentQueryCustomization` interface:

- [BeforeQueryExecuted](../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- [AfterQueryExecuted](../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- [AfterStreamExecuted](../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching)
- [NoTracking](../../../client-api/session/querying/how-to-customize-query#notracking)
- [RandomOrdering](../../../client-api/session/querying/how-to-customize-query#randomordering)
- [WaitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)

{PANEL:BeforeQueryExecuted}

Allows you to modify the index query just before it's executed.

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

To order results randomly, use the `RandomOrdering` method.

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

{PANEL:WaitForNonStaleResults}

Queries can be 'instructed' to wait for non-stale results for a specified amount of time using the `WaitForNonStaleResults` method. If the query won't be able to return 
non-stale results within the specified (or default) timeout, then a `TimeoutException` is thrown.

{NOTE: Cutoff Point}
If a query sent to the server specifies that it needs to wait for non-stale results, then RavenDB sets the cutoff Etag for the staleness check.
It is the Etag of the last document (or document tombstone), from the collection(s) processed by the index, as of the query arrived to the server.
This way the server won't be waiting forever for the non-stale results even though documents are constantly updated meanwhile.

If the last Etag processed by the index is greater than the cutoff then the results are considered as non-stale.
{NOTE/}


{CODE customize_8_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan? | Time to wait for an index to return non-stale results. The default is 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_8_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Subscribe to Events](../../../client-api/session/how-to/subscribe-to-events)

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [Querying](../../../client-api/configuration/querying)

### Indexes

- [Stale Indexes](../../../indexes/stale-indexes)  
