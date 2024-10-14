# Customize Query

---

{NOTE: }

* Use `Customize()` to set the following customization options on a specific [Query](../../../client-api/session/querying/how-to-query).   
  Can be set for both **dynamic** and **index** queries.

* Each such customization can also be implemented via the [DocumentQuery](../../../client-api/session/querying/document-query/what-is-document-query) API.

* A query can also be customized on the Store or Session level by subscribing to `OnBeforeQuery`.  
  Learn more in [Subscribing to Events](../../../client-api/session/how-to/subscribe-to-events). 

* Customization methods available:

  - [BeforeQueryExecuted](../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
  - [AfterQueryExecuted](../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
  - [AfterStreamExecuted](../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
  - [NoCaching](../../../client-api/session/querying/how-to-customize-query#nocaching)
  - [NoTracking](../../../client-api/session/querying/how-to-customize-query#notracking)
  - [Projection](../../../client-api/session/querying/how-to-customize-query#projection)
  - [RandomOrdering](../../../client-api/session/querying/how-to-customize-query#randomordering)
  - [Timings](../../../client-api/session/querying/how-to-customize-query#timings)
  - [WaitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)

* [Methods return value](../../../client-api/session/querying/how-to-customize-query#methods-return-value)


{NOTE/}

---

{PANEL: BeforeQueryExecuted}

* Use `BeforeQueryExecuted` to customize the query just before it is executed.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_1_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_1_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_1_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_1_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_1_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description                                                                                                                     |
|------------| ---- |---------------------------------------------------------------------------------------------------------------------------------|
| **action** | `Action<IndexQuery>` | An _Action_ method that operates on the query.<br>The query is passed in the [IndexQuery](../../../glossary/index-query) param. |

{NOTE/}

{PANEL/}

{PANEL: AfterQueryExecuted}

* Use `AfterQueryExecuted` to access the raw query result after it is executed.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_2_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_2_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_2_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_2_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_2_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description                                                                                                                 |
|------------| ---- |-----------------------------------------------------------------------------------------------------------------------------|
| **action** | `Action<QueryResult>` | An _Action_ method that receives the raw query result.<br>The query result is passed in the [QueryResult](../../../glossary/query-result) param. |

{NOTE/}

{PANEL/}

{PANEL: AfterStreamExecuted}

* Use `AfterStreamExecuted` to retrieve a raw (blittable) result of the streaming query.

* Learn more in [how to stream query results](../../../client-api/session/querying/how-to-stream-query-results).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_3_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_3_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_3_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_3_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_3_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description                                                                                                                                                                                  |
|------------| ---- |----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **action** | `Action<BlittableJsonReaderObject>` | An _Action_ method that recieves a single stream query result.<br>The stream result is passed in the [BlittableJsonReaderObject](../../../glossary/blittable-json-reader-object) param. |

{NOTE/}

{PANEL/}

{PANEL: NoCaching}

* By default, query results are cached. 

* You can use the `NoCaching` customization to disable query caching.

* Learn more in [disable caching per session](../../../client-api/session/configuration/how-to-disable-caching).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_4_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_4_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_4_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_4_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_4_5@ClientApi\Session\Querying\HowToCustomize.cs /}

{NOTE/}

{PANEL/}

{PANEL: NoTracking}

* By default, the [Session](../../../client-api/session/what-is-a-session-and-how-does-it-work) tracks all changes made to all entities that it has either loaded, stored, or queried for.

* You can use the `NoTracking` customization to disable entity tracking.  

* See [disable entity tracking](../../../client-api/session/configuration/how-to-disable-tracking) for all other options.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_5_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_5_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_5_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_5_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_5_5@ClientApi\Session\Querying\HowToCustomize.cs /}

{NOTE/}

{PANEL/}

{PANEL: Projection}

* By default, when [querying an index](../../../indexes/querying/query-index) and projecting query results,  
  the server will try to retrieve field values from the fields [stored in the index](../../../indexes/storing-data-in-index).  
  {NOTE: }
  Projecting means the query returns only specific document fields instead of the full document.  
  {NOTE/}

* If the index does Not store these fields, the field values will be retrieved from the documents.

* Use the `Projection` method to customize and modify this behavior.

* Note:  
  Entities resulting from a projecting query are Not tracked by the session.  
  Learn more about projections in:
  
  * [Project index query results](../../../indexes/querying/projections)
  * [Project dynamic query results](../../../client-api/session/querying/how-to-project-query-results)

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_6_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_6_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_6_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_6_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Index the_index@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

In the above example:  

  * Field _'FullName'_ is stored in the index (see index definition in the rightmost tab).  
    However, the server will try to fetch the value from the document since the default behavior was modified to `FromDocumentOrThrow`.  
  * An exception will be thrown since an _'Employee'_ document does not contain the property _'FullName'_.  
    (based on the Northwind sample data).  

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_6_5@ClientApi\Session\Querying\HowToCustomize.cs /}

* `Default`  
  Retrieve values from the stored index fields when available.  
  If fields are not stored then get values from the document,  
  a field that is not found in the document is skipped.
* `FromIndex`  
  Retrieve values from the stored index fields when available.  
  A field that is not stored in the index is skipped.  
* `FromIndexOrThrow`  
  Retrieve values from the stored index fields when available.  
  An exception is thrown if the index does not store the requested field.  
* `FromDocument`  
  Retrieve values directly from the documents store.  
  A field that is not found in the document is skipped.  
* `FromDocumentOrThrow`  
  Retrieve values directly from the documents store.  
  An exception is thrown if the document does not contain the requested field.  

{NOTE/}

{PANEL/}

{PANEL: RandomOrdering}

* Use `RandomOrdering` to order the query results randomly.  

* More ordering options are available in this [Sorting](../../../client-api/session/querying/sort-query-results) article.  

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_7_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_7_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_7_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_7_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_7_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description                                                                                              |
|------------| ------------- |-------------------------------------------------------------------------------------------------|
| **seed**   | `string` | Order the search results randomly using this seed.<br>Useful when executing repeated random queries. |

{NOTE/}

{PANEL/}

{PANEL: Timings}

* Use `Timings` to get detailed stats of the time spent by the server on each part of the query.

* The timing statistics will be included in the query results.

* Learn more in [how to include query timings](../../../client-api/session/querying/debugging/query-timings).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_9_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_9_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_9_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_9_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_9_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description |
|------------| ------------- | ----- |
| **timings** | `QueryTimings` | An out param that will be filled with the timings results |

{NOTE/}

{PANEL/}

{PANEL: WaitForNonStaleResults}

* All queries in RavenDB provide results using an index, even when you don't specify one.  
  See detailed explanation in [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* Use `WaitForNonStaleResults` to instruct the query to wait for non-stale results from the index.  

* A `TimeoutException` will be thrown if the query is not able to return non-stale results within the specified  
  (or default) timeout.
 
* Note: This feature is Not available when [streaming the query results](../../../client-api/session/querying/how-to-stream-query-results).  
  Calling _WaitForNonStaleResults_ with a streaming query will throw an exception.  

* Learn more about stale results in [stale indexes](../../../indexes/stale-indexes).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Query customize_8_1@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:Query_async customize_8_2@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:DocumentQuery customize_8_3@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TAB:csharp:RawQuery customize_8_4@ClientApi\Session\Querying\HowToCustomize.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE customize_8_5@ClientApi\Session\Querying\HowToCustomize.cs /}

| Parameters | Type | Description |
|------------| ------------- |-----------|
| **waitTimeout** | `TimeSpan?` | Time to wait for non-stale results. <br>Default is 15 seconds. |

{NOTE/}

{PANEL/}

{PANEL: Methods return value}

All of the above customization methods return the following:

| `Query` return value         | |
|-----------------------------| ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |

| `DocumentQuery` return value      | |
|---------------------------------| ----- |
| IQueryBase | Returns self for easier method chaining. |

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
