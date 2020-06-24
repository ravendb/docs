## `GetTimeSeriesOperation`
# Get a Time Series Data

---

{NOTE: }

Get a single time series' data using `GetTimeSeriesOperation`.  

* In this page:  
      * [`GetTimeSeriesOperation`](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-ts-data#gettimeseriesoperation)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-ts-data#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-ts-data#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-ts-data#usage-sample)  

{NOTE/}

---

{PANEL: `GetTimeSeriesOperation`}

Use `GetTimeSeriesOperation` to retrieve data from a single 
time series.  

* To retrieve data from multiple time series, use 
  [GetMultipleTimeSeriesOperation](../../../../../document-extensions/timeseries/client-api/store-operations/get-TS-data/get-multiple-TS-data).  

{PANEL/}

{PANEL: Syntax}

* **Definition**
  {CODE GetTimeSeriesOperation-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `timeseries` | `string` | Time series name |
    | `from` | `DateTime` | Range start |
    | `to` | `DateTime` | Range end |
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page |

* **Return Value**: **`TimeSeriesRangeResult`**  
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

      When a nonexistent time series is requested, 
      the return value will be `null`.  
      When a nonexistent entries-range is requested, 
      the return value will be a `TimeSeriesRangeResult` object with an 
      empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

{PANEL/}

{PANEL: Usage Flow}

* Pass `GetTimeSeriesOperation` -  
     Document ID, Time Series Name, Range Start, Range End.  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into a `dictionary of `TimeSeriesRangeResult` classes.  

{PANEL/}

{PANEL: Usage Sample}

* In this sample, we retrieve all the entries of a time series.  
   {CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
