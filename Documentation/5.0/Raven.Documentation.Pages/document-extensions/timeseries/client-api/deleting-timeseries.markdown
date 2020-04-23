# Deleting Time Series  
---

{NOTE: }

* All time series for a document are deleted when the document is deleted.  

* You can also use the [session.TimeSeriesFor.`Delete`]() method to remove a specific time series from a document.  

* In this page:
    - [`Delete ` Syntax]()
    - [`Delete ` Usage]()
    - [Code Sample]()
{NOTE/}

---

{PANEL: `Delete ` Syntax}

**code sample**

| Parameter | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `timeSeriesName` |  string | Time Series Name |
{PANEL/}

{PANEL: `Delete ` Usage}

*  **Flow**:  
  * Open a session  
  * Create an instance of `TimeSeriesFor`.  
      * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  * Execute `TimeSeriesFor.Delete`
  * Execute `session.SaveChanges` for the changes to take effect  

* **Note**:
    * A time series you deleted will be removed only after the execution of `SaveChanges()`.  
    * Deleting a document deletes its time series as well.  
    * `Delete` will **not** generate an error if the time series doesn't exist.  

{PANEL/}

{PANEL: Code Sample}
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
