# Patching Time Series
---

{NOTE: }

You can patch time-series to a document or a series of documents 
using either a `session` methods or a `document store` operation.  

* In this page:  
   * [Patching Time-Series Using `session.Advanced.Defer`](../../../document-extensions/timeseries/client-api/patching-time-series#patching-time-series-using-session.advanced.defer)  
   * [Patching Time-Series Using `PatchOperation`](../../../document-extensions/timeseries/client-api/patching-time-series#patching-time-series-using-patchoperation)  
{NOTE/}

---

{PANEL: Patching Time-Series Using `session.Advanced.Defer`}

To patch time-series entries to a document or to remove entries 
from a document, call `session.Advanced.Defer`.  
Instruct `session.Advanced.Defer` which actions to perform, 
by passing it a Java Script.  

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
{CODE timeseries_region_Patch-A-Document-A-Single-Time-Series-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we provide `session.Advanced.Defer`with a script that patches 
100 time-series entries to a document. Timestamps and values are drawn 
from an array, and other arguments are provided in the "Values" section.  
{CODE timeseries_region_Patch-Append-A-Document-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we remove a range of 50 time-series entries from a document.  
{CODE timeseries_region_Patch-Remove-From-A-Document-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Patching Time-Series Using `PatchOperation`}

Patch time-series data via the `document store` using `PatchOperation`.  
It has an advantage over `session.Advanced.Defer`, in that it allows you 
to patch multiple documents.  

Learn how to use `PatchOperation` with time-series [in the article 
dedicated to time-series operations](../../../).  

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
