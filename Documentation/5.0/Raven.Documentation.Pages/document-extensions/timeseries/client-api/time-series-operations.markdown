# Time-Series Operations

---

{NOTE: }

Time-series and their entries can be appended, removed and retrieved via both 
[session methods](../../../document-extensions/timeseries/client-api/api-overview#managing-time-series-using-session-methods) 
and `store operations`.  

* The main advantage `session` methods have over `store` operations is the 
  transactional guarantee `session` methods provide.  
* There are time-series actions that are only available via store operations, 
  including -  
   * Appending, removing and getting the data of **multiple time-series** 
     in a single operation.  
   * Managing time-series **rollup and retention policies**  
   * Performing **queries** and patching time-series data to 
     **multiple chosen documents**.  

{INFO: }
If you need to perform an action that can be accomplished by both 
a session method and a store operation, and neither offers a clear 
advantage over the other, simply choose the one you're more 
comfortable with.  
{INFO/}

* In this page:  
  * [`TimeSeriesBatchOperation`: Append and Remove Time-Series Data](../../../document-extensions/timeseries/client-api/time-series-operations#timeseriesbatchoperation:-append-and-remove-time-series-data)  
     * [Use `TimeSeriesBatchOperation` to Append](../../../document-extensions/timeseries/client-api/time-series-operations#use--to-append)  
     * [Use `TimeSeriesBatchOperation` to Remove](../../../document-extensions/timeseries/client-api/time-series-operations#use--to-remove)  
  * [`BulkInsert`: Append Time-Series In Bulk](../../../document-extensions/timeseries/client-api/time-series-operations#bulkinsert:-append-time-series-in-bulk)  
  * [`GetTimeSeriesOperation`: Get Time-Series Data](../../../document-extensions/timeseries/client-api/time-series-operations#gettimeseriesoperation:-get-time-series-data)  
     * [Get A Single Time-Series' Data](../../../document-extensions/timeseries/client-api/time-series-operations#get-a-single-time-series-data)  
     * [Get Multiple Time-Series Data](../../../document-extensions/timeseries/client-api/time-series-operations#get-multiple-time-series-data)  
  * [`ConfigureTimeSeriesOperation`: Manage Rollup and Retention Policies](../../../document-extensions/timeseries/client-api/time-series-operations#configuretimeseriesoperation:-manage-rollup-and-retention-policies)  
  * [`PatchOperation`: Patch Time-Series Data To Documents](../../../document-extensions/timeseries/client-api/time-series-operations#configuretimeseriesoperation:-manage-rollup-and-retention-policies)  
  * [`PatchByQueryOperation`: Patch Time-Series Data By Query](../../../document-extensions/timeseries/client-api/time-series-operations#patchbyqueryoperation:-patch-time-series-data-by-query)  
{NOTE/}

---

{PANEL: `TimeSeriesBatchOperation`: Append and Remove Time-Series Data}

`TimeSeriesBatchOperation` can append and remove single or multiple time-series 
entries.  
You can bundle a series of Append and/or Remove operations in a list, and 
execute tham all in a single call.  

To instruct `TimeSeriesBatchOperation` which actions to perform, pass it 
an `TimeSeriesOperation` instance for each Append or Remove action.  

* `TimeSeriesOperation`  
  {CODE TimeSeriesOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

### Use `TimeSeriesBatchOperation` To Append

---

#### Usage Flow  

* Create an instance of `TimeSeriesOperation`  
    * Populate it with a new `AppendOperation` list for every time-series entry you want to append.  
    * Populate each `AppendOperation` list with the properties of the time-series 
      entry that you want to append.  
* Create a `TimeSeriesBatchOperation` instance.  
    * Pass its constructor the document ID and the `TimeSeriesOperation` instance you've created.  
* Call `store.Operations.Send` to execute the operation.  
    * Pass it the `TimeSeriesBatchOperation` instance you've created.  

---

#### Usage Sample

Here, we append a time-series two entries using `TimeSeriesBatchOperation`.  
{CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

### Use `TimeSeriesBatchOperation` To Remove

---

#### Usage Flow  

* Create a `TimeSeriesOperation` instance  
   * Populate the `TimeSeriesOperation` instance with -  
      The time-series name.  
      A list of removals.  
   * The list of removals is constructed of `RemoveOperation` instances.  
      Each instance defines a range of entries that will be removed from the time-series.  
   * Set a removal range using -  
     `From` - the timestamp of the first time-series entry of the range  
     `To` - the timestamp of the last time-series entry to be removed.  
* Create a `TimeSeriesBatchOperation` instance.  
    * Pass its constructor the document ID and the `TimeSeriesOperation` instance you've created.  
* Call `store.Operations.Send` to execute the operation.  
    * Pass it the `TimeSeriesBatchOperation` instance you've created.  

---

#### Usage Sample

Here, we remove two ranges of entries from a time-series.  
{CODE timeseries_region_Remove-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `BulkInsert`: Append Time-Series In Bulk}

You can append time-series entries in bulk, using 
[BulkInsert](../../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation).  

---

#### Usage Flow

* Call `store.BulkInsert`.  
  BulkInsert's return value is an `BulkInsertOperation` instance.  
* Call the `BulkInsertOperation` instance's `TimeSeriesFor` method.  
   * Pass it the document ID, and the time-series name  
   * Its return value is a new `TimeSeriesBulkInsert` instance.  
* Populate the `TimeSeriesBulkInsert` instance with Append actions.  
  Pass each Append action -  
   * The timestamp of the entry you want to append  
   * The entry's new values  
   * The entry's tag  

---

#### Usage Samples

Here, we append a time-series two entries.  
{CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we use a loop to append a time-series a hundred entries.  
{CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  


{PANEL/}

{PANEL: `GetTimeSeriesOperation`: Get Time-Series Data}

Get time-series data using `GetTimeSeriesOperaion`.  
`GetTimeSeriesOperaion` has an advantage over `session.Get`, in that it 
allows you to retrieve data from multiple time-series of a selected document 
in a single call.  

There are two overloads of this operation.  

* The first overload retrieves the data of a single time-series.  
* The second overload retrieve the data of as many time-series as you like.  

---

### Get A Single Time-Series' Data

---

#### Usage Flow

* Pass `GetTimeSeriesOperation` -  
   * The document ID  
   * The time-series name  
   * Range start: Timestamp for the first time-series entry to be retrieved  
   * Range end: Timestamp for the last time-series entry to be retrieved  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into a `dictionary of `TimeSeriesRangeResult` classes.  
   * TimeSeriesRangeResult:
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

#### Usage Sample

Here, we retrieve all entries of a single time-series.  
{CODE timeseries_region_Get-Single-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

### Get Multiple Time-Series Data

---

#### Usage Flow

* Pass `GetTimeSeriesOperation` the document ID and a list of `TimeSeriesRange` instances.  
  Each `TimeSeriesRange` instance defines -  
   * **Name** - The time-series name  
   * **From** - Range start, the timestamp for the first time-series entry to be retrieved  
   * **To** - Range end, the timestamp for the last time-series entry to be retrieved  
* Call `store.Operations.Send` to execute the operation.  
* Data is returned into into a `dictionary of `TimeSeriesRangeResult` classes.  
   * TimeSeriesRangeResult:
     {CODE TimeSeriesRangeResult-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

#### Usage Sample

Here, we retrieve chosen entries from two time-series.  
{CODE timeseries_region_Get-Multiple-Time-Series@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `ConfigureTimeSeriesOperation`: Manage Rollup and Retention Policies}

Use `ConfigureTimeSeriesOperation` to manage time-series rollup and retention policies.  

Learn how to use this operation in the article dedicated to 
[rollup and retention](../../../document-extensions/timeseries/rollup-and-retention).  

{PANEL/}

{PANEL: `PatchOperation`: Patch Time-Series Data To Documents}

Use `PatchOperation` to run a Java Script that patches time-series 
entries to a document or removes them from it.  

---

#### Usage Flow  

* Pass `PatchOperation` -  
   * the document ID  
   * the change vector if you need to (or `null` if not)  
   * a new `PatchRequest` instance  
* Use the `PatchRequest` instance to pass `PatchOperation` 
  a Java Script that specifies whether to Append or Remove 
  time-series entries and how to perform it.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples  

Here, we use `PatchOperation` to patch a document a single 
time-series entry.  
The script draws its arguments from its "Values" section.  
{CODE TS_region-Operation_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we provide `PatchOperation`with a script that patches 
100 time-series entries to a document.  
Timestamps and values are drawn from an array, and other 
arguments are provided in the "Values" section.  
{CODE TS_region-Operation_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we use `PatchOperation` to remove a range of 50 time-series 
entries from a document.  
{CODE TS_region-Operation_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `PatchByQueryOperation`: Patch Time-Series Data By Query}

Use `PatchByQueryOperation` to run a query and patch the documents 
it finds. It is useful when you want to patch data time-series data 
to multiple documents.  
You can also use this operation to remove and get time-series data 
from multiple documents.  

---

#### Usage Flow  

* Create a `PatchByQueryOperation` operation.  
* Pass `PatchByQueryOperation` a new `IndexQuery` instance as an argument.  
* Add the `IndexQuery` instance a Java Script that specifies 
   the query you want to run.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples  

Here, the query we provide `PatchByQueryOperation` appends 
a time-series entry to all user documents.  
{CODE TS_region-PatchByQueryOperation-Append-To-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, the query removes time-series from located documents.  
{CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we get selected ranges of time-series entries from the documents 
located by the query.  
{CODE TS_region-PatchByQueryOperation-Get@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
