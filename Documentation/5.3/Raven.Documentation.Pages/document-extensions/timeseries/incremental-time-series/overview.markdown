# Document Extensions: Time Series Overview

---

{NOTE: }

* An **Incremental Time Series** is a [time series](../../../document-extensions/timeseries/overview) 
  whose values can be increased and decreased, behaving much like [counters](../../../document-extensions/counters/overview) 
  embedded in time series entries.  

* Multiple clients and cluster nodes can simultaneously modify the same time series entries 
  without conflict.  

* Multiple modifications to an entry value are accumulated by the cluster 
  to combine a unified value.  
  A user can retrieve both the unified value of a time series entry, and 
  the values that combine it.  

* Incremental Time series can be managed using [API methods](../../document-extensions/timeseries/client-api/overview) 
  and the [Studio](../../studio/database/document-extensions/time-series).  

* In this page:  
  * [Why Use Incremental Time Series?](../../document-extensions/timeseries/overview#overview)  
  * [Incremental Time Series Entries]()

* In this page:  
  * [Overview](../../document-extensions/timeseries/overview#overview)  
     * [RavenDB's Incremental Time Series Implementation](../../document-extensions/timeseries/overview#ravendbs-time-series-implementation)  
     * [Distributed Incremental Time Series](../../document-extensions/timeseries/overview#distributed-time-series)  
     * [Time Series Features](../../document-extensions/timeseries/overview#time-series-features)  
  * [Time Series Data](../../document-extensions/timeseries/overview#time-series-data)  
  * [Time Series Segments](../../document-extensions/timeseries/overview#time-series-segments)  
  * [Time Series Entries](../../document-extensions/timeseries/overview#time-series-entries)  
      * [Timestamps](../../document-extensions/timeseries/overview#timestamps)  
      * [Values](../../document-extensions/timeseries/overview#values)  
      * [Names](../../document-extensions/timeseries/overview#tags)  

{NOTE/}

---

{PANEL: Preview}

---

### Time Series -vs- Incremental Time Series

Incremental time series are basically time series, and their principles 
and in most part identical. Notice, however, a few significant changes.  

* **Rollups**  
  You **can** use rollups to aggregate incremental time series values.  
  The resulting time series, however, is non-incremental, and you can 
  no longer handle its values as counters.  
* **No Tags**  
  Incremental time series incorporate no tags, and therefore all 
  tag functionality (e.g. referencing a document) is unavailable.  

---

### Why Use Incremental Time Series?

* **A Time Series That Counts**  
  Many scenarios require us to both continuously update a counter **and** keep track of 
  the changes made in its value over time for future reference.  
  [Counters](../../../document-extensions/counters/overview) let us count, but not keep track of changes.  
  [Time series](../../../document-extensions/timeseries/overview) let us record changes over time, 
  but they are not designated for counting.  
  Incremental time series allows us to easily achieve both goals, by permitting clients to use 
  their values as counters **and** storing the values in an evolving time series.  

  An incremental time series can be used, for example, to store the number of downloads made 
  from a web page. The time series can then be queried for hourly changes in the number of 
  downloads over the last week, and the results can be plotted in a graph  

* **Parallel Counting**  
  Incremental time series also permit clients to increase or decrease values (a.k.a. "counters") 
  of the same time series entry, at the same time, with no conflicts  
  Multiple values collected from clients at the same time are accumulated, and a user can retrieve 
  both the accumulated value and the multiple values that combine it.  

  A real-life scenario that makes good use of this feature is a bunch of traffic cameras 
  installed in different directions of a large road intersection, counting passing cars.  
  All cameras transmit their findings to the same time series, simultaneously updating 
  the same entries.  
  An admin can then utilize both the unified values, counting all cars passing through 
  the junction in any given moment, and each camera's data flow for a more detailed look.  

{PANEL/}

{PANEL: Incremental Time Series Entries}
* Similar to non-incremental time series, incremental time series are composed of consecutive [entries](../../../document-extensions/timeseries/overview#time-series-entries).  
   * An entry is marked by a single [timestamp](../../../document-extensions/timeseries/overview#timestamps).  
   * An entry is also marked by the cluster node that created it.  
   * An entry can hold up to 32 values.  
   * Each entry value can be incremented and decremented.  
   * Multiple entries can share a timestamp, indicating updates from multiple sources.  
     The values from the different sources are unified to a single value.  
     You can retrieve both a counter's unified value, and the list of values that combine it.  
{PANEL/}



{PANEL: Overview}

#### Distributed Time Series

Distributed clients and nodes can modify time series concurrently; the 
modifications are merged by the cluster [without conflict](../../document-extensions/timeseries/design#no-conflicts).  


* **Time Series Querying and Aggregation**  
      
    * [LINQ and raw RQL queries](../../document-extensions/timeseries/querying/overview-and-syntax)  
      Flexible queries and aggregations can be executed using LINQ expressions and raw RQL 
      over time series **timestamps**, **tags** and **values**.  
* **Time Series Indexing**  
  Time series can be [indexed](../../document-extensions/timeseries/indexing).  
* [Rollup and Retention Policies](../../document-extensions/timeseries/rollup-and-retention)  
   * **Rollup Policies**  
     You can set time series rollup policies to aggregate large series into 
     smaller sets by your definitions.  
   * **Retention Policies**  
     You can set time series retention policies to automatically remove 
     time series entries that have reached their expiration date/time.  
* [Including Time Series](../../document-extensions/timeseries/client-api/session/include/overview)  
  You can include (pre-fetch) time series data while loading documents.  
  Included data is held by the client's session, and is delivered to the 
  user with no additional server calls.  
* **Patching**  
  You can patch time series data to your documents.  
  (visit the [API documentation](../../document-extensions/timeseries/client-api/overview) to learn more).  

{PANEL/}

{PANEL: Time Series Entries}

Each time series segment is composed of consecutive **time series entries**.  
Each entry is composed of a **timestamp**, 1 to 32 **values**, and an **optional tag**.  
    

---

#### Timestamps

{INFO: }
A single `DateTime` timestamp marks each entry in millisecond precision.  
{INFO/}

Timestamps are always indicated using [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time).  

Timestamps, and therefore time series entries, are always ordered **by time**, 
from the oldest timestamp to the newest.  
E.g. in a heart rate time series, timestamps would indicate the time in which each 
heart rate measurement has been taken.  

---

#### Values

{INFO: }
Up to 32 `double` **values** can be appended per-entry.  
{INFO/}

We allow storing as many as 32 values per entry, since appending multiple 
values may be a requirement for some time series. Here are a few examples.  

* A heart-rate time series  
  An entry with a single value (the heart-rate measurement taken by 
  a smart wrist-watch) is added to the time series every minute.  

* A truck-route time series  
  An entry with 2 values (the latitude and longitude reported by 
  a GPS device) is added to the time series every minute.  

* A stock-price time series  
  An entry with 5 values (stock price when the trade starts and 
  ends, its highest and lowest prices during the day, and its daily 
  trade volume) is added to the time series every day.  

---


{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../document-extensions/timeseries/rollup-and-retention)  

