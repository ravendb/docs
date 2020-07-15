# Session: Get Time Series Entries 

---

{NOTE: }

To get a range of time series entries, use one of the `TimeSeriesFor.Get` methods.  

{INFO: }

* [Include](../../../../../document-extensions/timeseries/client-api/session/include/overview) 
  time series data while [loading](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load) 
  or [querying](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query) 
  documents, to keep the data locally in the client's session and refrain from unnecessary additional trips to the server.  
* When caching is enabled, time series data is kept in the session cache as well.  

{INFO/}

* In this page:  
   * [`TimeSeriesFor.Get`](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#timeseriesfor.get)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#usage-samples)  

{NOTE/}

---

{PANEL: `TimeSeriesFor.Get`}

`TimeSeriesFor.Get` retrieves a range of entries from a single time series.  
     
* To retrieve multiple series' data, use the 
  [GetMultipleTimeSeriesOperation](../../../../../document-extensions/timeseries/client-api/operations/get#getmultipletimeseriesoperation) 
  document-store operation.  
* Retrieved data can be sliced to **pages** to get time series entries 
  gradually, one custom-size page at a time.  

{PANEL/}

{PANEL: Syntax}

* There are two `TimeSeriesFor.Get` methods:  
   {CODE TimeSeriesFor-Get-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE TimeSeriesFor-Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `from` | `DateTime?` | Range Start |
    | `to` | `DateTime?` | Range End |
    | `start` | `int` | Paging first entry. <br> E.g. 50 means the first page would start at the 50th time series entry. <br> Default: 0, for the first time-series entry. |
    | `pagesize` | `int` | Paging page-size. <br> E.g. set `pagesize` to 10 to retrieve pages of 10 entries. <br> Default: int.MaxValue, for all time series entries. |

* **Return Values**  
   * **`TimeSeriesEntry[]`** - an array of time series entry classes.
      {CODE TimeSeriesEntry-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **`TimeSeriesEntry<TValues>[]`** - 
     Time series values that can be referred to [by name](../../../../../document-extensions/timeseries/client-api/named-time-series-values).  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID,  
      -or-  
      Pass it an [entity tracked by the session](../../../../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../../../../client-api/session/querying/how-to-query) 
      or from [session.Load](../../../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time series name.  
* Call `TimeSeriesFor.Get`.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we retrieve all the entries of a time series, 
  using its parent-document's ID explicitly.  
   {CODE timeseries_region_Get-All-Entries-Using-Document-ID@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample we query for a document and get its "Heartrate" time series data.  
   {CODE timeseries_region_Pass-TimeSeriesFor-Get-Query-Results@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Here, we check whether a stock's closing-time price is rising from day to day (over three days).  
  Since each time series entry contains multiple StockPrice values, we include a sample that 
  uses [named time series values](../../../../../document-extensions/timeseries/client-api/named-time-series-values) 
  to make the code easier to read.  
   {CODE-TABS}
   {CODE-TAB:csharp:Native timeseries_region_Get-NO-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TAB:csharp:Named timeseries_region_Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TABS/}

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
