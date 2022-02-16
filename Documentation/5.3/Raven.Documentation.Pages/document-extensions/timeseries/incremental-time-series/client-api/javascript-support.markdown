# Time Series: JavaScript Support

{NOTE: }


* RavenDB's time-series 
  [Javascript Support](../../../../document-extensions/timeseries/client-api/javascript-support) 
  has been extended to support incremental time series.  

* You can use the Javascript [Increment](../../../../document-extensions/timeseries/incremental-time-series/client-api/javascript-support#increment) 
  method to create and modify incremental time series and their entries.  
  The method behaves the same way it does when it is called [using C#](../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment).  

* Incremental time series **cannot** use the non-incrmental time series 
  [Append](../../../../document-extensions/timeseries/client-api/javascript-support#section-1) method.  

* Other Javascript methods available for an incremental time series:  
   * [delete](../../../../document-extensions/timeseries/client-api/javascript-support#section-2)   
   * [get](../../../../document-extensions/timeseries/client-api/javascript-support#section-3)  

* In this page:  
  * [`Increment`](../../../../document-extensions/timeseries/incremental-time-series/client-api/javascript-support#increment)  
  * [Usage Samples](../../../../document-extensions/timeseries/incremental-time-series/client-api/javascript-support#usage-sample)  

{NOTE/}

---

{PANEL: `Increment`}

* There are four `Increment` methods:  
   * Increment a time series entry's array of values at the provided timestamp.   
     {CODE incremental_declaration_increment-values-array-at-provided-timestamp@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Increment a time series entry's array of values at the current time.  
     {CODE incremental_declaration_increment-values-array-at-current-time@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Increment an entry value at the provided timestamp.  
     (If the entry exists and has more than one value, only the first 
     value in its list will be incremented by the passed value.)  
     {CODE incremental_declaration_increment-value-at-provided-timestamp@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Increment an entry value at the current time.  
     (If the entry exists and has more than one value, only the first 
     value in its list will be incremented by the passed value.)  
     {CODE incremental_declaration_increment-value-at-current-time@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | DateTime | Time series entry's timestamp |
    | `values` | IEnumerable<double> | A list of delta values to increment the entry values by |
    | `value` | double | The delta to increment the entry value by |

* **Exceptions**  
  If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  

{PANEL/}

{PANEL: Usage Sample}

In this sample we use 
[session.Advanced.Defer](../../../../document-extensions/timeseries/client-api/session/patch#patching-using-session.advanced.defer) 
to patch an incremental time series.  
We go through a series of collected stock prices, and add a **2** factor to each collected stock price, 
that has been originally miscalculated.  
{CODE incremental_PatchIncrementalTimeSeries@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Patching Time Series**  
[Patching in a Session](../../../../document-extensions/timeseries/client-api/session/patch)  
[Patching Operation](../../../../document-extensions/timeseries/client-api/operations/patch#patchoperation)  
[Patch By Query Operation](../../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
