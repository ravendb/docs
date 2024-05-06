# Include Time Series with&nbsp;`Load`
---

{NOTE: }

* When loading a document via `session.load`,  
  you can _include_ all entries of a time series or a specific range of entries.

* The included time series data is stored within the session   
  and can be provided instantly when requested without any additional server calls.

* In this page:
   * [Include time series when loading a document](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load#include-time-series-when-loading-a-document)
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load#syntax)

{NOTE/}

---

{PANEL: Include time series when loading a document}

In this example, we load a document using `session.load`,  
and include a selected range of entries from time series "HeartRates".

{CODE:nodejs include_1@documentExtensions\timeSeries\client-api\includeWithLoad.js /}

{PANEL/}

{PANEL: Syntax}

**`session.load`**

{CODE:nodejs syntax_1@documentExtensions\timeSeries\client-api\includeWithLoad.js /}

| Parameter    | Type     | Description                                                                                   |
|--------------|----------|-----------------------------------------------------------------------------------------------|
| **id**       | `string` | Document ID to load.                                                                          |
| **options**  | `object` | object containing the `includes` builder that specifies which time series entries to include. |

**`includes`** builder methods:

{CODE:nodejs syntax_2@documentExtensions\timeSeries\client-api\includeWithLoad.js /}

| Parameter | Type     | Description                                                          |
|-----------|----------|----------------------------------------------------------------------|
| **name**  | `string` | Time series name.                                                    |
| **from**  | `Date`   | Time series range start (inclusive).<br>Default: minimum date value. |
| **to**    | `Date`   | Time series range end (inclusive).<br>Default: maximum date value.   |

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
