# Time Series: JavaScript Support
---

{NOTE: }

* With the introduction of time series, RavenDB has extended its [JavaScript support](../../../server/kb/JavaScript-engine)
  to include manipulations involving time series data when patching [single](../../../client-api/operations/patching/single-document#patching-how-to-perform-single-document-patch-operations)
  or [multiple](../../../client-api/operations/patching/set-based) documents.

* Time series capabilities can be achieved via JavaScript when using the following methods:
  * [session.advanced.defer](../../../document-extensions/timeseries/client-api/session/patch) - perform patch via the _session_
  * [PatchOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchoperation) - perform patch via a _store_ operation
  * [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation) - perform query & patch via a _store_ operation

* The server treats timestamps passed in the scripts as **UTC**, no conversion is applied by the client to local time.

* In this page:  
  * [JavaScript time series API methods](../../../document-extensions/timeseries/client-api/javascript-support#javascript-time-series-api-methods)  
     * [timeseries - choose a time series](../../../document-extensions/timeseries/client-api/javascript-support#section)  
     * [timeseries.append - append an entry](../../../document-extensions/timeseries/client-api/javascript-support#section-1)  
     * [timeseries.delete - delete entries](../../../document-extensions/timeseries/client-api/javascript-support#section-2)  
     * [timeseries.get - get entries](../../../document-extensions/timeseries/client-api/javascript-support#section-3)  
  * [Examples](../../../document-extensions/timeseries/client-api/javascript-support#examples)  

{NOTE/}

---

{PANEL: JavaScript time series API methods}

The JavaScript time series API includes these methods:  

---

#### `timeseries (doc, name)`  

Choose a time series by the ID of its owner document and by the series name.  

| Parameter | Type                                      | Description                                                                                              |
|-----------|-------------------------------------------|----------------------------------------------------------------------------------------------------------|
| **doc**   | `string` <br> or <br> `document instance` | Document ID, e.g. `timeseries('users/1-A', 'StockPrice')` <br><br> e.g. `timeseries(this, 'StockPrice')` |
| **name**  | `string`                                  | Time Series Name                                                                                         |

#### `timeseries.append`  

You can use two overloads, to append **tagged** or **untagged** time series entries.

* `timeseries.append (timestamp, values)`     
* `timeseries.append (timestamp, values, tag)`

| Parameter     | Type       | Description  |
|---------------|------------|--------------|
| **timestamp** | `Date`     | Timestamp    |
| **values**    | `number[]` | Values       |
| **tag**       | `string`   | Tag          |

#### `timeseries.delete (from, to)`  

Use this method to delete a range of entries from a document.  

| Parameter           | Type     | Description                                                                                         |
|---------------------|----------|-----------------------------------------------------------------------------------------------------|
| **from** (optional) | `Date`   | Entries will be deleted starting at this timestamp (inclusive).<br>Default: the minimum date value. |
| **to** (optional)   | `Date`   | Entries will be deleted up to this timestamp (inclusive).<br>Default: the maximum date value.       |

#### `timeseries.get (from, to)`  

Use this method to retrieve a range of time series entries.  

| Parameter           | Type     | Description                                                                                            |
|---------------------|----------|--------------------------------------------------------------------------------------------------------|
| **from** (optional) | `Date`   | Get time series entries starting from this timestamp (inclusive).<br> Default: The minimum date value. |
| **to** (optional)   | `Date`   | Get time series entries ending at this timestamp (inclusive).<br> Default: The maximum date value.     |

**Return Type**:  
Values are returned in an array of time series entries, i.e. -

{CODE-BLOCK:json}
[
	{
		"Timestamp" : ...
		"Tag": ...
		"Values": ...
		"IsRollup": ...
	},
	{
		"Timestamp" : ...
		"Tag": ...
		"Values": ...
		"IsRollup": ...
	}
	...
]
{CODE-BLOCK/}

{PANEL/}

{PANEL: Examples}

* This example shows a script that appends 100 entries to time series "HeartRates" in document "Users/john".  
  The script is passed to method [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session/patch).
  {CODE:nodejs js_support_1@documentExtensions\timeSeries\client-api\javascriptSupport.js /}

* This example shows a script that deletes time series "HeartRates" for documents that match the specified query.
  The script is passed to the [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation)  operation.
  {CODE:nodejs js_support_2@documentExtensions\timeSeries\client-api\javascriptSupport.js /}

{PANEL/}

## Related articles

**Javascript**  
[Knowledge Base: JavaScript Engine](../../../server/kb/javascript-engine)  

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Patching Time Series**  
[Patching in a Session](../../../document-extensions/timeseries/client-api/session/patch)  
[Patching Operation](../../../document-extensions/timeseries/client-api/operations/patch#patchoperation)  
[Patch By Query Operation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation)  

**Querying and Indexing**  
[Time Series Querying](../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
