# Aggregating Time Series Values

---

{NOTE: }

* Time series queries can easily generate powerful statistics by applying an aggregation function  
  (such as Min, Max, Count, Average, etc.) to a group of entries within a chosen time frame,  
  such as an hour or a week.

* For an overview of the available time series queries, please refer to [Time series querying](../../../document-extensions/timeseries/client-api/session/querying).

* In this page:  
  * [Grouping and aggregation options](../../../document-extensions/timeseries/querying/aggregation-and-projections#grouping-and-aggregation-options)  
  * [Examples](../../../document-extensions/timeseries/querying/aggregation-and-projections#examples)
      * [Aggregate entries with single value](../../../document-extensions/timeseries/querying/aggregation-and-projections#aggregate-entries-with-single-value)
      * [Aggregate entries with multiple values](../../../document-extensions/timeseries/querying/aggregation-and-projections#aggregate-entries-with-multiple-values)
      * [Aggregate entries without grouping by time frame](../../../document-extensions/timeseries/querying/aggregation-and-projections#aggregate-entries-without-grouping-by-time-frame)
      * [Aggregate entries filtered by referenced document](../../../document-extensions/timeseries/querying/aggregation-and-projections#aggregate-entries-filtered-by-referenced-document)
      * [Secondary grouping by tag](../../../document-extensions/timeseries/querying/aggregation-and-projections#secondary-grouping-by-tag)
      * [Project document data in addition to aggregated data](../../../document-extensions/timeseries/querying/aggregation-and-projections#project-document-data-in-addition-to-aggregated-data)

{NOTE/}

---

{PANEL: Grouping and aggregation options}

* **Group entries by time frame**:  
  First, you can group the time series entries based on the specified time frame.  
  The following time units are available:

  * `milliseconds` ( milliseconds / milli / ms)
  * `seconds`      ( seconds/ second / s )
  * `minutes`      ( minutes / minute / min )
  * `hours`        ( hours / hour / h )
  * `days`         ( days / day / d )
  * `months`       ( months / month / mon / mo )
  * `quarters`     ( quarters / quarter / q )
  * `years`        ( years / year / y )

* **Secondary grouping**:  
  After grouping by a time unit, you can also perform a _secondary grouping_ by the time series [tag](../../../document-extensions/timeseries/overview#tags).

* **Aggregate values**:  
  You can select one or more aggregation functions to retrieve aggregated values for each group.  
  The resulting aggregated values are **projected** to the client in the query result.  
  The following functions are available:

  * `min()` - the lowest value
  * `max()` - the highest value
  * `sum()` - sum of all values
  * `average()` - the average value
  * `first()` - value of first entry
  * `last()` - value of last entry
  * `percentile(<percentage>)` - The value under which the specified percentage of values fall
  * `slope` - the change in value divided by the change in time between the first and last entries 
  * `standardDeviation()` - the standard deviation of all values (a measure of how spread out the values are from the average)
  * `count()` - The result of Count() is always returned, even if you do not explicitly request it.
     * When each entry has a single value:  
       Returns the number of entries.  
     * When each entry has multiple values:  
       Returns an array of the size of the number of values.  
       Each array element contains the number of entries having a measurement for that value.  

{NOTE: }

**Execute all aggregation functions**:  
When a query groups entries by a time frame but does Not explicitly select a specific aggregation function,  
the server will implicitly execute ALL available aggregation functions (except for Percentile, Slope, and StandardDeviation) for each group.

{NOTE/}
{NOTE: }

**Get aggregated values without grouping**:  
When selecting aggregation functions WITHOUT first grouping the time series entries,  
the aggregation calculations will be executed over the entire set of time series entries instead of per group of entries.

{NOTE/}
{PANEL/}

{PANEL: Examples}

#### Aggregate entries with single value

* Each entry in the "HeartRates" time series within the Employees collection contains a single value.

* In this example, for each employee document, we group entries from the "HeartRates" time series  
  by a 1-hour time frame and then project the lowest and highest values of each group.

    {CODE-TABS}
{CODE-TAB:nodejs:Query aggregation_1@documentExtensions\timeSeries\querying\aggregatingValues.js /}
{CODE-TAB-BLOCK:sql:RQL}
  // Query collection Employees
  from "Employees"
  // Project the time series data:
  select timeseries (
      from HeartRates
      // Use 'group by' to group the time series entries by the specified time frame
      group by "1 hour"   // Group entries into consecutive 1-hour groups
      // Use 'select' to choose aggregation functions that will be evaluated for each group
      select min(), max() // Project the lowest and highest value of each group
  )
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Aggregate entries with multiple values:

* Each entry in the "StockPrices" time series within the Companies collection holds five values:  
  Values[0] - **Open** - stock price when trade opens  
  Values[1] - **Close** - stock price when trade ends  
  Values[2] - **High** - highest stock price during trade time  
  Values[3] - **Low** - lowest stock price during trade time  
  Values[4] - **Volume** - overall trade volume  

* In this example, for each company that is located in USA, we group entries from the "StockPrices" time series  
  by a 7-day time frame and then project the highest and lowest values of each group.

    {CODE-TABS}
{CODE-TAB:nodejs:Query aggregation_2@documentExtensions\timeSeries\querying\aggregatingValues.js /}
{CODE-TAB-BLOCK:sql:RQL_declare_syntax}
declare timeseries SP(c)
{
    from c.StockPrices
    where Values[4] > 500_000 // Query stock price behavior when trade volume is high
    group by "7 days"         // Group entries into consecutive 7-day groups
    select max(), min()       // Project the lowest and highest value of each group
}

from "Companies" as c
  // Query only USA companies:
  where c.Address.Country == "USA"
  // Project the time series data:
  select SP(c)
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:sql:RQL_select_syntax}
from "Companies" as c
// Query only USA companies:
where c.Address.Country = 'USA'
    // Project the time series data:
    select timeseries (
        from StockPrices
        where Values[4] > 500000 // Query stock price behavior when trade volume is high
        group by "7 day"         // Group entries into consecutive 7-day groups
        select max(), min()      // Project the lowest and highest value of each group
    )
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

* Since each entry holds 5 values, the query will project:  
  * 5 `Max` values for each group (the highest Values[0], highest Values[1], etc.) and
  * 5 `Min` values for each group (the lowest Values[0], lowest Values[1], etc.)

---

#### Aggregate entries without grouping by time frame:

* This example is similar to the one above, except that time series entries are Not grouped by a time frame.

* The highest and lowest values are collected from the entire set of time series entries that match the query criteria.

    {CODE-TABS}
{CODE-TAB:nodejs:Query aggregation_3@documentExtensions\timeSeries\querying\aggregatingValues.js /}
{CODE-TAB-BLOCK:sql:RQL}
declare timeseries SP(c)
{
    from c.StockPrices
    where Values[4] > 500_000
    select max(), min()
}

from "Companies" as c
where c.Address.Country == "USA"
select SP(c)
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

* Since no grouping is done, results wil include the highest and lowest Open, Close, High, Low, and Volume values 
  for ALL entries in the time series that match the query criteria.

---

#### Aggregate entries filtered by referenced document:

* The tag in each entry in the "StockPrices" series contains an Employee document ID.

* In this example, we load this [referenced document](../../../document-extensions/timeseries/querying/filtering#filter-by-referenced-document)
  and filter the entries by its properties.

    {CODE-TABS}
{CODE-TAB:nodejs:Query aggregation_4@documentExtensions\timeSeries\querying\aggregatingValues.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" as c
select timeseries(
    from StockPrices
    // Load the referenced document into variable 'employee'
    load Tag as employee
    // Filter entries by the 'Title' field of the employee document
    where employee.Title == "Sales Representative"
    group by "1 month"
    select min(), max()
)
{CODE-TAB-BLOCK/}
    {CODE-TABS/}
  
* Only entries that reference an employee with title 'Sales Representative' will be grouped by 1 month,  
  and the results will include the highest and lowest values for each group.

---

#### Secondary grouping by tag:

* In this example, we perform secondary grouping by the entries' tags.

* The tag in each entry in the "StockPrices" series contains an Employee document ID.

* "StockPrices" entries are grouped by 6 months and then by the tags of the entries within that time frame.

    {CODE-TABS}
{CODE-TAB:nodejs:Query aggregation_5@documentExtensions\timeSeries\querying\aggregatingValues.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
select timeseries (
    from StockPrices
    // Use the 'tag' keyword to perform a secondary grouping by the entries' tags.
    group by "6 months", tag  // Group by months and by tag
    select max(), min()       // Project the highest and lowest values of each group
)
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Project document data in addition to aggregated data:

* In addition to projecting the aggregated time series data, you can project data from the parent document that contains the time series.
 
* In this example, projecting the **company name** alongside the query results clearly associates each entry in the result set with a specific company.
  This provides immediate context and makes it easier to interpret the time series data.

    {CODE-TABS}
{CODE-TAB-BLOCK:sql:RQL_declare_syntax}
declare timeseries SP(c)
{
    from c.StockPrices
    where Values[4] > 500_000 // Query stock price behavior when trade volume is high
    group by "7 days"         // Group entries into consecutive 7-day groups
    select max(), min()       // Project the lowest and highest value of each group
}

from "Companies" as c
// Project the company's name along with the time series query results to make results more clear
select SP(c) as MinMaxValues, c.Name as CompanyName
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:sql:RQL_select_syntax}
from "Companies" as c
select timeseries (
    from StockPrices
    where Values[4] > 500000
    group by "7 day"
    select max(), min()
) as MinMaxValues,
c.Name as CompanyName // Project property 'Name' from the company document
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

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
[Indexed Time Series Queries](../../../document-extensions/timeseries/querying/using-indexes)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  

**Querying**  
[Querying: Projections](../../../indexes/querying/projections)  
