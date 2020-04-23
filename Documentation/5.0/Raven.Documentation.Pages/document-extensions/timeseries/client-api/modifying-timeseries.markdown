# Modifying Time Series
---

{NOTE: }

* Use the [TimeSeriesFor]().`Append` method to **create** a new series or **add values** to an existing series.  
* Use the [TimeSeriesFor]().`Remove` method to **remove** timeponits from a series.  

* In this page:  
   - Adding Time Points To a Series 
   - Removing Time Points From a Series  
{NOTE/}

---

{PANEL: Create a Series and Add Time Points}

* `Append` Syntax  
   - Appending timepoints using session.TimeSeriesFor.Append.  
     syntax (table)  
     code sample: a **single** time point  
     code sample: a **range** of time points  
     

   - Bulk operations using the **store** interface:  
     syntax (table)  
     code sample  
     {CODE-BLOCK:JSON}
     using(var bulk = store.BulkInsert())
     using(var ts = bulk.TimeSeriesFor(string documentId, string name))
     {
         ts.Append( ... )
     }
     {CODE-BLOCK/}

* `Append` Flow  
    - Open a session  
    - Create an instance of `TimeSeriesFor`.  
        * Either pass `TimeSeriesFor` an explicit document ID, -or-  
          Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
    - Execute `TimeSeriesFor.Append`
    - Execute `session.SaveChanges` for the changes to take effect  

**Note**:
Modifying a time series using `Append` only takes effect when `session.SaveChanges()` is executed.  
{PANEL/}

* **Code Sample**  

{PANEL: Remove Time Points From a Series}
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
