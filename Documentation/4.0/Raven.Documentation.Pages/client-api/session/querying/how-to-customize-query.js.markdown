# Session: Querying: How to Customize a Query

The following query customization options are available:

- ["beforeQueryExecuted" event](../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- ["afterQueryExecuted" event](../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- ["afterStreamExecuted" event](../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [noCaching()](../../../client-api/session/querying/how-to-customize-query#nocaching)
- [noTracking()](../../../client-api/session/querying/how-to-customize-query#notracking)
- [randomOrdering()](../../../client-api/session/querying/how-to-customize-query#randomordering)
- [waitForNonStaleResults()](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)

Query is an `EventEmitter`. It emits few events allowing you to customize its behavior.

{PANEL:BeforeQueryExecuted}

Allows you to modify the index query just before it's executed.

{CODE:nodejs customize_1_0@client-api\session\querying\howToCustomize.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | function | Action on the index query |

| Return Value | |
| ------------- | ----- |
| query | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_1_1@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:AfterQueryExecuted}

Allows you to retrieve a raw query result after it's executed.

{CODE:nodejs customize_1_0_0@client-api\session\querying\howToCustomize.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | function | Action on the query result |

| Return Value | |
| ------------- | ----- |
| query | Returns self for easier method chaining |

### Example

{CODE:nodejs customize_1_1_0@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:AfterStreamExecuted}

Allows you to retrieve a raw result of the streaming query.

{CODE:nodejs customize_1_0_1@client-api\session\querying\howToCustomize.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | function | Action on a single query result |

| Return Value | |
| ------------- | ----- |
| query | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_1_1_1@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:NoCaching}

By default, queries are cached. To disable query caching use the `noCaching()` customization.

{CODE:nodejs customize_2_0@client-api\session\querying\howToCustomize.js /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_2_1@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:NoTracking}

To disable entity tracking by `session` use `noTracking()`. Usage of this option will prevent holding the query results in memory.

{CODE:nodejs customize_3_0@client-api\session\querying\howToCustomize.js /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_3_1@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:RandomOrdering}

To order results randomly, use the `randomOrdering()` method.

{CODE:nodejs customize_4_0@client-api\session\querying\howToCustomize.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **seed** | string | Seed used for ordering. Useful when repeatable random queries are needed. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_4_1@client-api\session\querying\howToCustomize.js /}

{PANEL/}

{PANEL:WaitForNonStaleResults}

Queries can be 'instructed' to wait for non-stale results for a specified amount of time using the `waitForNonStaleResults()` method. If the query won't be able to return 
non-stale results within the specified (or default) timeout, then a `TimeoutException` is thrown.

{NOTE: Cutoff Point}
If a query sent to the server specifies that it needs to wait for non-stale results, then RavenDB sets the cutoff Etag for the staleness check.
It is the Etag of the last document (or document tombstone), from the collection(s) processed by the index, as of the query arrived to the server.
This way the server won't be waiting forever for the non-stale results even though documents are constantly updated meanwhile.

If the last Etag processed by the index is greater than the cutoff then the results are considered as non-stale.
{NOTE/}


{CODE:nodejs customize_8_0@client-api\session\querying\howToCustomize.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | number | Time to wait for an index to return non-stale results. The default is 15 seconds. |

| Return Value | |
| ------------- | ----- |
| query | Returns self for easier method chaining. |

### Example

{CODE:nodejs customize_8_1@client-api\session\querying\howToCustomize.js /}

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
