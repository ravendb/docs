# Incremental Time Series: Overview

---

{NOTE: }

* **Incremental Time Series** are [time series](../../../document-extensions/timeseries/overview) 
  whose values are designated to function as counters, resembling [Counters](../../../document-extensions/counters/overview) 
  embedded in time series entries.  
  
* Distributed clients and nodes can increase and decrease incremental time series 
  values concurrently; the modifications are merged by the cluster without conflict.  

* Incremental Time series can be created and managed using dedicated [API methods](../../../document-extensions/timeseries/incremental-time-series/client-api) 
  and via [Studio](../../../studio/database/document-extensions/time-series#incremental-time-series).  

* In this page:  
  * [Why Use Incremental Time Series?](../../document-extensions/timeseries/overview#overview)  
  * [Incremental Time Series Entries]()

{NOTE/}

---

{PANEL: Preview}

### Why Use Incremental Time Series?

* **A Time Series That Counts**  
  Many scenarios require us to continuously update a counter, and yet keep track of 
  the changes made in its value over time for future reference.  
  [Counters](../../../document-extensions/counters/overview) let us count, but not keep track of changes.  
  [Time series](../../../document-extensions/timeseries/overview) let us record changes over time, 
  but are not designated for counting.  
  **Incremental time series** allow us to easily achieve both goals, permitting clients to use 
  entry values as counters and store value modifications in an evolving time series.  
  <br>
   Use case:  
   A web page admin can store the ongoing number of downloads made by visitors, in an 
   incremental time series. The time series can then be queried for hourly or daily changes 
   in the number of downloads over the last week, and the results can be plotted in a graph  

* **Parallel Modification**  
  An incremental time series entry can be modified by multiple clients and cluster nodes without 
  conflict. Within the entry, the value that arrives from each node is stored under the node's ID.  
  When users retrieve values from a time series entry, they are permitted to retrieve both the 
  entry's "Current Value", accumulated from all node values, and the value stored by each node.  
  <br>
  Use case:  
  A real-life scenario that makes good use of this feature is a bunch of traffic cameras 
  installed in different directions of a large road intersection, counting passing cars.  
  The cameras are connected to different cluster nodes, that transmit their modifications 
  simultaneously to the same entries.  
  An admin can then utilize both the accumulated values, counting all cars passing through 
  the junction in any given moment, and each camera's data flow for a more detailed look.  
 
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

