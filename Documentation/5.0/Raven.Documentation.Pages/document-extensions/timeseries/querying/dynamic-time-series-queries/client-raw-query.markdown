## Client Raw RQL Time-Series Query
---

{NOTE: }

* You can run a time-series query using raw RQL via `session.Advanced.RawQuery`.  
    
* [Run a Time-Series Raw RQL Query](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-raw-query#run-a-time-series-raw-rql-query)  
   * [Syntax](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-raw-query#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-raw-query#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-raw-query#usage-samples)  

{NOTE/}

---

{PANEL: Run a Time-Series Raw RQL Query}

* You can run both aggregating and non-aggregating queries
  using `Advanced.RawQuery`.  
   * Aggregating queries will return an aggregated array.  
   * Non-aggregating queries will return a list of raw results.  
* You can define an **offset** to ensure that timestamps are 
  created using a time zone of your choice rather than the 
  server's local time.  
  **Note that an offset can be defined only in a `declare` function.**  

{PANEL/}

{PANEL: Syntax}

* **`Advanced.RawQuery`**  
   * **Definition**  
      {CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `query` | string | Raw RQL Query |

   * **Return Value**:  
      * **`IRawDocumentQuery<TimeSeriesAggregationResult>`** 
        for aggregated data.  
      * **`IRawDocumentQuery<TimeSeriesRawResult>`** 
        for non-aggregated data.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.RawQuery`  
  Pass it your query  
* Get the results using `TimeSeriesAggregationResult` for aggregated data  
  -or-
  `TimeSeriesRawResult` for non-aggregated data  
{PANEL/}

{PANEL: Usage Samples}

* In this sample, we run a query that does not aggregate data, and get 
  its result using a `TimeSeriesRawResult` list.  
  The query uses the **time-series function** `declare` syntax.  
  The query also defines an **offset**, to ensure that timestamps 
  reflect the client's time zone.  

    {CODE timeseries_region_Raw-Query-Non-Aggregated@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}


* In this sample, we run a query that aggregates data and get 
  its result using a `TimeSeriesAggregationResult` array.
    {CODE timeseries_region_Raw-Query-Aggregated@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
