# Time-Series API Overview
---

{NOTE: }

The Time-Series API includes a set of [session](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
methods and [store](../../../client-api/what-is-a-document-store) 
[operations](../../../client-api/operations/what-are-operations).  
You can use the API to **append** (create and update), **get**, 
**remove**, **include**, **patch** and **query** time-series data.  

* In this page:  
  * [Creating and Removing Time-Series Data](../../../document-extensions/timeseries/client-api/api-overview#creating-and-removing-time-series-data)  
  * [`session` Methods -vs- `document-store` Operations](../../../document-extensions/timeseries/client-api/api-overview#session-methods--vs--document-store-operations)  
  * [Available Time-Series `session` methods](../../../document-extensions/timeseries/client-api/api-overview#available-time-series-session-methods)  
  * [Available Time-Series `store` Operations](../../../document-extensions/timeseries/client-api/api-overview#available-time-series-store-operations)  

{NOTE/}

---

{PANEL: Creating and Removing Time-Series Data}

A time-series is constructed of time-series **entries**, which can 
be created and removed using the API.  
There is no need to explicitly create or delete a time-series.  

* A time-series is created when the first entry is appended to it.  
* A time-series is deleted When all entries are removed from it.  

{PANEL/}

{PANEL: `session` Methods -vs- `document-store` Operations}

Some time-series functions are available through both `session` methods 
and `document-store` operations:  
You can **append**, **remove**, **get** and **patch** time-series data 
through both interfaces.  

---

There are also functionalities unique to each interface.  

* **Time-series functionalities unique to the `session`interface**:  
   * `session` methods provide a **transactional guarantee**.  
     Use them when you want to guarantee that your actions would 
     be either fully completed or fully reverted.  
     You can, for instance, gather multiple session actions 
     (e.g. the update of a time-series and the modification 
     of a document) and execute them in a single transaction 
     by calling `session.SaveChanges`, to ensure that they 
     would all be completed or all be reverted.  
   * You can use `session` methods to `include` time-series while 
     loading documents.  
     Included time-series data is held by the client's session, 
     and can be handed to the user the instant it is required.  
* **Time-series functionalities unique to the `store`interface**:  
   * Getting the data of **multiple time-series** in a single operation.  
   * Managing time-series **rollup and retention policies**.  
   * Patching time-series data to **multiple documents** located by a query.  
{PANEL/}

{PANEL: Available Time-Series `session` methods}

* [TimeSeriesFor.Append](../../../document-extensions/timeseries/client-api/session-methods/append-ts-data)  
  Use this method to **Append entries to a time-series** 
  (creating the series if it didn't previously exist).  
* [TimeSeriesFor.Remove](../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data)  
  Use this method to **Remove a range of entries from a time-series** 
  (removing the series completely if all entries have been removed).  
 * [TimeSeriesFor.Get](../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-entries)  
  Use this method to **Retrieve raw time-series entries** 
  for all entries or for a chosen entries range.  
* [Advanced.GetTimeSeriesFor](../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-names)  
  Use this method to **Retrieve time-series Names**.  
  Series names are fetched by `GetTimeSeriesFor` directly from their parent documents' 
  metadata, requiring no additional server roundtrips.  
* [session.Advanced.Defer](../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data)  
  Use this method to **patch time-series data to a document**.  
* **To include time-series data** -  
   * [Use IncludeTimeSeries while loading a document via session.Load](../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load)  
   * [Use IncludeTimeSeries while retrieving a document via session.Query](../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query)  
   * [Use RQL while running a raw query](../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries)  
{PANEL/}

{PANEL: Available Time-Series `store` Operations}

* [TimeSeriesBatchOperation](../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-TS-data)  
  Use this operation to **append and remove time-series data**.  
  You can bundle a series of Append and/or Remove operations in a list and 
  execute them in a single call.  
* [GetTimeSeriesOperation](../../../document-extensions/timeseries/client-api/store-operations/get-TS-data)  
  Use this operation to **retrieve time-series data**.  
  `GetTimeSeriesOperation` has an advantage over `session.TimeSeries.Get`, in allowing 
  you to retrieve data from multiple time-series of a selected document in a single call.  
* [ConfigureTimeSeriesOperation](../../../document-extensions/timeseries/rollup-and-retention)  
  Use this operation to **manage time-series roll-up and retention policies**.  
* [PatchOperation](../../../document-extensions/timeseries/client-api/store-operations/patch-TS-data/patch-a-document)  
  Use this operation to **patch time-series data to a single document**.  
* [PatchByQueryOperation](../../../document-extensions/timeseries/client-api/store-operations/patch-TS-data/patch-queried-documents)  
  Use this operation to **patch time-series data to documents located 
  by a query**.  
* [BulkInsert.TimeSeriesFor.Append](../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk)  
  Use this operation to **append a large quantity of time-series data**.  

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
