# Patch Time-Series Data

---

{NOTE: }

* Use `PatchOperation` to patch time-series data to a single document 
  loaded by its ID.  
* Use `PatchByQueryOperation` to query your database and 
  patch time-series data to the documents your query locates.  

* In this page:  
  * [`PatchOperation`: Patch a Document](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchoperation:-patch-a-document)  
     * [Definition](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#definition)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-samples)  
  * [`PatchByQueryOperation`: Patch Queried Documents](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchbyqueryoperation:-patch-queried-documents)  
     * [Definition](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#definition-1)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-flow-1)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-samples-1)  
{NOTE/}

---

{PANEL: `PatchOperation`: Patch a Document}

`PatchOperation` allows you to run a Java Script that patches 
time-series entries to a loaded document, or removes entries 
from it.  

---

#### `PatchOperation` Definition  

---

#### Usage Flow  

* Pass `PatchOperation` -  
   * the document ID  
   * the change vector if you need to (or `null` if not)  
   * a new `PatchRequest` instance  
* Use the `PatchRequest` instance to pass `PatchOperation` 
  a Java Script that specifies whether to Append or Remove 
  time-series entries and how to perform it.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples  

Here, we use `PatchOperation` to patch a document a single 
time-series entry.  
The script draws its arguments from its "Values" section.  
{CODE TS_region-Operation_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we provide `PatchOperation`with a script that patches 
100 time-series entries to a document.  
Timestamps and values are drawn from an array, and other 
arguments are provided in the "Values" section.  
{CODE TS_region-Operation_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we use `PatchOperation` to remove a range of 50 time-series 
entries from a document.  
{CODE TS_region-Operation_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `PatchByQueryOperation`: Patch Queried Documents}

`PatchByQueryOperation` allows you to run a query and patch 
time-series data to all the documents it finds.  
You can use this operation not only to append data, but also to 
remove or get time-series data from located documents.  

---

#### `PatchByQueryOperation` Definition  

---

#### Usage Flow  

* Create a `PatchByQueryOperation` operation.  
* Pass `PatchByQueryOperation` a new `IndexQuery` instance as an argument.  
* Add the `IndexQuery` instance a Java Script that specifies 
   the query you want to run.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples  

Here, the query we provide `PatchByQueryOperation` appends 
a time-series entry to all user documents.  
{CODE TS_region-PatchByQueryOperation-Append-To-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, the query removes time-series from located documents.  
{CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we get selected ranges of time-series entries from the documents 
located by the query.  
{CODE TS_region-PatchByQueryOperation-Get@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
