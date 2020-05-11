# Append and Remove Time-Series Data

---

{NOTE: }

Append and Remove single or multiple time-series entries 
using the document-store operation `TimeSeriesBatchOperation`.  
`TimeSeriesBatchOperation` allows you to bundle a series of 
Append and/or Remove operations in a list, and execute them 
all in a single call.  

* In this page:  
  * [`TimeSeriesBatchOperation` Definition](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#timeseriesbatchoperation-definition)  
  * [Using `TimeSeriesBatchOperation` to Append TS Data](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#using-timeseriesbatchoperation-to-append-ts-data)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-sample)  
  * [Using `TimeSeriesBatchOperation` to Remove TS Data](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#using-timeseriesbatchoperation-to-remove-ts-data)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-flow-1)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-sample-1)  
{NOTE/}

---

{PANEL: `TimeSeriesBatchOperation` Definition}

To instruct `TimeSeriesBatchOperation` which actions to perform, pass it 
an `TimeSeriesOperation` instance for each Append or Remove action.  

* `TimeSeriesOperation`  
  {CODE TimeSeriesOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}


{PANEL: Using `TimeSeriesBatchOperation` to Append TS Data}

#### Usage Flow  

* Create an instance of `TimeSeriesOperation`  
    * Populate it with a new `AppendOperation` list for every time-series entry you want to append.  
    * Populate each `AppendOperation` list with the properties of the time-series 
      entry that you want to append.  
* Create a `TimeSeriesBatchOperation` instance.  
    * Pass its constructor the document ID and the `TimeSeriesOperation` instance you've created.  
* Call `store.Operations.Send` to execute the operation.  
    * Pass it the `TimeSeriesBatchOperation` instance you've created.  

---

#### Usage Sample

Here, we append a time-series two entries using `TimeSeriesBatchOperation`.  
{CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}


{PANEL: Using `TimeSeriesBatchOperation` to Remove TS Data}

---

#### Usage Flow  

* Create a `TimeSeriesOperation` instance  
   * Populate the `TimeSeriesOperation` instance with -  
      The time-series name.  
      A list of removals.  
   * The list of removals is constructed of `RemoveOperation` instances.  
      Each instance defines a range of entries that will be removed from the time-series.  
   * Set a removal range using -  
     `From` - the timestamp of the first time-series entry of the range  
     `To` - the timestamp of the last time-series entry to be removed.  
* Create a `TimeSeriesBatchOperation` instance.  
    * Pass its constructor the document ID and the `TimeSeriesOperation` instance you've created.  
* Call `store.Operations.Send` to execute the operation.  
    * Pass it the `TimeSeriesBatchOperation` instance you've created.  

---

#### Usage Sample

Here, we remove two ranges of entries from a time-series.  
{CODE timeseries_region_Remove-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
