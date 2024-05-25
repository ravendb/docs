# Get Time Series Operation
---

{NOTE: }

* Use `GetTimeSeriesOperation` to retrieve entries from a **single** time series.  
  You can also retrieve entries from a single time series using the Session's [TimesSeriesFor.Get](../../../../document-extensions/timeseries/client-api/session/get/get-entries) method.

* Use `GetMultipleTimeSeriesOperation` to retrieve entries from **multiple** time series.

* For a general _Operations_ overview, see [What are Operations](../../../../client-api/operations/what-are-operations).

* In this page:  
      * [Get entries - from single time series](../../../../document-extensions/timeseries/client-api/operations/get#get-entries---from-single-time-series)  
         * [Example](../../../../document-extensions/timeseries/client-api/operations/get#example)  
         * [Syntax](../../../../document-extensions/timeseries/client-api/operations/get#syntax)  
      * [Get entries - from multiple time series](../../../../document-extensions/timeseries/client-api/operations/get#get-entries---from-multiple-time-series)  
         * [Example](../../../../document-extensions/timeseries/client-api/operations/get#example-1)  
         * [Syntax](../../../../document-extensions/timeseries/client-api/operations/get#syntax-1)  

{NOTE/}

---

{PANEL: Get entries - from single time series}

---

### Example

In this example, we retrieve all entries from a single time series.  

{CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

### Syntax

{CODE GetTimeSeriesOperation-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter             | Type        | Description                                                                                                                                                           |
|-----------------------|-------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **docId**             | `string`    | Document ID                                                                                                                                                           |
| **timeseries**        | `string`    | Time series name                                                                                                                                                      |
| **from**              | `DateTime?` | Get time series entries starting from this timestamp (inclusive).<br> Default: `DateTime.MinValue`                                                                    |
| **to**                | `DateTime?` | Get time series entries ending at this timestamp (inclusive).<br> Default: `DateTime.MaxValue`                                                                        |
| **start**             | `int`       | The position of the first result to retrieve (for paging)<br>Default: 0                                                                                                                                              |
| **pageSize**          | `int`       | Number of results per page to retrieve (for paging)<br>Default: `int.MaxValue`                                                                                                                                                  |
| **returnFullResults** | `bool`      | This param is only relevant when [getting incremental time series data](../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get) |

**Return Value**: 

{CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{INFO: }

* Details of class `TimeSeriesEntry ` are listed in [this syntax section](../../../../document-extensions/timeseries/client-api/session/get/get-entries#syntax).
* If the requested time series does Not exist, the returned object will be `null`.
* No exceptions are generated.

{INFO/}

{PANEL/}

{PANEL: Get entries - from multiple time series}

---

### Example

In this example, we retrieve entries from the specified ranges of two time series.

{CODE timeseries_region_Get-Multiple-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

### Syntax

{CODE GetMultipleTimeSeriesOperation-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter             | Type                           | Description                                                                                                                                                           |
|-----------------------|--------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **docId**             | `string`                       | Document ID                                                                                                                                                           |
| **ranges**            | `IEnumerable<TimeSeriesRange>` | Provide a `TimeSeriesRange` object for each time series from which you want to retrieve data                                                                          |
| **start**             | `int`                          | The position of the first result to retrieve (for paging)<br>Default: 0                                                                                               |
| **pageSize**          | `int`                          | Number of results per page to retrieve (for paging)<br>Default: `int.MaxValue`                                                                                        |
| **returnFullResults** | `bool`                         | This param is only relevant when [getting incremental time series data](../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get) |
 
{CODE TimeSeriesRange-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

**Return Value**:

{CODE TimeSeriesDetails-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* If any of the requested time series do not exist, the returned object will be `null`.

* When an entries range that does not exist are requested,  
  the return value for the that range is a `TimeSeriesRangeResult` object with an empty `Entries` property.  

* No exceptions are generated.

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
