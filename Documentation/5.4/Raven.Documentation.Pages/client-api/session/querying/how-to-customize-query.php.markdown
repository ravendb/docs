# Customize Query

---

{NOTE: }

* Use the below methods to set customization options on a specific [query](../../../client-api/session/querying/how-to-query).  
  Customizations can be set for both **dynamic** and **index** queries.

* A query can also be customized on the store or session level by 
  [subscribing to `OnBeforeQuery`](../../../client-api/session/how-to/subscribe-to-events). 

* Available customization methods :
  - [`addBeforeQueryExecutedListener` and `removeBeforeQueryExecutedListener`](../../../client-api/session/querying/how-to-customize-query#addbeforequeryexecutedlistener-and-removebeforequeryexecutedlistener)
  - [`addAfterQueryExecutedListener` and `removeAfterQueryExecutedListener`](../../../client-api/session/querying/how-to-customize-query#addafterqueryexecutedlistener-and-removeafterqueryexecutedlistener)
  - [`noCaching`](../../../client-api/session/querying/how-to-customize-query#nocaching)
  - [`noTracking`](../../../client-api/session/querying/how-to-customize-query#notracking)
  - [Projection](../../../client-api/session/querying/how-to-customize-query#randomordering)
  - [`randomOrdering`](../../../client-api/session/querying/how-to-customize-query#randomordering)
  - [`waitForNonStaleResults`](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
* [Methods return value](../../../client-api/session/querying/how-to-customize-query#methods-return-value)


{NOTE/}

---

{PANEL: `addBeforeQueryExecutedListener` and `removeBeforeQueryExecutedListener`}

* Use these methods to customize the query just before it is executed.

**Example**

{CODE:php customize_1_1@ClientApi\Session\Querying\HowToCustomize.php /}

{NOTE: }

**Syntax**

{CODE:php customize_1_0@ClientApi\Session\Querying\HowToCustomize.php /}

| Parameters | Type | Description                                          |
|------------| ---- |------------------------------------------------------|
| **$action** | `Closure` | An _$action_ method that operates on the query |

{NOTE/}

{PANEL/}

{PANEL: `addAfterQueryExecutedListener` and `removeAfterQueryExecutedListener`}

* Use these methods to access the raw query result after it is executed.

**Example**

{CODE:php customize_1_1_0@ClientApi\Session\Querying\HowToCustomize.php /}

{NOTE: }

**Syntax**

{CODE:php customize_1_0_0@ClientApi\Session\Querying\HowToCustomize.php /}

| Parameters | Type | Description |
|------------| ---- |-------------|
| **$action** | `Closure` | An _Action_ method that receives the raw query result |

{NOTE/}

{PANEL/}

{PANEL: `noCaching`}

* By default, query results are cached. 

* You can use the `noCaching` customization to disable query caching.

* Learn more in [disable caching per session](../../../client-api/session/configuration/how-to-disable-caching).

**Example**

{CODE:php customize_2_1@ClientApi\Session\Querying\HowToCustomize.php /}

**Syntax**

{CODE:php customize_2_0@ClientApi\Session\Querying\HowToCustomize.php /}

{PANEL/}

{PANEL: `noTracking`}

* By default, the [session](../../../client-api/session/what-is-a-session-and-how-does-it-work) tracks all changes made to all entities that it has either loaded, stored, or queried for.

* You can use the `noTracking` customization to disable entity tracking.  

* See [disable entity tracking](../../../client-api/session/configuration/how-to-disable-tracking) for all other options.

**Example**

{CODE:php customize_3_1@ClientApi\Session\Querying\HowToCustomize.php /}

**Syntax**

{CODE:php customize_3_0@ClientApi\Session\Querying\HowToCustomize.php /}

{PANEL/}

{PANEL: Projection}

* By default, when [querying an index](../../../indexes/querying/query-index) and projecting query results,  
  the server will try to retrieve field values from the fields [stored in the index](../../../indexes/storing-data-in-index).  
  {NOTE: }
  Projecting means the query returns only specific document fields instead of the full document.  
  {NOTE/}

* If the index does Not store these fields, the field values will be retrieved from the documents.

* Use the `projection` method to customize and modify this behavior.

* Note:  
  Entities resulting from a projecting query are Not tracked by the session.  
  Learn more about projections in:
  
  * [Project index query results](../../../indexes/querying/projections)
  * [Project dynamic query results](../../../client-api/session/querying/how-to-project-query-results)

**Example**

{CODE:php projectionbehavior_query@ClientApi\Session\Querying\HowToCustomize.php /}

**Syntax**

{CODE:php projectionbehavior@ClientApi\Session\Querying\HowToCustomize.php /}

* `default`  
  Retrieve values from the stored index fields when available.  
  If fields are not stored then get values from the document,  
  a field that is not found in the document is skipped.
* `fromIndex`  
  Retrieve values from the stored index fields when available.  
  A field that is not stored in the index is skipped.  
* `fromIndexOrThrow`  
  Retrieve values from the stored index fields when available.  
  An exception is thrown if the index does not store the requested field.  
* `fromDocument`  
  Retrieve values directly from the documents store.  
  A field that is not found in the document is skipped.  
* `fromDocumentOrThrow`  
  Retrieve values directly from the documents store.  
  An exception is thrown if the document does not contain the requested field.  

{PANEL/}

{PANEL: `randomOrdering`}

* Use `randomOrdering` to order the query results randomly.  

* More ordering options are available in this [Sorting](../../../client-api/session/querying/sort-query-results) article.  

**Example**

{CODE:php customize_4_1@ClientApi\Session\Querying\HowToCustomize.php /}

**Syntax**

{CODE:php customize_4_0@ClientApi\Session\Querying\HowToCustomize.php /}

| Parameters | Type | Description                                                                                                |
|------------| ------------- |---------------------------------------------------------------------------------------------------|
| **$seed**   | `?string` | Order the search results randomly using this seed.<br>Useful when executing repeated random queries. |

{PANEL/}

{PANEL: `waitForNonStaleResults`}

* All queries in RavenDB provide results using an index, even when you don't specify one.  
  See detailed explanation in [Queries always provide results using an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

* Use `waitForNonStaleResults` to instruct the query to wait for non-stale results from the index.  

* A `TimeoutException` will be thrown if the query is not able to return non-stale results within the specified  
  (or default) timeout.
 
* Learn more about stale results in [stale indexes](../../../indexes/stale-indexes).

{NOTE: }

**Example**

{CODE:php customize_8_1@ClientApi\Session\Querying\HowToCustomize.php /}

{NOTE/}

{NOTE: }

**Syntax**

{CODE:php customize_8_0@ClientApi\Session\Querying\HowToCustomize.php /}

| Parameters | Type | Description |
|------------| ------------- |-----------|
| **$waitTimeout** | `Duration` | Time to wait for non-stale results. <br>Default is 15 seconds. |

{NOTE/}

{PANEL/}

{PANEL: Methods return value}

All of the above customization methods return the following:

| Return value         | |
|-----------------------------| ----- |
| `DocumentQueryCustomizationInterface` | Returns self for easier method chaining |

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
