# Querying: Choosing Time Series Range

---

{NOTE: }

* Queries can be performed over whole time series or over a chosen range 
  of time series entries, e.g. only entries collected during the last 7 days.  
    
* In this page:  
  * [Choosing Query Range](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-query-range)
      * [`first` and `last`](../../../document-extensions/timeseries/querying/choosing-query-range#and-)
  * [Client Usage Examples](../../../document-extensions/timeseries/querying/choosing-query-range#client-usage-examples)
      * [Choosing a Range Using LINQ](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-a-range-using-linq)
      * [Choosing a Range Using Raw RQL](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-a-range-using-raw-rql)

{NOTE/}

---

{PANEL: Choosing Query Range}

In an RQL query, use `between` and `and` to specify a range of time series 
entries to query. The entries are chosen by their timestamps, in UTC format.  

{CODE-BLOCK: sql}
from Users as jog where Age < 30
select timeseries(
   from HeartRate 
   between 
      '2020-05-27T00:00:00.0000000Z'
     and 
      '2020-06-23T00:00:00.0000000Z'
{CODE-BLOCK/}
  
  * `between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z'`  
    Retrieve all entries between these two timestamps.  
      
      {INFO: }
      You can use the Studio to try these queries.  
      Using the studio, you can use parameters for a clearer query.  
      E.g. -  
      {CODE-BLOCK: sql}
      $from = '2020-05-27T00:00:00.0000000Z'
$to = '2020-06-23T00:00:00.0000000Z'

from Users as jog where Age < 30
select timeseries(
   from HeartRate 
   between $from and $to
)
      {CODE-BLOCK/}
      {INFO/}

#### `first` and `last`

You can use the keywords `first` and `last` to specify a range of entries at the 
beginning or end of the time series. The range is specified using a whole 
number of one of these units:  

* **Seconds**  
* **Minutes**  
* **Hours**  
* **Days**  
* **Months**  
* **Quarters**  
* **Years**  

Within the `select timeseries` clause of the query, write e.g. `first 1 second` or 
`last 3 quarters`.

{CODE-BLOCK: sql}
from Users
select timeseries(
   from HeartRates 
   last 1 day
)
{CODE-BLOCK/}

{PANEL/}

{PANEL: Client Usage Examples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  

* Learn how to run a LINQ time series query [here](../../../document-extensions/timeseries/client-api/session/querying#time-series-linq-queries).  
* Learn how to run a raw RQL time series query [here](../../../document-extensions/timeseries/client-api/session/querying#client-raw-rql-queries).  

{INFO/}

---

### Choosing a Range Using LINQ

To choose a range as part of a LINQ query, pass `RavenQuery.TimeSeries` 
a `from` and a `to` DateTime values.  
Omitting these values will load the entire series.  

* **`RavenQuery.TimeSeries` Definitions**  
   {CODE RavenQuery-TimeSeries-Definition-Without-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE RavenQuery-TimeSeries-Definition-With-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
            

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `documentInstance` | `object` | Document Instance |
    | `name` | `string` | Time Series Name |
    | `from` (optional) | `DateTime` | Range Start <br> Default: `DateTime.Min` |
    | `to` (optional) | `DateTime` | Range End <br> Default: `DateTime.Max` |
  
* In this example, we select a three-days range from the HeartRate time series.
  {CODE ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

#### `FromFirst()` and `FromLast()`

To select only the first or last entries, use the methods `FromFirst()` or `FromLast()` 
(only one of them can be used at a time).  

{CODE-BLOCK: csharp}
ITimeSeriesQueryable FromLast(Action<ITimePeriodBuilder> timePeriod);

ITimeSeriesQueryable FromFirst(Action<ITimePeriodBuilder> timePeriod);
{CODE-BLOCK/}

In this example, we select only the entries in the last 30 minutes of the time series 
"HeartRates".  

{CODE-BLOCK: csharp}
var result = session.Query<Person>()
    .Select(p => 
    RavenQuery.TimeSeries(p, "HeartRates")
        .FromLast(g => g.minutes(30))
        .ToList());
{CODE-BLOCK/}

---

### Choosing a Range Using Raw RQL

To choose a range as part of a raw RQL query, use the `between` and `and` keywords.  

In this example, a raw RQL query chooses the profiles of users under the age of 30 and 
retrieves a 24-hours range from each.  
An **offset** is defined, adding two hours to retrieved timestamps to adjust them 
to the client's local time zone.  
 {CODE-TABS}
 {CODE-TAB:csharp:Declare-Syntax ts_region_Raw-Query-Non-Aggregated-Declare-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
 {CODE-TAB:csharp:Select-Syntax ts_region_Raw-Query-Non-Aggregated-Select-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
 {CODE-TABS/}

#### `first` and `last`

To select only the first or last entries, use the keywords `first` or `last` 
(only one of them can be used at a time). Within the `select timeseries` 
clause of the query, write e.g. `first 1 second` or `last 3 quarters`.  

In this example, we select only the entries in the last 12 hours of the time series 
"HeartRates".  

{CODE-BLOCK: sql}
from People as doc
select timeseries(
    from doc.HeartRates 
    last 12h
    group by 1h
    select min(), max(), avg()
)
{CODE-BLOCK/}

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
