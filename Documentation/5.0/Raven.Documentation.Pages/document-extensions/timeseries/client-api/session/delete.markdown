# Session: Delete Time Series

---

{NOTE: }

Delete a range of time series entries using `TimeSeriesFor.Delete`.  

* You can delete a **single time series entry** or a **range of entries**.  

* In this page:  
      * [`TimeSeriesFor.Delete`](../../../../document-extensions/timeseries/client-api/session/delete#timeseriesfor.Delete)  
       * [Syntax](../../../../document-extensions/timeseries/client-api/session/delete#syntx)  
       * [Usage Flow](../../../../document-extensions/timeseries/client-api/session/delete#usage-flow)  
       * [Usage Samples](../../../../document-extensions/timeseries/client-api/session/delete#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesFor.Delete`}

`TimeSeriesFor.Delete` is used for the removal of time series and 
time series entries.  

* There is no need to explicitly delete a time series; 
  the series is deleted when all its entries are deleted.  
* Attempting to delete nonexistent entries results in a no-op, 
  generating no exception.  


{PANEL/}

{PANEL: Syntx}

* There are two `TimeSeriesFor.Delete` methods:  
   * Delete a single time series entry.
     {CODE TimeSeriesFor-Delete-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * Delete a range of time series entries.
     {CODE TimeSeriesFor-Delete-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `at` | `DateTime` | Timestamp of the time series entry you want to delete. |
    | `from` (optional) | `DateTime?` | Delete the range of time series entries starting at this timestamp. <br> Default: `DateTime.Min` |
    | `to` (optional) | `DateTime?` | Delete the range of time series entries ending at this timestamp. <br> Default: `DateTime.Max` |


| from | `DateTime` | Range Start (optional) <br>
| to | `DateTime` | Range End (optional) <br> Default: `DateTime.Max` 



* **Return Value**  
  No return value.  

* **Exceptions**  
   * `DocumentDoesNotExistException` is thrown If the document doesn't exist.  
   * Attempting to delete nonexistent entries results in a no-op and does not generate an exception.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID,  
      -or-  
      Pass it an [entity tracked by the session](../../../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../../../client-api/session/querying/how-to-query) 
      or from [session.Load](../../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time series name.  
* Call `TimeSeriesFor.Delete`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we delete a single entry from a time series.  
   {CODE timeseries_region_Delete-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample we delete a range of entries from a time series.  
   {CODE timeseries_region_TimeSeriesFor-Delete-Time-Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
