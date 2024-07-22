# Patch Time Series Entries

---

{NOTE: }

* Patching multiple time series entries (append or delete entries) can be performed via the _Session_  
  using [session.advanced.defer](../../../../client-api/operations/patching/single-document#session-api-using-defer), as described below.
  * You can handle a single document at a time.
  * The patching action is defined by the provided [JavaScript](../../../../document-extensions/timeseries/client-api/javascript-support).
  
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
* Call `session.advanced.defer` and pass it the `PatchCommandData` command.  
  Note that you can call _defer_ multiple times prior to calling _saveChanges_.
* Call `session.saveChanges()`.  
  All patch requests added via _defer_ will be sent to the server for execution when _saveChanges_ is called.

{PANEL/}

{PANEL: Patching examples}

#### Append multiple entries:

In this example, we append 100 time series entries with random heart rate values to a document.  

{CODE:nodejs patch_1@documentExtensions\timeSeries\client-api\patchTimeSeries.js /}

---

#### Delete multiple entries:

In this example, we remove a range of 50 time series entries from a document.  

{CODE:nodejs patch_2@documentExtensions\timeSeries\client-api\patchTimeSeries.js /}

{PANEL/}

{PANEL: Syntax}

A detailed syntax description for `PatchCommandData` & `PatchRequest` can be found in the following section:  
[Session API using defer syntax](../../../../client-api/operations/patching/single-document#session-api-using-defer-syntax).

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
