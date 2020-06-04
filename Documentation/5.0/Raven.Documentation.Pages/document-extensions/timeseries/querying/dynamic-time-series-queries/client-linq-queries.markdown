# Time-Series LINQ Queries
---

{NOTE: }

* A client can call `session.Query`to find documents whose time-series 
  it wishes to query.  
  The document query can then be extended using LINQ expressions, to 
  specify which time-series data of the located documents is to be retrieved.  
* As always, RavenDB will translate the queries to RQL before transmitting 
  them to the server for execution.  

* In this page:  
   * [Time-Series LINQ Queries](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#time-series-linq-queries)  
   * [Syntax](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#usage-flow)  
   * [Selecting Time-Series and Range](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#selecting-time-series-and-range)  
   * [Filtering - `Where`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#filtering---where)  
      * [Using Tags as References - `LoadTag`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#using-tags-as-references---)  
   * [Aggregation and Projection - `GroupBy` and `Select`](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/client-linq-queries#aggregation-and-projection---groupby-and-select)  

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
  [aggregates time-series entries](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#aggregation---group-by), 
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

{PANEL: Selecting Time-Series and Range}

Right after the document query, we select the time-series and, optionally, the 
[range of time-series entries](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#choose-query-range---between-x-and-y) 
we want to retrieve, using the `RavenQuery.TimeSeries` method.  

* **`RavenQuery.TimeSeries` Definition**:
  {CODE RavenQuery-TimeSeries-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `documentInstance` | `object` | Document Instance |
        | `name` | `string` | Time-Series Name |
        | `from` | `DateTime` | Range Start Timestamp (optional) |
        | `to` | `DateTime` | Range End Timestamp (optional) |

* In this sample, we select a three-days range from the HeartRate time-series.  
  {CODE ts_region_LINQ-3-Range-Selection@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Filtering - `Where`}

The `Where` expression can be used in different parts of the query, and filter different entities.  

When we use `Where` right after the document selection, it filters the **documents** whose 
time-series we query.  
When we use `Where` after the time-series selection, it [filters time-series entries](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#filtering---where).  
  
* The first occurance of `Where` in the following example, filters documents.  
  The second occurance of `Where` filters entries.
  {CODE ts_region_LINQ-4-Where@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{INFO: }
Time-series entries can be filtered by their **Tags**, **Values**, and **Timestamps**.
{CODE-BLOCK: JSON}
public DateTime Timestamp { get; set; }
public double[] Values { get; set; }
public string Tag { get; set; }
public bool IsRollup { get; set; }
{CODE-BLOCK/}

{INFO/}

---

#### Using Tags as References - `LoadTag`

A time-series entry's **tag** may [contain a document's ID](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#using-tags-as-references---).  
We can use the `LoadTag` LINQ expression to load the document that a tag refers to.  
We can then filter our result-set by the contents of this document, using `Where`.  

* Consider this example.
  {CODE ts_region_LINQ-5-LoadTag@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * We go through companies StockPrice time-series and load documents that 
     time-series entries refer us to.  
     The documents are Employee profiles.  
   * We then check the employee documents, and filter the results by their addresses.  
   * This can be helpful, for example, when the employees are stockbrokers and we look 
     for local brokers for our stocks.  

{PANEL/}

{PANEL: Aggregation and Projection - `GroupBy` and `Select`}

The `GroupBy` expression allows us to [aggregate result-sets](../../../../document-extensions/timeseries/querying/dynamic-time-series-queries/dynamic-queries#aggregation---group-by) 
by a resolution we choose.  
Once the result-set is aggregated, we can select from each group by various criteria 
and project the selection to the client.  

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
