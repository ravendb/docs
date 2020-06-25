# Time Series & JavaScript

{NOTE: }

Some time series functions are provided not by dedicated time series methods, 
but by general-purpose methods that gain time series functionality by running 
a user-custom [JavaScript](../../../server/kb/javascript-engine) that calls 
time series API methods.  

* Methods that gain time series functionality this way are:  
  * [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data) - 
    `session` patching method  
  * [PatchOperation](../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchoperation) - 
    `store` patching operation  
  * [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchbyqueryoperation) - 
    `store` patch-by-query operation  

* In this page:  
  * [Time Series JS API methods](../../../document-extensions/timeseries/client-api/ts-javascript-api#time series-js-api-methods)  
     * [timeseries - Choose a Time Series](../../../document-extensions/timeseries/client-api/ts-javascript-api#section)  
     * [timeseries.append - Append Untagged Entry](../../../document-extensions/timeseries/client-api/ts-javascript-api#section-1)  
     * [timeseries.append - Append Tagged Entry](../../../document-extensions/timeseries/client-api/ts-javascript-api#section-2)  
     * [timeseries.remove - Remove Entries Range](../../../document-extensions/timeseries/client-api/ts-javascript-api#section-3)  
     * [timeseries.get - Get Entries Range](../../../document-extensions/timeseries/client-api/ts-javascript-api#section-4)  
  * [Usage Samples](../../../document-extensions/timeseries/client-api/ts-javascript-api#usage-samples)  

{NOTE/}

---

{PANEL: Time Series JS API methods}

The JS time series API includes these methods:  

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

* In this sample, we pass [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data) 
  a script that patches a document 100 time series entries.  
  {CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}


* In this sample, we pass [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchbyqueryoperation) 
  a script that runs a document query and removes the HeartRate time series from 
  located documents.  
   {CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Patching Time Series Data**  
[Patching in a Session](../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data)  
[Patching Operation](../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchoperation)  
[Patc By Query Operation](../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchbyqueryoperation)  

**Querying and Indexing**  
[Time Series Querying](../../../document-extensions/timeseries/querying/queries-overview-and-syntax)  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
