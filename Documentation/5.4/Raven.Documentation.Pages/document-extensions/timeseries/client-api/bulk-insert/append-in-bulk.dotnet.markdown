# Append Time Series with Bulk Insert

---

{NOTE: }

* `store.BulkInsert` is RavenDB's high-performance data insertion operation.  

* The `bulkInsert.TimeSeriesFor` interface provides similar functionality to the [session.TimeSeriesFor](../../../../document-extensions/timeseries/client-api/session/append),  
  but without the overhead associated with the _Session_, resulting in significantly improved performance.

* In this page:  
  * [Usage](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#usage)  
  * [Examples](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#examples)
      * [Append single entry](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#append-single-entry)
      * [Append multiple entries](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#append-multiple-entries)
      * [Append multiple values per entry](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#append-multiple-values-per-entry)
      * [Append multiple time series](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#append-multiple-time-series) 
  * [Syntax](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#syntax)

{NOTE/}

{PANEL: Usage}

**Flow**:

* Call `store.BulkInsert` to create a `BulkInsertOperation` instance.
* Call `TimeSeriesFor` on that instance and pass it:
    * The document ID  
      (An exception will be thrown if the specified document does Not exist).
    * The time series name  
      (Appending entries to a time series that doesn't yet exist yet will create the time series).
* To append an entry, call `Append` and pass it:
    * The entry's Timestamp
    * The entry's Value or Values
    * The entry's Tag (optional)

**Note**:

* To append multiple entries, call `Append` as many times as needed.
* Ensure there is at least a 1-millisecond interval between each timestamp.
* The client converts all timestamps to **UTC** before sending the batch to the server.
* Multiple time series can be appended in the same `BulkInsertOperation`. See this [example](../../../../document-extensions/timeseries/client-api/bulk-insert/append-in-bulk#append-multiple-time-series) below.

{PANEL/}

{PANEL: Examples}

#### Append single entry:

In this example, we append a single entry with a single value to time series "HeartRates". 
{CODE timeseries_region_Use-BulkInsert-To-Append-single-entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Append multiple entries:

In this example, we append 100 entries with a single value to time series "HeartRates". 
{CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Append multiple values per entry:

In this example, we append multiple values per entry in time series "HeartRates".
{CODE BulkInsert-overload-2-Two-HeartRate-Sets@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

#### Append multiple time series:

In this example, we append multiple time series in different documents in the same batch.
{CODE timeseries_region_Use-BulkInsert-To-Append-multiple-timeseries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Syntax}

**`BulkInsert.TimeSeriesFor`**

{CODE-BLOCK: JSON}
public TimeSeriesBulkInsert TimeSeriesFor(string id, string name)
{CODE-BLOCK/}

| Parameter   | Type     | Description      |
|-------------|----------|------------------|
| **id**      | `string` | Document ID      |
| **name**    | `string` | Time Series Name |

**`TimeSeriesFor.Append`** overloads:

{CODE Append-Operation-Definition-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE Append-Operation-Definition-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter     | Type                  | Description               |
|---------------|-----------------------|---------------------------|
| **timestamp** | `DateTime`            | TS-entry's timestamp      |
| **value**     | `double`              | A single value            |
| **values**    | `ICollection<double>` | Multiple values           |
| **tag**       | `string`              | TS-entry's tag (optional) |

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
