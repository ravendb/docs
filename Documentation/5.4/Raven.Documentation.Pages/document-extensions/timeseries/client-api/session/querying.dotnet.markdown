# Time Series Querying 
---

{NOTE: }

* Time series data can be effectively queried in RavenDB, 
  allowing users to access and analyze information based on specific time intervals.
  
* Time series queries can be made using:  
  * The high-level `Query` method utilizing LINQ,  
  * The lower-level API `DocumentQuery`,  
  * Or directly through [RQL](../../../../client-api/session/querying/what-is-rql),
    which can be provided to a `RawQuery` or executed from the Studio's [Query view](../../../../studio/database/queries/query-view).

* In this page:  
    * [Query](../../../../document-extensions/timeseries/client-api/session/querying#query)
        * [Query usage](../../../../document-extensions/timeseries/client-api/session/querying#query-usage)
        * [Query examples](../../../../document-extensions/timeseries/client-api/session/querying#query-examples)  
        * [Query syntax](../../../../document-extensions/timeseries/client-api/session/querying#query-syntax)
    * [DocumentQuery](../../../../document-extensions/timeseries/client-api/session/querying#documentquery)
        * [DocumentQuery usage](../../../../document-extensions/timeseries/client-api/session/querying#documentquery-usage)
        * [DocumentQuery examples](../../../../document-extensions/timeseries/client-api/session/querying#documentquery-examples)
        * [DocumentQuery Syntax](../../../../document-extensions/timeseries/client-api/session/querying#documentquery-syntax)
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

### Query usage

* Open a session  
* Call `session.Query`:
  * Extend the query using LINQ expressions
  * Provide a `Where` query predicate to locate documents whose time series you want to query  
  * Use `Select` to choose a time series and project time series data
  * Execute the query
* Results will be in the form:
  * `TimeSeriesRawResult` for non-aggregated data, or -  
  * `TimeSeriesAggregationResult` for aggregated data  
* Note:  
  The RavenDB client translates the LINQ query to [RQL](../../../../client-api/session/querying/what-is-rql) before transmitting it to the server for execution.

---

### Query examples

* This LINQ query filters users by their age and retrieves their HeartRates time series.  
  The first occurence of `Where` filters the documents.  
  The second `Where` filters the time series entries.
  {CODE-TABS}
{CODE-TAB:csharp:Query ts_region_LINQ-1-Select-Timeseries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Users" as q
where q.Age < 30
select timeseries(from q.HeartRates where (Tag == "watches/fitbit"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* In this example, we select a three-day range from the HeartRates time series.  
  {CODE-TABS}
{CODE-TAB:csharp:Query ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Users" as q
select timeseries(from q.HeartRates between "2020-05-17T00:00:00.0000000" and "2020-05-17T00:03:00.0000000")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* In this example, we retrieve a company's stock trade data.  
  Note the usage of named values, so we may address trade Volume [by name](../../../../document-extensions/timeseries/client-api/named-time-series-values).  
  {CODE-TABS}
{CODE-TAB:csharp:Native timeseries_region_Unnamed-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB:csharp:Named timeseries_region_Named-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

* In this example, we group heart-rate data of people above the age of 72 into 1-day groups,  
  and retrieve each group's average heart rate and number of measurements.  
  The aggregated results are retrieved as `List<TimeSeriesAggregationResult>`.
  {CODE ts_region_LINQ-6-Aggregation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

### Query syntax

* The `session.Query` syntax is available [here](../../../../client-api/session/querying/how-to-query#syntax).

* To define a time series query use `RavenQuery.TimeSeries` within the query `Select` clause.

* `RavenQuery.TimeSeries` overloads:

    {CODE RavenQuery-TimeSeries-Definition-Without-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE RavenQuery-TimeSeries-Definition-With-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

    | Parameter            | Type       | Description                             |
    |----------------------|------------|-----------------------------------------|
    | **documentInstance** | `object`   | Document Instance                       |
    | **name**             | `string`   | Time Series Name                        |
    | **from** (optional)  | `DateTime` | Range Start<br> Default: `DateTime.Min` |
    | **to** (optional)    | `DateTime` | Range End<br> Default: `DateTime.Max`   |

* `RavenQuery.TimeSeries` can be extended with the following time series methods:

    {CODE-BLOCK:csharp}
Offset(TimeSpan offset);
Scale(double value);
FromLast(Action<ITimePeriodBuilder> timePeriod);
FromFirst(Action<ITimePeriodBuilder> timePeriod);
LoadByTag<TEntity>();
GroupBy(string s);
GroupBy(Action<ITimePeriodBuilder> timePeriod);
Where(Expression<Func<TimeSeriesEntry, bool>> predicate);
    {CODE-BLOCK/}

{PANEL/}

{PANEL: DocumentQuery}

### DocumentQuery usage

* Open a session
* Call `session.Advanced.DocumentQuery`:
    * Extend the query using RavenDB's fluent API methods
    * Provide a `WhereEquals` query predicate to locate documents whose time series you want to query
    * Use `SelectTimeSeries` to choose a time series and project time series data
    * Execute the query
* Results will be in the form:
    * `TimeSeriesRawResult` for non-aggregated data, or -
    * `TimeSeriesAggregationResult` for aggregated data
* Note:  
  The RavenDB client translates query to [RQL](../../../../client-api/session/querying/what-is-rql) before transmitting it to the server for execution.

---

### DocumentQuery examples

* A _DocumentQuery_ using only the `From()` method.  
  The query returns all entries from the 'HeartRates' time series.
  {CODE TS_DocQuery_1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* A _DocumentQuery_ using `Between()`.  
  The query returns only entries from the specified time range.
  {CODE TS_DocQuery_2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* A _DocumentQuery_ using `FromFirst()`.  
  The query returns the first three days of the 'HeartRates' time series.  
  {CODE TS_DocQuery_3@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* A _DocumentQuery_ using `FromLast()`.  
  The query returns the last three days of the 'HeartRates' time series.  
  {CODE TS_DocQuery_4@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* A _DocumentQuery_ that loads the related `Monitor` documents that are specified in the time entries tags.  
  The results are then filtered by their content. 
  {CODE-TABS}
{CODE-TAB:csharp:Query TS_DocQuery_5@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB:csharp:Class TS_DocQuery_class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

---

### DocumentQuery syntax

The session [DocumentQuery](../../../../client-api/session/querying/document-query/what-is-document-query),
which is accessible from `session.Advanced`, can be extended with several useful time series methods.
To access these methods, begin with method `SelectTimeSeries()`:

{CODE-BLOCK: csharp}
IDocumentQuery SelectTimeSeries(Func<ITimeSeriesQueryBuilder, TTimeSeries> timeSeriesQuery);
{CODE-BLOCK/}

`SelectTimeSeries()` takes an `ITimeSeriesQueryBuilder`. The builder has the following methods:

{CODE-BLOCK: csharp}
From(string name);
Between(DateTime start, DateTime end);
FromLast(Action<ITimePeriodBuilder> timePeriod);
FromFirst(Action<ITimePeriodBuilder> timePeriod);
LoadByTag<TTag>();
//LoadByTag is extended by a special version of Where():
Where(Expression<Func<TimeSeriesEntry, TTag, bool>> predicate);
{CODE-BLOCK/}

| Parameter                  | Type                                            | Description                                                                                                                                                               |
|----------------------------|-------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **name**                   | `string`                                        | The name of the time series (in one or more documents) to query                                                                                                           |
| **start**                  | `DateTime`                                      | First parameter for `Between()`.<br>The beginning of the time series range to filter.                                                                                     |
| **end**                    | `DateTime`                                      | Second parameter for `Between()`.<br>The end of the time series range to filter.                                                                                          |
| **timePeriod**             | `Action<ITimePeriodBuilder>`                    | Expression returning a number of time units representing a time series range either at the beginning or end of the queried time series.                                   |
| `LoadByTag` type parameter | `TTag`                                          | Time series entry tags can be just strings, but they can also be document IDs, representing a reference to a related document. `LoadByTag` takes the type of the entity. |
| **predicate**              | `Expression<Func<TimeSeriesEntry, TTag, bool>>` |

`FromLast()` and `FromFirst()` take an `ITimePeriodBuilder`, which is used to represent a range of time from milliseconds to years:

{CODE-BLOCK: csharp}
public interface ITimePeriodBuilder
{
    Milliseconds(int duration);
    Seconds(int duration);
    Minutes(int duration);
    Hours(int duration);
    Days(int duration);
    Months(int duration);
    Quarters(int duration);
    Years(int duration);
}
{CODE-BLOCK/}

#### Return Value:

* **`List<TimeSeriesAggregationResult>`**  for aggregated data.  
  When the query [aggregates time series entries](../../../../document-extensions/timeseries/querying/aggregation-and-projections),
  the results are returned in an aggregated array.

* **`List<TimeSeriesRawResult>`** for non-aggregated data.  
  When the query doesn't aggregate time series entries, the results are returned in a list of time series results.

{PANEL/}

{PANEL: RawQuery}

### RawQuery usage

* Open a session  
* Call `session.Advanced.RawQuery`, pass it the raw RQL that will be sent to the server 
* Results will be in the form:  
   * `TimeSeriesRawResult` for non-aggregated data, or -
   * `TimeSeriesAggregationResult` for aggregated data
* Note:  
  The raw query transmits the provided RQL to the server as is, without checking or altering its content.

---

### RawQuery examples

* In this example, we retrieve all HearRates time series for all users under 30.
  {CODE-TABS}
{CODE-TAB:csharp:RawQuery ts_region_LINQ-2-RQL-Equivalent@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

* In this example, a raw RQL query retrieves 24 hours of heart rate data from users under the age of 30.  
  The query does not aggregate data, so results are in the form of a `TimeSeriesRawResult` list.  
  We define an **offset**, to adjust retrieved results to the client's local time-zone.
  {CODE-TABS}
{CODE-TAB:csharp:Declare-Syntax ts_region_Raw-Query-Non-Aggregated-Declare-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB:csharp:Select-Syntax ts_region_Raw-Query-Non-Aggregated-Select-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

* In this example, the query aggregates 7 days of HeartRates entries into 1-day groups.  
  From each group, two values are selected and projected to the client:  
  the **min** and **max** hourly HeartRates values.  
  The aggregated results are in the form of a `TimeSeriesAggregationResult` list.
  {CODE ts_region_Raw-Query-Aggregated@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

### RawQuery syntax

{CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter  | Type     | Description          |
|------------|----------|----------------------|
|  **query** | `string` | The RQL query string |

**Return Value**:

* **`List<TimeSeriesAggregationResult>`**  for aggregated data.  
  When the query [aggregates time series entries](../../../../document-extensions/timeseries/querying/aggregation-and-projections),
  the results are returned in an aggregated array.

* **`List<TimeSeriesRawResult>`** for non-aggregated data.  
  When the query doesn't aggregate time series entries, the results are returned in a list of time series results.

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
