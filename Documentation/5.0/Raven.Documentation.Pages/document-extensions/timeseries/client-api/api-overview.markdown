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
  * [TimeSeriesFor.Append](../../../document-extensions/timeseries/client-api/append-time-series-entries)  
     Use this method to create and update time-series and time-series entries.  
     When an existing time-series entry is appended, it is updated with the new data. 
     When a non-existing time-series entry is appended, it is created.  
     An attempt to append a time-series entry to a non-existing time-series, 
     creates the time-series and appends it this entry.  
  * [CountersFor.Remove](../../../document-extensions/timeseries/client-api/remove-time-series-entries)  
    Use this method to remove a time-series entry from a time-series.  
    When a time-series' last entry is removed, the time-series is removed as well.  
  * [TimeSeriesFor.Get](../../../document-extensions/timeseries/client-api/get-time-series)  
    Use this method to retrieve time-series data.  
    You can specify the time-series entry, or range of entries, whose values 
    you want to get.  

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

Time-series and their entries can be appended, removed and retrieved via both 
[session methods](../../../document-extensions/timeseries/client-api/api-overview#managing-time-series-using-session-methods)`
and [store operations](../../../document-extensions/timeseries/client-api/time-series-operations).  

* The main advantage session methods have over store operations is the 
  transactional guarantee they provide.  
* There are actions that are only available via store operations.  

If you need to perform an action for which you have both a session method and 
a store operation and neither has a clear advantage over the other, simply 
choose the one you're more comfortable with.  

---

#### Commonly Used Time-Series Operations

* [`TimeSeriesBatchOperation`](../../../document-extensions/timeseries/client-api/time-series-operations#timeseriesbatchoperation:-append-and-remove-time-series-data)  
  Use this operation to append and remove time-series and time-series entries.  
  `TimeSeriesBatchOperation` has an advantage over `session.Append` and 
  `session.Remove`, in allowing you to bundle a series of Append and/or 
  Remove operations in a list and execute tham in a single call.  
* [`BulkInsert`](../../../document-extensions/timeseries/client-api/time-series-operations#bulkinsert:-append-time-series-in-bulk)  
  Use this operation to append time-series entries in bulk.  
* [`GetTimeSeriesOperation`](../../../document-extensions/timeseries/client-api/time-series-operations#gettimeseriesoperation:-get-time-series-data)  
  Use this operation to retrieve time-series data.  
  `GetTimeSeriesOperation` has an advantage over `session.Get`, in allowing 
  you to retrieve data from multiple time-series of a selected document in 
  a single call.  
* [`ConfigureTimeSeriesOperation`](../../../document-extensions/timeseries/client-api/time-series-operations#configuretimeseriesoperation:-manage-rollup-and-retention-policies)  
  Use this operation to manage time-series roll-up and retention policies.  
* [`PatchOperation`](../../../document-extensions/timeseries/client-api/time-series-operations#configuretimeseriesoperation:-manage-rollup-and-retention-policies)  
  Use this operation to run a Java Script that patches time-series entries 
  to a document or removes them from it.  
* [`PatchByQueryOperation`](../../../document-extensions/timeseries/client-api/time-series-operations#patchbyqueryoperation:-patch-time-series-data-by-query)  
  Use this operation to run a query and patch time-series entries to found 
  documents.  

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
