# Choosing Time Series Range

---

{NOTE: }

* Queries can retrieve data from the entire time series or from a specific range of entries,  
  such as those collected in the last 7 days.

* For an overview of the available time series queries and their syntax, please refer to [Time series querying](../../../document-extensions/timeseries/client-api/session/querying).

* In this page:
    * [Choose range in a query](../../../document-extensions/timeseries/querying/choosing-query-range#choose-range-in-a-query)
      * [Specify range](../../../document-extensions/timeseries/querying/choosing-query-range#specify-range)
      * [Retrieve first or last entries](../../../document-extensions/timeseries/querying/choosing-query-range#retrieve-first-or-last-entries)
    * [Choose range - RQL syntax](../../../document-extensions/timeseries/querying/choosing-query-range#choose-range---rql-syntax)
      * [`between` and `and`](../../../document-extensions/timeseries/querying/choosing-query-range#and)
      * [`first` and `last`](../../../document-extensions/timeseries/querying/choosing-query-range#and-1)

{NOTE/}

---

{PANEL: Choose range in a query}

{NOTE: }

#### Specify range

* Provide 'from' & 'to' DateTime values to the time series query to retrieve entries only from that range (inclusive).
  Omitting these parameters will retrieve the entire series.

* The provided DateTime values are treated by the server as UTC.  
  The client does Not perform any conversion to UTC prior to sending the request to the server.

* Note: calling 'offset' will only adjust the timestamps in the returned results to your local time (optional).

* In this example, we specify a 10-minute range from which to retrieve "HeartRates" entries for UK employees.

{CODE-TABS}
{CODE-TAB:nodejs:Query choose_range_1@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB:nodejs:Query_using_params choose_range_2@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB:nodejs:RawQuery choose_range_3@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB:nodejs:RawQuery_using_params choose_range_4@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB-BLOCK:sql:RQL}
// RQL:
from "employees" as employee
where employee.Address.Country == "UK"
select timeseries(
    from employee.HeartRates
    between "2020-05-17T00:00:00.0000000"
    and "2020-05-17T00:10:00.0000000"
    offset "03:00"
)

// RQL with parameters:
from "employees"
where Address.Country = $p0
select timeseries(
    from HeartRates
    between $from and $to
    offset "03:00"
)
{
  "p0":   "UK",
  "from": "2020-05-17T00:00:00.0000000",
  "to":   "2020-05-17T00:10:00.0000000"
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

#### Retrieve first or last entries

* Use `first` to specify the time frame from the start of the time series.  
  Use `last` to specify the time frame from the end of the time series.  
  Only one of them can be used in the same query function.  

* In this example, we select only the entries in the last 30 minutes of time series "HeartRates".

{CODE-TABS}
{CODE-TAB:nodejs:Query choose_range_5@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB:nodejs:RawQuery choose_range_6@documentExtensions\timeSeries\querying\queryRange.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" as e 
select timeseries(
    from e.HeartRates
    last 30 min
    offset "03:00"
)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: Choose range - RQL syntax}

{NOTE: }

#### `between`&nbsp;and&nbsp;`and`

---

* Use the `between` and `and` keywords to retrieve time series entries from the specified range (inclusive).  
  Provide the timestamps in UTC format.
  E.g.:  

    {CODE-TABS}
{CODE-TAB-BLOCK:sql:RQL_select_syntax}
from "Employees"
where Address.Country == "UK"
select timeseries(
    from HeartRates
    between "2020-05-17T00:00:00.0000000Z" // start of range
    and "2020-05-17T01:00:00.0000000Z"     // end of range
)

// Results will include only time series entries within the specified range for employees from UK.
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:sql:RQL_declare_syntax}
declare timeseries getHeartRates(employee)
{
    from HeartRates
    between "2020-05-17T00:00:00.0000000Z" // start of range
    and "2020-05-17T01:00:00.0000000Z"     // end of range
}

from "Employees" as e
where e.Address.Country == "UK"
select getHeartRates(e) 

// Results will include only time series entries within the specified range for employees from UK.
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

* RQL queries can be run from the [query view](../../../studio/database/queries/query-view) in the Studio.  
  Using the Studio, you can use parameters in the following way for a clearer query.  
  
    {CODE-BLOCK:sql}
$from = "2020-05-17T00:00:00.0000000Z"
$to = "2020-05-17T01:00:00.0000000Z"

from "Employees"
where Address.Country == "UK"
select timeseries(
    from HeartRates
    between $from and $to  // using parameters
)
    {CODE-BLOCK/}

{NOTE/}

{NOTE: }

#### `first`&nbsp;and&nbsp;`last`

---

* Use `first` to specify the time frame from the start of the time series.  
  Use `last` to specify the time frame from the end of the time series.  
  Only one of them can be used in the same query function. E.g.:  

    {CODE-BLOCK: sql}
// Retrieve all entries from the last day, starting from the end of time series "HeartRates"
from "Employees"
select timeseries(
    from HeartRates
    last 1 day
)
    {CODE-BLOCK/}

    {CODE-BLOCK: sql}
// Retrieve the first 10 minutes of entries from the beginning of time series "HeartRates"
from "Employees"
select timeseries(
    from HeartRates
    first 10 min
)
    {CODE-BLOCK/}

* The range is specified using a whole number of one of the following units.  

    * **seconds**  ( seconds/ second / s )
    * **minutes**  ( minutes / minute / min )
    * **hours**    ( hours / hour / h )
    * **days**     ( days / day / d )
    * **months**   ( months / month / mon / mo )
    * **quarters** ( quarters / quarter / q )
    * **years**    ( years / year / y )
    * Note: **milliseconds** are currently not supported by 'first' and 'last' in a time series query.

{NOTE/}
{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Time Series Indexing**  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Time Series Queries**  
[Filtering](../../../document-extensions/timeseries/querying/filtering)  
[Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  
[Indexed Time Series Queries](../../../document-extensions/timeseries/querying/using-indexes)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
