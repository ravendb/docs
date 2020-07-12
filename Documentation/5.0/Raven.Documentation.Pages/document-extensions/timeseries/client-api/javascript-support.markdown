# Time Series: JavaScript Support

{NOTE: }

With the introduction of time series, RavenDB's support for the execution 
of [JavaScript-based operations](../../../server/kb/JavaScript-engine) 
over [single](../../../client-api/operations/patching/single-document#patching-how-to-perform-single-document-patch-operations) 
and [multiple](../../../client-api/operations/patching/set-based) 
documents has been extended to allow manipulations involving time series.  

* Methods that gain time series functionality this way are:  
  * [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session/patch) - 
    `session` patching method  
  * [PatchOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchoperation) - 
    `store` patching operation  
  * [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation) - 
    `store` patch-by-query operation  

* In this page:  
  * [JavaScript API methods](../../../document-extensions/timeseries/client-api/javascript-support#javascript-api-methods)  
     * [timeseries - Choose a Time Series](../../../document-extensions/timeseries/client-api/javascript-support#section)  
     * [timeseries.append - Append an Entry](../../../document-extensions/timeseries/client-api/javascript-support#section-1)  
     * [timeseries.delete - Delete Entries Range](../../../document-extensions/timeseries/client-api/javascript-support#section-3)  
     * [timeseries.get - Get Entries Range](../../../document-extensions/timeseries/client-api/javascript-support#section-4)  
  * [Usage Samples](../../../document-extensions/timeseries/client-api/javascript-support#usage-samples)  

{NOTE/}

---

{PANEL: JavaScript API methods}

The JavaScript time series API includes these methods:  

---

#### `timeseries (doc, name)`  

Choose a time series by the ID of its owner document and by the series name.  

       | Parameter | Type | Explanation 
       |:---:|:---:| --- |
       | doc | `string` <br> or <br> `document instance` | Document ID, e.g. `timeseries('users/1-A', 'StockPrice')` <br> <br> e.g. `timeseries(this, 'StockPrice')`  
       | name | `string` | Time Series Name  

---

#### `timeseries.append`  

* You can use two overloads, to append **tagged** or **untagged** time series entries.  
   * `timeseries.append (timestamp, values)`  
   * `timeseries.append (timestamp, values, tag)`

* Parameters:  

      | Parameter | Type | Explanation
      |:---:|:---:| --- |
      | timestamp | `DateTime` | Timestamp 
      | values | `double[]` | Values 
      | tag | `string` | Tag 

---

#### `timeseries.delete (from, to)`  

Use this method to delete a range of entries from a document.  

| Parameter | Type | Explanation 
|:---:|:---:| --- |
| from (optional) | `DateTime` | Range Start <br> Default: `DateTime.Min` 
| to (optional) | `DateTime` | Range End <br> Default: `DateTime.Max` 

---

#### `timeseries.get (from, to)`  

Use this method to retrieve a range of time series entries.  

| Parameter | Type | Explanation 
|:---:|:---:| --- |
| from (optional) | `DateTime` | Range Start <br> Default: `DateTime.Min` 
| to (optional) | `DateTime` | Range End <br> Default: `DateTime.Max` 

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we pass [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session/patch) 
  a script that appends a document 100 time series entries.  
  {CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}


* In this sample, we pass [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation) 
  a script that runs a document query and deletes the HeartRate time series from matching documents.  
   {CODE TS_region-PatchByQueryOperation-Delete-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles

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
