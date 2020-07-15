# Bulk Insert: How to Append Time Series

---

{NOTE: }

* `store.BulkInsert` is RavenDB's high-performance data insertion operation.  
  Using its `TimeSeriesFor` interface's `Append` method resembles using 
  [session.TimeSeriesFor](../../../../../document-extensions/timeseries/client-api/session/append), 
  but `session` liabilities are omitted so a much greater speed is gained.  

* You can bulk-insert **a single time series** at a time.  

* To bulk-insert an additional time series concurrently, open an additional **BulkInsertOperation** 
  instance.  

* In this page:  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#usage-samples)  

{NOTE/}

{PANEL: Syntax}

*   `BulkInsert.TimeSeriesFor`
    {CODE BulkInsert.TimeSeriesFor-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `id` | `string` | Document ID |
     | `name` | `string` | Time Series Name |

---

* There are two `TimeSeriesFor.Append` overloads:  
   {CODE Append-Operation-Definition-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE Append-Operation-Definition-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `timestamp` | `DateTime` | TS-entry's timestamp |
     | `value` | `double` | A single value |
     | `values` | `ICollection<double>` | Multiple values |
     | `tag` | `string` | TS-entry's tag (optional) |

{PANEL/}

{PANEL: Usage Flow}

* Create a `store.BulkInsert` instance.  
* Pass the instance's `TimeSeriesFor` -  
   * Document ID  
   * Time series name  
* Call `Append` as many times as you like. Pass it -  
   * The entry's Timestamp  
   * The entry's Value or Values  
   * The entry's Tag (optional)  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we append two entries to a time series.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we append a hundred entries to a time series.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we append two sets of values to a time series.  
   {CODE BulkInsert-overload-2-Two-HeartRate-Sets@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
