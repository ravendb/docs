## Time-Series Queries:
# Aggregarion and Projection

---

{NOTE: }

* **Aggregation**  
  Query result-sets can be aggregated (grouped) by a chosen time frame, 
  e.g. a day (grouping the results in multiple day-long data frames).  

* **Projection**  
  Several criteria (e.g. `Min`, `Max`, `Average`) can be used to 
  `select` results and project them to the client.  
  Selecting results from an aggregated result-set, will select 
  matching results from each group (e.g. the Min value of each group).  
    
* In this page:  
  * [Aggregation](../../../)  
  * [Projection](../../../)  
  * [Client Usage Samples](../../../)  

{NOTE/}

---

{PANEL: Aggregation}

Use `group by` to group the result-set in groups of a chosen resolution.  

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' 
            and '2020-06-23T00:00:00.0000000Z'
       where Tag='watches/fitbit'
       group by '7 days'
)
{CODE-BLOCK/}

* `group by '7 days'`  
  In this case: the result-set will be composed of groups of Heartrate entries, 
  each group containing 7 days of entries (first group: 2020-05-27 to 
  2020-06-02, second group: 2020-06-03 to 2020-06-09, and so on).  

{INFO: You can group by these time units:}  

* **Milliseconds**  
* **Seconds**  
* **Minutes**  
* **Hours**  
* **Days**  
* **Months**  
* **Quarters**  
* **Years**  

{INFO/}

{PANEL/}

{PANEL: Projection}

Use `select` to choose the values that would be projected to the client.  

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' 
            and '2020-06-23T00:00:00.0000000Z'
       where Value < 80
       group by '7 days'
       select max()
)
{CODE-BLOCK/}

* `select max()`  
   * Since each time-series entry may have multiple values, 
     max() will check all the first values of time-series entries and 
     find the highest first-value, check all the second values of time-series 
     entries and find the highest second-value, and so on.  
   * The projected result will be an array of values.  
     The array's length will be as the length of the series-entry with highest 
     number of values.  
     Its first value will be the highest first value, its second value 
     the highest second value, and so on.  

{INFO: You can select values by these criteria:}

* **Min()** - the lowest value  
* **Max()** - the highest value  
* **Sum()** - sum of all values  
* **Average()** - average value  
* **First()** - values of the first series entry  
* **Last()** - values of the last series entry  
* **Count()** - overall number of values in series entries  

{INFO/}

{INFO: Selecting values from aggregated and non-aggregated result-sets}

* When values are selected from a result-set that **hasn't** been aggregated (grouped),  
  they are selected from the entire result-set.  
* When values are selected from a result-set that **has** been aggregated,  
  they are selected per-group.  

{INFO/}

{PANEL/}

{PANEL: Client Usage Samples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  
* Learn how to run a raw RQL time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries).  
* Learn how to run a LINQ time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries).  
{INFO/}

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
