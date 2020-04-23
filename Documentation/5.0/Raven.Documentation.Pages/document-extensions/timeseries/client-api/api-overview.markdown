# Time Series API Overview
---

{NOTE: }

* ...  

* In this page:  
  * [Managing Time Series]()  
  * []()  
{NOTE/}

---

{PANEL: Managing Time Series}

Create, modify and delete time series using the `TimeSeriesFor` Session object.  
You can Use `TimeSeriesFor` by **explicitly passing it a document ID** (without pre-loading the document).  
You can also use `TimeSeriesFor` by passing it **the document object**.  

**code sample**

* **Time Series methods**:  
  - `TimeSeriesFor.Append`: Add time points to those of an existing series, or create a new series if it doesn't exist.  
  - `TimeSeriesFor.Delete`: Delete a time series.  
  - `TimeSeriesFor.Remove`: Remove specific time points from a series.  
  - `TimeSeriesFor.Get`: Retrieve timepoints.  

*  **Usage Flow**:  
  * Open a session.  
  * Create an instance of `TimeSeriesFor`.  
      * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  * Use time series methods to manage the document's time series.  
  * If you execute [Append]() or [Delete](), call `session.SaveChanges` for the action to take effect on the server.  

* `Append` timepoints
  A time series can be created with a single time point, and 
  populated over time by appending it additional time points.  
  Time points that haven't been populated yet are marked nAn (not a number).  

* `Get` timepoints  
  [session.TimeSeriesFor.Get]()  

* `Remove` timepoints  
  [session.TimeSeriesFor.Remove]()  
   * Removing a single time point  
   * Removing a range of time points  

* `Delete` a time series  
  [session.TimeSeriesFor.Delete]()  

---

#### Success and Failure

* As long as the document exists, time series actions (Append, Get, Delete etc.) always succeed.
* When a transaction that includes a time series modification fails for any reason (e.g. xxx), 
  the time series modification is reverted.  

---

####Managing Time Series Using `Operations`

* In addition to working with the high-level Session, you can manage Time Series using the low-level [Operations]().  

* [TimeSeriesBatchOperation]() [does this exist?]

  can operate on a set of time series of different documents in a single request.

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
