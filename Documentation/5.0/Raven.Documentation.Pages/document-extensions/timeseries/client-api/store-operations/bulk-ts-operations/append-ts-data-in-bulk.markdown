## `BulkInsert.TimeSeriesFor.Append`
# Append Time Series In Bulk

---

{NOTE: }

* `store.BulkInsert` is RavenDB's high-performance data insertion operation.  
  Its `TimeSeriesFor` interface resembles [session.TimeSeriesFor](../../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data) 
  in its ability to append multiple time series entries in a single transaction, 
  while omitting the liabilities of the `session` interface to gain much higher speed.  

* In this page:  
   * [`BulkInsert.TimeSeriesFor.Append`](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#bulkinsert.timeseriesfor.append)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#syntax)  
      * [Overload 1 - Bulk-Insert Single-Value Entries](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value)  
      * [Overload 2 - Bulk-Insert Multiple-Values Entries](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-samples)  

{NOTE/}

{PANEL: `BulkInsert.TimeSeriesFor.Append`}

Create a `BulkInsert` instance, pass its `TimeSeriesFor` method the document's ID 
and time series name, and then call `TimeSeriesFor.Append` as many times as you need 
to. All the Append operations you perform this way will be executed in 
a single transaction.  

{PANEL/}

{PANEL: Syntax}

* **`TimeSeriesFor` Definition**
    {CODE-BLOCK: JSON}
    public TimeSeriesBulkInsert TimeSeriesFor(string id, string name)
    {CODE-BLOCK/}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `id` | `string` | Document ID |
    | `name` | `string` | Time Series Name |

* **Return Value**: **`TimeSeriesBulkInsert`**  

---

* There are two Append overloads:  
   * [Overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) - 
     Each appended entry has a single value.  
   * [Overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values) - 
     Each appended entry has multiple values.  

---

#### Overload 1: Each appended entry has a single value  

* **Definition**
    {CODE-BLOCK:JSON}
    public void Append(DateTime timestamp, double value, string tag = null)
    {CODE-BLOCK/}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | `DateTime` | TS-entry's timestamp |
    | `value` | `double` | A single value |
    | `tag` | `string` | TS-entry's tag (optional) |

* **Return Value**: **`void`**  

---

#### Overload 2: Each appended entry has multiple values  

* **Definition**
    {CODE-BLOCK:JSON}
    public void Append(DateTime timestamp, ICollection<double> values, string tag = null)
    {CODE-BLOCK/}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | `DateTime` | TS-entry's timestamp |
    | `values` | `ICollection<double>` | Multiple values |
    | `tag` | `string` | TS-entry's tag (optional) |

* **Return Value**: **`void`**  

{PANEL/}

{PANEL: Usage Flow}

* Create a `store.BulkInsert` instance.  
* Pass the instance's `TimeSeriesFor` -  
  **The document ID**  
  **The time series name**  
* Call `TimeSeriesFor.Append` as many times as you like. Pass it -  
   * The appended entry's **timestamp**  
   * **A single value** ([overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value))  
     -or-  
     **Multiple values** ([overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values))  
   * The appended entry's **tag** (optional)  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we use [overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) 
  to append a time series two entries in a single transaction.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use [overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) 
  to append a time series a hundred entries in a single transaction.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use [overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values) 
  to append a time series two saets of values in a single transaction.  
   {CODE BulkInsert-overload-2-Two-HeartRate-Sets@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/api-overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/queries-overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
