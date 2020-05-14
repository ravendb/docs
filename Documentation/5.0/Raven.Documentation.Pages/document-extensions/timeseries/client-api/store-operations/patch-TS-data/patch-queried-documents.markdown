## `PatchByQueryOperation`
# Patch TS Data to Queried Documents

---

{NOTE: }

* Use [PatchByQueryOperation](../../../../../client-api/operations/patching/set-based#patchbyqueryoperation) 
  to run a query and patch time-series data to the documents you find.

* In this page:  
  * [`PatchByQueryOperation`](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-queried-documents#patchbyqueryoperation)  
     * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-queried-documents#syntax)  
     * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-queried-documents#usage-flow)  
     * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-queried-documents#usage-samples)  
{NOTE/}

---

{PANEL: `PatchByQueryOperation`}

`PatchByQueryOperation` runs a Java Script to perform a query and patch 
data to documents it finds.  

You can use this script to -  

* Append time-series data to documents  
* Remove time-series data from documents  
* Get documents' time-series data  

{PANEL/}

{PANEL: Syntax}

* **`store.Operations.Send` Definition**  
  {CODE Store-Operations-send-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **`PatchByQueryOperation` Definition**  
  {CODE PatchByQueryOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `queryToUpdate` | `IndexQuery` | The query, including the Java Script |
        | `QueryOperationOptions` | `options` | Additional options <br> Default: `null` |

* **Return Value**: `Operation`   

{PANEL/}

{PANEL: Usage Flow}

* Create a `PatchByQueryOperation` operation.  
* Pass `PatchByQueryOperation` a new `IndexQuery` instance.  
* Add the `IndexQuery` instance a Java Script that specifies 
   the query you want to run.  
* Call `store.Operations.Send` to execute the operation.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, `PatchByQueryOperation` Appends a time-series entry to all 
  the documents in the User collection.  
   {CODE TS_region-PatchByQueryOperation-Append-To-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* Here, our script Removes time-series from documents it finds.  
   {CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we Get ranges of time-series entries from the documents located by 
  the query.  
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
