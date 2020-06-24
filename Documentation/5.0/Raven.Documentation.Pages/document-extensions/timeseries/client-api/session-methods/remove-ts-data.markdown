## `TimeSeriesFor.Remove`
# Remove Time Series Data

---

{NOTE: }

Remove time series data using `TimeSeriesFor.Remove`.  

* You can remove a **single time series entry** or a **range of entries**.  

* In this page:  
      * [`TimeSeriesFor.Remove`](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#timeseriesfor.remove)  
      * [Syntax](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#syntx)  
         * [Overload 1 - Remove a Single Time Series Entry.](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-1)  
         * [Overload 2 - Remove a Range Of Time Series Entries](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-2)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesFor.Remove`}

`TimeSeriesFor.Remove` is used for the removal of time series and 
time series entries.  

* There is no need to explicitly remove a time series; 
  the series is removed when all its entries are removed.  
* Attempting to remove nonexistent entries results in a noop 
  and generates no exception.  


{PANEL/}

{PANEL: Syntx}

There are two `TimeSeriesFor.Remove` methods:  
[Overload 1](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-1) 
- Remove a single time series entry.  
[Overload 2](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-2) 
- Remove a range of time series entries.  

---

#### Overload 1:  

* **Definition**
  {CODE TimeSeriesFor-Remove-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `at` | DateTime | Timestamp of the time series entry you want to remove |

* **Return Value**  
  No return value.  

* **Exceptions**  
   * If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  
   * Attempting to remove nonexistent entries results in a noop and does not generate an exception.  

---

#### Overload 2:  

* **Definition**
     {CODE TimeSeriesFor-Remove-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Parameters  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `from` | DateTime | Remove the range of time series entries starting at this timestamp |
     | `to` | DateTime | Remove the range of time series entries ending at this timestamp |

* **Return Value**  
  No return value.  

* **Exceptions**  
   * If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  
   * Attempting to remove nonexistent entries results in a noop and does not generate an exception.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.Query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time series name.  
* Call `TimeSeriesFor.Remove`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

In this sample we use the [first overload](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-1) 
to remove a single entry from a time series.  
{CODE timeseries_region_Remove-TimeSeriesFor-Single-Time Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

In this sample we use the [second overload](../../../../document-extensions/timeseries/client-api/session-methods/remove-ts-data#overload-2) 
to remove a range of entries from a time series.  
{CODE timeseries_region_TimeSeriesFor-Remove-Time Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
