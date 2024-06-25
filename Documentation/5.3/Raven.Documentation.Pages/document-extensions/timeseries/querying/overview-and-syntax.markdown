# Querying: Time Series Querying Overview & Syntax

---

{NOTE: }

* Time series querying is native to RavenDB's RQL.  
  
* Clients can express time series queries in RQL and LINQ expressions to, 
  for example, expose the behavior of a process that populates a time series 
  over time, and to locate documents related to chosen time series entries.  

* Queries can be executed over time series indexes.  

* In this page:  
  * [Time Series Queries](../../../document-extensions/timeseries/querying/overview-and-syntax#time-series-queries)
  * [Server and Client Queries](../../../document-extensions/timeseries/querying/overview-and-syntax#server-and-client-queries)  
  * [Dynamic and Indexed Queries](../../../document-extensions/timeseries/querying/overview-and-syntax#dynamic-and-indexed-queries)  
  * [Scaling Query Results](../../../document-extensions/timeseries/querying/overview-and-syntax#scaling-query-results)  
  * [Syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax)  
     * [`select timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-creating-a-time-series-section)  
     * [`declare timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-declaring-a-time-series-function)  
     * [Combine Time Series and Javascript Functions](../../../document-extensions/timeseries/querying/overview-and-syntax#combine-time-series-and-javascript-functions)  
     * [Use Studio To Experiment](../../../document-extensions/timeseries/querying/overview-and-syntax#use-studio-to-experiment)  

{NOTE/}

---

{PANEL: Time Series Queries}

Time series query can -  

* [Choose a range of time series entries](../../../document-extensions/timeseries/querying/choosing-query-range) 
  to query from.  
* [Filter](../../../document-extensions/timeseries/querying/filtering) 
  time series entries by their tags, values and timestamps.  
* [Aggregate](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
  time series entries into groups by a chosen time resolution, e.g. gather the prices 
  of a stock that's been collected over the past two months to week-long groups. Entries can 
  also be aggregated by their tags.  
* Select entries by various criteria, e.g. by the min and max values of each aggregated group, 
  and [project](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
  them to the client.  
* Calculate [statistical measures](../../../document-extensions/timeseries/querying/statistics): 
  the percentile, slope, or standard deviation of a time series.  

{PANEL/}

{PANEL: Server and Client Queries}

Time series queries are executed by the server and their results are projected 
to the client, so they require very little client computation resources.  

* The server runs time series queries using RQL.  
* Clients can phrase time series queries in **raw RQL** or using **LINQ expressions** 
  (which will be automatically translated to RQL before their execution by the server).  

{PANEL/}

{PANEL: Dynamic and Indexed Queries}

Time series indexes are not created automatically by the server when making a dynamic query.  
Static time series indexes can be created by clients (or using the Studio).  

* Use **dynamic queries** when time series you query are not indexed,  
  or when you prefer that RavenDB would choose an index automatically.  
  See [queries always use an index](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index). E.g. -
   {CODE-BLOCK: javascript}
//Look for time series named "HeartRates" in user profiles of users born after 1990
from Employees as e 
where Birthday > '1990-01-01'
select timeseries(
    from HeartRates
)
   {CODE-BLOCK/}

* [Indexed queries](../../../document-extensions/timeseries/querying/using-indexes) 
  can be performed over static indexes and their results. E.g. -
   {CODE-BLOCK: javascript}
from index 'SimpleIndex'
where Tag = 'watches/fitbit'
   {CODE-BLOCK/}

{PANEL/}

{PANEL: Scaling Query Results}

Time series query results can be **scaled**, multiplied by some number. This doesn't 
change the values themselves, only the output of the query. This can serve as a stage 
in a data processing pipeline, or just for the purposes of displaying the data in a 
more undertandable format.  

There are many different use cases for this. For example, suppose your time series
records the changing speeds of different vehicles as they travel through a city, 
but some of your data is in miles per hour, and some of it in kilometers per hour. In 
this case scaling can be used for unit conversion.  

Another use case has to do with the compression of time series data. Numbers with 
very high precision (i.e., many digits after the decimal point) are less compressible 
than numbers with low precision. So for the purpose of storage, you might want to 
change a value like `0.000018` to `18`. Then, when you query the data, you can scale 
by `0.000001` to restore the original value.  

Scaling is a part of both RQL and LINQ syntax:  

* In **RQL**, use `scale <double>` in a time series query, and input your scaling 
  factor as a double.  
* In **LINQ**, use `.Scale(<double>)`.  

---

#### Examples:  

{CODE-TABS}
{CODE-TAB-BLOCK:sql:RQL}
from Patients
select timeseries(
    from HeartRate
    scale 60
)
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:csharp:LINQ}
var query = session.Query<Patient>()
    .Select(p => RavenQuery.TimeSeries(p, "HeartRate")
    .Scale(60)
    .ToList());
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

You can query time series using two equivalent syntaxes, 
choose the syntax you're comfortable with.  

* [`select timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-creating-a-time-series-section)  
* [`declare timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-declaring-a-time-series-function)  

---

#### `select timeseries` Syntax: Creating a Time Series Section

This syntax allows you to encapsulate your query's time series functionality 
in a `select timeseries` section.  

{CODE-BLOCK: javascript}
//Look for time series named "HeartRate" in user profiles of users under 30.

from Employees as e 
where Birthday > '1990-01-01'
select timeseries(
    from HeartRate
)
{CODE-BLOCK/}

* `from Employees as e where Birthday > '1990-01-01'`  
  This **document query** locates the documents whose time series we want to query.  
  
    {INFO: }
    A typical time series query starts by locating a single document.  
    For example, to query a time series of stock prices, we can first 
    locate a specific company's profile in the Companies collection, 
    and then query the StockPrices time series that belongs to this profile.  
      {CODE-BLOCK: javascript}
      from Companies
      where Name = 'Apple'
      select timeseries(
          from StockPrices
      )
      {CODE-BLOCK/}
    {INFO/}

* `select timeseries`  
  The `select` clause defines the time series query.  

* `from HeartRate`  
  The `from` keyword is used to select the time series we'd query, by its name.  

---

#### `declare timeseries` Syntax: Declaring a Time Series Function

This syntax allows you to declare a time series function and call it 
from your query. It introduces greater flexibility to your queries as 
you can, for example, pass arguments to/by the time series function.  

Here is a query in both syntaxes. It picks users whose age is under 30, 
and if they own a time series named "HeartRate", retrieves a range of 
its entries.  


| With Time Series Function | Without Time Series Function |
|:---:|:---:|
| {CODE-BLOCK: javascript}
declare timeseries ts(jogger){
    from jogger.HeartRate 
    between 
       '2020-05-27T00:00:00.0000000Z'
      and 
       '2020-06-23T00:00:00.0000000Z'
}

from Users as jog where Age < 30
select ts(jog)
{CODE-BLOCK/}| {CODE-BLOCK: javascript} 
 from Users as jog where Age < 30
 select timeseries(
    from HeartRate 
    between 
       '2020-05-27T00:00:00.0000000Z'
      and 
       '2020-06-23T00:00:00.0000000Z')
    {CODE-BLOCK/}|

---

#### Combine Time Series and Javascript Functions

You can declare and use both **time series functions** and 
[custom JavaScript functions](../../../client-api/session/querying/what-is-rql#declare) 
in a query.  
JavaScript functions can then call time series functions, pass them arguments, 
use and manipulate their results.  

{NOTE: }
Custom Javascript functions return a flat set of values rather than a nested 
array, to ease the projection of retrieved values.  
{NOTE/}

**In the sample below**, a Javascript function is called by the query's 
`select` clause to fetch and format a set of time series values, which are then 
projected by the query.  
To retrieve the values, the Javascript function calls the time series function.  

{CODE-TABS}
{CODE-TAB-BLOCK:javascript:RQL}
declare timeseries retrieveHeartRateValues(e)
{
    from e.HeartRates
}

declare function ts(e) {
    // Call time series function to retrieve employees heartrate values
    var r = retrieveHeartRateValues(e);
    var results = [];
    // structure the results
    for(var i = 0 ; i < r.Results.length; i ++) {
        results.push({
            Timestamp: r.Results[i].Timestamp, 
            Value: r.Results[i].Values[0].toFixed(2), 
            Tag: r.Results[i].Tag  ?? "default"})
    }
    return results;
}

from Employees as e
// Call the custom Javascript function to get a structure of values to project
select ts(e)
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:LINQ DefineCustomFunctions@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

This is the custom `ModifiedTimeSeriesEntry` class we use in the LINQ sample:  
{CODE DefineCustomFunctions_ModifiedTimeSeriesEntry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Use Studio To Experiment

You can use [Studio](../../../studio/database/document-extensions/time-series) 
to try the samples provided here and test your own queries.  

!["Time Series Query in Studio"](images/time-series-query.png "Time Series Query in Studio")

{PANEL/}


## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Time Series Indexing**  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Time Series Queries**  
[Range Selection](../../../document-extensions/timeseries/querying/choosing-query-range)  
[Filtering](../../../document-extensions/timeseries/querying/filtering)  
[Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  
[Indexed Time Series Queries](../../../document-extensions/timeseries/querying/using-indexes)  
[Statistical Measures](../../../document-extensions/timeseries/querying/statistics)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
