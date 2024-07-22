# Time Series Rollups and Retention
---

{NOTE: }

Many time series applications produce massive amounts of data at a steady rate.  
**Time Series Policies** help you manage your data in two ways:  

* Creating **Rollups**:  
  Summarizing time series data by aggregating it into the form of a new, lower-resolution time series.

* Limiting **Retention**:  
  Controlling the duration for which time series data is kept before deletion.
 
* In this page:  
  * [Time series policies](../../document-extensions/timeseries/rollup-and-retention#time-series-policies)  
  * [Examples](../../document-extensions/timeseries/rollup-and-retention#examples)  
      * [Create time series policy](../../document-extensions/timeseries/rollup-and-retention#create-time-series-policies)  
      * [Retrieve rollup data](../../document-extensions/timeseries/rollup-and-retention#retrieve-rollup-data)
  * [Syntax](../../document-extensions/timeseries/rollup-and-retention#syntax)

{NOTE/}

---

{PANEL: Time series policies}

#### What are rollups?

A rollup is a time series that summarizes the data from another time series, 
with each rollup entry representing a specific time frame in the original time series.  
Each rollup entry contains 6 values that aggregate the data from all the entries in the original time frame:  

* *First* - the value of the first entry in the frame.  
* *Last* - the value of the last entry.  
* *Min* - the smallest value.  
* *Max* - the largest value.  
* *Sum* - the sum of all the values in the frame.  
* *Count* - the total number of entries in the frame.  

This results in a much more compact time series that still contains useful information about the original time series (also called "raw" time series).

#### Rollup policies:

Rollup time series are created automatically according to rollup policies that can be defined from Studio or client code.  

* A rollup policy applies to all time series of every document in the given collection. 

* Each collection can be configured to have multiple policies which are applied sequentially:
  * The raw time series is first rolled up using the policy with the shortest aggregation frame.
  * Subsequently, the resulting rollup time series is further aggregated using the policy with the next shortest aggregation frame,  
    and so on.

[Querying with group-by](../../document-extensions/timeseries/querying/aggregation-and-projections) 
will transparently traverse over the rollups to retrieve the relevant results.  

Let's look at an example of rollup data:  

!["Rollup time series entries"](images/rollup-1.png "A rollup time series' entries")

**1) Name:**  
The name of a rollup time series has this format: `<name of raw time series>@<name of time series policy>`  
It is a combination of the name of the raw time series and the name of the time series policy separated by `@`.  
In the image above these are "HeartRates" and "byHour" respectively.  
For this reason, neither a time series name nor a policy name can have the character `@` in it.

**2) Timestamp:**  
The aggregation frame always begins at a round number of one of these time units: a second, minute, hour, day, week, month, or year. 
So the frame includes all entries starting at a round number of time units, and ending at a round number *minus one millisecond* 
(since milliseconds are the minimal resolution in RavenDB time series). 
The timestamp for a rollup entry is the beginning of the frame it represents.  

For example, if the aggregation frame is three days, a frame will start and end at a time stamps like:  
`2020-01-01 00:00:00` - `2020-01-03 23:59:59.999`.

**3) Values:**  
Each group of six values represents one value from the original entries. 
If the raw time series has `n` values per entry, the rollup time series will have `6 * n` per entry: 
the first six summarize the first raw value, the next six summarize the next raw value, and so on. 
The aggregated values have the names: `"First (<name of raw value>)", "Last (<name of raw value>)", ...` respectively.  

Because time series entries are limited to 32 values, rollups are limited to the first five values of an original time series entry, or 30 aggregate values.  

{PANEL/}

{PANEL: Examples}

#### Create time series policies:

{CODE rollup_and_retention_0@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}

---

#### Retrieve rollup data:

* Retrieving entries from a rollup time series is similar to getting the raw time series data.

* Learn more about using `TimeSeriesFor.Get` in [Get time series entries](../../document-extensions/timeseries/client-api/session/get/get-entries).

{CODE rollup_and_retention_1@DocumentExtensions\TimeSeries\RollupAndRetention.cs /}

{PANEL/}

{PANEL: Syntax}

### The time series policies

* Raw policy:  
  * Used to define the retention time of the raw time series. 
  * Only one such policy per collection can be defined.
  * Does not perform aggregation.

* Rollup policy:  
  * Used to define the aggregation time frame and retention time for the rollup time series.
  * Multiple policies can be defined per collection.

{CODE-BLOCK: csharp}
public class RawTimeSeriesPolicy : TimeSeriesPolicy
{
    public TimeValue RetentionTime;
}

public class TimeSeriesPolicy
{
    public string Name;
    public TimeValue RetentionTime; { get; protected set; }
    public TimeValue AggregationTime; { get; private set; }
}
{CODE-BLOCK/}

| Property             | Description                                                                                                                                                                                                    |
|----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Name**             | This string is used to create the name of the rollup time series.<br/>`Name` is added to the raw time series name - with `@` as a separator,<br>e.g.: `<name of raw time series>@<name of time series policy>` |
| **RetentionTime**    | Time series entries older than this time span (see `TimeValue` below) are automatically deleted.                                                                                                               |
| **AggregationTime**  | The time series data being rolled up is divided into parts of this length of time, rounded to nearest time units. Each part is aggregated into an entry of the rollup time series.                             |

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

Each of the above `TimeValue` methods returns a `TimeValue` object representing a whole number of the specified time units. 
These methods are used to define the aggregation and retention spans om time series policies.  

{INFO: }
The main reason we use `TimeValue` rather than something like `TimeSpan` is that `TimeSpan` doesn't have a notion of 'months' 
because a calendar month is not a standard unit of time (as it can range from 28 to 31 days).  
`TimeValue` enables you to define retention and aggregation spans specifically tailored to calendar months.
{INFO/}

---

### The time series configuration object

{CODE-BLOCK: csharp}
public class TimeSeriesConfiguration
{
    public Dictionary<string, TimeSeriesCollectionConfiguration> Collections;
}

public class TimeSeriesCollectionConfiguration
{
    public bool Disabled;
    public List<TimeSeriesPolicy> Policies;
    public RawTimeSeriesPolicy RawPolicy;
}
{CODE-BLOCK/}

| Property        | Description                                                                                                               |
|-----------------|---------------------------------------------------------------------------------------------------------------------------|
| **Collections** | Populate this `Dictionary` with the collection names and their corresponding `TimeSeriesCollectionConfiguration` objects. |
| **Disabled**    | If set to `true`, rollup processes will stop, and time series data will not be deleted by retention policies.             |
| **Policies**    | Populate this `List` with your rollup policies.                                                                           |
| **RawPolicy**   | The `RawTimeSeriesPolicy`, the retention policy for the raw time series.                                                  |

---

### The time series configuration operation

{CODE-BLOCK: csharp}
public ConfigureTimeSeriesOperation(TimeSeriesConfiguration configuration);
{CODE-BLOCK/}

Learn more about operations in: [What are operations](../../client-api/operations/what-are-operations).  

---

### Casting time series entries

Time series entries are of one of the following classes:  

{CODE-BLOCK: csharp}
public class TimeSeriesEntry {   }
public class TimeSeriesEntry<T> : TimeSeriesEntry {   }
public class TimeSeriesRollupEntry<TValues> : TimeSeriesEntry {   }
{CODE-BLOCK/}

If you have an existing rollup entry of type `TimeSeriesEntry`,  
you can cast it to a `TimeSeriesRollupEntry` using `AsRollupEntry()`.  

{CODE-BLOCK: csharp}
public static TimeSeriesRollupEntry<T> AsRollupEntry<T>(this TimeSeriesEntry<T> entry);
{CODE-BLOCK/}

You can cast a `TimeSeriesRollupEntry` to a `TimeSeriesEntry` directly.  
Its values will consist of all the `First` values of the rollup entry.  

{CODE-BLOCK: csharp}
var rollupEntry = new TimeSeriesRollupEntry<int>(new DateTime(2020,1,1));
TimeSeriesEntry<int> TSEntry = (TimeSeriesEntry<int>)rollupEntry;
{CODE-BLOCK/}

Read more about time series with generic types [here](../../document-extensions/timeseries/client-api/named-time-series-values).

{PANEL/}

## Related articles  
###Studio  
- [Time Series Interface in Studio](../../studio/database/document-extensions/time-series)

###Time Series  
- [Time Series Overview](../../document-extensions/timeseries/overview)  
- [API Overview](../../document-extensions/timeseries/client-api/overview)  

###Client-API  
- [What Are Operations?](../../client-api/operations/what-are-operations)

