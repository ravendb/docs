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

Use `GetTimeSeriesOperation` to retrieve the distinct values stored per-node for the requested entries.  

---

### Syntax

* `GetTimeSeriesOperation` Definition:  
  {CODE incremental_declaration_GetTimeSeriesOperation@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `docId` | `string` | Document ID |
    | `timeseries` | `string` | Time series name |
    | `from` (optional) | `DateTime?` | Range start  <br> Default: `DateTime.Min` ||
    | `to` (optional) | `DateTime?` | Range end  <br> Default: `DateTime.Max` ||
    | `start` | `int` | Start of first Page |
    | `pageSize` | `int` | Size of each page, counted in [entries with unique timestamps](../../../../../document-extensions/timeseries/incremental-time-series/overview#incremental-time-series-structure) |
    | `returnFullResults` | `bool` | If true, retrieve the values stored per-node. <br> If false, return `null ` in `TimeSeriesEntry.NodeValues`. |
     

* **Return Value**: **`TimeSeriesRangeResult<TimeSeriesEntry>`**  
  {CODE-BLOCK:csharp}
public class TimeSeriesRangeResult 
    {
        public DateTime From, To;
        public TimeSeriesEntry[] Entries;
        
        // The number of unique values
        public long? TotalResults; 
        
    {CODE-BLOCK/}

    {CODE-BLOCK:csharp}
public class TimeSeriesEntry 
    {
        public DateTime Timestamp { get; set; }
        public double[] Values { get; set; }
        public string Tag { get; set; }
        public bool IsRollup { get; set; }
        
        // The nodes distribution per each entry
        public Dictionary<string, double[]> NodeValues { get; set; } 
    {CODE-BLOCK/}

     * `TimeSeriesRangeResult.TotalResults` will contain the number of **unique** values.  
       If the time series contains entries with multiple values (remember 
       that since this is an incremental time series this means duplications 
       of the same number at the same timestamp) all values will be aggregated 
       in `TotalResults` to a single unique value.  
     * Requesting a time series that doesn't exist will return `null`.  
     * Requesting an entries range that doesn't exist will return a `TimeSeriesRangeResult` object 
       with an empty `Entries` property.  

* **Exceptions**  
  Exceptions are not generated.  

---

### Usage Sample

* In this sample we retrieve 50 entries from an incremental time series that contains 
  two per-node values in each entry.  
  We then calculate where the next `Get` operation should start, and run another `Get` 
  operation starting there.  
  {CODE incremental_GetTimeSeriesOperation@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `GetMultipleTimeSeriesOperation`}

To retrieve data from **multiple** time series, 
use [GetMultipleTimeSeriesOperation](../../../../../document-extensions/timeseries/client-api/operations/get#getmultipletimeseriesoperation).  

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
