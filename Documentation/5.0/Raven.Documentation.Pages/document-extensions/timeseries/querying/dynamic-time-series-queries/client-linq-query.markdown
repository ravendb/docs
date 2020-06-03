# Dynamic Time-Series Queries 
---

{NOTE: }

* Time-series queries can be phrased by a client using raw RQL or LINQ functions 
  (which are translated to RQL as well).  
* Queries can be performed over whole time-series, and over chosen ranges of
  time-series entries.  
* Queries can filter the results using `Where`, group them using `GroupBy`, 
  and project selected results using `Select`.  
    
* In this page:  
  * [Dynamic Queries](../../../document-extensions/timeseries/querying/dynamic-queries#dynamic-queries)  
  * [Query Syntax: Server-Side](../../../document-extensions/timeseries/querying/dynamic-queries#query-syntax:-server-side)  
  * [Query Syntax: Client-Side](../../../document-extensions/timeseries/querying/dynamic-queries#query-syntax:-client-side)  
  * [Filtering](../../../document-extensions/timeseries/querying/dynamic-queries#filtering)  
  * [Aggregated Queries](../../../document-extensions/timeseries/querying/dynamic-queries#aggregated-queries)  

{NOTE/}

---

{PANEL: Dynamic Queries}

Use dynamic queries when the time-series you query are unindexed 
or when you prefer that RavenDB's [query optimizer](../../../indexes/querying/what-is-rql#query-optimizer) 
would locate their indexes itself.  

{INFO: }
Note that time-series are **not** automatically indexed.  
To index a time-series, define a [static index](../../../document-extensions/timeseries/indexing/map-indexing) 
for it manually.  
When a time-series **is** indexed, you can query it using a dynamic query 
or define an [indexed query](../../../document-extensions/timeseries/querying/indexed-queries) 
for it.  
{INFO/}

{PANEL/}

{PANEL: Query Syntax: Server-Side}

Time-series RQL queries have this basic format:  

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate)
{CODE-BLOCK/}

* **from Users as u where Age < 30**  
  This is simply a document query that locates the documents whose time-series 
  you want to query.  
  
    {INFO: }
    It is common to specify an even more concrete document query, to focus the 
    search on a particular document.  
    If you want to query a time-series that is updated with stock prices, 
    for example, it would make sense to first find a specific company 
    (a single "Company" document) whose stock prices interest you, and 
    then query this company's StockPrices time-series.  
    {INFO/}

* **select timeseries**  
  Define the time-series part of the query within this block.  

* **from HeartRate**  
  The name of the time-series to be queried.  
  In this case: all time-series named "HeartRate" (for users under the age of 30).  

---

#### Entries Range 

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z')
{CODE-BLOCK/}
  
  
* **between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z'**  
  Use `between` to indicate which range of time-series entries is to be queried upon.  
  If the query ends here, all entries between the given timestamps will be retrieved.  
  If the query continues, later parts will relate only to this range.  

---

#### Filtering 

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z'
       where Tag='watches/fitbit')
{CODE-BLOCK/}
  
* **where Tag='watches/fitbit'**  
  Use `where` to filter time-series entries by their tags or values.  
  In this case: only entries tagged 'watches/fitbit' are retrieved.  
   * To filter by value: **where Value < 80**  

---

#### Aggregation

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z'
       where Tag='watches/fitbit'
       group by 7days)
{CODE-BLOCK/}

* **group by 7days**  
  Use `group by` to aggregate retrieved entries in groups by a chosen resolution.  
  In this case: the result-set will be composed of groups of Heartrate entries, 
  each group containing 7 days of Heartrate entries (first group: 2020-05-27 to 
  2020-06-02, second group: 2020-06-03 to 2020-06-09, and so on).

---

#### Projection

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
    between '2020-05-27T00:00:00.0000000Z' and '2020-06-23T00:00:00.0000000Z'
    where Value < 80
    group by 7days
    select min(), max())
{CODE-BLOCK/}

* **select min(), max()**  
  Use `select` to choose results that would be projected to the client.  
  You can select results by:  
   * Min() - Series smallest value  
   * Max() - Series largest value  
   * Sum() - Series entries sum  
   * Average() - Series Values average  
   * First() - Value of the first series' entry  
   * Last - Value of the last series' entry  
   * Count - Number of time-series entries  
  
  If you haven't [aggregated]() the result-set, selecting min() for example 
  will yield a single minimum value for the entire result-set.  
  
  If you **have** aggregated the result-set, selecting min() for example will 
  compose an array of minimum values, one per group.  

{PANEL/}

{PANEL: Declare: Creating a Time-Series Function}

You can create a time-series function using `declare`, 
and use this function from your query.  
Here is, for example, the same query with and without 
a time-series function.  

| With Time-Series Function | Without Time-Series Function |
|:---:|:---:|
| {CODE-BLOCK: JSON}
declare timeseries ts(jogger){
    from jogger.HeartRate 
    between 
       '2020-05-27T00:00:00.0000000Z'
      and 
       '2020-06-23T00:00:00.0000000Z'
    where Value < 80
    group by 7days
    select min(), max()
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
       '2020-06-23T00:00:00.0000000Z'
    where Value < 80
    group by 7days
    select min(), max())
    {CODE-BLOCK/}|

{PANEL/}

{PANEL: Query Syntax: Client-Side}

You can submit a time-series query from your client using either a raw RQL query, 
or LINQ expressions.  

---

#### Raw RQL

Raw RQL queries are similar 

---

#### Offset

---

#### Time-Series LINQ Queries

{PANEL/}

{PANEL: Filtering}

---

#### Filter By TS-Entry Properties

---

#### `LoadTag`

{PANEL/}

{PANEL: Aggregated Queries}

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
