# Include Time Series with&nbsp;`query`
---

{NOTE: }

* When querying via `session.query` for documents that contain time series,  
  you can request to include their time series data in the server response.

* The included time series data is stored within the session   
  and can be provided instantly when requested without any additional server calls.

* In this page:  
   * [Include time series when making a query](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query#include-time-series-when-making-a-query)
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query#syntax)

{NOTE/}

---

{PANEL: Include time series when making a query}

In this example, we retrieve a document using `session.query`  
and _include_ entries from the document's "HeartRates" time series.

{CODE-TABS}
{CODE-TAB:nodejs:Query include_1@documentExtensions\timeSeries\client-api\includeWithQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "users"
where name = "John" 
include timeseries("HeartRates", null, null)
limit null, 1
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

**`include`** builder methods:

{CODE:nodejs syntax@documentExtensions\timeSeries\client-api\includeWithQuery.js /}

| Parameter | Type     | Description                                                          |
|-----------|----------|----------------------------------------------------------------------|
| **name**  | `string` | Time series Name.                                                    |
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
