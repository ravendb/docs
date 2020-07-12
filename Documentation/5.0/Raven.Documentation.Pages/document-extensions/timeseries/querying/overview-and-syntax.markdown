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
  * [Syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax)  
     * [`select timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-creating-a-time-series-section)  
     * [`declare timeseries` syntax](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax-declaring-a-time-series-function)  


{NOTE/}

---

{PANEL: Time Series Queries}

Time series query can -  

* [Choose a range of time series entries](../../../document-extensions/timeseries/querying/choosing-query-range) 
  to query from.  
* [Filter](../../../document-extensions/timeseries/querying/filtering) 
  time series entries by their tags, values and timestamps.  
* [Aggregate](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
  time series entries into groups by a chosen resolution, e.g. gather the prices 
  of a stock that's been collected over the past two months to week-long groups).  
* Select entries by various criteria, e.g. by the min and max values of each aggregated group, 
  and [project](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
  them to the client.  

{PANEL/}

{PANEL: Server and Client Queries}

Time series queries are executed by the server and their results are projected 
to the client, so they require very little client computation resources.  

* The server runs time series queries using RQL.  
* Clients can phrase time series queries in **raw RQL** or using **LINQ expressions** 
  (which will be automatically translated to RQL before their execution by the server).  

{PANEL/}

{PANEL: Dynamic and Indexed Queries}

Time series indexes are not created automatically by the server, but static time series 
indexes can be created by clients (or using the Studio).  

* Use **dynamic queries** when time series you query are unindexed 
  or when you prefer that RavenDB would choose an index automatically 
  using its [query optimizer](../../../indexes/querying/what-is-rql#query-optimizer). E.g. - 
   {CODE-BLOCK: JSON}
//Look for time series named "HeartRate" in user profiles of users under 30.
from Users as u where Age < 30
    select timeseries(
    from HeartRate
)
   {CODE-BLOCK/}

* [Indexed queries](../../../document-extensions/timeseries/querying/indexed-queries) 
  can be performed over static indexes and their results. E.g. -
   {CODE-BLOCK: JSON}
from index 'SimpleIndex'
    where Tag = 'watches/fitbit'
   {CODE-BLOCK/}

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

{CODE-BLOCK: JSON}
//Look for time series named "HeartRate" in user profiles of users under 30.

from Users as u where Age < 30
select timeseries(
    from HeartRate
)
{CODE-BLOCK/}

* `from Users as u where Age < 30`  
  This **document query** locates the documents whose time series we want to query.  
  
    {INFO: }
    A typical time series query starts by locating a single document.  
    For example, to query a stock prices time series we can locate 
    a specific company's profile in the Companies collection first, 
    and then query the StockPrices time series that extends this profile.  
      {CODE-BLOCK: JSON}
      from Companies as c where Name = 'Apple'
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
| {CODE-BLOCK: JSON}
declare timeseries ts(jogger){
    from jogger.HeartRate 
    between 
       '2020-05-27T00:00:00.0000000Z'
      and 
       '2020-06-23T00:00:00.0000000Z'
}

from Users as jog where Age < 30
select ts(jog)
{CODE-BLOCK/}| {CODE-BLOCK: JSON} 
 from Users as jog where Age < 30
 select timeseries(
    from HeartRate 
    between 
       '2020-05-27T00:00:00.0000000Z'
      and 
       '2020-06-23T00:00:00.0000000Z')
    {CODE-BLOCK/}|

---

Queries can use both declared time series functions and custom JavaScript functions.  
The JavaScript functions can then call the time series functions, pass them arguments, 
use and manipulate their results.  

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
[Indexed Time Series Queries](../../../document-extensions/timeseries/querying/indexed-queries)

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
