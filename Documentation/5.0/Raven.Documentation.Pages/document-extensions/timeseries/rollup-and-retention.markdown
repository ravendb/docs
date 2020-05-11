# Time-Series Rollup and Retention
---

{NOTE: }

For time-series applications that produce massive amounts of data at a steady rate, 
**Time-Series Policies** allow you to manage the excess data in two ways:  

* *Retention* - limiting the amount of time that time-series data is kept.  

* *Rollup* - the process of summarizing time-series data by aggregating it into the 
form of a new, more compact time-series.  

The same collection can have multiple time-series policies - each with its own rollup 
aggregation span.  

* In this page:  
  * [Time-Series Policies]()  
  * [Time-Series Configuration]()  
  * [Code Samples]()  

{NOTE/}

---

{PANEL: Rollup Time-Series}  

The **rollup** process divides a time-series into chunks of a specified amount 
of time (such as a minute or a day) and aggregates all the entries in that 
chunk into six useful values:  

  * *First* - the value of the first entry in the aggregation.  
  * *Last* - the value of the last entry.  
  * *Min* - the smallest value.  
  * *Max* - the largest value.  
  * *Sum* - the sum of all the values in the aggregation.  
  * *Count* - the total number of entries in the aggregation.  

A new rollup time-series is created with one entry for each aggregation. Each 
entry contains the above six values for *each of the values* of the original 
(or "raw") time series. That is, if the raw time-series has two values per 
entry, the rollup time-series will have 12 per entry - the first six will be 
aggregations of the first raw value, the next six will be aggregations of the 
second raw value, and so on. The timestamp of each entry is the end of the 
time span that it aggregates.  

!["Rollup time-series entries"](images/rollup-1.png)

The rollup time-series belongs to the same document as the original (or "raw") 
time-series. Its name will be a combination of the name of the raw time-series 
and the name of the time-series policy:  
"`<name of raw time-series>@<name of time-series policy>`"  

{CODE-BLOCK: csharp}
public class TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime;
    public TimeValue AggregationTime;
}
{CODE-BLOCK/}

| Property | Description |
| - | - |
| `Name` | This `string` is used to create the names of rollup time-series created by this policy.<br/>`Name` is added to the name of the raw time-series (with `@` as a separator) to create the name of the resulting rollup time-series.* |
| `RetentionTime` | 

 The rollup's name is a combination of the raw time-series name. For a base time-series called "exampleTS", and a policy with `Name` set to "rollup", the resulting rollup is called "exampleTS@rollup".

{PANEL/}

{PANEL: Time-Series Configuration}


{CODE-BLOCK: csharp}
public class TimeSeriesCollectionConfiguration
{
    public bool Disabled;
    public List<TimeSeriesPolicy> Policies;

}

public class TimeSeriesConfiguration
{
    public Dictionary<string, TimeSeriesCollectionConfiguration> Collections;
}
{CODE-BLOCK/}
{PANEL/}

{PANEL: Examples}
How to create time-series policies for a collection and pass them to the server:
{CODE rollup_and_retention_0@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  
How to access a rollup time series:
{CODE rollup_and_retention_1@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  
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
