# Session: Patch Time Series

---

{NOTE: }

* To patch time series entries to a document, use `session.Advanced.Defer`.  
   * You can pass `Defer` a script to Append, Get, and Remove time series entries.  
   * You can handle a single document at a time.  

* [Patching Using `session.Advanced.Defer`](../../../../document-extensions/timeseries/client-api/session/patch#patching-using-session.advanced.defer)  
   * [Syntax](../../../../document-extensions/timeseries/client-api/session/patch#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session/patch#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session/patch#usage-samples)  

{NOTE/}

---

{PANEL: Patching Using `session.Advanced.Defer`}

{INFO: }

* [Defer](../../../../client-api/operations/patching/single-document#non-typed-session-api) 
  is used for patching in general, not necessarily for time series data patching.  
* To patch time series data, you need to [customize the JavaScript](../../../../document-extensions/timeseries/client-api/javascript-support) 
  that `Defer` uses.  
* Learn more about time series JavaScript [here](../../../../document-extensions/timeseries/client-api/javascript-support).  

{INFO/}

{PANEL/}

{PANEL: Syntax}

* **`PatchCommandData`**  
   * **Definition**
     {CODE PatchCommandData-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
     Learn more about `PatchCommandData` [here](../../../../client-api/operations/patching/single-document#non-typed-session-api).

* **`PatchRequest`**  
   * **Definition**
     {CODE PatchRequest-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
     Learn more about `PatchRequest` [here](../../../../client-api/operations/patching/single-document#non-typed-session-api).  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.Defer` and pass it a `PatchCommandData` instance.  
* Pass the `PatchCommandData` constructor method -  
   * the document ID  
   * the change vector (or `null`)  
   * a `PatchRequest` instance with a JavaScript that appends or removes time series entries.  
* Call `session.SaveChanges()` to perform the patch.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we pass `Defer`a script that appends a document 100 time series 
  entries with random heartrate values.  
  {CODE TS_region-Session_Patch-Append-100-Random-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample, we remove a range of 50 time series entries from a document.  
  {CODE TS_region-Session_Patch-Delete-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

## Related articles

**Time Series and JavaScript**  
[The Time Series JavaScript API](../../../../document-extensions/timeseries/client-api/javascript-support)  

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
