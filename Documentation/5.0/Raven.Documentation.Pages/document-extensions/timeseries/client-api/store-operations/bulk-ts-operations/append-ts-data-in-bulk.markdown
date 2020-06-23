## `BulkInsert.TimeSeriesFor.Append`
# Append Time-Series In Bulk

---

{NOTE: }

To add a large quantity of time-series entries to your database, 
use `BulkInsert.TimeSeriesFor.Append`.  

* In this page:  
   * [`BulkInsert.TimeSeriesFor.Append`](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#bulkinsert.timeseriesfor.append)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#syntax)  
      * [Overload 1 - Bulk-Insert Single-Value Entries](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value)  
      * [Overload 2 - Bulk-Insert Multiple-Values Entries](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-samples)  

{NOTE/}

{PANEL: `BulkInsert.TimeSeriesFor.Append`}

The `store.BulkInsert` operation is highly efficient in appending large quantities 
of data to the database. To append time-series data in bulk, use the operation's 
`TimeSeriesFor.Append` method.  

{PANEL/}

{PANEL: Syntax}

There are two bulk-insert Append methods:  
[Overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) 
- Each appended entry has a single value.  
[Overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values) 
- Each appended entry has multiple values.  

---

#### Overload 1: Each appended entry has a single value  

* **Definition**
  {CODE BulkInsert-Append-Single-Value-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | `DateTime` | TS-entry's timestamp |
    | `value` | `double` | Single TS-entry value |
    | `tag` | `string` | TS-entry's tag (optional) |

* **Return Value**: **`void`**  

---

#### Overload 2: Each appended entry has multiple values  

* **Definition**
  {CODE BulkInsert-Append-Multiple-Values-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | `DateTime` | TS-entry's timestamp |
    | `values` | `IEnumerable<double>` | Multiple TS-entry values |
    | `tag` | `string` | TS-entry's tag (optional) |

* **Return Value**: **`void`**  

{PANEL/}

{PANEL: Usage Flow}

* Create a `store.BulkInsert` operation.  
* Create an instance of the operation's `TimeSeriesFor` interface.  
* Pass `TimeSeriesFor`'s constructor the document ID and time-series name  
* Call `TimeSeriesFor.Append`.  
  Pass it -  
   * The appended entry's **timestamp**  
   * **A single value** ([overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value))  
     -or-  
     **Multiple values** ([overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values))  
   * The appended entry's **tag** (optional)  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we use [overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) 
  to append a time-series two entries.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use [overload 1](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-1-each-appended-entry-has-a-single-value) 
  to append a time-series a hundred entries.  
   {CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use [overload 2](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#overload-2-each-appended-entry-has-multiple-values) 
  to append a time-series a hundred multi-values entries.  
   {CODE BulkInsert-overload-2-Append-100-Multi-Value-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
