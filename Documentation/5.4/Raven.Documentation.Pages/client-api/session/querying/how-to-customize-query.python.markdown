# Customize Query

---

{NOTE: }

* Use the below customization methods over a specific [query](../../../client-api/session/querying/how-to-query).  

* The customization methods can be set for both **dynamic** and **index** queries.  

* A query can also be customized on the Store or Session level by by calling `add_before_query`.  
  Learn more in [Subscribing to Events](../../../client-api/session/how-to/subscribe-to-events). 

* Customization methods available:

  - [before_query_executed](../../../client-api/session/querying/how-to-customize-query#before_query_executed)
  - [after_query_executed](../../../client-api/session/querying/how-to-customize-query#after_query_executed)
  - [after_stream_executed](../../../client-api/session/querying/how-to-customize-query#after_stream_executed)
  - [no_caching](../../../client-api/session/querying/how-to-customize-query#no_caching)
  - [no_tracking](../../../client-api/session/querying/how-to-customize-query#no_tracking)
  - [projection](../../../client-api/session/querying/how-to-customize-query#projection)
  - [random_ordering](../../../client-api/session/querying/how-to-customize-query#random_ordering)
  - [timings](../../../client-api/session/querying/how-to-customize-query#timings)
  - [wait_for_non_stale_results](../../../client-api/session/querying/how-to-customize-query#wait_for_non_stale_results)

* [Methods return value](../../../client-api/session/querying/how-to-customize-query#methods-return-value)


{NOTE/}

---

{PANEL: `before_query_executed`}

* Use `before_query_executed` to customize the query just before it is executed.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_1_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_1_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_1_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description                                                                                                                     |
|------------| ---- |---------------------------------------------------------------------------------------------------------------------------------|
| **action** | `Callable[[IndexQuery], None]` | An _action_ method that operates on the query.<br>The query is passed in the [IndexQuery](../../../glossary/index-query) param. |

{NOTE/}

{PANEL/}

{PANEL: `after_query_executed`}

* Use `after_query_executed` to access the raw query result after it is executed.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_2_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_2_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_2_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description                                                                                                                 |
|------------| ---- |-----------------------------------------------------------------------------------------------------------------------------|
| **action** | `Callable[[QueryResult], None]` | An _action_ method that receives the raw query results.<br>The query result is passed in the [QueryResult](../../../glossary/query-result) param. |

{NOTE/}

{PANEL/}

{PANEL: `after_stream_executed`}

* Use `after_stream_executed` to retrieve a raw (blittable) result of the streaming query.

* Learn more in [how to stream query results](../../../client-api/session/querying/how-to-stream-query-results).

{NOTE: }

**Syntax**

{CODE:python customize_3_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description                                                                                                                                                                                  |
|------------| ---- |----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **action** | `Callable[[dict], None])` | An _action_ method that recieves a single stream query result.<br>The stream result is passed in the `dict` param. |

{NOTE/}

{PANEL/}

{PANEL: `no_caching`}

* By default, query results are cached. 

* You can use the `no_caching` customization method to disable query caching.

* Learn more in [disable caching per session](../../../client-api/session/configuration/how-to-disable-caching).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_4_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_4_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_4_5@ClientApi\Session\Querying\HowToCustomize.py /}

{NOTE/}

{PANEL/}

{PANEL: `no_tracking`}

* By default, the [Session](../../../client-api/session/what-is-a-session-and-how-does-it-work) tracks all changes made to all entities that it has either loaded, stored, or queried for.

* You can use the `no_tracking` customization to disable entity tracking.  

* See [disable entity tracking](../../../client-api/session/configuration/how-to-disable-tracking) for all other options.

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_5_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_5_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_5_5@ClientApi\Session\Querying\HowToCustomize.py /}

{NOTE/}

{PANEL/}

{PANEL: `projection`}

* By default, when [querying an index](../../../indexes/querying/query-index) and projecting query results,  
  the server will try to retrieve field values from the fields [stored in the index](../../../indexes/storing-data-in-index).  
  {NOTE: }
  Projecting means the query returns only specific document fields instead of the full document.  
  {NOTE/}

* If these fields are not stored in the index, the field values will be retrieved from the documents.  

* Use the `select_fields` method to customize and modify this behavior.  

* Note:  
  Entities resulting from a projecting query are Not tracked by the session.  
  Learn more about projections in:
    * [Project index query results](../../../indexes/querying/projections)
    * [Project dynamic query results](../../../client-api/session/querying/how-to-project-query-results)

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_6_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_6_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:Index the_index@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

In the above example:  

  * The _'full_name'_ field is stored in the index (see index definition in the rightmost tab).  
    However, the server will try to fetch the value from the document since the default behavior was modified to `FROM_DOCUMENT_OR_THROW`.  
  * An exception will be thrown since an _'Employee'_ document does not contain the _'full_name'_ property.  
    (based on the Northwind sample data).  

{NOTE/}


<!--
{NOTE: }

**Syntax**

{CODE:python customize_6_5@ClientApi\Session\Querying\HowToCustomize.py /}

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
-->

{PANEL/}

{PANEL: `random_ordering`}

* Use `random_ordering` to order the query results randomly.  

* Learn [here](../../../client-api/session/querying/sort-query-results) about additional ordering options.  

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_7_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_7_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_7_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description                                                                                              |
|------------| ------------- |-------------------------------------------------------------------------------------------------|
| **seed**   | `str` | Order the search results randomly using this seed.<br>Useful when executing repeated random queries. |

{NOTE/}

{PANEL/}

{PANEL: `timings`}

* When executing a query, you can retrieve query statistics that include the time spent by the server on each part of the query.  

* To do this, define a callback function that takes `QueryTimings` as an argument and applies whatever 
  logic you want to apply.  

* Then pass your function as an argument to the `query.timings` method and use the retrieved `QueryTimings` object.  

* Learn more in [how to include query timings](../../../client-api/session/querying/debugging/query-timings).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_9_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_9_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_9_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description |
|------------| ------------- | ----- |
| **timings_callback** | `Callable[[QueryTimings], None]` | An _action_ that will be called with the timings results |

{NOTE/}

{PANEL/}

{PANEL: `wait_for_non_stale_results`}

* All RavenDB queries provide results using an index, even when an index is not specified.  
  See detailed explanation in [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* If `wait_for_non_stale_results` is used, the query will wait for non-stale results from the index.  

* A `TimeoutException` will be thrown if the query is unable to return non-stale results within the specified  
  (or default) timeout.
 
* Note: This feature is Not available when [streaming the query results](../../../client-api/session/querying/how-to-stream-query-results).  
  Calling `wait_for_non_stale_results` with a streaming query will throw an exception.  

* Learn more about stale results in [stale indexes](../../../indexes/stale-indexes).

{NOTE: }

**Example**

{CODE-TABS}
{CODE-TAB:python:Query customize_8_1@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TAB:python:RawQuery customize_8_4@ClientApi\Session\Querying\HowToCustomize.py /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:python customize_8_5@ClientApi\Session\Querying\HowToCustomize.py /}

| Parameters | Type | Description |
|------------| ------------- |-----------|
| **wait_timeout** | `timedelta` | Time to wait for non-stale results.<br>Default: 15 seconds |

{NOTE/}

{PANEL/}

{PANEL: `Methods return value`}

All of the above customization methods return the following:

| `document_query` return value | |
|-------------------------------| ----- |
| `DocumentQuery[_T]` | Returns self for easier method chaining |

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
