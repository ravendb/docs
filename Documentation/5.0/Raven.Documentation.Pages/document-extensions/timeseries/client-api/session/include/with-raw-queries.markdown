# Session: Include Time Series With Raw Queries

---

{NOTE: }

You can include time series data while running a raw RQL query 
via `session.Advanced.RawQuery`.  

* [Include Time Series Data with `Advanced.RawQuery`](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#include-time-series-data-with-advanced.rawquery)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#syntax)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries#usage-sample)  

{NOTE/}

---

{PANEL: Include Time Series Data with `Advanced.RawQuery`}

To include time series data while querying via `Advanced.RawQuery`, 
use the `include timeseries` expression in your RQL query.  

{PANEL/}

{PANEL: Syntax}

* **`Advanced.RawQuery`**  
   * **Definition**  
      {CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `query` | string | Raw RQL Query |

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.RawQuery`  
  Use `include timeseries` in your query  
* Pass `include timeseries` its arguments:  
   * **Time series name**  
   * **Range start**  
   * **Range end**  

{PANEL/}

{PANEL: Usage Sample}

In this sample, we use a raw query to retrieve a document 
and include entries from the document's "Heartrate" time series.  

{CODE timeseries_region_Raw-Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

The entries we Get after the query, are retrieved 
**from the session's cache**.  

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
