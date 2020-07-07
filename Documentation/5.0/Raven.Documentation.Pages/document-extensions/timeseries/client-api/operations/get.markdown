# Operations: How to Get Time Series

---

{NOTE: }

Get a single time series' entries using `GetTimeSeriesOperation`.  
Get multiple time series' entries using `GetMultipleTimeSeriesOperation`.  

* In this page:  
      * [`GetTimeSeriesOperation`](../../../../document-extensions/timeseries/client-api/operations/get#gettimeseriesoperation)  
         * [Syntax](../../../../document-extensions/timeseries/client-api/operations/get#syntax)  
         * [Usage Flow](../../../../document-extensions/timeseries/client-api/operations/get#usage-flow)  
         * [Usage Samples](../../../../document-extensions/timeseries/client-api/operations/get#usage-sample)  
      * [`GetMultipleTimeSeriesOperation`](../../../../document-extensions/timeseries/client-api/operations/get#getmultipletimeseriesoperation)  
         * [Syntax](../../../../document-extensions/timeseries/client-api/operations/get#syntax-1)  
         * [Usage Flow](../../../../document-extensions/timeseries/client-api/operations/get#usage-flow-1)  
         * [Usage Sample](../../../../document-extensions/timeseries/client-api/operations/get#usage-sample-1)  

{NOTE/}

---

{PANEL: `GetTimeSeriesOperation`}

Use `GetTimeSeriesOperation` to retrieve entries from a single 
time series.  

---

### Syntax

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

   * Requesting a time series that doesn't existing will return `null`.  
   * Requesting an entries range that doesn't exist will return a `TimeSeriesRangeResult` object with an 
     empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

---

### Usage Flow

* Pass `GetTimeSeriesOperation` -  
     Document ID, Time Series Name, Range Start, Range End.  
* Call `store.Operations.Send` to execute the operation.  
* Entries are returned into a `dictionary of `TimeSeriesRangeResult` classes.  

---

### Usage Sample

* In this sample, we retrieve all the entries of a time series.  
   {CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `GetMultipleTimeSeriesOperation`}

Use `GetMultipleTimeSeriesOperation` to retrieve data from 
multiple time series.  

---

### Syntax

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

    * When a time series or an entries range that do not exist are requested, 
      the return value for the erronous range is a `TimeSeriesRangeResult` 
      object with an empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

---

### Usage Flow

* Pass `GetMultipleTimeSeriesOperation` -  
   * The time series parent-Document ID  
   * A **TimeSeriesRange** instance for each entries range you want 
     it to retrieve.  
* Populate each TimeSeriesRange instance with a **time Series name**, 
  a **range start** timestamp (`From`), and a **range end** timestamp (`To`).  
* Call `store.Operations.Send` to execute the operation.  

---

### Usage Sample

* In this sample, we retrieve chosen entries from two time series.  
   {CODE timeseries_region_Get-Multiple-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
