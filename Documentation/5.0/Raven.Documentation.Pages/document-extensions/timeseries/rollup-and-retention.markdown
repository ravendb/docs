# Time-Series Rollup and Retention
---

{NOTE: }

Many time-series applications produce massive amounts of data at a steady rate. 
**Time-Series Policies** help you to manage your data in two ways:  

* *Rollup* - the process of summarizing time-series data by aggregating it into the 
form of a new, lower resolution time-series.  

* *Retention* - limiting the amount of time that time-series data is kept before 
being deleted.  

There are two kinds of policies:  

1. `RawTimeSeriesPolicy` - defines *only* a retention span, which applies to the 
original, or "raw", time-series.  
2. `TimeSeriesPolicy` - defines the creation of rollup time-series and the retention 
span for those time-series.  

* In this page:  
  * [Time-Series Policies](../../document-extensions/timeseries/rollup-and-retention#time-series-policies)  
  * [Usage Flow and Syntax](../../document-extensions/timeseries/rollup-and-retention#usage-flow-and-syntax)  
  * [Samples](../../document-extensions/timeseries/rollup-and-retention#samples)  

{NOTE/}

---

{PANEL: Time-Series Policies}  

A collection can be configured with multiple time-series policies which apply to all 
time-series in the collection.  
*Raw* time-series policies define the retention span for all raw time-series - there 
is up to one of these per collection. The other time-series policies each define the 
creation of rollups. Each raw time-series gets each of the rollups defined by the 
collection configuration. A rollup time-series belongs to the same document as the 
raw time-series.  
<br/>
###Rollups  

The **rollup** process divides time-series entries into chunks of a specified span 
of time (such as a minute, or a day). The data from all the entries in this span is 
aggregated into six useful values:  

  * *First* - the value of the first entry in the span.  
  * *Last* - the value of the last entry.  
  * *Min* - the smallest value.  
  * *Max* - the largest value.  
  * *Sum* - the sum of all the values in the span.  
  * *Count* - the total number of entries in the span.  
<br/>
!["Rollup time-series entries"](images/rollup-1.png "A rollup time-series' entries")

**Values**:  
Time-series entry can have an array of numerical values, so each of these six 
aggregations are made for each of the values. A new "rollup" time-series is created 
with one entry for each span. If the raw time-series has *n* values per entry, the 
rollup time-series will have _6*n_ per entry: the first six will be the summary of 
the first raw value, the next six will be aggregations of the next raw value, and 
so on.  

**Timestamp**:  
The aggregation span is a round number of one of these time units: a second, day, 
week, month, or year. The span includes all entries starting at a round number of time 
units, and ending at another round number *minus one millisecond* inclusive - since 
milliseconds are the minimum resolution in RavenDB time-series. The timestamp for a 
rollup entry is the *end* of the span it represents, so it is a round number of time 
units minus one millisecond.  

**Name**:  
A rollup time-series name has this format:  
`"<name of raw time-series>@<name of time-series policy>"`  
It is a combination of the name of the raw time-series and the name of the 
time-series policy separated by a `@` character - in the image above these are 
"Heartrate & Blood Pressure" and "ByMinute" respectively.  

{PANEL/}

{PANEL: Usage Flow and Syntax}  

To configure time-series policies for one or more collections:  

* Create time-series policy [objects](../../document-extensions/timeseries/rollup-and-retention#the-two-types-of-time-series-policy).  
* Use those to populate `TimeSeriesCollectionConfiguration` [objects](../../document-extensions/timeseries/rollup-and-retention#and-) 
for each collection you want to configure.  
* Use _those_ to populate a `TimeSeriesConfiguration` [object](../../document-extensions/timeseries/rollup-and-retention#and-) 
which will belong to the whole database.  
* Finally, use the `ConfigureTimeSeriesOperation` [operation](../../document-extensions/timeseries/rollup-and-retention#the-time-series-configuration-operation) 
to send the new configurations to the server.  
<br/>
###Syntax  

####The two types of time-series policy:

{CODE-BLOCK: csharp}
public class TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime;
    public TimeValue AggregationTime;
}

public class RawTimeSeriesPolicy : TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime;
    // Does not perform aggregation
}
{CODE-BLOCK/}

| Property | Description |
| - | - |
| `Name` | This `string` is used to create the names of the rollup time-series created by this policy.<br/>`Name` is added to the name of the raw time-series - with `@` as a separator - to create the name of the resulting rollup time-series. |
| `RetentionTime` | Time-series entries older than this time span (see `TimeValue` below) are automatically deleted. Applies to the original, or "raw" time-series in the case of `RawTimeSeriesPolicy`, or to the rollup time-series created by a `TimeSeriesPolicy`. |
| `AggregationTime` | The time-series data being rolled up is divided at round time units, into parts of this length of time. Each of these parts is aggregated into an entry of the rollup time-series. |
<br/>
####The `TimeValue` struct

{CODE-BLOCK: csharp}
public struct TimeValue : IDynamicJson, IEquatable<TimeValue>
{
    
    public static TimeValue FromSeconds(int seconds);
    public static TimeValue FromMinutes(int minutes);
    public static TimeValue FromHours(int hours);
    public static TimeValue FromDays(int days);
    public static TimeValue FromMonths(int months);
    public static TimeValue FromYears(int years);
}
{CODE-BLOCK/}

Each of the above `TimeValue` methods returns a `TimeValue` object representing a 
whole number of the specified time units. These are passed as the aggregation and 
retention span of time-series policies.  
<br/>
####`TimeSeriesCollectionConfiguration` and `TimeSeriesConfiguration`

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

| Property | Description |
| - | - |
| `Disabled` | If set to `true`, rollup processes will stop, and time-series data will not be deleted by retention policies. |
| `Policies` | Populate this `List` with your time-series policies, including the `RawTimeSeriesPolicy`. The order does not matter. |
| `Collections` | Populate this `Dictionary` with the `TimeSeriesCollectionConfiguration`s and the name of the corresponding collection. |
<br/>
####The Time-Series Configuration Operation

{CODE-BLOCK: csharp}
public ConfigureTimeSeriesOperation(TimeSeriesConfiguration configuration);
{CODE-BLOCK/}

Pass this your `TimeSeriesConfiguration`, see usage example below.

{PANEL/}

{PANEL: Samples}

How to create time-series policies for a collection and pass them to the server:  

{CODE rollup_and_retention_0@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  

How to access a rollup time series:  

{CODE rollup_and_retention_1@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}  
{PANEL/}

## Related articles  
###Studio  
* [Time-Series Interface in Studio]()

###Time-Series  
* [Time Series Overview](../../document-extensions/timeseries/overview)  
* [API Overview](../../document-extensions/timeseries/client-api/api-overview)  

###Client-API  
* [What Are Operations?](../../client-api/operations/what-are-operations)

