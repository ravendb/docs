# Time Series Overview
---

{NOTE: }

* A Time Series is a dynamic array of keys coupled with timestamps, that 
  extends a document (much like [Counters]() and [attachments]() do). 
  Time Series are commonly used to measure the changes in an ongonig 
  process over time.  
  
      You can, for example, populate a time series with the data streamed 
  from a wearable pulse monitor. The data regarding each heartbeat is 
  reported along with its timestamp, so the person's heart rate can be 
  concluded and monitored.  
  
* Time Series can be created and managed using a dedicated API, 
  and via RavenDB's [Studio](../../../studio/database/documents/document-view/additional-features/timeseries).  

* In this page:  
  * [Why use Time Series?](../document-extensions/timeseries/timeseries-overview#why-use-time-series?)  
  * [Overview](../../../client-api/session/counters/overview#overview)  
  * [Managing Time Series](../../../client-api/session/counters/overview#managing-counters)  
      * [Time Series Methods](../../../client-api/session/counters/overview#yime-series-methods)  
      * [Managing Counters using `Operations`](../../../client-api/session/counters/overview#managing-timeseries-using-operations)  
{NOTE/}

---

{PANEL: Why use Time Series?}

Time series ease the storage and usage of long data sets created by fast-pace, dynamic processes.  
They are referred to by documents' metadata, but **stored separately** so high-rate updates would 
not invoke a document-change procedure.  
A native Time Series API makes it easy to create dynamic statistics related to monitored processes.  

Time series are mainly used for data analysis. 
Queries are often performed over a time window, e.g. an hour or a week, 
to produce measurements like the min, average and max values during this window, 
or to create a curve.  
Useful statistics can be performed, for example, over memory measurements sent by 
a device over a day, extrapolating high and low usage peaks and comparing the data 
to that of other days.  

---

####Tracking Changes Over Time
...

---

####Storing a Rapidly Updated Series
...

{PANEL/}

{PANEL: Overview}

---

####Rollup and Rotation

* Rollup  
  ... [write about rollup aggregation]  
* Rotation  
  An expiration period can be defined so that time values that exceed it would be removed.  
  ...

---

####Plotting

...[for the Studio documentation: presenting queried time series values as a graph]  

---

####Time Series and Conflicts
...

---

####Time Series Cost
...

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
