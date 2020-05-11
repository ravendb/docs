# Get Time-Series Data

---

{NOTE: }

Get time-series data using `GetTimeSeriesOperaion`.  

`GetTimeSeriesOperaion` allows you to retrieve data from multiple 
time-series of a selected document in a single call.  

* In this page:  
  * [`GetTimeSeriesOperation` Definition](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#gettimeseriesoperation-definition)  
  * [Get A Single Time-Series' Data](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#get-a-single-time-series)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-flow)  
     * [Usage Sample](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-sample)  
  * [Get Multiple Time-Series Data](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#get-multiple-time-series-data)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-flow-1)  
     * [Usage Sample](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-sample-1)  
{NOTE/}

---

{PANEL: `GetTimeSeriesOperation` Definition}

There are two `GetTimeSeriesOperation` overloads.  

* Use the first overload to retrieve a single time-series' data.  
* Use the second overload to retrieve multiple time-series' data.  

{PANEL/}

{PANEL: Get A Single Time-Series' Data}

---

#### Usage Flow

* Pass `GetTimeSeriesOperation` -  
   * The document ID  
   * The time-series name  
   * Range start: Timestamp for the first time-series entry to be retrieved  
   * Range end: Timestamp for the last time-series entry to be retrieved  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into a `dictionary of `TimeSeriesRangeResult` classes.  
   * TimeSeriesRangeResult:
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

#### Usage Sample

Here, we retrieve all entries of a single time-series.  
{CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}


{PANEL: Get Multiple Time-Series Data}

---

#### Usage Flow

* Pass `GetTimeSeriesOperation` the document ID and a list of `TimeSeriesRange` instances.  
  Each `TimeSeriesRange` instance defines -  
   * **Name** - The time-series name  
   * **From** - Range start, the timestamp for the first time-series entry to be retrieved  
   * **To** - Range end, the timestamp for the last time-series entry to be retrieved  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into into a `dictionary of `TimeSeriesRangeResult` classes.  
   * TimeSeriesRangeResult:
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

#### Usage Sample

Here, we retrieve chosen entries from two time-series.  
{CODE timeseries_region_Get-Multiple-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
