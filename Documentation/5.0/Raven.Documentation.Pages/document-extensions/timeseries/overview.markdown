# Time Series Overview
---

{NOTE: }

* A huge number of systems, including an expanding variety of IoT devices, 
  produce continuous streams of values that can be collected and used for 
  various needs. **Time series** are vectors of data points that are designated 
  to collect values over time, store them consecutively, and manage them 
  with high efficiency and performance.  

* RavenDB Time series can be managed and utilized using a thorough set of 
  [API methods](../../document-extensions/timeseries/client-api/overview), 
  the [Studio](../../studio/database/document-extensions/time-series), 
  and [various RavenDB features](../..//document-extensions/timeseries/time-series-and-other-features).  

* Time series data is **compressed** to lower storage usage and transaction time.  

* In this page:  
  * [Overview](../../document-extensions/timeseries/overview#overview)  
     * [RavenDB's Time Series Implementation](../../document-extensions/timeseries/overview#ravendbs-time-series-implementation)  
     * [Distrubuted Time Series](../../document-extensions/timeseries/overview#distrubuted-time-series)  
     * [Time Series as Document Extensions](../../document-extensions/timeseries/overview#time-series-as-document-extensions)  
     * [Time Series Features](../../document-extensions/timeseries/overview#time-series-features)  
  * [Time Series Data](../../document-extensions/timeseries/overview#time-series-data)  
  * [Separate Name and Data Storage](../../document-extensions/timeseries/overview#separate-name-and-data-storage)  
  * [Time Series Segments](../../document-extensions/timeseries/overview#time-series-segments)  
      * [Transactions Performance](../../document-extensions/timeseries/overview#transactions-performance)  
      * [Common-Queries Performance](../../document-extensions/timeseries/overview#common-queries-performance)  
  * [Time Series Entries](../../document-extensions/timeseries/overview#time-series-entries)  
      * [Timestamps](../../document-extensions/timeseries/overview#timestamps)  
      * [Values](../../document-extensions/timeseries/overview#values)  
      * [Tags](../../document-extensions/timeseries/overview#tags)  

{NOTE/}

---

{PANEL: Overview}

Time series can be **aggregated** and **queried** to illustrate process 
behavior, predict future developments, track noticeable value changes, 
and create other helpful statistics.  

Here are a few examples for value streams that can be easily and effectively 
handled by time series.  

* _A sequence of heartrate values can be collected from a smart 
  wrist-watch_, and be used to build a person's training program.  
* _Weather-stations' measurements_ collected over a chosen time period 
  can be compared to equivalent past periods to predict the weather.  
* _Bandwidth usage reports of a home cable modem monitor_ can be used 
  to build a better charging plan.  
* _Coordinates sent by delivery trucks' GPS trackers_ can be collected 
  and analyzed to secure the vehicles and improve the service.  
* _Daily changes in stock prices_ can be used to build investment plans.  

## RavenDB's Time Series Implementation  

Time series functionality is fully integrated into RavenDB's 
distributed environment and document model.  

---

#### Distrubuted Time Series

Distributed clients and nodes can modify time series concurrently; the 
modifications are merged by the cluster [without conflict](../../document-extensions/timeseries/design#no-conflicts).  

---

#### Time Series as Document Extensions

RavenDB’s Time Series, like its 
[distributed counters](../../client-api/session/counters/overview), 
[attachments](../../client-api/session/attachments/what-are-attachments) 
and [document revisions](../../client-api/session/revisions/what-are-revisions), 
are **document extensions**.  

* A time series always extends a single specific document.  
  The context and source of the time series can be kept clear this way, 
  and time series management can use the comfort and strength of the 
  document interface.  
  A barometer's specifications document, for example, can be the parent 
  document for a time series that is populated with measurements taken 
  by a barometer of this specification.  

* Like the other document extensions, time series can take part in fully 
  transactional operations.  

---

#### Time Series Features

Notable time series features include -  

* **Highly-Efficient Storage Management**  
  Time series data is [compressed](../../document-extensions/timeseries/design#compression) 
  and [segmented](../../document-extensions/timeseries/overview#time-series-segments) 
  to minimize storage usage and transmission time.  
* **A Thorough Set of API Methods**  
  The [time series API](../../document-extensions/timeseries/client-api/overview)**  
  includes a variety of `session methods` and `store operations`.  
* **Full GUI Support**  
  Time series can be viewed and managed using the [Studio](../../studio/database/document-extensions/time-series).  
* **Time Series Querying and Aggregation**  
    * [High-performance common queries](../../document-extensions/timeseries/overview#common-queries-performance)  
      The results of a set of common queries are prepared in advance in time series segments' 
      headers, so the response to querying for a series **minimum value**, for example, is 
      returned nearly instantly.  
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
  Included data is held by the client's sesion, and is delivered to the 
  user with no additional server calls.  
* **Patching**  
  You can patch time series data to your documents.  
  (visit the [API documentation](../../document-extensions/timeseries/client-api/overview) to learn more).  

{PANEL/}

{PANEL: Time Series Data}

Time series **names** are kept in their parent documents' metadata, while their 
**data** is kept [separately](../../document-extensions/timeseries/overview#separate-name-and-data-storage).  
Time series data is **compressed** and composed of consecutive 
[segments](../../document-extensions/timeseries/overview#time-series-segments) and 
[entries](../../document-extensions/timeseries/overview#time-series-entries).  

{PANEL/}

{PANEL: Separate Name and Data Storage}

The separation of names and data prevents time series value updates from 
invoking document-change events, keeping documents' availability and performance 
whatever size their time series grow to be and however frequent their value-updates 
are.  

{PANEL/}

{PANEL: Time Series Segments}

Time series are composed of consecutive **segments**.  
When a time series is created, its values are held in a single segment.  
As the number of values grow (or when a certain amount of time has passed 
since the last entry appendage), segments are added to the series.  

{INFO: }
Segments are managed automatically by RavenDB, clients do not need to do 
anything in this regard.  
{INFO/}

---

#### Transactions Performance

Time series segmentation heightens performance and minimizes transaction 
and query time, since only the relevant segments of even a very long series 
would be retrieved and queried, and only relevant segments would be updated.  

---

#### Common Queries Performance
Segmentation also helps provide results for common queries extremely 
fast, since results for such queries as `Min`, `Max` and others are 
automatically stored and updated in segment headers, and are always 
available for instant retrieval.  
  
{PANEL/}

{PANEL: Time Series Entries}

Each time series segment is composed of consecutive **time series entries**.  
Each entry is composed of a **timestamp**, 1 to 32 **values**, and an **optional tag**.  
    

---

#### Timestamps

{INFO: }
A single `DateTime` timestamp marks each entry in a millisecond precision.  
{INFO/}

Timestamps are always indicated using [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time).  

Timestamps, and therefore time series entries, are always ordered **by time**, 
from the oldest timestamp to the newest.  
E.g. in a heartrate time series, timestamps would indicate the time in which each 
heartrate measurement has been taken.  

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

#### Tags

{INFO: }
A single **optional** `string` tag can be added per entry.  
{INFO/}

Tags are designated to provide information regarding their entries.  

* **Descriptive Tags**  
  A tag can be a **short descriptive text**.  

* **Reference Tags**  
  A tag can also contain a document's ID, and function as a **reference to this document**.  
  
    A reference-tag is preferable when we want the tag to be very short and yet refer us 
    to an unlimited source of information.    
  
    Reference-tags can be used to [filter time series data](../../document-extensions/timeseries/querying/filtering#using-tags-as-references---) 
    during a query.  
    **E.g.**, the query can -  
    **1.** load time series entries whose tags refer to device-specification documents.  
    **2.** retrieve and examine the specification document referred to by each entry.  
    **3.** project to the client only values measured by Japanese devices.  

    {INFO: }
    Prefer re-using a few tags many times over using many unique tags,  
    to minimize memory and storage usage and optimize time series performance.  
    {INFO/}

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

