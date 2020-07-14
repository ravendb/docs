# Session: Appending & Updating Time Series

---

{NOTE: }

* Create and update time series and their entries using `TimeSeriesFor.Append`.  

* You can append a single [time series entry](../../../../document-extensions/timeseries/design#time-series-entries) at a time.  
  Note, however, that you can `Append` as many times as you need to before calling 
  `session.SaveChanges`, to append multiple entries in a single transaction.  

* In this page:  
      * [`TimeSeriesFor.Append`](../../../../document-extensions/timeseries/client-api/session/append#timeseriesfor.append)  
         * [Syntax](../../../../document-extensions/timeseries/client-api/session/append#syntax)  
         * [Usage Flow](../../../../document-extensions/timeseries/client-api/session/append#usage-flow)  
         * [Usage Samples](../../../../document-extensions/timeseries/client-api/session/append#usage-samples)  
{NOTE/}



---

{PANEL: `TimeSeriesFor.Append`}

* `TimeSeriesFor.Append` is used for the creation of time series and 
  time series entries, and for the modification of entries values.  
   * **Creating a Time Series**  
     Append an entry to a time series that doesn't exist yet,  
     to create the time series and add it the new entry.  
   * **Creating a Time Series Entry**  
     Append an existing time series a new entry,  
     to add the entry to this series at the specified timestamp.  
   * **Modifying Entry Values**  
     Append a time series an entry it already has,  
     to update the existing entry with the new data. 

{PANEL/}

{PANEL: Syntax}

* There are two `TimeSeriesFor.Append` methods:  
   * Append an entry with a single value.  
     {CODE TimeSeriesFor-Append-definition-double@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * Append an entry with multiple values.  
     {CODE TimeSeriesFor-Append-definition-inum@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | DateTime | Time series entry's timestamp |
    | `value` | double | Entry's value |
    | `values` | IEnumerable<double> | Entry's values |
    | `tag` | string | Entry's tag <br> The tag is optional. |

* **Exceptions**  
  If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID,  
      -or-  
      Pass it an [entity tracked by the session](../../../../client-api/session/loading-entities), e.g. a document object returned from [session.Query](../../../../client-api/session/querying/how-to-query) or from [session.Load](../../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time series name.  
* Call `TimeSeriesFor.Append`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we append an entry with a single value.  
  Though We run a loop to append multiple entries, all entries are appended in a single 
  transaction when `SaveChanges` is executed.  
   {CODE timeseries_region_TimeSeriesFor-Append-TimeSeries-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* Here, we append multi-value StockPrice entries.  
  Notice the clarity gained by [naming the values](../../../../document-extensions/timeseries/client-api/named-time-series-values).  
   {CODE-TABS}
   {CODE-TAB:csharp:Native timeseries_region_Append-Unnamed-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TAB:csharp:Named timeseries_region_Append-Named-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TABS/}

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
