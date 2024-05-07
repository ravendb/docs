# Include Time Series with&nbsp;`Load`
---

{NOTE: }

* When loading a document via `session.Load`,  
  you can _include_ all entries of a time series or a specific range of entries.

* The included time series data is stored within the session   
  and can be provided instantly when requested without any additional server calls.

* In this page:
   * [Include time series when loading a document](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load#include-time-series-when-loading-a-document)
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load#syntax)

{NOTE/}

---

{PANEL: Include time series when loading a document}

In this example, we load a document using `session.Load`,  
and include a selected range of entries from time series "HeartRates".

{CODE timeseries_region_Load-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Syntax}

**`session.Load`**

{CODE Load-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter    | Type                         | Description    |
|--------------|------------------------------|----------------|
| **id**       | `string`                     | Document ID    |
| **includes** | `Action<IIncludeBuilder<T>>` | Include Object |

**`Include`** builder methods:

{CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter | Type        | Description                                                           |
|-----------|-------------|-----------------------------------------------------------------------|
| **name**  | `string`    | Time series name.                                                     |
| **from**  | `DateTime?` | Time series range start (inclusive).<br>Default: `DateTime.MinValue`. |
| **to**    | `DateTime?` | Time series range end (inclusive).<br>Default: `DateTime.MaxValue`.   |

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
