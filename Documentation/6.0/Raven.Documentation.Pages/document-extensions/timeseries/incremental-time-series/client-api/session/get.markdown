# Session: Get Incremental Time Series Entries 

---

{NOTE: }

* To get a range of incremental time series entries, use one of the `IncrementalTimeSeriesFor.Get` methods.  
* This method retrieves only the **accumulated values** of incremental time series entries.  
  To retrieve the values stored by each node, use [GetTimeSeriesOperation](../../../../../document-extensions/timeseries/incremental-time-series/client-api/operations/get).  

{INFO: }

* [Include](../../../../../document-extensions/timeseries/client-api/session/include/overview) 
  time series data while [loading](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load) 
  or [querying](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query) 
  documents, to keep the data locally in the client's session and refrain from unnecessary additional trips to the server.  
* When caching is enabled, time series data is kept in the session cache as well.  

{INFO/}

* In this page:  
   * [`IncrementalTimeSeriesFor.Get`](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/get#incrementaltimeseriesfor.get)  
      * [Syntax](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/get#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/incremental-time-series/client-api/session/get#usage-flow)  
      * [Code Samples](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#code-samples)  

{NOTE/}

---

{PANEL: `IncrementalTimeSeriesFor.Get`}

`IncrementalTimeSeriesFor.Get` retrieves a range of entries from a single time series.  
     
* To retrieve multiple series' data, use the 
  [GetMultipleTimeSeriesOperation](../../../../../document-extensions/timeseries/client-api/operations/get#getmultipletimeseriesoperation) 
  document-store operation.  
* Retrieved data can be sliced to **pages** to get time series entries 
  gradually, one custom-size page at a time.  

{PANEL/}

{PANEL: Syntax}

* `IncrementalTimeSeriesFor.Get` method:  
   {CODE incremental_declaration_session-get-time-series@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `from` | `DateTime?` | Range Start |
    | `to` | `DateTime?` | Range End |
    | `start` | `int` | Paging first entry. <br> E.g. 50 means the first page would start at the 50th time series entry. <br> Default: 0, for the first time-series entry. |
    | `pagesize` | `int` | Paging page-size. <br> E.g. set `pagesize` to 10 to retrieve pages of 10 entries. <br> Default: int.MaxValue, for all time series entries. |

* **Return Values**  
   * **`TimeSeriesEntry[]`** - an array of time series entry classes.

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
* Call `IncrementalTimeSeriesFor.Get`.  

{PANEL/}

{PANEL: Code Samples}

* In this sample we retrieve all the entries of a time series.  
   {CODE incremental_get-time-series@DocumentExtensions\TimeSeries\IncrementalTimeSeriesTests.cs /}

* Find additional samples [here](../../../../../document-extensions/timeseries/client-api/session/get/get-entries).  

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
