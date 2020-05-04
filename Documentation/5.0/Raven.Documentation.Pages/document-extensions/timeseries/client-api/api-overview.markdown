# Time-Series API Overview
---

{NOTE: }

The Time-Series API includes a set of [session](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
methods and [store](../../../client-api/what-is-a-document-store) 
[operations](../../../client-api/operations/what-are-operations) 
for the creation, updating, usage and removal of time-series and time-series entries.  
You can also use the API to [include](../../../client-api/session/loading-entities#load-with-includes), 
[patch](../../../client-api/operations/patching/set-based#patching-how-to-perform-set-based-operations-on-documents) 
and [query](../../../client-api/session/querying/how-to-query) 
time-series.  

* In this page:  
  * [Creating and Removing Time-Series](../../../document-extensions/timeseries/client-api/api-overview#creating-and-removing-time-series)  
  * [Managing Time-Series Using `session` methods](../../../document-extensions/timeseries/client-api/api-overview#managing-time-series-using-session-methods)  
  * [Managing Time-Series Using `store` Operations](../../../document-extensions/timeseries/client-api/api-overview#managing-time-series-using-store-operations)  
  * [Success, Failure and Conflicts](../../../document-extensions/timeseries/client-api/api-overview#success,-failure-and-conflicts)  

{NOTE/}

---

{PANEL: Creating and Removing Time-Series}

There is no need to explicitly create or remove a time-series.  
When the first time-series entry is appended to a time-series (with a timestamp, 
a value, and optionally a tag), the time-series is created.  
When the last entry is removed, the series is removed.  

{PANEL/}

{PANEL: Managing Time-Series Using `session` methods}

* **Transactions**  
  Manage time-series using the `session` interface when you want to guarantee 
  that your actions would be executed in a transactional context.  
  `session` actions either fully succeed or are fully reverted.  
  The session also gathers multiple actions (being, for example, 
  appending a time-series value and modifying a document, or the 
  modifications of several different time-series) and attempts to 
  execute them in a single transaction, either completing or reverting 
  them all.  
* **Caching**  
  Another advantage of the `session` interface is the caching of 
  retrieved data in the client's local cache. Having retrieved a time-series 
  once, the client wouldn't need to load it (or any part of it) from 
  the server again as long as it hasn't been updated by other clients.  

---

#### `session.TimeSeriesFor`: Append, Get and Remove 

You can manage time-series using the session `TimeSeriesFor` object.  

* **`TimeSeriesFor`Methods**:  
  * `TimeSeriesFor.Append`  
     `Append` is used to create and update time-series and time-series entries.  
     When an existing time-series entry is appended, it is updated with the new data. 
     When a non-existing time-series entry is appended, it is created.  
     An attempt to append a time-series entry to a non-existing time-series, 
     creates the time-series and appends it this entry.  
  * `TimeSeriesFor.Get`  
    `Get` retrieves a value or a range of values from a time-series at the 
    specified timestamps.  
  * `CountersFor.Remove`  
    `Remove` deletes a time-series entry from a time-series.  
    When a time-series' last entry is removed, the time-series is removed as well.  

*  **Usage Flow**:  
  * Open a session.  
  * Create an instance of `TimeSeriesFor`.  
      * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  * Use a time-series method.  
  * If you execute `Append`or `Remove, call `session.SaveChanges` 
    for the action to take effect on the server.  

* **Usage Samples**  
  * You can Use `TimeSeriesFor` by **explicitly passing it a document ID** 
    (without pre-loading the document).  
    Here, TimeSeriesFor relates to a document by its explicit ID.  
      {CODE timeseries_region_TimeSeriesFor_without_document_load@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  
  * You can also use `TimeSeriesFor` by passing it **the document object**.  
    Here, TimeSeriesFor relates to a document object that's been loaded earlier.  
    This can be useful when you want, for example, to manage time-series 
    for document objects returned by queries.  
      {CODE timeseries_region_TimeSeriesFor_with_document_load@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

#### `session.Advanced.GetTimeSeriesFor`: Retrieve TimeSeries Names

You can use session.Advanced.GetTimeSeriesFor to retrieve a document's 
time-series' names.  

{CODE timeseries_region_Retrieve-TimeSeries-Names@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: Managing Time-Series Using `store` Operations}

There are time-series actions that can be performed by both store operations 
and session methods. Each interface offers its advantages, e.g. the transactional 
guarantee provided by the session interface vs low-level commands' lack of it 
(in case you prefer not to bundle separate operations into a single transaction, 
for example).  

An action that is currently available only through store operations, 
is the loading of a document's multiple time-series by `GetTimeSeriesOperation`.  


---

#### `GetTimeSeriesOperation`: Get Time-Series Data  

You can use this method to retrieve data from several diffrent time-series 
of a document.  

* Pass `GetTimeSeriesOperation` a `TimeSeriesRange` construct to provide 
  it with the **start** and **end** time and dates and with the name of 
  the time-series you want to update.  

* Retrieved results are put in a class holding a dictionary of 
  `TimeSeriesRangeResult` classes.  

---

#### `TimeSeriesBatchOperation`: Append or Remove data from a time-series  
    
* Pass `TimeSeriesBatchOperation` a `TimeSeriesOperation` construct to 
  instruct it what to do.  

{PANEL/}

{PANEL: Success, Failure and Conflicts}

---

####Success
As long as the document exists, updating a Time Serie will always succeed.  

---

####Transactions
When a transaction that includes a time-series modification fails for any 
reason, the Counter modification is reverted.  

---

####No Conflicts

Time-series actions do not cause conflicts.  

* **Updates By Multiple Cluster Nodes**  
   * When a time-series' data is replicated by multiple nodes, the data 
     from all nodes is merged into a single series.  
   * When multiple nodes append **different values** at the same timestamp, 
     the **bigger value** is chosen for this entry.  
   * When multiple nodes apppend **different amount of values** for the same 
     timestamp, the **bigger amount of values** is chosen for this entry.  
   * When an existing value at a certain timestamp is deleted by a node 
     and updated by another node, the deletion is chosen.  

* **Updates By Multiple Clients Of The Same Node**  
   * When a time-series' value at a certain timestamp is appended by 
     multiple clients more or less simultaneously, the last one is chosen.  
   * When an existing value at a certain timestamp is deleted by a client 
     and updated by another client, the last action is chosen.  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time-Series Management]()  

**Client-API - Session Articles**:  
[Time-Series Overview]()  
[Creating and Modifying Time-Series]()  
[Deleting Time-Series]()  
[Retrieving Time-Series Values]()  
[Time-Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time-Series Operations]()  
