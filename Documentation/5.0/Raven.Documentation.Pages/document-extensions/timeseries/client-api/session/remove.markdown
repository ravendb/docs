# Session: Remove Time Series Data

---

{NOTE: }

Remove time series data using `TimeSeriesFor.Remove`.  

* You can remove a **single time series entry** or a **range of entries**.  

* In this page:  
      * [`TimeSeriesFor.Remove`](../../../../document-extensions/timeseries/client-api/session/remove#timeseriesfor.remove)  
       * [Syntax](../../../../document-extensions/timeseries/client-api/session/remove#syntx)  
       * [Usage Flow](../../../../document-extensions/timeseries/client-api/session/remove#usage-flow)  
       * [Usage Samples](../../../../document-extensions/timeseries/client-api/session/remove#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesFor.Remove`}

`TimeSeriesFor.Remove` is used for the removal of time series and 
time series entries.  

* There is no need to explicitly remove a time series; 
  the series is removed when all its entries are removed.  
* Attempting to remove nonexistent entries results in a no-op, 
  generating no exception.  


{PANEL/}

{PANEL: Syntx}

* There are two `TimeSeriesFor.Remove` methods:  
   * Remove a single time series entry.
     {CODE TimeSeriesFor-Remove-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * Remove a range of time series entries.
     {CODE TimeSeriesFor-Remove-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `at` | `DateTime` | Timestamp of the time series entry you want to remove |
    | `from` | `DateTime` | Remove the range of time series entries starting at this timestamp |
    | `to` | `DateTime` | Remove the range of time series entries ending at this timestamp |

* **Return Value**  
  No return value.  

* **Exceptions**  
   * `DocumentDoesNotExistException` is thrown If the document doesn't exist.  
   * Attempting to remove nonexistent entries results in a no-op and does not generate an exception.  

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
* Call `TimeSeriesFor.Remove`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we remove a single entry from a time series.  
   {CODE timeseries_region_Remove-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample we remove a range of entries from a time series.  
   {CODE timeseries_region_TimeSeriesFor-Remove-Time-Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
