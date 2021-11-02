# Incremental Time Series: Overview

---

{NOTE: }

* **Incremental Time Series** are [time series](../../../document-extensions/timeseries/overview) 
  whose values are designated to function as counters, resembling [Counters](../../../document-extensions/counters/overview) 
  embedded in time series entries.  
  
* Distributed clients and nodes are permitted to increase and decrease incremental 
  time series values concurrently without conflict.  

* Incremental Time series can be created and managed using dedicated [API methods](../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview) 
  and via [Studio](../../../studio/database/document-extensions/time-series#incremental-time-series).  

* In this page:  
  * [Overview](../../../document-extensions/timeseries/incremental-time-series/overview#overview)  
     * [What are Incremental Time Series and Why Use Them](../../../document-extensions/timeseries/incremental-time-series/overview#what-are-incremental-time-series-and-why-use-them)  
     * [How does it work](../../../document-extensions/timeseries/incremental-time-series/overview#how-does-it-work)  
  * [Incremental Time Series -vs- Other Features](../../..//document-extensions/timeseries/incremental-time-series/overview#incremental-time-series--vs--other-features)  
  * [Incremental Time Series Structure](../../../document-extensions/timeseries/incremental-time-series/overview#incremental-time-series-structure)  

  

{NOTE/}

---

{PANEL: Overview}

### What are Incremental Time Series and Why Use Them

* **A Time Series That Counts**  
  Many scenarios require us to continuously update a counter, and yet keep track of 
  the changes made in its value over time.  
  [Counters](../../../document-extensions/counters/overview) let us count, but not keep track of changes.  
  [Time series](../../../document-extensions/timeseries/overview) let us record changes over time, 
  but are not designated for counting.  
  Incremental time series allow us to easily achieve both goals, by permitting clients 
  to **use entry values as counters, while storing their modifications in an evolving time series**.  
   {NOTE: Use Case}
   A web page admin can store the ongoing number of downloads made by visitors, in an 
   incremental time series. The time series records download numbers over time, can 
   be queried at any time for hourly or daily changes over the passing week or month, 
   and a graph of the results can be plotted using Studio or any other tool.  
   {NOTE/}

* **Parallel Modification**  
  An incremental time series entry can be **modified by multiple clients and cluster nodes without 
  conflict**. Within the entry, the value that arrives from each node is stored separately from values 
  that arrived from other nodes. When users retrieve values from a time series entry, they can choose 
  whether to retrieve only the total sum, accumulated from values sent by all clients and nodes, 
  or get the value stored by each node.  
   {NOTE: Use Case}
   A real-life scenario that makes good use of this feature is a bunch of traffic cameras 
   installed in different directions of a large road intersection, counting passing cars.  
   Each camera reports to its own cluster node, and the cluster collects the data into 
   a single time series. Each time series entry contains data from all nodes.  
   An admin can then query both the accumulated values, counting all cars passing through 
   the junction at any given moment, and each camera's data separately for a detailed look 
   at its side of the junction.  
   {NOTE/}

### How does it work

* Clients can create and modify incremental time series entries' values 
  using [Session](../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview) 
  and [Operations](../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get) 
  API calls, and via [Studio](../../../studio/database/document-extensions/time-series#incremental-time-series).  
* Each cluster node collects incremental time series modifications from its users.  
  Concurrent modifications, made by clients to the same time series entry, are merged 
  by the node into a single value and stored in the entry under the ID of this node.  
* Values kept in the same time series entry by different nodes, can be 
  retrieved as a [single accumulated value](../../../document-extensions/timeseries/incremental-time-series/client-api/session/get) 
  as well as [separately](../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get).  
* This process is conflict-free, in both the clients' and in the nodes' level.  

{PANEL/}

{PANEL: Incremental Time Series -vs- Other Features}

* **Incremental Time Series and Non-Incremental Time series**  
   * Time series users [append](../../../document-extensions/timeseries/client-api/session/append) 
     values into it, in most cases with no intention to change them. A Heartrate 
     or a Stockprices time series needs no alteration.  
     Incremental time series values, on the other hand, are made to change, and users 
     always [Increment](../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment) 
     (or decrease, using a nagative number) them, making their changes in relation to 
     the values already stored. 
   * **No Tag**  
     Incremental Time Series incorporate [no tags](../../../document-extensions/timeseries/incremental-time-series/overview#incremental-time-series--vs--other-features), and tag functions 
     like referencing documents by their ID is unavailable for them.  
   * **Name Convention**  
     While non-incremental time series has no real name convention, Incremental 
     time series must always be given a name that starts with "INC:".  
     It can be in higher or lower case as you prefer.  
   * Incremental time series use their own [Session structure](../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview#methods), 
     but the [Operations](../../../document-extensions/timeseries/client-api/overview#available-time-series-store-operationsdocument-extensions/timeseries/client-api/overview#available-time-series-store-operations) 
     layer is valid for them as it is for non-incremental time series 
     and they can be [patched](../../../document-extensions/timeseries/client-api/operations/patch), 
     [indexed](../../../document-extensions/timeseries/indexing) and 
     [queried](../../../document-extensions/timeseries/querying/overview-and-syntax) 
     just like non-incremental time series.  

* Incremental Time Series and [Rollups](../../../document-extensions/timeseries/rollup-and-retention)  
  Rollup **can** be implemented over incremental time series, for 
  speedy filtering and size reduction in the original series.  
  **However**, the resulting rollup time series is **not* incremental.  
  It can be handled via `TimeSeriesFor`, not `IncrementalTimeSeriesFor`, 
  and its values can no longer be incremented.  

{PANEL/}

{PANEL: Incremental Time Series Structure}

* The basic structure and behavior of incremental time series are very similar 
  to [those of non-incremental time series](../../../document-extensions/timeseries/overview#time-series-data).  
* They are attached to documents like time series are (in Studio they are even accessed 
  through the familiar Time Series View).  
* They are divided into segments and entries just like non-incremental time series.  
* Each entry is signed by a single timestamp.  
  However, there may be cases in which multiple entries bear the same timestamp 
  (when clients outside the cluster update the time series, their values are stored 
  in entries of their own).  
* The main structural difference is their ability to store multiple per-node values 
  in their entries and manage them separately.  
* The number of values that can be stored per time series entry remains 32, but 
  as multiple nodes can store values in the same entry - incremental time series 
  entries may very well contain more data.  

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  

