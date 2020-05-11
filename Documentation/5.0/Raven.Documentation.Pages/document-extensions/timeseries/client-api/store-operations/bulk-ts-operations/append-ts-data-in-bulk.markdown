# `BulkInsert`: Append Time-Series In Bulk

---

{NOTE: }

Use `BulkInsert` when you want to append a large quantity of 
time-series data to a document.  

* In this page:  
  * [`BulkInsert` Definition](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#bulkinsert-definition)  
  * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-flow)  
  * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/bulk-ts-operations/append-ts-data-in-bulk#usage-samples)  
{NOTE/}

{PANEL: `BulkInsert` Definition}

{PANEL/}


{PANEL: Usage Flow}

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

{PANEL/}

{PANEL: Usage Samples}

Here, we append a time-series two entries.  
{CODE timeseries_region_Use-BulkInsert-To-Append-2-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we use a loop to append a time-series a hundred entries.  
{CODE timeseries_region_Use-BulkInsert-To-Append-100-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
