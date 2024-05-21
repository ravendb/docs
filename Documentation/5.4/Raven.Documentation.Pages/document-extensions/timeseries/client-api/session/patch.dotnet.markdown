# Patch Time Series Entries

---

{NOTE: }

* Patching multiple time series entries (append or delete entries) can be performed via the _Session_  
  using [session.Advanced.Defer](../../../../client-api/operations/patching/single-document#session-api-using-defer), as described below.
  * You can handle a single document at a time.
  * The patching action is defined by the provided [JavaScript script](../../../../document-extensions/timeseries/client-api/javascript-support).
  
* Patching time series entries can also be done directly on the _Store_ via [Operations](../../../../client-api/operations/what-are-operations),  
  where multiple documents can be handled at a time. Learn more in [Patching time series operations](../../../../document-extensions/timeseries/client-api/operations/patch).

* In this page:
   * [Usage](../../../../document-extensions/timeseries/client-api/session/patch#usage)  
   * [Patching examples](../../../../document-extensions/timeseries/client-api/session/patch#patching-examples)
     * [Append multiple entries](../../../../document-extensions/timeseries/client-api/session/patch#append-multiple-entries) 
     * [Delete multiple entries](../../../../document-extensions/timeseries/client-api/session/patch#delete-multiple-entries) 
   * [Syntax](../../../../document-extensions/timeseries/client-api/session/patch#syntax)

{NOTE/}

---

{PANEL: Usage}

* Open a session
* Construct a `PatchCommandData` instance and pass it the following:
    * The document ID that contains the time series
    * The document change vector (or `null`)
    * A `PatchRequest` instance with a JavaScript that appends or removes time series entries
* Call `session.Advanced.Defer` and pass it the `PatchCommandData` command.  
  Note that you can call _Defer_ multiple times prior to calling _SaveChanges_.
* Call `session.SaveChanges()`.  
  All patch requests added via _Defer_ will be sent to the server for execution when _SaveChanges_ is called.

{PANEL/}

{PANEL: Patching examples}

{NOTE: }

<a id="append-multiple-entries" /> __Append multiple entries__:

In this example, we append 100 time series entries with random heart rate values to a document.  

{CODE TS_region-Session_Patch-Append-100-Random-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{NOTE/}

{NOTE: }

<a id="delete-multiple-entries" /> __Delete multiple entries__:

In this example, we remove a range of 50 time series entries from a document.  

{CODE TS_region-Session_Patch-Delete-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

**`PatchCommandData`**

{CODE PatchCommandData-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Learn more about `PatchCommandData` [here](../../../../client-api/operations/patching/single-document#session-api-using-defer).

---

**`PatchRequest`**

{CODE PatchRequest-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Learn more about `PatchRequest` [here](../../../../client-api/operations/patching/single-document#session-api-using-defer).

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
