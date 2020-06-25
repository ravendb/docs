## `session.Advanced.RawQuery`
# Time Series Raw RQL Queries
---

{NOTE: }

* Clients can express time series queries in RQL and send them to the server 
  for execution using the `session.Advanced.RawQuery` method.  
  

{INFO: }
Learn more about time series queries in the [section dedicated to this subject](../../../../../document-extensions/timeseries/querying/queries-overview).  
{INFO/}

    
* [Client Raw RQL Queries](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries#client-raw-rql-queries)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries#syntax)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries#usage-samples)  

{NOTE/}

---

{PANEL: Client Raw RQL Queries}

To send a raw RQL query to the server, use `session.Advanced.RawQuery`.  
`RawQuery`transmits queries to the server without checking or altering 
their contents, time series contents or otherwise  

{PANEL/}

{PANEL: Syntax}

* **`session.Advanced.RawQuery`**  
   * **Definition**  
      {CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `query` | string | Raw RQL Query |

   * **Return Value**:  
      * **`IRawDocumentQuery<TimeSeriesAggregationResult>`**  for aggregated data.  
        When the query 
        [aggregates time series entries](../../../../../document-extensions/timeseries/querying/aggregation-and-projection), 
        the results are returned in an aggregated array.  
      * **`IRawDocumentQuery<TimeSeriesRawResult>`** for non-aggregated data.  
        When the query **doesn't aggregate** time series entries, 
        the results are returned in a list of time series results.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.RawQuery`  
  Pass it your query  
* Retrieve the results into  
   `TimeSeriesAggregationResult` for aggregated data  
    -or-  
   `TimeSeriesRawResult` for non-aggregated data  
{PANEL/}

{PANEL: Usage Samples}

* In this sample, a raw RQL query retrieves 24 hours of HeartRate data from users under the age of 30.  
  The query does not aggregate data, so we retrieve its results using a `TimeSeriesRawResult` list.  
  We define an **offset**, to adjust retrieved results to the client's local time-zone.  

    {CODE-TABS}
    {CODE-TAB:csharp:Declare-Syntax ts_region_Raw-Query-Non-Aggregated-Declare-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TAB:csharp:Select-Syntax ts_region_Raw-Query-Non-Aggregated-Select-Syntax@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TABS/}

---

* In this sample, the query aggregates 7 days of HeartRate entries into 1-day groups.  
  From each group, two values are selected and projected to the client: the **min** 
  and **max** hourly HeartRate values.  
  The aggregated results are retrieved using a `TimeSeriesAggregationResult` array.
    {CODE ts_region_Raw-Query-Aggregated@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}


## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/api-overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/queries-overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
