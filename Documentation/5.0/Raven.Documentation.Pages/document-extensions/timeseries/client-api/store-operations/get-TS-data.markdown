# Get Time-Series Data

---

{NOTE: }

Get time-series data using `GetTimeSeriesOperaion`.  

* In this page:  
      * [`GetTimeSeriesOperation`](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#gettimeseriesoperation)  
      * [Syntax](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#syntax)  
         * [Overload 1 - Retrieve a Single Time-Series' Data](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-1-retrieve-a-single-time-series-data)  
         * [Overload 2 - Retrieve Multiple Time-Series' Data](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-2-retrieve-multiple-time-series-data)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#usage-samples)  

{NOTE/}

---

{PANEL: `GetTimeSeriesOperation`}

Use `GetTimeSeriesOperaion` to retrieve data from a single 
time-series or from multiple time-series.  

{PANEL/}

{PANEL: Syntax}

There are two `GetTimeSeriesOperation` methods:  
[Overload 1](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-1-retrieve-a-single-time-series-data) 
- Retrieve a single time-series' data.  
[Overload 2](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-2-retrieve-multiple-time-series-data) 
- Retrieve multiple time-series' data.  

---

#### Overload 1: Retrieve a Single Time-Series' Data  

* **Definition**
  {CODE GetTimeSeriesOperation-Definition-Overload1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `timeseries` | `string` | Time-series name |
    | `from` | `DateTime` | Range start |
    | `to` | `DateTime` | Range end |
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page |

* **Return Value**: **`TimeSeriesRangeResult`**  
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* **Exceptions**  
  Exceptions are not generated.  

---

#### Overload 2: Retrieve Multiple Time-Series' Data  

* **Definition**
  {CODE GetTimeSeriesOperation-Definition-Overload2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `ranges` | `IEnumerable<TimeSeriesRange>` <br> {CODE TimeSeriesRange-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}| A list of time-series ranges to Get |
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page |

* **Return Value**: a dictionary of `TimeSeriesRangeResult` classes.  
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* **Exceptions**  
  Exceptions are not generated.  

{PANEL/}

{PANEL: Usage Flow}

* Pass `GetTimeSeriesOperation` -  
   * For a single time-series:  
     Document ID, Time-Series Name, Range Start, Range End.  
   * For multiple time-series:  
     Document ID, a List of `TimeSeriesRange` instances.  
     Each `TimeSeriesRange` instance defines a Time-Series Name, Range Start, and Range End.  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into a `dictionary of `TimeSeriesRangeResult` classes.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we use the [first overload](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-1-retrieve-a-single-time-series-data) 
  to retrieve all entries of a single time-series.  
   {CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use the [second overload](../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data#overload-2-retrieve-multiple-time-series-data) 
  to retrieve chosen entries from two time-series.  
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
