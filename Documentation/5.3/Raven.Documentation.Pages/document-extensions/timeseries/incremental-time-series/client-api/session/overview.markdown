# Session: Overview

---


{NOTE: }

* Incremental Time Series can be created and managed using a set of [session](../../../../../client-api/session/what-is-a-session-and-how-does-it-work) 
  API methods, whose functionality is mostly identical to that of 
  [non-incremental time series session methods](../../../../../document-extensions/timeseries/client-api/overview#available-time-series-session-methods).  

* A significant difference between the client APIs of non-incremental and incremental time series 
  is the lack of an [Append](../../../../../document-extensions/timeseries/client-api/session/append) 
  method for the latter, since values are not appended but incremented.  
  An [Increment](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment) 
  method is available instead, for the creation and modification if incremental time series.  

* In this page:  
  * [Session API Methods](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview#session-api-methods)  
     * [IncrementalTimeSeriesFor Methods](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview#methods)  
     * [Additional Session Methods](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/overview#additional-session-methods)  
{NOTE/}

---

{PANEL: Session API Methods}

---

### `IncrementalTimeSeriesFor` Methods

The `IncrementalTimeSeriesFor` class gathers useful incremental time series 
session API methods, including [Increment](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment), 
[Get](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/get), 
and [Delete](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/delete).  

To use it -  

* Open a session  
* Create an instance of `IncrementalTimeSeriesFor`  
    * Either pass `IncrementalTimeSeriesFor` an explicit document ID,  
      -or-  
      Pass it an [entity tracked by the session](../../../../../client-api/session/loading-entities), e.g. a document object returned from [session.Query](../../../../../client-api/session/querying/how-to-query) or from [session.Load](../../../../../client-api/session/loading-entities#load).  
    * Pass `IncrementalTimeSeriesFor` the time series name.  
      The name **must** begin with "inc:" to identify the time series as incremental.  
* Call a `IncrementalTimeSeriesFor` method  
* Call `session.SaveChanges` for the action to take place.  


---

### Additional Session Methods

Additional session API methods handle incremental time series the 
same way they do non-incremental time series, allowing you to -  

* [Include](../../../../../document-extensions/timeseries/client-api/session/include/overview) incremental time series,  
* [Patch](../../../../../document-extensions/timeseries/client-api/session/patch) incremental time series,  
* and [Query](../../../../../document-extensions/timeseries/client-api/session/querying) incremental time series.  


{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
