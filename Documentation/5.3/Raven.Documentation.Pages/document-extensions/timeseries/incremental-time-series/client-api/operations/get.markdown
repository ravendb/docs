# Operations: Get Incremental Time Series

---

{NOTE: }

* Get time series entries using `GetTimeSeriesOperation`.  
* Using this method, you can retrieve node values from incremental time series entries.  

* In this page:  
      * [`GetTimeSeriesOperation`](../../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get#gettimeseriesoperation)  
         * [Syntax](../../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get#syntax)  
         * [Usage Flow](../../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get#usage-flow)  
         * [Code Samples](../../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get#usage-sample)  

{NOTE/}

---

{PANEL: `GetTimeSeriesOperation`}

Use `GetTimeSeriesOperation` to retrieve incremental time series entries, including 
the values stored in entries by each node.  

---

### Syntax

* **Definition**
  {CODE incremental_declaration_GetTimeSeriesOperation@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `timeseries` | `string` | Time series name |
    | `from` (optional) | `DateTime?` | Range start  <br> Default: `DateTime.Min` ||
    | `to` (optional) | `DateTime?` | Range end  <br> Default: `DateTime.Max` ||
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page |
    | `returnFullResults` | `bool` | If true, retrieve node values from entries |
     

* **Return Value**: **`TimeSeriesRangeResult<TimeSeriesEntry>`**  

   * Requesting a time series that doesn't exist will return `null`.  
   * Requesting an entries range that doesn't exist will return a `TimeSeriesRangeResult` object 
     with an empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

---

### Usage Flow

* Pass `GetTimeSeriesOperation` -  
   * Document ID  
   * Incremental Time Series Name  
   * Range Start  
   * Range End.  
   * **0** as the value of `start`, if you want to start with the first entry.  
   * the number of results you want to retrieve, as the value of `pageSize`.  
   * **true** as the value of `returnFullResults`, if you want to retrieve node values from time series entries.  
* Call `store.Operations.Send` to execute the operation.  
* Entries are returned into a `dictionary of `TimeSeriesRangeResult` classes.  
* Calculate where the next `Get` operation needs to start.  
   {NOTE: To calculate where the next page should start:}
     * The value you set in `pageSize`, counts the first value of each entry.  
       But each entry may have additional per-node values.  
       The per-node values are counted by the `Get` operation, and returned in a `SkippedResults` value.  
     * To find where the next page starts, add:  
        * `start` (your current starting point)  
        * `pageSize`  
        * `SkippedResults`  
   {NOTE/}
* Run the next `Get` operation with your calculation result as its `start` entry.  

---

### Usage Sample

* In this sample we retrieve 50 entries, calculate where the next `Get` operation should start, 
  and run another `Get` operation starting there.  
  {CODE incremental_GetTimeSeriesOperation@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `GetMultipleTimeSeriesOperation`}

Use `GetMultipleTimeSeriesOperation` to retrieve data from 
multiple time series.  

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
