# Time-Series Overview
---

{NOTE: }

* A huge number of systems, including an expanding variety of IoT devices, 
  produce continuous streams of values that can be collected and used for 
  various needs. **Time-series** are vectors of data points that are designated 
  to collect values over time, store them consecutively, and manage them 
  with high efficiency and performance.  

* RavenDB Time-series can be managed and utilized using a thorough set of 
  [API methods](../../document-extensions/timeseries/client-api/api-overview), 
  the [Studio](../../studio/database/document-extensions/time-series), 
  and [various RavenDB features](../../document-extensions/timeseries/timeseries-and-other-features/time-series-and-other-features-overview#time-series-and-other-features---overview).  

* Time-series functionality is fully integrated into RavenDB's 
  distributed environment and document model.  

* Time-series data is **compressed** to lower storage usage and transaction time.  

* In this page:  
  * [Overview](../../document-extensions/timeseries/overview#overview)  
     * [RavenDB's Time-Series Implementation](../../document-extensions/timeseries/overview#ravendbs-time-series-implementation)  
     * [Distrubuted Time-Series](../../document-extensions/timeseries/overview#distrubuted-time-series)  
     * [Time-Series as Document Extensions](../../document-extensions/timeseries/overview#time-series-as-document-extensions)  
     * [Time-Series Features](../../document-extensions/timeseries/overview#time-series-features)  
  * [Time-Series Data](../../document-extensions/timeseries/overview#time-series-data)  
  * [Separate Name and Data Storage](../../document-extensions/timeseries/overview#separate-name-and-data-storage)  
  * [Time-Series Segments](../../document-extensions/timeseries/overview#time-series-segments)  
      * [Transactions Performance](../../document-extensions/timeseries/overview#transactions-performance)  
      * [Common-Queries Performance](../../document-extensions/timeseries/overview#common-queries-performance)  
  * [Time-Series Entries](../../document-extensions/timeseries/overview#time-series-entries)  
      * [The Timestamp](../../document-extensions/timeseries/overview#the-timestamp)  
      * [The Values](../../document-extensions/timeseries/overview#the-values)  
      * [The Tag](../../document-extensions/timeseries/overview#the-tag)  

{NOTE/}

---

{PANEL: Overview}

Time-series can be **aggregated** and **queried** to illustrate process 
behavior, predict future developments, track noticeable value changes, 
and create other helpful statistics.  

Here are a few examples for value streams that can be easily and effectively 
handled by time-series.  

* _A sequence of heartrate values can be collected from a smart 
  wrist-watch_, and be used to build a person's training program.  
* _Weather-stations' measurements_ collected over a chosen time period 
  can be compared to equivalent past periods to predict the weather.  
* _Bandwidth usage reports of a home cable modem monitor_ can be used 
  to build a better charging plan.  
* _Coordinates sent by delivery trucks' GPS trackers_ can be collected 
  and analyzed to secure the vehicles and improve the service.  
* _Daily changes in stock prices_ can be used to build investment plans.  

## RavenDB's Time-Series Implementation  

Time-series functionality is fully integrated in RavenDB's 
distributed environment and document model.  

---

#### Distrubuted Time-Series

Distributed clients and nodes can modify time-series concurrently; the 
modifications are merged by the cluster with [no conflict](../../document-extensions/timeseries/design#no-conflicts).  

---

#### Time-Series as Document Extensions

RavenDB’s Time-Series, like its distributed counters, attachments and document 
revisions, are **document extensions**.  

   {INFO: }
    A time-series always extends a single specific document.  
    The context and source of the time-series can be kept clear this way, 
    and time-series management can use the comfort and strength of the 
    document interface.  
    A barometer's specifications document, for example, can be the parent 
    document for a time-series that is populated with measurements taken 
    by a barometer of this specification.  
   {INFO/}

---

#### Time-series Features

Notable time-series features include -  

* **Highly-Efficient Storage Management**  
  Time-series data is [compressed and segmented](../../document-extensions/timeseries/overview#efficient-storage-and-querying) 
  to minimize storage usage and transmission time.  
* **A Thorough Set of API Methods**  
  The [time-series API](../../document-extensions/timeseries/client-api/api-overview)**  
  includes a variety of `session methods` and `store operations`.  
* **Full GUI Support**  
  Time-series can be viewed and managed using the [Studio](../../studio/database/document-extensions/time-series).  
* **Time-Series Querying and Aggregation**  
    * [High-performance common queries](../../document-extensions/timeseries/overview#efficient-storage-and-querying)  
      The results of a set of common queries are prepared in advance in time-series segments' 
      headers, so the response to querying for a series **minimum value**, for example, is 
      returned nearly instantly.  
    * [LINQ and raw RQL queries](../../document-extensions/timeseries/querying/queries-overview-and-syntax)  
      Flexible queries and aggregations can be executed using LINQ expressions and raw RQL 
      over time-series **timestamps**, **tags** and **values**.  
* **Time-Series Indexing**  
  Time-series can be [indexed by clients](../../ocument-extensions/timeseries/indexing) or using the Studio.  
* [Rollup and Retention Policies](../../document-extensions/timeseries/rollup-and-retention)  
   * **Rollup Policies**  
     You can set time-series rollup policies to aggregate large series into 
     smaller sets by your definitions.  
   * **Retention Policies**  
     You can set time-series retention policies to automatically remove 
     time-series entries that have reached their expiration date/time.  
* [Including Time-Series](../../document-extensions/timeseries/client-api/session-methods/include-ts-data/include-ts-overview)  
  You can include (pre-fetch) time-series data while loading documents.  
  Included data is held by the client's sesion, and is instantly delivered 
  to the user when it is requested.  
* **Patching**  
  You can patch time-series data to your documents.  
  (visit the [API documentation](../../document-extensions/timeseries/client-api/api-overview) to learn more).  

{PANEL/}

{PANEL: Time-Series Data}

Time-series **names** are kept in their parent documents' metadata, while their 
**data** is kept [separately](../../).  
Time-series data is **compressed** and composed of consecutive 
[segments](../../) and [entries](../../).  

{PANEL/}

{PANEL: Separate Name and Data Storage}

A time-series’ **name** is kept at its parent-document's metadata, while 
its **data** is kept separately.  

The separation of names and data prevents time-series value updates from 
invoking document-change events, keeping documents' availability and performance 
whatever size their time-series grow to be and however frequent their value-updates 
are.  

{PANEL/}

{PANEL: Time-Series Segments}

Time-series are composed of consecutive **segments**.  
When a time-series is created, its values are held in a single segment.  
As the number of values grow (or when a certain amount of time has passed 
since the last entry appendage), segments are added to the series.  

{INFO: }
Segments are managed automatically by RavenDB, clients do not need to do 
anything in this regard.  
{INFO/}

---

#### Transactions Performance

Time-series segmentation heightens performance and minimizes transaction 
and query time, since only the relevant segments of even a very long series 
would be retrieved and queried, and only relevant segments would be updated.  

---

#### Common Queries Performance
Segmentation also helps provide results for common queries extremely 
fast, since results for such queries as `Min`, `Max`, `Average` and 
others are automatically stored and updated in segment headers, and 
are always available for instant retrieval.  
  
{PANEL/}

{PANEL: Time-Series Entries}

Each time-series segment is composed of consecutive **time-series entries**.  
Each entry is composed of a **timestamp**, 1 to 32 **values**, and an **optional tag**.  
    

---

#### The Timestamp

{INFO: }
A single `DateTime` timestamp marks each entry in a millisecond precision.  

E.g. in a heartrate time-series, timestamps would indicate the time in which each 
heartrate measurement has been taken.  
{INFO/}

Timestamps, and therefore time-series entries, are always ordered **by time**, 
from the oldest timestamp to the newest.  
Timestamps are the reference points for the appendage and removal of values.  

---

#### The Values

{INFO: }
Up to 32 `double` **values** can be appended per-entry.  
{INFO/}

We allow storing as many as 32 values per entry, since appending multiple 
values may be a requirement for some time-series. Here are a few examples.  

* A heart-rate time-series  
  An entry with a single value (the heart-rate measurement taken by 
  a smart wrist-watch) is added to the time-series every minute.  

* A truck-route time-series  
  An entry with 2 values (the latitude and longitude reported by 
  a GPS device) is added to the time-series every minute.  

* A stock-price time-series  
  An entry with 5 values (stock price when the trade starts and 
  ends, its highest and lowest prices during the day, and its daily 
  trade volume) is added to the time-series every day.  

---

#### The Tag

{INFO: }
A single **optional** `string` tag can be added per entry.  
{INFO/}

Tags are designated to provide information regarding their entries.  
A tag can be a **short descriptive text**.  
A tag can also be a **reference to a document** by the document's ID.  

Using tags as references to documents, **is the preferabble way to use them**. 
Here is why.  

* The short (ID-long) reference, e.g. "watches/fitbit", refers us 
  to a text that is as long and as structured as we'd like it to be.  
* While time-series entries are queried, the query can load the 
  documents their tags refer to and filter the results by the 
  documents' contents.  
  E.g., the query can -  
  **1.** load time-series entries whose tags refer to device-specification documents.  
  **2.** retrieve and examine the specification document referred to by each entry.  
  **3.** project to the client only values measured by Japanese devices.  

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
