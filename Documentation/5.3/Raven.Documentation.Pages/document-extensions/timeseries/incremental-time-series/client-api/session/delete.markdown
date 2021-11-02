# Session: Delete Incremental Time Series

---

{NOTE: }

Delete a range of incremental time series entries using `IncrementalTimeSeriesFor.Delete`.  

* You can delete a **single entry** or a **range of entries**.  

* In this page:  
      * [`IncrementalTimeSeriesFor.Delete`](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/delete#incrementaltimeseriesfor.delete)  
       * [Syntax](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/delete#syntax)  
       * [Usage Flow](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/delete#usage-flow)  
       * [Code Samples](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/delete#code-samples)  
{NOTE/}

---

{PANEL: `IncrementalTimeSeriesFor.Delete`}

`IncrementalTimeSeriesFor.Delete` is used for the removal of incremental time series and 
their entries.  

* There is no need to explicitly delete an incremental time series; 
  the series is deleted when all its entries are deleted.  
* Attempting to delete nonexistent entries results in a no-op, 
  generating no exception.  


{PANEL/}

{PANEL: Syntax}

* There are two `IncrementalTimeSeriesFor.Delete` methods:  
   * Delete a range of time series entries  
     -or-  
     if values are omitted, delete the entire series.  
     {CODE incremental_delete-values-range-or-all-values@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Delete a single time series entry.  
     {CODE incremental_delete-value-at-timestamp@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `from` (optional) | `DateTime?` | Delete the range of entries starting at this timestamp. |
    | `to` (optional) | `DateTime?` | Delete the range of entries ending at this timestamp. |
    | `at` | `DateTime` | Timestamp of the entry to be deleted. |

* **Return Value**  
  No return value.  

* **Exceptions**  
   * `DocumentDoesNotExistException` is thrown If the document doesn't exist.  
   * Attempting to delete nonexistent entries results in a no-op and does not generate an exception.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `IncrementalTimeSeriesFor`.  
    * Either pass `IncrementalTimeSeriesFor` an explicit document ID,  
      -or-  
      Pass it an [entity tracked by the session](../../../../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../../../../client-api/session/querying/how-to-query) 
      or from [session.Load](../../../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time series name.  
* Call `IncrementalTimeSeriesFor.Delete`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Code Samples}

* Delete a single entry:  
   {CODE incremental_delete-single-entry@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* Delete a range of entries:  
   {CODE incremental_delete-entries-range@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
