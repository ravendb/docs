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

Rollup time series are created automatically according to rollup policies that can be defined from the Studio or from the client code.  

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

{NOTE: }

#### Create time series policies

---

{CODE:nodejs rollup_1@documentExtensions\timeSeries\rollupAndRetention.js /}

{NOTE/}
{NOTE: }

#### Retrieve rollup data

---

* Retrieving entries from a rollup time series is similar to getting the raw time series data.

* Learn more about using `timeSeriesFor.get` in [Get time series entries](../../document-extensions/timeseries/client-api/session/get/get-entries).

{CODE:nodejs rollup_2@documentExtensions\timeSeries\rollupAndRetention.js /}

{NOTE/}
{PANEL/}

{PANEL: Syntax}

---

### The time series policies

* Raw policy:  
  * Used to define the retention time of the raw time series. 
  * Only one such policy per collection can be defined.
  * Does not perform aggregation.

* Rollup policy:  
  * Used to define the aggregation time frame and retention time for the rollup time series.
  * Multiple policies can be defined per collection.

{CODE:nodejs syntax_1@documentExtensions\timeSeries\rollupAndRetention.js /}

| Property             | Description                                                                                                                                                                                                    |
|----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **name**             | This string is used to create the name of the rollup time series.<br/>`Name` is added to the raw time series name - with `@` as a separator,<br>e.g.: `<name of raw time series>@<name of time series policy>` |
| **retentionTime**    | Time series entries older than this `TimeValue` are automatically deleted.                                                                                                               |
| **aggregationTime** | The time series data being rolled up is divided into parts of this length of time, rounded to nearest time units. Each part is aggregated into an entry of the rollup time series.                             |

{CODE:nodejs syntax_2@documentExtensions\timeSeries\rollupAndRetention.js /}

{INFO: }
The main reason we use `TimeValue` rather than something like `TimeSpan` is that `TimeSpan` doesn't have a notion of 'months' 
because a calendar month is not a standard unit of time (as it can range from 28 to 31 days).  
`TimeValue` enables you to define retention and aggregation spans specifically tailored to calendar months.
{INFO/}

---

### The time series configuration object

{CODE:nodejs syntax_3@documentExtensions\timeSeries\rollupAndRetention.js /}

| Property        | Description                                                                                                             |
|-----------------|-------------------------------------------------------------------------------------------------------------------------|
| **collections** | Populate this dictionary with the collection names and their corresponding `TimeSeriesCollectionConfiguration` objects. |
| **disabled**    | If set to `true`, rollup processes will stop, and time series data will not be deleted by retention policies.           |
| **policies**    | Populate this list with your rollup policies.                                                                          |
| **rawPolicy**   | The `RawTimeSeriesPolicy`, the retention policy for the raw time series.                                                |

---

### The time series configuration operation

{CODE:nodejs syntax_4@documentExtensions\timeSeries\rollupAndRetention.js /}

| Parameter         | Description                                                  |
|-------------------|--------------------------------------------------------------|
| **configuration** | The `TimeSeriesConfiguration` object to deploy to the server |

Learn more about operations in: [What are operations](../../client-api/operations/what-are-operations).  

{PANEL/}

## Related articles  
###Studio  
- [Time Series Interface in Studio](../../studio/database/document-extensions/time-series)

###Time Series  
- [Time Series Overview](../../document-extensions/timeseries/overview)  
- [API Overview](../../document-extensions/timeseries/client-api/overview)  

###Client-API  
- [What Are Operations?](../../client-api/operations/what-are-operations)

