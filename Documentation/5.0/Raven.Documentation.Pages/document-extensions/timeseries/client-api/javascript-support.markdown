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
     * [timeseries.append - Append Untagged Entry](../../../document-extensions/timeseries/client-api/javascript-support#section-1)  
     * [timeseries.append - Append Tagged Entry](../../../document-extensions/timeseries/client-api/javascript-support#section-2)  
     * [timeseries.remove - Remove Entries Range](../../../document-extensions/timeseries/client-api/javascript-support#section-3)  
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
       |:---:|:---:|:---:|
       | doc | `string` | Document ID  
       | name | `string` | Time Series Name  

---

#### `timeseries.append (timestamp, values)`  

Use this method to append time series entries to a document.   

| Parameter | Type | Explanation
|:---:|:---:|:---:|
| timestamp | `DateTime` | Timestamp 
| values | `double[]` | Values 

---

#### `timeseries.append (timestamp, values, tag)`

Use this method to append to a document time series entries with tags.  

| Parameter | Type | Explanation 
|:---:|:---:|:---:|
| timestamp | `DateTime` | Timestamp 
| values | `double[]` | Values 
| tag | `string` | Tag 

---

#### `timeseries.remove (from, to)`  

Use this method to remove a range of entries from a document.  

| Parameter | Type | Explanation 
|:---:|:---:|:---:|
| from | `DateTime` | Range Start 
| to | `DateTime` | Range End 

---

#### `timeseries.get (from, to)`  

Use this method to retrieve a range of time series entries.  

| Parameter | Type | Explanation 
|:---:|:---:|:---:|
| from | `DateTime` | Range Start 
| to | `DateTime` | Range End 

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we pass [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session/patch) 
  a script that appends a document 100 time series entries.  
  {CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}


* In this sample, we pass [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation) 
  a script that runs a document query and removes the HeartRate time series from 
  matching documents.  
   {CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
