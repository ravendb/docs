# Indexed Time-Series Queries 
---

{NOTE: }

* Time-series indexes are not generated automatically by the server, but static 
  indexes can be created by clients or using the Studio. As any other index, static 
  time-series indexes can be queried.  
  
* Querying a time-series index projects the contents of fields specified 
  in the index' `select` clause. The result-set can then be further filtered 
  using LINQ expressions.  
  
* In this page:  
  * [Indexed Queries](../../../../document-extensions/timeseries/querying/indexed-time-series-queries/indexed-queries#indexed-queries)  
  * [Syntax](../../../../document-extensions/timeseries/querying/indexed-time-series-queries/indexed-queries#syntax)  
  * [Usage Samples](../../../../document-extensions/timeseries/querying/indexed-time-series-queries/indexed-queries#usage-samples)  

{NOTE/}

---

{PANEL: Indexed Queries}

You can query time-series indexes using `session.Query` and 
`session.Advanced.DocumentQuery`, and enhance the queries 
using LINQ expressions.  

{PANEL/}

{PANEL: Syntax}

* Definitions  
   
   * `session.Query`  
     {CODE-BLOCK: JSON}
     IRavenQueryable<T> Query<T, TIndexCreator>() where TIndexCreator : AbstractCommonApiForIndexes, new();
     {CODE-BLOCK/}
   * `DocumentQuery`  
     {CODE-BLOCK: JSON}
     IDocumentQuery<T> DocumentQuery<T, TIndexCreator>() where TIndexCreator : AbstractCommonApiForIndexes, new();
     {CODE-BLOCK/}

* **Parameters** 

        | Parameters | Description |
        |:-------------|:-------------|
        | `T` | Results Container |
        | `TIndexCreator` | Index |

* **Return Values**  

    As return values are specific to each index, we need to define 
    a matching results container.  

    In the following sample we define a **map index** that collects three fields from the "HeartRate" 
    time-series, a "Results" container for the results, and an **index query** that uses both.
    {CODE ts_region_Index-TS-Queries-6-Index-Definition-And-Results-Class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE ts_region_Index-TS-Queries-1-session-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Usage Samples}

* **Indexed Queries**  
  To query a time-series index, call `session.Query` or `session.Advanced.DocumentQuery`.  
   {CODE-TABS}
   {CODE-TAB:csharp:session.Query ts_region_Index-TS-Queries-1-session-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TAB:csharp:DocumentQuery ts_region_Index-TS-Queries-3-DocumentQuery@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   {CODE-TABS/}


* **Enhancing index queries**  

   * When you call `session.Query`, You can add LINQ expressions to your query.  
     {CODE ts_region_Index-TS-Queries-2-session-Query-with-Linq@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * You can add LINQ-like expressions to a DocumentQuery as well.  
     {CODE ts_region_Index-TS-Queries-4-DocumentQuery-with-Linq@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Async queries**  
You can run asynchronous queries if you don't want your application to wait for the results.  
{CODE ts_region_Index-TS-Queries-5-session-Query-Async@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
