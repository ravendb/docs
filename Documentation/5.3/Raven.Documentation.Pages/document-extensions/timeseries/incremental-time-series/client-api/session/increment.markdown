# Session: Create & Modify Incremental Time Series

---

{NOTE: }

* Create and modify incremental time series and their entries using `IncrementalTimeSeriesFor.Increment`.  

* There is no need to explicitly create or delete a time series.  
   * A time series is created when its first entry is incremented.  
   * A time series is deleted when all entries are [deleted](../../../../../) from it.  

* You can add a single [incremental time series entry](../../../../../document-extensions/timeseries/design#time-series-entries) at a time.  
  Note, however, that you can `Increment` as many times as you need to before calling 
  `session.SaveChanges`, to create multiple entries in a single transaction.  

* In this page:  
      * [`IncrementalTimeSeriesFor.Increment`](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment#timeseriesfor.append)  
         * [Syntax](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment#syntax)  
         * [Usage Flow](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment#usage-flow)  
         * [Code Samples](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/increment#code-samples)  
{NOTE/}



---

{PANEL: `IncrementalTimeSeriesFor.Increment`}

* `IncrementalTimeSeriesFor.Increment` is used for the creation of incremental time series and 
  their entries, and for the modification of entry values.  
   * **Creating a new Incremental Time Series**  
     Incrementing entry values for an incremental time series that doesn't exist yet will 
     create the new incremental time series with this entry.  
   * **Creating an Incremental Time Series Entry**  
     Incrementing an entry value for an entry that doesn't exist yet will add the entry to 
     this series at the specified timestamp.  
   * **Modifying Entry Values**  
     Increment a value for an existing entry by a number of your choice.  

{PANEL/}

{PANEL: Syntax}

* There are four `IncrementalTimeSeriesFor.Increment` methods:  
   * Increment a time series entry's array of values at the provided timestamp.   
     {CODE incremental_declaration_increment-values-array-at-provided-timestamp@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Increment a time series entry's array of values at the current time.  
     {CODE incremental_declaration_increment-values-array-at-current-time@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
   * Increment an entry value at the provided timestamp.  
     {CODE incremental_declaration_increment-value-at-provided-timestamp@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
     If the entry exists and has more than one value, only the first 
     value in its list will be incremented by the passed value.  
   * Increment an entry value at the current time.  
     {CODE incremental_declaration_increment-value-at-current-time@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
     If the entry exists and has more than one value, only the first 
     value in its list will be incremented by the passed value.  

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | DateTime | Time series entry's timestamp |
    | `values` | IEnumerable<double> | A list of delta values to increment the entry values by |
    | `value` | double | The delta to increment the entry value by |

* **Exceptions**  
  If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Create an instance of `IncrementalTimeSeriesFor` and pass it:  
    * An explicit document ID,  
      -or-  
      An [entity tracked by the session](../../../../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../../../../client-api/session/querying/how-to-query) 
      or from [session.Load](../../../../../client-api/session/loading-entities#load).  
    * The time series name.  
      The name **must** begin with "INC:" (can be upper or lower case) to identify the time series as incremental.  
* Call `IncrementalTimeSeriesFor.Increment`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Code Samples}

* Increment an array of values in an incremental time series entry.  
  {CODE incremental_create-incremental-time-series@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}
    * If the entry doesn't exist, it will be created with the provided values.  
    * If the entry exists, its values will be increased by the provided values.  
       * a negative number will decrease the current value.  
    * If the time series doesn't exist, it will be created with this first entry.  

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
