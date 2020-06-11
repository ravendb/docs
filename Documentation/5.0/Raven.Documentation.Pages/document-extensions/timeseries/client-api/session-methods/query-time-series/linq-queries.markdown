## `session.Query`
# Time-Series LINQ Queries
---

{NOTE: }

* To query time-series using LINQ expressions, use `session.Query`.  
* RavenDB will translate a LINQ query to RQL before transmitting 
  it to the server for execution.  

{INFO: }
Learn more about time-series queries in the [section dedicated to this subject](../../../../../document-extensions/timeseries/querying/queries-overview).  
{INFO/}

* In this page:  
   * [Time-Series LINQ Queries](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries#time-series-linq-queries)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries#usage-samples)  

{NOTE/}

---

{PANEL: Time-Series LINQ Queries}

To build a time-series LINQ query, run a document query using `session.Query` 
and extend it using LINQ expressions.  
Here is a simple LINQ query that chooses users by their age and retrieves their 
HeartRate time-series, and the RQL equivalent for this query.  

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
  [aggregates time-series entries](../../../document-extensions/timeseries/querying/aggregation), 
  the results are returned in an aggregated array.  
* **`IRavenQueryable<TimeSeriesRawResult>`** for non-aggregated data.  
  When the query **doesn't aggregate** time-series entries, 
  the results are returned in a list of time-series results.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Query`.  
   - Run a document query to locate documents whose time-series you want to query.  
   - Extend the query using LINQ expressions to find and project time-series data.  
     Start with `Select` to choose a time-series.  
* Retrieve the results using -  
  `TimeSeriesAggregationResult` for aggregated data  
   -or-  
  `TimeSeriesRawResult` for non-aggregated data  
{PANEL/}

{PANEL: Usage Samples}

* In this sample, we select a three-days range from the HeartRate time-series.  
  {CODE ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* The first occurance of `Where` in the following example, filters documents.  
  The second occurance of `Where` filters entries.
  {CODE ts_region_LINQ-4-Where@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In the following sample, we group heart-rate data of people above the age of 72 
  into 1-day groups, and retrieve each group's average heartrate and number of measurements.  
  The aggregated results are retrieved into an `IRavenQueryable<TimeSeriesAggregationResult>` array.  
  {CODE ts_region_LINQ-6-Aggregation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
