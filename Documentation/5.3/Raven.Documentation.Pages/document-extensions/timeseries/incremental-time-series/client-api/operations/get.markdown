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
    | `returnFullResults` | `bool` | If true, retrieve the values stored per-node |
     

* **Return Value**: **`TimeSeriesRangeResult<TimeSeriesEntry>`**  
  {CODE-BLOCK:csharp}
public class TimeSeriesRangeResult 
    {
        public DateTime From, To;
        public TimeSeriesEntry[] Entries;
        
        // The actual number of values
        public long? TotalResults; 
        
        // Helps to calculate next start
        public int? SkippedResults; 
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
* Entries are returned into a dictionary of `TimeSeriesRangeResult` classes.  
* Calculate where the next `Get` operation needs to start.  
   {NOTE: To calculate where the next page should start:}

    * The value you set in pageSize indicates the location of the first entry to retrieve.
    * More than one entry can exist for the same timestamp since when different nodes increment 
      a value for the same timestamp, the entries stored on those nodes will have the same 
      timestamp, resulting in 'duplicate' entries.  
    * The number of these duplicated entries is returned in the SkippedResults  
    * To find where the next page starts, add (see code sample below):  
        * Your current starting point  
        * The returned `Entries.Length`  
        * The returned `SkippedResults`  
   {NOTE/}
* Run the next `Get` operation with your calculation result as its `start` entry.  

---

### Usage Sample

* In this sample we retrieve 50 entries from an incremental time series that contains 
  two per-node values in each entry.  
  We then calculate where the next `Get` operation should start, and run another `Get` 
  operation starting there.  
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
