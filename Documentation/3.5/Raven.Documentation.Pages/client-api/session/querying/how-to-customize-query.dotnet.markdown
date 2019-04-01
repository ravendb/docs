# Session: Querying: How to customize query?

Following query customization options are available in `IDocumentQueryCustomization` interface:

- [BeforeQueryExecution](../../../client-api/session/querying/how-to-customize-query#beforequeryexecution)
- [CustomSortUsing](../../../client-api/session/querying/how-to-customize-query#customsortusing)
- [Highlight](../../../client-api/session/querying/how-to-customize-query#highlight)
- [Include](../../../client-api/session/querying/how-to-customize-query#include)
- [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching)
- [NoTracking](../../../client-api/session/querying/how-to-customize-query#notracking)
- [RandomOrdering](../../../client-api/session/querying/how-to-customize-query#randomordering)
- [RelatesToShape](../../../client-api/session/querying/how-to-customize-query#relatestoshape)
- [SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer](../../../client-api/session/querying/how-to-customize-query#setallowmultipleindexentriesforsamedocumenttoresulttransformer)
- [SetHighlighterTags](../../../client-api/session/querying/how-to-customize-query#sethighlightertags)
- [ShowTimings](../../../client-api/session/querying/how-to-customize-query#showtimings)
- [SortByDistance](../../../client-api/session/querying/how-to-customize-query#sortbydistance)
- [Spatial](../../../client-api/session/querying/how-to-customize-query#spatial)
- [TransformResults](../../../client-api/session/querying/how-to-customize-query#transformresults)
- [WaitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- [WaitForNonStaleResultsAsOf](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasof)
- [WaitForNonStaleResultsAsOfLastWrite](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasoflastwrite)
- [WaitForNonStaleResultsAsOfNow](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresultsasofnow)
- [WithinRadiusOf](../../../client-api/session/querying/how-to-customize-query#withinradiusof)

{PANEL:BeforeQueryExecution}

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

{PANEL:CustomSortUsing}

Allows you to use custom sorter on the server. Dedicated article can be found [here](../../../indexes/querying/sorting#custom-sorting).

{CODE customize_12_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **typeName** | string | AssemblyQualifiedName of a custom sorter available on server-side. |
| **descending** | bool | indicates if results should be ordered descending or ascending |

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

By default, queries are cached. To disable query caching use `NoCaching` customization.

{CODE customize_2_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_2_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:NoTracking}

To disable entity tracking by `Session` use `NoTracking`. Usage of this option will prevent holding query results in memory.

{CODE customize_3_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_3_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:RandomOrdering}

To order results randomly use `RandomOrdering` method.

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

{PANEL:RelatesToShape}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}

{PANEL:SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer}

If set to true, multiple index entries will be send from the same document (assuming the index project them) to the result transformer function. Otherwise, those entries will be consolidate an the transformer will be called just once for each document in the result set.

{CODE customize_5_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **val** | bool | Indicates if multiple index entries can be send from the same document to result transformer. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_5_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:SetHighlighterTags}

Please check our dedicated [article](../../../client-api/session/querying/how-to-use-highlighting).

{PANEL/}

{PANEL:ShowTimings}

By default, detailed timings (duration of Lucene search, loading documents, transforming results) in queries are turned off, this is due to small overhead that calculation of such timings produces.

{CODE customize_6_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

Returned timings:

- Query parsing
- Lucene search
- Loading documents
- Transforming results

### Example

{CODE customize_6_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:SortByDistance}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

## Spatial

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}

{PANEL:TransformResults}

`TransformResults` is a **client-side** function that allows any transformations on query results.

{CODE customize_7_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **resultsTransformer** | Func<IndexQuery, IEnumerable&lt;object&gt;, IEnumerable&lt;object&gt;> | Transformation function. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_7_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:WaitForNonStaleResults}

{NOTE This methods should be used only for testing purposes and are considered **EXPERT ONLY** /}

Queries can be 'instructed' to wait for non-stale results for specified amount of time using `WaitForNonStaleResults` method. It is not advised to use this method, because on live databases indexes might never become unstale.

{CODE customize_8_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan | Time to wait for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_8_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOf}

Queries can be 'instructed' to wait for non-stale results as of cutoff DateTime or Etag for specified amount of time using `WaitForNonStaleResultsAsOf` method.

{CODE customize_9_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **cutOff** | DateTime | Minimum modified date of last indexed document. If last indexed document modified date is greater than this value the results are considered non-stale. |
| **cutOffEtag** | Etag | Minimum Etag of last indexed document. If last indexed document etag is greater than this value the results are considered non-stale. |
| **waitTimeout** | TimeSpan | Time to wait for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOfLastWrite}

Queries can be 'instructed' to wait for non-stale results as of last write made by any session belonging to the current DocumentStore using `WaitForNonStaleResultsAsOfLastWrite` method.

This method internally uses `WaitForNonStaleResultsAsOf` and passes last written Etag, which DocumentStore tracks, to `cutOffEtag` parameter.

{CODE customize_10_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan | Time to wait for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_10_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:WaitForNonStaleResultsAsOfNow}

`WaitForNonStaleResultsAsOfNow` internally uses `WaitForNonStaleResultsAsOf` and passes `DateTime.Now` to `cutOff` parameter.

{CODE customize_11_0@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **waitTimeout** | TimeSpan | Time to wait for index to return non-stale results. Default: 15 seconds. |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

### Example

{CODE customize_11_1@ClientApi\Session\Querying\HowToCustomize.cs /}

{PANEL/}

{PANEL:WithinRadiusOf}

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

{PANEL/}
