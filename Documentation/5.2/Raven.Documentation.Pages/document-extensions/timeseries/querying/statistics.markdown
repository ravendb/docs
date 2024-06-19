# Statistical Measures

---

{NOTE: }

* Queries can calculate the **percentile**, **slope**, and **standard deviation** 
of a time series, or of a range of entries within a time series.  

* For time series that have more than one value per entry, these methods return 
one measure for the first values in each entry, another measure for the 
second values in each entry, and so on.  

* In this page:  
  * [Syntax](../../../document-extensions/timeseries/querying/statistics#syntax)  
  * [Examples](../../../document-extensions/timeseries/querying/statistics#examples)  

{NOTE/}

---

{PANEL: Syntax}

### Percentile  

{INFO: }
A [percentile](https://en.wikipedia.org/wiki/Percentile) of a time series 
is the value that divides the time series values by some ratio, when they 
are arranged from smallest to largest.  

For example, a 90th percentile is greater than 90% of the values in the 
series, and less than the remaining 10%.  
{INFO/}

* RQL method: `percentile()`  
* LINQ method: `Percentile()`  

The percentile method can be used to calculate any percentile in a time 
series or range of time series entries. It takes one `double` value that is greater than
0 and less than or equal to 100. This represents the percent of the time series values that 
should be smaller than the result.  

See examples [below](../../../document-extensions/timeseries/querying/statistics#examples).  

---

### Slope  

* RQL method: `slope()`  
* LINQ method: `slope()`  

The slope of a time series or range of time series entries is the difference 
between the first and last values of the range (disregarding the values in 
between) divided by the difference in time.  

Queries that use this method must also [aggregate](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
the time series, grouping the entries into whole numbers of time units.  

The difference in time is measured in milliseconds. Use [scaling](../../../document-extensions/timeseries/querying/overview-and-syntax#scaling-query-results)
to adjust the results to your preferred units.  

See examples [below](../../../document-extensions/timeseries/querying/statistics#examples).  

---

### Standard Deviation

* RQL method: `stddev()`  
* LINQ method: `StandardDeviation()`  

These methods return the [standard deviation](https://en.wikipedia.org/wiki/Standard_deviation) 
of time series values.  

See examples [below](../../../document-extensions/timeseries/querying/statistics#examples).  

---

### Result Format

Queries with these methods return results with the following format:  

{CODE-BLOCK: json}
{
    "From": <first entry's timestamp>,
    "To": <last entry's timestamp>,
    "Count": [
        <number of first values from each entry>,
        <number of second values from each entry>,
        ...
    ],
    <"Percentile"/"Slope"/"Standard Deviation">: [
        <double - measure for first values from each entry>,
        <double - measure for second values from each entry>,
        ...
    ]
}
{CODE-BLOCK/}

If the query uses `group by` aggregation, there will be one of 
these results for each of the aggregates.  

{PANEL/}

{PANEL: Examples}

### Percentile

{CODE-TABS}
{CODE-TAB:csharp:LINQ LINQ_percentile@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TAB:csharp:Raw-RQL RQL_percentile@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TABS/}

### Slope

{CODE-TABS}
{CODE-TAB:csharp:LINQ LINQ_slope@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TAB:csharp:Raw-RQL RQL_slope@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TABS/}

### Standard Deviation

{CODE-TABS}
{CODE-TAB:csharp:LINQ LINQ_stddev@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TAB:csharp:Raw-RQL RQL_stddev@DocumentExtensions\TimeSeries\Querying\Statistics.cs /}
{CODE-TABS/}

{PANEL/}

## Related articles  

### Time Series  

- [Time Series Overview](../../../document-extensions/timeseries/overview)  
- [Studio Time Series Management](../../../studio/database/document-extensions/time-series)  
- [Time Series Indexing](../../../document-extensions/timeseries/indexing)  
- [Range Selection](../../../document-extensions/timeseries/querying/choosing-query-range)  
- [Filtering](../../../document-extensions/timeseries/querying/filtering)  
- [Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  
- [Indexed Time Series Queries](../../../document-extensions/timeseries/querying/using-indexes)  

### Client API  

- [How To Query](../../../client-api/session/querying/how-to-query)  

### Indexes

- [Querying an Index](../../../indexes/querying/query-index)  
