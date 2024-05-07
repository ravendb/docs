# Include Time Series with&nbsp;`Query`
---

{NOTE: }

* When querying via `session.Query` for documents that contain time series,  
  you can request to include their time series data in the server response.

* The included time series data is stored within the session   
  and can be provided instantly when requested without any additional server calls.

* In this page:  
   * [Include time series when making a query](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query#include-time-series-when-making-a-query)
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query#syntax)

{NOTE/}

---

{PANEL: Include time series when making a query}

In this example, we retrieve a document using `session.Query`   
and _include_ its time series entries from time series "HeartRates".  

{CODE timeseries_region_Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Syntax}

**`Include`** builder methods:

{CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter | Type        | Description                                                           |
|-----------|-------------|-----------------------------------------------------------------------|
| **name**  | `string`    | Time series Name.                                                     |
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
