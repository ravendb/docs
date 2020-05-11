# Remove Time-Series Data

---

{NOTE: }

Remove time-series data using the session method `TimeSeriesFor.Remove`.  
You can remove a **single time-series entry** or a **range of entries**.  

* **Removing a time-series**  
  Removing all entries of a time-series, removes the whole series.  

* In this page:  
   * [`TimeSeriesFor.Remove` Definition](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#timeseriesfor.remove-definition)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesFor.Remove` Definition}

To remove a time-series entry, use one of `TimeSeriesFor`'s two `Remove` methods.  
One overload method removes a single time-series entry (by its timestamp), 
and the second method removes a range of time-series entries (from a start 
timestamp to an end timestamp).  

* **First Overload**: Remove a single time-series entry.  
     {CODE TimeSeriesFor-Remove-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `at` | DateTime | Timestamp of the time-series entry you want to remove |

* **Second Overload**: Remove a range of time-series entries.  
     {CODE TimeSeriesFor-Remove-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `from` | DateTime | Remove the range of time-series entries starting at this timestamp |
     | `to` | DateTime | Remove the range of time-series entries ending at this timestamp |

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Remove`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

Here, we remove a single time-series entry from a time-series.  
{CODE timeseries_region_Remove-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we remove a range of time-series entries from a time-series.  
{CODE timeseries_region_TimeSeriesFor-Remove-Time-Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
