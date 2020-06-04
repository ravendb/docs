# Dynamic Time-Series Queries 
---

{NOTE: }

* Time-series queries can be phrased by a client using raw RQL or LINQ functions 
  (which are also translated to RQL before their execution by the server).  
* Queries can be performed over whole time-series, and over chosen ranges of
  time-series entries.  
* Queries can filter results using `Where`, group them using `GroupBy`, 
  and project selected results using `Select`.  
    
* In this page:  
  * [Dynamic Queries](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#dynamic-queries)  
  * [Syntax](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#syntax)  
     * [`select timeseries` Syntax: Creating a Time-Series Section](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#syntax-creating-a-time-series-section)  
     * [`declare timeseries` Syntax: Declaring a Time-Series Function](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#syntax-declaring-a-time-series-function)  
  * [Choose Query Range - `between` x `and` y](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#choose-query-range---between-x-and-y)  
  * [Filtering - `where`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#filtering---where)  
     * [Using Tags as References - `load tag`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#using-tags-as-references---)  
  * [Aggregation - `group by`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#aggregation---group-by)  
  * [Projection - `select`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#projection---select)  

{NOTE/}

---

{PANEL: Dynamic Queries}

Use dynamic queries when time-series you query are unindexed 
or when you prefer RavenDB to choose an index automatically 
using its [query optimizer](../../../../indexes/querying/what-is-rql#query-optimizer).  

{PANEL/}

{PANEL: Syntax}

You can query time-series using two different syntaxes.  
Choose the syntax you're more comfortable with.  

* [`select timeseries` syntax](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#syntax-creating-a-time-series-section)  
* [`declare timeseries` syntax](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#syntax-declaring-a-time-series-function)  

---

#### `select timeseries` Syntax: Creating a Time-Series Section

This syntax allows you to encapsulate your query's time-series functionality 
in a `select timeseries` section.  

{CODE-BLOCK: JSON}
//Look for time-series named "HeartRate" in user profiles of users under 30.

from Users as u where Age < 30
select timeseries(
    from HeartRate
)
{CODE-BLOCK/}

* `from Users as u where Age < 30`  
  This **document query** locates the documents whose time-series we want to query.  
  
    {INFO: }
    A typical time-series query starts by locating a single document.  
    For example, to query a stock prices time-series we can locate 
    a specific company's profile in the Companies collection first, 
    and then query the StockPrices time-series that extends this profile.  
      {CODE-BLOCK: JSON}
      from Companies as c where Name = 'Apple'
      select timeseries(
          from StockPrices
      )
      {CODE-BLOCK/}
    {INFO/}

* `select timeseries`  
  The `select` clause defines the time-series query.  

* `from HeartRate`  
  The `from` keyword is used to select the time-series we'd query, by its name.  

---

#### `declare timeseries` Syntax: Declaring a Time-Series Function

This syntax allows you to declare a time-series function and call it 
from your query. It introduces greater flexibility to your queries as 
you can, for example, pass arguments to/by the time-series function.  

Here is a query in both syntaxes. It picks users whose age is under 30, 
and if they own a time-series named "HeartRate" retrieves a range of entries 
from this series.  


| With Time-Series Function | Without Time-Series Function |
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

{PANEL/}

{PANEL: Choose Query Range - `between` x `and` y}

Use `between` and `and` to specify a range of time-series entries to query.  
The range start and end entries are chosen by their timestamps, in UTC format.  


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
    If the query ends here, all entries between the given timestamps will be retrieved.  
    If the query continues, any additional filtering will relate only to this range.  

{PANEL/}

{PANEL: Filtering - `where`}

Use `where` to filter time-series entries by their **tags** or **values**.  

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' 
            and '2020-06-23T00:00:00.0000000Z'
       where Tag='watches/fitbit'
)
{CODE-BLOCK/}
  
* `where Tag='watches/fitbit'`  
  In this example we retrieve time-series entries whose tag is 'watches/fitbit'.  
  To filter entries by their by value, use **Value**. E.g.:  
  `where Value < 80`  

---

#### Using Tags as References - `load tag`

Use the `load Tag ` expression to **load a document** whose ID is stored in 
a time-series entry's tag.  
Use `load Tag ` with `where` to **filter your results by properties of the 
loaded document**, as we do in the following example.  

{CODE-BLOCK: JSON}
from Companies as c where c.Address.Country = "USA"
select timeseries(
    from StockPrice
    load Tag as emp
    where emp.Title == "Vice President, Sales"
)   
{CODE-BLOCK/}

* `from Companies as c where c.Address.Country = "USA"`  
   Here, we choose companies that reside in the USA.  
* `from StockPrice`  
   Here we choose these companies' **StockPrice** time-series.  
* `load Tag as emp`  
   We know in advance, that the tag of each time-series entry contains 
   the ID of an Employee document.  
   Here, we use the `load tag` expression to load the employee profile 
   referred to by each tag.  
* `where emp.Title == "Vice President, Sales"`  
   And finally, we filter time-series entries by the **Title** property 
   of employee documents they refer to.  

The result-set of this sample includes time-series entries with 
references (IDs stored in their tags) to a Sales Vice-President's profile.  


{PANEL/}

{PANEL: Aggregation - `group by`}

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

{PANEL: Projection - `select`}

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
