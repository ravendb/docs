# Time Series Querying 
---

{NOTE: }

* Time series data can be effectively queried in RavenDB, 
  allowing users to access and analyze information based on specific time intervals.
  
* Time series queries can be made using:  
  * The `query` method 
  * Or directly through [RQL](../../../../client-api/session/querying/what-is-rql),
    which can be provided to a `rawQuery` or executed from the Studio's [Query view](../../../../studio/database/queries/query-view).

* In this page:  
    * [Query](../../../../document-extensions/timeseries/client-api/session/querying#query)
        * [Query usage](../../../../document-extensions/timeseries/client-api/session/querying#query-usage)
        * [Query examples](../../../../document-extensions/timeseries/client-api/session/querying#query-examples)  
        * [Query syntax](../../../../document-extensions/timeseries/client-api/session/querying#query-syntax)
    * [RawQuery](../../../../document-extensions/timeseries/client-api/session/querying#rawquery)
        * [RawQuery usage](../../../../document-extensions/timeseries/client-api/session/querying#rawquery-usage)
        * [RawQuery examples](../../../../document-extensions/timeseries/client-api/session/querying#rawquery-examples)
        * [RawQuery syntax](../../../../document-extensions/timeseries/client-api/session/querying#rawquery-syntax)
    
{NOTE/}

{INFO: }
Learn more about time series queries in the [section dedicated to this subject](../../../../document-extensions/timeseries/querying/overview-and-syntax).  
{INFO/}

---

{PANEL: Query}

---

### Query usage

* Open a session  
* Call `session.query`:
  * Provide a query predicate to locate documents whose time series you want to query  
  * Use `selectTimeSeries` to choose a time series and project time series data
  * Execute the query
* Results will be in the form:
  * `TimeSeriesRawResult` for non-aggregated data, or -  
  * `TimeSeriesAggregationResult` for aggregated data  
* Note:  
  The RavenDB client translates the query to [RQL](../../../../client-api/session/querying/what-is-rql) before transmitting it to the server for execution.

---

### Query examples

{NOTE: }

This query filters users by their age and retrieves their HeartRates time series.

{CODE-TABS}
{CODE-TAB:nodejs:Query query_1@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "users"
where age < 30
select timeseries(
    from "HeartRates"
    where Tag == "watches/fitbit"
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: } 
In this example, we select a 5-minute range from the HeartRates time series.  

{CODE-TABS}
{CODE-TAB:nodejs:Query query_2@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Users"
select timeseries(
    from "HeartRates"
    between "2024-05-19T18:13:17.466Z" and "2024-05-19T18:18:17.466Z"
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

* In this example, we retrieve a company's stock trade data.  
* Note the usage of named values, so we may address trade Volume [by name](../../../../document-extensions/timeseries/client-api/named-time-series-values).  
* This example is based on the sample entries that were entered in [this example](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values).

{CODE-TABS}
{CODE-TAB:nodejs:Native query_3@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB:nodejs:Named query_4@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "companies"
where address.city == "New York"
select timeseries(
    from StockPrices
    between $start and $end
    where Tag == "AppleTech"
)
{"start":"2024-05-20T07:54:07.259Z","end":"2024-05-23T07:54:07.259Z"}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

* In this example, we group heart-rate data of people above the age of 72 into 1-day groups,
* For each group, we retrieve the number of measurements, the minimum, maximum, and average heart rate.

{CODE-TABS}
{CODE-TAB:nodejs:Native query_5@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "users"
where age > 72
select timeseries(
    from HeartRates between $start and $end
    where Tag == "watches/fitbit"
    group by '1 day'
    select count(), min(), max(), avg()
)
{"start":"2024-05-20T09:32:58.951Z","end":"2024-05-30T09:32:58.951Z"}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

---

### Query syntax

The `session.query` syntax is available [here](../../../../client-api/session/querying/how-to-query#syntax).

Extend the `session.query` method with `selectTimeSeries()`.

{CODE:nodejs syntax_1@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}

| Parameter             | Type                | Description                                                                     |
|-----------------------|---------------------|---------------------------------------------------------------------------------|
| **timeSeriesQuery**   | `(builder) => void` | The time series query builder                                                   |
| **projectionClass**   | `object`            | The query result type<br>`TimeSeriesRawResult` or `TimeSeriesAggregationResult` |

The time series query builder has one method:

{CODE:nodejs syntax_2@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}

| Parameter      | Type     | Description                                   |
|----------------|----------|-----------------------------------------------|
| **queryText**  | `string` | The time series query part, expressed in RQL. |

| Return value                    | Description                                                                                                               |
|---------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| `TimeSeriesRawResult[]`         | The returned value for non-aggregated data                                                                                |
| `TimeSeriesAggregationResult[]` | The returned value for [aggregated data](../../../../document-extensions/timeseries/querying/aggregation-and-projections) |

{CODE:nodejs syntax_3@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}

{PANEL/}

{PANEL: RawQuery}

---

### RawQuery usage

* Open a session  
* Call `session.advanced.rawQuery`, pass it the raw RQL that will be sent to the server 
* Results will be in the form:  
    * `TimeSeriesRawResult` for non-aggregated data, or -
    * `TimeSeriesAggregationResult` for aggregated data
* Note:  
  The raw query transmits the provided RQL to the server as is, without checking or altering its content.

---

### RawQuery examples

{NOTE: }

In this example, we retrieve all HearRates time series for all users under 30.

{CODE-TABS}
{CODE-TAB:nodejs:RawQuery query_6@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from users where age < 30
select timeseries(
    from HeartRates
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

* In this example, a raw RQL query retrieves 24 hours of heart rate data from users under 30.  
* The query does not aggregate data, so results are in the form of a `TimeSeriesRawResult` list.  
* We define an **offset**, to adjust retrieved results to the client's local time-zone.

{CODE-TABS}
{CODE-TAB:nodejs:Declare-Syntax query_7@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB:nodejs:Select-Syntax query_8@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
// declare syntax
// ==============

declare timeseries getHeartRates(user)
{
    from user.HeartRates
    between $start and $end
    offset '03:00'
}

from users as u where age < 30
select getHeartRates(u)
{"start":"2024-05-20T11:52:22.316Z","end":"2024-05-21T11:52:22.316Z"}

// select syntax
// =============

from Users as u where Age < 30
select timeseries (
    from HeartRates 
        between $start and $end
        offset "03:00"
)
{"start":"2024-05-20T11:55:56.701Z","end":"2024-05-21T11:55:56.701Z"}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

* In this example, the query aggregates 7 days of HeartRates entries into 1-day groups.  
* From each group, two values are selected and projected to the client:  
  the **min** and **max** hourly HeartRates values.  
* The aggregated results are in the form of a `TimeSeriesAggregationResult` list.

{CODE-TABS}
{CODE-TAB:nodejs:RawQuery query_9@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from users as u
select timeseries(
    from HeartRates between $start and $end
    group by '1 day'
    select min(), max()
    offset "03:00"
)
{"start":"2024-05-20T12:06:40.595Z","end":"2024-05-27T12:06:40.595Z"}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

---

### RawQuery syntax

{CODE:nodejs syntax_4@documentExtensions\timeSeries\client-api\queryTimeSeries.js /}

| Parameter  | Type     | Description          |
|------------|----------|----------------------|
|  **query** | `string` | The RQL query string |

The return value is the same as listed under the [query syntax](../../../../document-extensions/timeseries/client-api/session/querying#query-syntax).

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
