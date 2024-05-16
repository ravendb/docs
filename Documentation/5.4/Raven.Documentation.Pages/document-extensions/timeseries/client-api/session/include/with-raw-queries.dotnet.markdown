# Include Time Series with Raw Queries
---

{NOTE: }

* Use `include timeseries` in your RQL expression in order to include time series data when making  
  a raw query via [session.Advanced.RawQuery](../../../../../client-api/session/querying/how-to-query#session.advanced.rawquery).

* The included time series data is stored within the session and can be provided instantly when requested  
  without any additional server calls.

* In this page:
   * [Include time series when making a raw query](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#include-time-series-when-making-a-raw-query)
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#syntax)

{NOTE/}

---

{PANEL: Include time series when making a raw query}

In this example, we use a raw query to retrieve a document   
and _include_ entries from the document's "HeartRates" time series.  

{CODE timeseries_region_Raw-Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Syntax}

**`Advanced.RawQuery`**

{CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter | Type     | Description       |
|-----------|----------|-------------------|
| **query** | `string` | The raw RQL query |

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
