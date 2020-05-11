# Include Time-Series Data 
---

{NOTE: }

[Include](../../../client-api/session/loading-entities#load-with-includes) 
a document's time-series data while loading the document.  
The included time-series data is held by the client's session, 
so it can be handed to the user with no additional trip to the 
server the instant it is required.  

* Time-series data can be included while -  
   * Loading a document using `session.Load`  
   * Loading a document **by a query** via `session.Query`  
   * Loading a document **by a raw query** via `session.Advanced.RawQuery`  

* In this page:  
  * [Include Time-Series Data While Loading Via `Session.Load`](../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data#include-time-series-data-while-loading-via-session.load)  
  * [Include Time-Series Data During a Query](../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data#include-time-seriess-data-during-a-query)  
     * [Via `Session.Query`](../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data#include-ts-data-while-querying-via-)  
     * [Via `session.Advanced.RawQuery`](../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data#include-ts-data-while-querying-via--1)  

{NOTE/}

---

{PANEL: Include Time-Series Data While Loading Via `Session.Load`}

To include a time-series or a part of it while loading a document 
via `session.Load`, use `IncludeTimeSeries`.  

---

#### Usage Flow  

* Open a session  
* Call `session.Load`  
  Pass it two arguments:  
   * Document ID  
   * `IncludeTimeSeries` and its argumrnts: the time-series name, range start, and range end.  

---

#### `IncludeTimeSeries` Definition

{CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Parameters:  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `name` | string | Time-series Name |
     | `from` | DateTime | Time-series range start |
     | `to` | DateTime | Time-series range end |

---

#### Usage Samples  

Here, we **load** a document using `session.Load` and **include** 
a selected range of entries from a time-series named "Heartrate".  
The included entries are then held by the session.  
Then, we **Get** the same entries we've previously included. 
Getting other entries (e.g. minutes 300 to 320 of the time-series) 
would still work, but the values would be retrieved not from 
the session but from the server, with a much costlier delay.  
{CODE timeseries_region_Load-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Include Time-SeriesS Data During a Query}

---

## Include TS Data While Querying Via `Session.Query`  

To include time-series or parts of them while loading documents 
via `session.Query`, use `IncludeTimeSeries`.  

---

#### Usage Flow  

* Open a session  
* Call `session.Query`  
  Pass it `IncludeTimeSeries` as an include-builder argument  
* Pass `IncludeTimeSeries` its arguments: the time-series name, range start, and range end.  

---

#### `IncludeTimeSeries` Definition

{CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Parameters:  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `name` | string | Time-series Name |
     | `from` | DateTime | Time-series range start |
     | `to` | DateTime | Time-series range end |

---

#### Usage Samples  

Here, we **load** a document using `session.Query` and **include** 
a whole time-series named "Heartrate".  
When we **Get** the time-series data, its values are retrieved 
from the session, with no additional trip to the server.  
{CODE timeseries_region_Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

## Include TS Data While Querying Via `session.Advanced.RawQuery`  


To include time-series or parts of them while querying via `session.Advanced.RawQuery`, 
simply embed an `include timeseries` statement in your RQL.  

---

#### Usage Flow  

* Open a session  
* Call `session.Advanced.RawQuery`  
  Use `include timeseries` in your query, with its arguments: the time-series name, range start, and range end.  
* Pass `IncludeTimeSeries` its arguments: the time-series name, range start, and range end.  

---

#### Usage Sample

Here, we include the whole "Heartrate" time-series while running a raw query to 
load the document that owns it.  

{CODE timeseries_region_Raw-Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
