# Data Gap Filling

---

{NOTE: }

* Time series queries can add extra data points into the gaps between entries. 
  These data points get values extrapolated from the entries on either side of 
  the gap. This is called _interpolation_.  

* There are two interpolation methods available:  
  1. Nearest - add values equal to the value of the nearest entry.  
  2. Linear - place the data points on a straight line between the entries on 
     either side.  

* In this page:  
  * [Syntax](../../../document-extensions/timeseries/querying/gap-filling#syntax)  
  * [Examples](../../../document-extensions/timeseries/querying/gap-filling#examples)  

{NOTE/}

---

{PANEL: Syntax}

To add interpolation to a time series query, start by grouping the data by some 
unit.  
For example, suppose you have a time series with an entry for every hour, 
but several hours are missing (1am, 3pm, etc.), and you want to fill those 
gaps. You will want to group by 1 hour.  
Or suppose you have a time series with an entry for every hour, and you want to 
fill in the gap with one data point per minute: you will group by 1 minute.  
See [here](../../../document-extensions/timeseries/querying/aggregation-and-projections) 
to learn about aggregation in queries.  

Next, use:  

* For RQL queries: `interpolation()`  

The two interpolation modes are:  

1. **Nearest**: add entries with values equal to the closest time series entry 
before or after this data point. If the data point is exactly in the middle 
between two entries, the data point gets the value of the earlier entry.  

2. **Linear**: the data point is placed on a projected line between two entries. 
For example, if the entry for 1:00 PM has a value 100, and the entry for 2:00 PM 
has a value 130, the interpolated data point for 1:40 PM will have the value 120.  

One data point is added for each aggregated time unit that does not contain any 
values. When time series entries have multiple values, an interpolation will add 
one data point for each pair of values found on *both* sides of the gap.  

{PANEL/}

{PANEL: Examples}

{CODE-TABS}
{CODE-TAB:java:RQL RQL_Query@DocumentExtensions\TimeSeries\Querying\GapFilling.java /}
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
[Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
