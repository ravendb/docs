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
   * The retrieval of multiple time-series' data in a single server call  
   * Managing time-series rollup and retention policies  
   * Appending time-series in bulk  

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
* Use `store.Operations.Send` to execute the operation.  
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
* Use `store.Operations.Send` to execute the operation.  
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
