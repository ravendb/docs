# Remove Time-Series Entries

---

{NOTE: }

You can use the session `TimeSeriesFor.Remove` method to remove 
a **single time-series entry**, or a **range of time-series entries**.  

You can also use the store `TimeSeriesBatchOperation` operation to 
remove time-series entries.  

* A time-series is removed, when all its time-series entries are removed.  
* When a document is deleted, all its time-series are deleted with it.  

* In this page:  
   * [Removing Time-Series Entries Using `session.TimeSeriesFor.Remove`](../../../document-extensions/timeseries/client-api/remove-time-series-entries#removing-time-series-entries-using-session.timeseriesfor.remove)  
   * [Removing Time-Series Entries Using `TimeSeriesBatchOperation`](../../../document-extensions/timeseries/client-api/remove-time-series-entries#removing-time-series-entries-using-timeseriesbatchoperation)  
{NOTE/}

---

{PANEL: Removing Time-Series Entries Using `session.TimeSeriesFor.Remove`}

#### `TimeSeriesFor.Remove` Definition

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

---

#### Usage Flow  

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Remove`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

---

#### Code Samples  

Here, we remove a single time-series entry from a time-series.  
{CODE timeseries_region_Remove-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Here, we remove a range of time-series entries from a time-series.  
{CODE timeseries_region_TimeSeriesFor-Remove-Time-Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Removing Time-Series Entries Using `TimeSeriesBatchOperation`}

`TimeSeriesBatchOperation` can append or remove multiple time-series entries.  
To instruct it which operations to perform, provide it with `TimeSeriesOperation` constructs.  

* `TimeSeriesOperation`  
  {CODE TimeSeriesOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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

#### Code Sample

Here, we remove two ranges of entries from a time-series.  
{CODE timeseries_region_Remove-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
