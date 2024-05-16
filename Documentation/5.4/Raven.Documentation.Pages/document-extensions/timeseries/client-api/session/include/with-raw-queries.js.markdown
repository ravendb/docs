# Include Time Series with Raw Queries
---

{NOTE: }

* Use `include timeseries` in your RQL expression in order to include time series data when making  
  a raw query via [session.advanced.rawQuery](../../../../../client-api/session/querying/how-to-query#session.advanced.rawquery).

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

{CODE:nodejs include_1@documentExtensions\timeSeries\client-api\includeWithRawQuery.js /}

{PANEL/}

{PANEL: Syntax}

**`advanced.rawQuery`**

{CODE:nodejs syntax@documentExtensions\timeSeries\client-api\includeWithRawQuery.js /}

| Parameter          | Type     | Description                                  |
|--------------------|----------|----------------------------------------------|
| **query**          | `string` | The raw RQL query.                           |
| **documentType**   | `object` | The document class type (an optional param). |

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
