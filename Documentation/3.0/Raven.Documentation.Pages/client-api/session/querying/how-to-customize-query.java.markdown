# Session: Querying: How to customize query?

Following query customization options are available in `IDocumentQueryCustomization` interface:

- [beforeQueryExecution](../../../client-api/session/querying/how-to-customize-query#beforequeryexecution)
- [customSortUsing](../../../client-api/session/querying/how-to-customize-query#customsortusing)
- [highlight](../../../client-api/session/querying/how-to-customize-query#highlight)
- [include](../../../client-api/session/querying/how-to-customize-query#include)
- [noCaching](../../../client-api/session/querying/how-to-customize-query#nocaching)
- [noTracking](../../../client-api/session/querying/how-to-customize-query#notracking)
- [randomOrdering](../../../client-api/session/querying/how-to-customize-query#randomordering)
- [relatesToShape](../../../client-api/session/querying/how-to-customize-query#relatestoshape)
- [setAllowMultipleIndexEntriesForSameDocumentToResultTransformer](../../../client-api/session/querying/how-to-customize-query#setallowmultipleindexentriesforsamedocumenttoresulttransformer)
- [setHighlighterTags](../../../client-api/session/querying/how-to-customize-query#sethighlightertags)
- [showTimings](../../../client-api/session/querying/how-to-customize-query#showtimings)
- [sortByDistance](../../../client-api/session/querying/how-to-customize-query#sortbydistance)
- [spatial](../../../client-api/session/querying/how-to-customize-query#spatial)
- [transformResults](../../../client-api/session/querying/how-to-customize-query#transformresults)
- [waitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [waitForNonStaleResultsAsOf](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- [waitForNonStaleResultsAsOfLastWrite](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasoflastwrite)
- [waitForNonStaleResultsAsOfNow](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasofnow)
- [withinRadiusOf](../../../client-api/session/querying/how-to-customize-query#withinradiusof)

{PANEL:BeforeQueryExecution}

Allows you to modify the index query just before it is executed.

{CODE:java customize_1_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **action** | Action1<[IndexQuery](../../../glossary/index-query)> | Action that will modify IndexQuery. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_1_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:CustomSortUsing}

Allows you to use custom sorter on the server. Dedicated article can be found [here](../../../indexes/querying/sorting#custom-sorting).

{CODE:java customize_12_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **typeName** | String | AssemblyQualifiedName of a custom sorter available on server-side. |
| **descending** | boolean | indicates if results should be ordered descending or ascending |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

{PANEL/}

{PANEL:Highlight}

Please check our dedicated [article](../../../client-api/session/querying/how-to-use-highlighting).

{PANEL/}

{PANEL:Include}

Please check our dedicated [article](../../../indexes/querying/handling-document-relationships).

{PANEL/}

{PANEL:NoCaching}

By default, queries are cached. To disable query caching use `noCaching` customization.

{CODE:java customize_2_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_2_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:NoTracking}

To disable entity tracking by `session` use `noTracking`. Usage of this option will prevent holding query results in memory.

{CODE:java customize_3_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_3_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:RandomOrdering}

To order results randomly use `randomOrdering` method.

{CODE:java customize_4_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **seed** | String | Seed used for ordering. Useful when repeatable random queries are needed. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_4_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:RelatesToShape}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}

{PANEL:SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer}

If set to true, multiple index entries will be send from the same document (assuming the index project them) to the result transformer function. Otherwise, those entries will be consolidate an the transformer will be called just once for each document in the result set.

{CODE:java customize_5_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **val** | boolean | Indicates if multiple index entries can be send from the same document to result transformer. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_5_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:SetHighlighterTags}

Please check our dedicated [article](../../../client-api/session/querying/how-to-use-highlighting).

{PANEL/}

{PANEL:ShowTimings}

By default, detailed timings (duration of Lucene search, loading documents, transforming results) in queries are turned off, this is due to small overhead that calculation of such timings produces.

{CODE:java customize_6_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

Returned timings:

- Query parsing
- Lucene search
- Loading documents
- Transforming results

### Example

{CODE:java customize_6_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:SortByDistance}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}

{PANEL:Spatial}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}

{PANEL:WaitForNonStaleResults}

{NOTE This methods should be used only for testing purposes and are considered **EXPERT ONLY** /}

Queries can be 'instructed' to wait for non-stale results for specified amount of time using `waitForNonStaleResults` method. It is not advised to use this method, because on live databases indexes might never become unstale.

{CODE:java customize_8_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan | Time to wait (in miliseconds) for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_8_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOf}

Queries can be 'instructed' to wait for non-stale results as of cutoff DateTime or Etag for specified amount of time using `WaitForNonStaleResultsAsOf` method.

{CODE:java customize_9_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **cutOff** | DateTime | Minimum modified date of last indexed document. If last indexed document modified date is greater than this value the results are considered non-stale. |
| **cutOffEtag** | Etag | Minimum Etag of last indexed document. If last indexed document etag is greater than this value the results are considered non-stale. |
| **waitTimeout** | long | Time to wait (in miliseconds) for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOfLastWrite}

Queries can be 'instructed' to wait for non-stale results as of last write made by any session belonging to the current DocumentStore using `WaitForNonStaleResultsAsOfLastWrite` method.

This method internally uses `waitForNonStaleResultsAsOf` and passes last written Etag, which DocumentStore tracks, to `cutOffEtag` parameter.

{CODE:java customize_10_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | long | Time to wait (in miliseconds) for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_10_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOfNow}

`waitForNonStaleResultsAsOfNow` internally uses `waitForNonStaleResultsAsOf` and passes `new Date()` to `cutOff` parameter.

{CODE:java customize_11_0@ClientApi\Session\Querying\HowToCustomize.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | long | Time to wait (in miliseconds) for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE:java customize_11_1@ClientApi\Session\Querying\HowToCustomize.java /}

{PANEL/}

{PANEL:WithinRadiusOf}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}
