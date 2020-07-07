# Operations: Append in Bulk

---

{NOTE: }

* `store.BulkInsert` is RavenDB's high-performance data insertion operation.  
  Its `TimeSeriesFor` interface resembles [session.TimeSeriesFor](../../../../../document-extensions/timeseries/client-api/session/append) 
  in its ability to append multiple time series entries in a single transaction, 
  while omitting the liabilities of the `session` interface to gain much higher speed.  

* In this page:  
   * [`BulkInsert.TimeSeriesFor.Append`](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#bulkinsert.timeseriesfor.append)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/operations/bulk-insert/append-in-bulk#usage-samples)  

{NOTE/}

{PANEL: `BulkInsert.TimeSeriesFor.Append`}

Create a `BulkInsert` instance, pass its `TimeSeriesFor` method the document's ID 
and time series name, and then call `TimeSeriesFor.Append` as many times as you need 
to. All the Append operations you perform this way will be executed in 
a single transaction.  

{PANEL/}

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
* Call `TimeSeriesFor.Append` as many times as you like. Pass it -  
   * The entry's Timestamp  
   * The entry's Value or Values  
   * The entry's Tag (optional)  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we append a time series two entries in a single transaction.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we append a time series a hundred entries in a single transaction.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we append a time series two saets of values in a single transaction.  
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
