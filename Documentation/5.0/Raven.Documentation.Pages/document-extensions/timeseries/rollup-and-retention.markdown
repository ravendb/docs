# Time Series Rollups and Retention
---

{NOTE: }

Many time series applications produce massive amounts of data at a steady rate. 
**Time Series Policies** help you to manage your data in two ways:  

* Creating **Rollups** - summarizing time series data by aggregating it into the 
form of a new, lower resolution time series.  

* Limiting a time series' **Retention** - the amount of time that time series data 
is kept before being deleted.  

* In this page:  
  * [Time Series Policies](../../document-extensions/timeseries/rollup-and-retention#time-series-policies)  
  * [Usage Flow and Syntax](../../document-extensions/timeseries/rollup-and-retention#usage-flow-and-syntax)  
  * [Samples](../../document-extensions/timeseries/rollup-and-retention#samples)  

{NOTE/}

---

{PANEL: Time Series Policies}

#### What are Rollups?

A **rollup** is a time series that summarizes the data from another time series, 
with each rollup entry representing a specific time frame in the original time 
series. Each rollup entry contains 6 values that aggregate the data from all the 
entries in the original time frame:  

* *First* - the value of the first entry in the frame.  
* *Last* - the value of the last entry.  
* *Min* - the smallest value.  
* *Max* - the largest value.  
* *Sum* - the sum of all the values in the frame.  
* *Count* - the total number of entries in the frame.  

This results in a much more compact time series that still contains useful 
information about the original time series (also called the "named" or "raw" 
time series). Rollup time series are created automatically according to 
**rollup policies**. Rollup policies apply to all time series of every document 
in the given collection. Each collection can be configured to have multiple 
policies.  

The rollup policies for a given collection are not applied independently. A given 
raw time series is rolled up using the policy with the shortest aggregation 
frame. Then that rollup time series is rolled up using the policy with the 
_next_ shortest aggregation frame, and so on.  

[Querying with group-by](..\..\document-extensions\timeseries\querying\aggregation-and-projections) 
will transparently traverse over the rollups to retrieve the relevant results.  

Let's look at an example of rollup data:  
<br/>
!["Rollup time series entries"](images/rollup-1.png "A rollup time series' entries")
<br/>
**1) Name:**  
A rollup time series' name has this format:  
`"<name of raw time series>@<name of time series policy>"`  
It is a combination of the name of the raw time series and the name of the 
time series policy separated by a `@` character - in the image above these are 
"HeartRates" and "byHour" respectively. For this reason, neither 
a time series name nor a policy name can have the character `@` in it.

**2) Timestamp:**  
The aggregation frame always begins at a round number of one of these time units: a 
second, minute, hour, day, week, month, or year. So the frame includes all entries 
starting at a round number of time units, and ending at a round number *minus 
one millisecond* (since milliseconds are the minimal resolution in RavenDB 
time series). The timestamp for a rollup entry is the beginning of the frame it 
represents.  

For example, if the aggregation frame is three days, a frame will start and end at a 
time stamps like:  
`2020-01-01 00:00:00` - `2020-01-03 23:59:59.999`.

**3) Values:**  
Each group of six values represents one value in the original entries. If the raw 
time series has *n* values per entry, the rollup time series will have _6*n_ per entry: 
the first six summarize the first raw value, the next six summarize the next raw value, 
and so on. The aggregate values have the names:  
`"First (<name of raw value>)", "Last (<name of raw value>)", ...` respectively.  
Because time series entries are limited to 32 values, rollups are limited to 
the first five values of an original time series entry, or 30 aggregate values.  




{PANEL/}

{PANEL: Usage Flow and Syntax}  

To configure time series policies for one or more collections:  

* Create time series policy [objects](../../document-extensions/timeseries/rollup-and-retention#the-two-types-of-time-series-policy).  
* Use those to populate `TimeSeriesCollectionConfiguration` [objects](../../document-extensions/timeseries/rollup-and-retention#and-) 
for each collection you want to configure.  
* Use _those_ to populate a `TimeSeriesConfiguration` [object](../../document-extensions/timeseries/rollup-and-retention#and-) 
which will belong to the whole database.  
* Finally, use the `ConfigureTimeSeriesOperation` [operation](../../document-extensions/timeseries/rollup-and-retention#the-time-series-configuration-operation) 
to send the new configurations to the server.  
<br/>
###Syntax  

####The two types of time series policy:

{CODE-BLOCK: csharp}
// Rollup policies
public class TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime;
    public TimeValue AggregationTime;
}

// A retention policy for the raw TS
// Only one per collection
public class RawTimeSeriesPolicy : TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime;
    // Does not perform aggregation
}
{CODE-BLOCK/}

`TimeSeriesPolicy`:  

| Property | Description |
| - | - |
| `Name` | This `string` is used to create the names of the rollup time series created by this policy.<br/>`Name` is added to the name of the raw time series - with `@` as a separator - to create the name of the resulting rollup time series. |
| `RetentionTime` | Time series entries older than this time span (see `TimeValue` below) are automatically deleted. |
| `AggregationTime` | The time series data being rolled up is divided at round time units, into parts of this length of time. Each of these parts is aggregated into an entry of the rollup time series. |

`RawTimeSeriesPolicy`:  

| Property | Description |
| - | - |
| `Name` | This `string` is used to create the names of the rollup time series created by this policy.<br/>`Name` is added to the name of the raw time series - with `@` as a separator - to create the name of the resulting rollup time series. |
| `RetentionTime` | Time series entries older than this time span (see `TimeValue` below) are automatically deleted. |
<br/>
####The `TimeValue` struct

{CODE-BLOCK: csharp}
public struct TimeValue
{
    public static TimeValue FromSeconds(int seconds);
    public static TimeValue FromMinutes(int minutes);
    public static TimeValue FromHours(int hours);
    public static TimeValue FromDays(int days);
    public static TimeValue FromMonths(int months);
    public static TimeValue FromYears(int years);
}
{CODE-BLOCK/}

`Each of the above `TimeValue` methods returns a `TimeValue` object representing a 
whole number of the specified time units. These are passed as the aggregation and 
retention spans of time series policies.  

{INFO: }
The main reason we use `TimeValue` rather than something like `TimeSpan` is that 
`TimeSpan` doesn't have a notion of 'months', because a calendar month is not a 
standard unit of time (since it ranges from 28-31 days). `TimeValue` enables you 
to define retention and aggregation spans for a calendar month.  
{INFO/}
<br/>
####`TimeSeriesCollectionConfiguration` and `TimeSeriesConfiguration`

{CODE-BLOCK: csharp}
public class TimeSeriesCollectionConfiguration
{
    public bool Disabled;
    public List<TimeSeriesPolicy> Policies;
    public RawTimeSeriesPolicy RawPolicy;
}

public class TimeSeriesConfiguration
{
    public Dictionary<string, TimeSeriesCollectionConfiguration> Collections;
}
{CODE-BLOCK/}

| Property | Description |
| - | - |
| `Disabled` | If set to `true`, rollup processes will stop, and time series data will not be deleted by retention policies. |
| `Policies` | Populate this `List` with your rollup policies |
| `RawPolicy` | The `RawTimeSeriesPolicy`, the retention policy for the raw time series |
| `Collections` | Populate this `Dictionary` with the `TimeSeriesCollectionConfiguration`s and the names of the corresponding collections. |
<br/>
#### The Time Series Configuration Operation

{CODE-BLOCK: csharp}
public ConfigureTimeSeriesOperation(TimeSeriesConfiguration configuration);
{CODE-BLOCK/}

Pass this your `TimeSeriesConfiguration`, see usage example below. How to use an [operation](../../client-api/operations/what-are-operations).  
<br/>
#### Casting Time Series Entries

Time series entries are of one of the following classes:  

{CODE-BLOCK: csharp}
public class TimeSeriesEntry {   }
public class TimeSeriesEntry<T> : TimeSeriesEntry {   }
public class TimeSeriesRollupEntry<TValues> : TimeSeriesEntry {   }
{CODE-BLOCK/}

Read more about time series with generic types [here](../../../document-extensions/timeseries/client-api/named-time-series-value).

If you have an existing rollup entry of type `TimeSeriesEntry`, you can 
cast it to a `TimeSeriesRollupEntry` using `AsRollupEntry()`.  

{CODE-BLOCK: csharp}
public static TimeSeriesRollupEntry<T> AsRollupEntry<T>(this TimeSeriesEntry<T> entry);
{CODE-BLOCK/}

You can cast a `TimeSeriesRollupEntry` to a `TimeSeriesEntry` directly. 
Its values will consist of all the `First` values of the rollup entry.  

{CODE-BLOCK: csharp}
var rollupEntry = new TimeSeriesRollupEntry<int>(
                          new DateTime(2020,1,1));

TimeSeriesEntry<int> TSEntry = (TimeSeriesEntry<int>)rollupEntry;
{CODE-BLOCK/}

{PANEL/}

{PANEL: Samples}

How to create time series policies for a collection and pass them to the server:  

{CODE rollup_and_retention_0@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  

How to access a rollup time series:  

{CODE rollup_and_retention_1@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  
{PANEL/}

## Related articles  
###Studio  
- [Time Series Interface in Studio](../../studio/database/document-extensions/time-series)

###Time Series  
- [Time Series Overview](../../document-extensions/timeseries/overview)  
- [API Overview](../../document-extensions/timeseries/client-api/overview)  

###Client-API  
- [What Are Operations?](../../client-api/operations/what-are-operations)

