# Time-Series Overview
---

{NOTE: }

* Time-series are vectors of data points, whose values are ordered by time. 
  You can populate time-series with the values produced by various systems 
  (including an ever-growing number of IoT devices), and simplify the management 
  and usage of the collected data.  

* A time-series can contain for example -  
   * A sequence of heartrate values, collected from a smart wrist-watch  
   * The measurements of a weather-station's thermometer  
   * Peaks and lows in a cable-modem usage  
   * Coordinates sent by a delivery truck's GPS tracker  
   * Daily changes in stock prices  

* Create and manage time-series using API methods or the 
  [Studio](../../studio/database/document-extensions/time-series).  

* In this page:  
  * [The Time-Series Interface](../../document-extensions/timeseries/overview#the-time-series-interface)  
     * [Time-Series Features](../../document-extensions/timeseries/overview#time-series-features)  
  * [Time-Series Are Document Extensions](../../document-extensions/timeseries/overview#time-series-are-document-extensions)  
     * [Document Extensions](../../document-extensions/timeseries/overview#document-extensions)  
     * [Data Storage](../../document-extensions/timeseries/overview#data-storage)  
  * [Time-Series Distribution](../../document-extensions/timeseries/overview#time-series-distribution)  
  * [Time-Series Entries](../../document-extensions/timeseries/overview#time-series-entries)  

{NOTE/}

---

{PANEL: The Time-Series Interface}

RavenDB's time-series functionality is fully integrated into its 
[document model](../../document-extensions/timeseries/overview#time-series-are-document-extensions), 
[distributed environment](../../document-extensions/timeseries/overview#time-series-distribution), 
and various other features.  

It is a comfortable, simple and powerful interface that can be used 
in numerous ways. E.g. -  

* A time-series can be updated routinely with data collected from a wearable pulse 
  monitor. The data can then be rolled up to create a low-resolution dataset, and 
  plotted for a graphical view of the heartrate during a day or a week.  
* A time-series can collect the data sent by a cable modem, to check its 
  consumption in different hours.  

---

#### Time-Series Features  

* **Highly-Efficient Storage Management**  
  Time-series storage is extremely efficient thanks, among other 
  reasons, to its usage of RavenDB's low-level storage engine.  
* **A Thorough Set of [API Methods](../../document-extensions/timeseries/client-api/api-overview)**  
  The time-series API includes `session methods` and `store operations`.  
* **[Full GUI Support](../../studio/database/document-extensions/time-series)**  
  You can watch and manage time-series using the GUI.  
* **Time-Series Querying and Aggregation**  
  High-performance queries and aggregations can be performed over 
  time-series [tags and values](../../document-extensions/timeseries/overview#time-series-entries) 
  using LINQ expressions and raw RQL.  
* **Time-Series Indexing**  
  Time-series can be indexed using LINQ expressions.  
* **[Rollup and Retention Policies](../../document-extensions/timeseries/rollup-and-retention)**  
   * **Rollup Policies**  
     You can set time-series rollup policies to aggregate large series into 
     smaller sets by your definitions.  
   * **Retention Policies**  
     You can set time-series retention policies to automatically remove series 
     that have reached an expiration threshhold.  
* **[Including Time-Series](../../document-extensions/timeseries/client-api/session-methods/include-ts-data/include-ts-overview)**  
  You can include (pre-fetch) time-series data while loading documents, 
  so they'd be held by the client's sesion and instantly delivered to the 
  user when they are requested.  
* **Patching**  
  You can patch time-series data to your documents.  
  (visit the [API documentation](../../document-extensions/timeseries/client-api/api-overview) to learn more).  

{PANEL/}

{PANEL: Time-Series Are Document Extensions}

RavenDB’s Time-Series, like its distributed counters, attachments and document 
revisions, are **document extensions**.  

---

#### Document Extensions  

A time-series always extends a specific document.  

If a time-series is populated with events related to a product, 
for example, it may extend a document with this product's specifications.  

One advantage of this method, is that the time-series' context, 
be it a product, a device, a company or a process. is always clear.  

A second advantage is that time-series are comfortable managed using 
the solid document interface.  

---

#### Data Storage  

The time-series’ name is kept at its owner-document’s metadata, while its values 
are kept separately. This way, changes in time-series values do not cause document 
changes.  
This is important because time-series are expected to grow large and their values 
to change often.  

Time-series storage management is highly efficient both for storage and for querying.  

{PANEL/}


{PANEL: Time-Series Distribution}

Distributed clients and nodes can modify time-series concurrently and the 
modifications are merged by the cluster with [no conflict](../../document-extensions/timeseries/design#no-conflicts).  

{PANEL/}

{PANEL: Time-Series Entries}

A time-series is composed of **time-series entries**.  
Each entry consists of -  

* A **timestamp** that indicates the measurement datetime in a millisecond precision.  
* Up to 32 **values**  
* An optional **tag** that relates the time-series entry to its source,  
  e.g. a document ID for the specifications of the device that provided 
  the values.  

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
