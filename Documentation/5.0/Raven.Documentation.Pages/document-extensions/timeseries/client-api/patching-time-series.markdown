# Patching Time Series
---

{NOTE: }

You can patch time-series data to a document using either 
a `session` method or `document store` operations.  

* In this page:  
   * [Patching Time-Series Date Using `session.Advanced.Defer`](../../../document-extensions/timeseries/client-api/patching-time-series#patching-time-series-data-using-session.advanced.defer)  
   * [Patching Time-Series Date Using `store operations`](../../../document-extensions/timeseries/client-api/patching-time-series#patching-time-series-date-using-store-operations)  
{NOTE/}

---

{PANEL: Patching Time-Series Data Using `session.Advanced.Defer`}

Use `session.Advanced.Defer` to run a Java Script that patches time-series 
entries to a document or removes them from it.  

You can handle a single document at a time.  
Since this is a `session` method however, you can call 
`session.Advanced.Defer` multiple times and call `session.saveChanges()` 
to execute them all in a single transaction.  

---

#### Usage Flow  

* Open a session  
* Call `session.Advanced.Defer` and pass it a `PatchCommandData` instance.  
  Pass the `PatchCommandData` constructor method -  
   * the document ID  
   * the change vector if you need to (or `null` if not)  
   * a new `PatchRequest` instance  
* Use `PatchRequest` to pass `session.Advanced.Defer` a Java Script that 
  specifies whether to Append or Remove time-series entries and how to 
  perform it.  
* Call `session.SaveChanges()` to execute the patch.  
   
---

#### Usage Samples  

Here, we use `session.Advanced.Defer` to patch a document a single 
time-series entry.  
The script draws its arguments from its "Values" section.  
{CODE TS_region-Session_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we provide `session.Advanced.Defer`with a script that patches 
100 time-series entries to a document. Timestamps and values are drawn 
from an array, and other arguments are provided in the "Values" section.  
{CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we remove a range of 50 time-series entries from a document.  
{CODE TS_region-Session_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Patching Time-Series Date Using `store operations`}

There are two document-store patching operations.  

* The first is `PatchOperation`.  
  Use it to run a Java Script that patches time-series entries to 
  a document or removes them from it.  
  Learn how to use it [here](../../../document-extensions/timeseries/client-api/time-series-operations#configuretimeseriesoperation:-manage-rollup-and-retention-policies).  
* The second is `PatchByQueryOperation`.  
  Use it to query your database and perform time-series operations 
  on located documents.  
  `PatchByQueryOperation` is very helpful when you want to perform 
  time-series actions on multiple documents.  
  Learn how to use it [here](../../../document-extensions/timeseries/client-api/time-series-operations#patchbyqueryoperation:-patch-time-series-data-by-query).  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
