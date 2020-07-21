# Session: Time Series Querying 
---

{NOTE: }

   {INFO: }
   Learn more about time series queries in the [section dedicated to this subject](../../../../document-extensions/timeseries/querying/overview-and-syntax).  
   {INFO/}

* **LINQ Queries**  
  To query time series using LINQ expressions, use `session.Query`.  
  RavenDB will translate a LINQ query to RQL before transmitting 
  it to the server for execution.  
* **RQL Queries**  
  Clients can express time series queries in RQL and send them to the server 
  for execution using the `session.Advanced.RawQuery` method.  

* In this page:  
   * [Time Series LINQ Queries](../../../../document-extensions/timeseries/client-api/session/querying#time-series-linq-queries)  
      * [Syntax](../../../../document-extensions/timeseries/client-api/session/querying#syntax)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session/querying#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session/querying#usage-samples)  
   * [Client Raw RQL Queries](../../../../document-extensions/timeseries/client-api/session/querying#client-raw-rql-queries)  
      * [RQL Queries Syntax](../../../../document-extensions/timeseries/client-api/session/querying#rql-queries-syntax)  
      * [RQL Queries Usage Flow](../../../../document-extensions/timeseries/client-api/session/querying#rql-queries-usage-flow)  
      * [RQL Queries Usage Samples](../../../../document-extensions/timeseries/client-api/session/querying#rql-queries-usage-samples)  

{NOTE/}

---

{PANEL: Time Series LINQ Queries}

To build a time series LINQ query, run a document query using `session.Query` 
and extend it using LINQ expressions.  
Here is a simple LINQ query that chooses users by their age and retrieves their 
HeartRate time series, and the RQL equivalent for this query.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ ts_region_LINQ-1-Select-Timeseries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB:csharp:RQL ts_region_LINQ-2-RQL-Equivalent@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

`session.Query` Definition:  
{CODE Query-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

Learn more about `session.Query` [here](../../../../client-api/session/querying/how-to-query#session.query).  

**Return Value**:  

* **`IRavenQueryable<TimeSeriesAggregationResult>`**  for aggregated data.  
  When the query 
  [aggregates time series entries](../../../../document-extensions/timeseries/querying/aggregation-and-projections), 
  the results are returned in an aggregated array.  
* **`IRavenQueryable<TimeSeriesRawResult>`** for non-aggregated data.  
  When the query **doesn't aggregate** time series entries, 
  the results are returned in a list of time series results.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Query`.  
   - Run a document query to locate documents whose time series you want to query.  
   - Extend the query using LINQ expressions to find and project time series data.  
     Start with `Select` to choose a time series.  
* Retrieve the results using -  
  `TimeSeriesAggregationResult` for aggregated data  
   -or-  
  `TimeSeriesRawResult` for non-aggregated data  
{PANEL/}

{PANEL: Usage Samples}

* In this sample, we select a three-days range from the HeartRate time series.  
  {CODE ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* The first occurance of `Where` in the following example, filters documents.  
  The second occurance of `Where` filters entries.
  {CODE ts_region_LINQ-4-Where@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Here, we retrieve a company's stock trade data.  
  Note the usage of named values, so we may address trade Volume [by name](../../../../document-extensions/timeseries/client-api/named-time-series-values).  
   {CODE-TABS}
   {CODE-TAB:csharp:Native timeseries_region_Unnamed-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TAB:csharp:Named timeseries_region_Named-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TABS/}

* Here, we group heart-rate data of people above the age of 72 
  into 1-day groups, and retrieve each group's average heart rate and number of measurements.  
  The aggregated results are retrieved into an `IRavenQueryable<TimeSeriesAggregationResult>` array.  
  {CODE ts_region_LINQ-6-Aggregation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Client Raw RQL Queries}

To send a raw RQL query to the server, use `session.Advanced.RawQuery`.  
`RawQuery` transmits queries to the server without checking or altering 
their contents, time series contents or otherwise  

{PANEL/}

{PANEL: RQL Queries Syntax}

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
        [aggregates time series entries](../../../../document-extensions/timeseries/querying/aggregation-and-projections), 
        the results are returned in an aggregated array.  
      * **`IRawDocumentQuery<TimeSeriesRawResult>`** for non-aggregated data.  
        When the query **doesn't aggregate** time series entries, 
        the results are returned in a list of time series results.  

{PANEL/}

{PANEL: RQL Queries Usage Flow}

* Open a session  
* Call `session.Advanced.RawQuery`  
  Pass it your query  
* Retrieve the results into  
   `TimeSeriesAggregationResult` for aggregated data  
    -or-  
   `TimeSeriesRawResult` for non-aggregated data  
{PANEL/}

{PANEL: RQL Queries Usage Samples}

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
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
