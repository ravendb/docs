## `GetMultipleTimeSeriesOperation`
# Get Multiple Time Series Data

---

{NOTE: }

Get multiple time series data using `GetMultipleTimeSeriesOperation`.  

* In this page:  
      * [`GetMultipleTimeSeriesOperation`](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-multiple-ts-data#getmultipletimeseriesoperation)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-multiple-ts-data#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-multiple-ts-data#usage-flow)  
      * [Usage Sample](../../../../../document-extensions/timeseries/client-api/store-operations/get-ts-data/get-multiple-ts-data#usage-sample)  

{NOTE/}

---

{PANEL: `GetMultipleTimeSeriesOperation`}

Use `GetMultipleTimeSeriesOperation` to retrieve data from 
multiple time series.  

{PANEL/}

{PANEL: Syntax}

* **Definition**
  {CODE GetMultipleTimeSeriesOperation-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `ranges` | `IEnumerable<TimeSeriesRange>` | Ranges of Time Series Entries |
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page |

     Pass `GetMultipleTimeSeriesOperation` a **TimeSeriesRange** instance 
     For each entries range you want it to retrieve.
     {CODE TimeSeriesRange-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Return Value**: **`TimeSeriesRangeResult`**  
     {CODE TimeSeriesDetails-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

       When a nonexistent time series or entries-range is requested, 
       the return value for the erronous range is a `TimeSeriesRangeResult` 
       object with an empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

{PANEL/}

{PANEL: Usage Flow}

* Pass `GetMultipleTimeSeriesOperation` -  
   * The time series parent-Document ID  
   * A **TimeSeriesRange** instance for each entries range you want 
     it to retrieve.  
* Populate each TimeSeriesRange instance with a **time Series name**, 
  a **range start** timestamp (`From`), and a **range end** timestamp (`To`).  
* Call `store.Operations.Send` to execute the operation.  

{PANEL/}

{PANEL: Usage Sample}

* In this sample, we retrieve chosen entries from two time series.  
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
