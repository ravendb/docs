## Time-Series Queries:
# Choosing Query Range

---

{NOTE: }

* Queries can be performed over whole time-series or over a chosen range 
  of time-series entries, e.g. only entries collected during the last 7 days.  
    
* In this page:  
  * [Choosing Query Range](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-query-range)  
  * [Client Usage Samples](../../../document-extensions/timeseries/querying/choosing-query-range#client-usage-samples)
     * [Choosing a Range Using LINQ](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-a-range-using-linq)
     * [Choosing a Range Using Raw RQL](../../../document-extensions/timeseries/querying/choosing-query-range#choosing-a-range-using-raw-rql)

{NOTE/}

---

{PANEL: Choosing Query Range}

In an RQL query, use `between` and `and` to specify a range of time-series 
entries to query. The entries are chosen by their timestamps, in UTC format.  

{CODE-BLOCK: JSON}
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
    (If the query continues, any additional filtering will pick data from this range.)  
      
      {INFO: }
      You can use the Studio to try these queries.  
      Using the studio, you can use parameters for a clearer query.  
      E.g. -  
      {CODE-BLOCK: JSON}
      $from = '2020-05-27T00:00:00.0000000Z'
$to = '2020-06-23T00:00:00.0000000Z'

from Users as jog where Age < 30
select timeseries(
   from HeartRate 
   between $from and $to
)
      {CODE-BLOCK/}
      {INFO/}

{PANEL/}

{PANEL: Client Usage Samples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  

* Learn how to run a LINQ time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries).  
* Learn how to run a raw RQL time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries).  

{INFO/}

---

#### Choosing a Range Using LINQ

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
    | `name` | `string` | Time-Series Name |
    | `from` | `DateTime` | Range Start |
    | `to` | `DateTime` | Range End |
  
* In this sample, we select a three-days range from the HeartRate time-series.
  {CODE ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Choosing a Range Using Raw RQL

To choose a range as part of a raw RQL query, use the `between` and `and` keywords.  

In this sample, a raw RQL query chooses the profiles of users under the age of 30 and 
retrieves a 24-hours range from each.  
An **offset** is defined, adding two hours to retrieved timestamps to adjust them 
to the client's local time-zone.  
 {CODE-TABS}
 {CODE-TAB:csharp:Declare-Syntax ts_region_Raw-Query-Non-Aggregated-Declare-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
 {CODE-TAB:csharp:Select-Syntax ts_region_Raw-Query-Non-Aggregated-Select-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
 {CODE-TABS/}

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
