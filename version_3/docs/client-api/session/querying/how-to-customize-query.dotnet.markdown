# Querying : How to customize query?

Following query customization options are available in `IDocumentQueryCustomization` interface:

- [BeforeQueryExecution](../../../client-api/session/querying/how-to-customize-query#beforequeryexecution)
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

## BeforeQueryExecution

Allows you to modify the index query just before it is executed.

{CODE customize_1_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**   

action
:   Type: Action<[IndexQuery]()>   
Action that will modify IndexQuery.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_1_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## Highlight

Please check our dedicated [article](../../../client-api/session/querying/how-to-use-highlighting).

## Include

Please check our dedicated [article]().

## NoCaching

By default, queries are cached. To disable query caching use `NoCaching` customization.

{CODE customize_2_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_2_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## NoTracking

To disable entity tracking by `Session` use `NoTracking`. Usage of this option will prevent holding query results in memory.

{CODE customize_3_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_3_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## RandomOrdering

To order results randomly use `RandomOrdering` method.

{CODE customize_4_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

seed
:   Type: string   
Seed used for ordering. Useful when repeatable random queries are needed.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_4_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## RelatesToShape

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

## SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer

If set to true, multiple index entries will be send from the same document (assuming the index project them) to the result transformer function. Otherwise, those entries will be consolidate an the transformer will be called just once for each document in the result set.

{CODE customize_5_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

val
:   Type: bool   
Indicates if multiple index entries can be send from the same document to result transformer.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_5_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## SetHighlighterTags

Please check our dedicated [article](../../../client-api/session/querying/how-to-use-highlighting).

## ShowTimings

By default, detailed timings (duration of Lucene search, loading documents, transforming results) in queries are turned off, this is due to small overhead that calculation of such timings produces.

{CODE customize_6_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_6_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## SortByDistance

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

## Spatial

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

## TransformResults

`TransformResults` is a **client-side** function that allows any transformations on query results.

{CODE customize_7_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

resultsTransformer
:   Type: Func<IndexQuery, IEnumerable&lt;object&gt;, IEnumerable&lt;object&gt;>   
Transformation function.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_7_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## WaitForNonStaleResults

{NOTE This methods should be used only for testing purposes and are considered **EXPERT ONLY** /}

Queries can be 'instructed' to wait for non-stale results for specified amount of time using `WaitForNonStaleResults` method. It is not advised to use this method, because on live databases indexes might never become unstale.

{CODE customize_8_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

waitTimeout
:   Type: TimeSpan   
Time to wait for index to return non-stale results. Default: 15 seconds.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_8_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## WaitForNonStaleResultsAsOf

Queries can be 'instructed' to wait for non-stale results as of cutoff DateTime or Etag for specified amount of time using `WaitForNonStaleResultsAsOf` method.

{CODE customize_9_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

cutOff
:   Type: DateTime   
Minimum modified date of last indexed document. If last indexed document modified date is greater than this value the results are considered non-stale.

cutOffEtag
:   Type: Etag   
Minimum Etag of last indexed document. If last indexed document etag is greater than this value the results are considered non-stale.

waitTimeout
:   Type: TimeSpan   
Time to wait for index to return non-stale results. Default: 15 seconds.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

## WaitForNonStaleResultsAsOfLastWrite

Queries can be 'instructed' to wait for non-stale results as of last write made by any session belonging to the current DocumentStore using `WaitForNonStaleResultsAsOfLastWrite` method.

This method internally uses `WaitForNonStaleResultsAsOf` and passes last written Etag, which DocumentStore tracks, to `cutOffEtag` parameter.

{CODE customize_10_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

waitTimeout
:   Type: TimeSpan   
Time to wait for index to return non-stale results. Default: 15 seconds.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_10_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## WaitForNonStaleResultsAsOfNow

`WaitForNonStaleResultsAsOfNow` internally uses `WaitForNonStaleResultsAsOf` and passes `DateTime.Now` to `cutOff` parameter.

{CODE customize_11_0@ClientApi\Session\Querying\HowToCustomize.cs /}

**Parameters**

waitTimeout
:   Type: TimeSpan   
Time to wait for index to return non-stale results. Default: 15 seconds.

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

### Example

{CODE customize_11_1@ClientApi\Session\Querying\HowToCustomize.cs /}

## WithinRadiusOf

Please check our dedicated [article](../../../client-api/session/querying/how-to-query-a-spatial-index).

#### Related articles

TODO