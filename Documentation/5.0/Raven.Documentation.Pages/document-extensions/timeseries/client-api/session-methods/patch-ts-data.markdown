# Patch Time-Series Date
---

{NOTE: }

Patch time-series data to a document using the session` method 
`Advanced.Defer`.  

* In this page:  
   * [Patch Time-Series Data Using `session.Advanced.Defer`](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#patch-time-series-data-using-advanced.defer)  
      * [Method Definition](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#method-definition)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-samples)  

{NOTE/}

---

{PANEL: Patch Time-Series Data Using `Advanced.Defer`}

Use `Advanced.Defer` to run a Java Script that -  

* Patches time-series entries to a document  
* Removes time-series entries from a document  

{INFO: }
You can handle a single document at a time.  
Since this is a `session` method however, you can call 
`Advanced.Defer` multiple times and call `session.saveChanges()` 
to execute them all in a single transaction.  
{INFO/}

---

#### Method Definition  

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
