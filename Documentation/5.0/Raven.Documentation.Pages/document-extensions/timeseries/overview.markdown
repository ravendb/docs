# Time Series Overview
---

{NOTE: }

* RavenDB’s distributed time-series are series of numeric values 
  ordered by time.
  You can use time-series to track changes over time in various process like – 
   * the heartrate measured by a wrist watch
   * the measurements of a thermometer
   * the peaks and lows of a home cable consumption

* Create and manage time-series using API methods, or through 
  the [Studio](../../studio/database/documents/document-view/additional-features/counters).  

* In this page:  
  * [Why Use Time-Series?](../../document-extensions/timeseries/overview#why-use-time-series?)  
  * [Time-Series Are Document Extensions](../../document-extensions/timeseries/overview#time-series-are-document-extensions)  
  * [Managing Time-Series](../../document-extensions/timeseries/overview#managing-time-series)  
  * [Time-Series Entries](../../document-extensions/timeseries/overview#time-series-entries)  
  * [The Time-Series API](../../document-extensions/timeseries/overview#the-time-series-api)  

{NOTE/}

---

{PANEL: Why use Time-Series?}

The need to track and monitor series of dynamically changing values, particularly 
fast-pacing and large series, is advocated by the rapid growth in the usage of 
wearable devices, monitoring tools, navigation aids, security sensors and countless 
other systems whose output over time may or may not be accumulated and put to good 
use - depending on the awareness to its existence and value and on the resources 
allocated to process it.  

RavenDB offers cross-system support for the procession, usage and storage 
of time-series.  

{INFO: }
There are numerous applications to time-series' like -  

* Routinely updating a time-series with the **data collected from a wearable pulse 
  monitor**. The data can then be rolled up in a comfortable resolution, queried, 
  and plotted for a graphical view of the heartrate during the day.  
* Measuring the **changes in a home cables consumption** during the day  
* A statistical curve can be created over **memory measurements** reported by 
  a device over a day, extrapolating high and low usage peaks and comparing the 
  data to that of other days.  
{INFO/}

{PANEL/}

{PANEL: Time-Series Are Document Extensions}

RavenDB’s Time-Series, like its distributed counters, attachments and document 
revisions, are **document extensions**.  
A time-series is always attached to an owner document, so its relation to a user, 
a device, a company or a process remains clear at all times.  

The time-series’ name is kept at its owner-document’s metadata, while its values 
are kept separately. This way, changes in time-series values do not cause document 
changes.  

{PANEL/}

{PANEL: Managing Time-Series }

RavenDB's time-series support covers many of its other features.  

* **Indexing**  
  Time-series can be indexed to minimize query time.  
* **Queries**  
  Time-series **can be queried**.  
  {INFO: }
  Queries are often performed over a time window, e.g. an hour or a week, 
  to produce measurements like the minimal, average or maximal measurement 
  during this window or to create a statistical curve.  
  {INFO/}
* **Rollup**  
  You can set a time-series **rollup policy**, to aggregate it in a comfortable, 
  usable resolutions.  
* **Retention**  
  You can set a time-series **retention policy** to automatically remove 
  time-series that fill their expiration period.  
* **Including**  
  You can **include** time-series while loading documents, explicitly or by query, 
  so they would be held by the session and instantly delivered when the user needs 
  them.  
* **Patching**  
  Time-series can be **patched to documents**, explicitly or by query.  
* **Backup**  
  You can set an ongoing backup task to backup time-series.  

{PANEL/}

{PANEL: Time-Series Entries}

Each **time-series entry** is consisted of -  

* A **timestamp** that indicates the measurement datetime in a millisecond precision.  
* Up to 32 **values**  
* An optional **tag** that relates the time-series entry to its source,  
  e.g. a document ID for the specifications of the device that provided 
  the values.  


{PANEL/}

{PANEL: The Time-Series API}

The time-series API includes `session` methods and `document-store` operations.  
To learn how to manage time-series using the API, read the [articles 
dedicated to it](../../document-extensions/timeseries/client-api/api-overview).  



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
