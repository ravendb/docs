## Time-Series Queries:
# Aggregarion and Projection

---

{NOTE: }

* **Aggregation**  
  Queris can easily create powerful statistics by aggregating time-series (or 
  chosen ranges of time-series entries) into groups by chosen time frames like 
  an hour or a week, and retrieving values from each group by criteria like `Min` 
  for the lowest value, `Count` for the number of values in the group, etc.  
  
* **Projection** by criteria  
  Queries can explicitly select the criteria by which values would be retrieved 
  and projected to the client.  
  When a query does **not** select specific criteria, RavenDB will consider it 
  an implicit selection of **all** criteria and project to the client the values 
  from each group, that match each criteria.  

    {INFO: Projecting values from Aggregated and Non-Aggregated result-sets}

    * When values are selected from a time-series (or a range of time-series 
     entries) that **has** been aggregated, they are selected per-group.  
    * When values are selected from a series or a range that **hasn't** 
      been aggregated, they are selected from the entire result-set.  

    {INFO/}




* In this page:  
  * [Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projection#aggregation-and-projection)  
  * [Client Usage Samples](../../../document-extensions/timeseries/querying/aggregation-and-projection#client-usage-samples)  

{NOTE/}

---

{PANEL: Aggregation and Projection}

In an RQL query, use the `group by` expression to aggregate 
time-series (or ranges of time-series entries) in groups by 
a chosen resolution, and the `select` keyword to choose and 
project entries by a chosen criteria.    

{INFO: You can aggregate entries by these time units:}  

* **Milliseconds**  
* **Seconds**  
* **Minutes**  
* **Hours**  
* **Days**  
* **Months**  
* **Quarters**  
* **Years**  

{INFO/}

{INFO: You can `select` values for projection by these criteria:}

* **Min()** - the lowest value  
* **Max()** - the highest value  
* **Sum()** - sum of all values  
* **Average()** - average value  
* **First()** - values of the first series entry  
* **Last()** - values of the last series entry  
* **Count()** - overall number of values in series entries  

{INFO/}

* In this sample, we group entries of the HeartRate time-series.  
  Each HeartRate entry holds a single value.
    {CODE-BLOCK: JSON}
from Users as u where Age < 30
    select timeseries(
        from HeartRate between 
            '2020-05-17T00:00:00.0000000Z' 
            and '2020-05-23T00:00:00.0000000Z'
                where Tag == 'watches/fitbit'
        group by '1 days'
        select min(), max()
    )
    {CODE-BLOCK/}
   * **group by '1 days'**  
     We group each user's HeartRate time-series entries 
     in consequtive 1-day groups.  
   * **select min(), max()**  
     We then select the lowest (`Min`) and highest (`Max`) 
     values of each group, and project them to the client.  

* In this sample, we group entries of the StockPrice time-series.  
  Each StockPrice entry holds five values:  
  Values[0] - **Open** price  
  Values[1] - **Close** price  
  Values[2] - **High** price  
  Values[3] - **Low** price  
  Values[4] - Trade **Volume**  
    {CODE-BLOCK: JSON}
declare timeseries SP(c) 
{
    from c.StockPrice 
    where Values[4] > 500000
        group by '7 day'
        select max(), min()
}
from Companies as c
where c.Address.Country = 'USA'
select c.Name, SP(c)
    {CODE-BLOCK/}
   * **group by '7 day'**  
     We group each company's StockPrice time-series entries 
     in consequtive 7-day groups.  
   * **select max(), min()**  
     We then select the lowest (`Min`) and highest (`Max`) 
     values of each group, and project them to the client.  
     Since each entry holds 5 values, the query will project 
     5 `Min` values for each group (the lowest Values[0], 
     the lowest Values[1], etc.) and 5 `Max` values for each 
     group (the highest Values[0], the highest Values[1], etc.).  
   * **select c.Name, SP(c)**  
     Projecting the company's name along with the `Min` and `Max` 
     time-series values clarifies the query results.  

* This sample is similar to the previous one, except that 
  time-series entries are **not aggregated**.  
    {CODE-BLOCK: JSON}
declare timeseries SP(c) 
{
    from c.StockPrice 
    where Values[4] > 500000
        select max(), min()
}
from Companies as c
where c.Address.Country = 'USA'
select c.Name, SP(c)
    {CODE-BLOCK/}
   * **select max(), min()**  
     Since there is no aggregation, time-series entries are 
     selected from the entire result-set and only 10 values 
     will be projected to the client: 5 Min values and 5 Max 
     values for the entire range.  

{PANEL/}

{PANEL: Client Usage Samples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  
* Learn how to run a raw RQL time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries).  
* Learn how to run a LINQ time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries).  
{INFO/}

To aggregate time-series entries, use `group by` in a raw RQL query or `GroupBy()` 
in a LINQ query.  
To select time-series values for projection, use `select` in a raw RQL query 
or `Select()` in a LINQ query.  

* These are three forms of the same query, using raw RQL "declare" and "select" syntaxes 
  and LINQ.  
    {CODE-TABS}
    {CODE-TAB:csharp:Raw-RQL-Select-Syntax ts_region_Raw-RQL-Select-Syntax-Aggregation-and-Projection-StockPrice@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TAB:csharp:Raw-RQL-Declare-Syntax ts_region_Raw-RQL-Declare-Syntax-Aggregation-and-Projection-StockPrice@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TAB:csharp:LINQ ts_region_LINQ-Aggregation-and-Projection-StockPrice@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
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
