## Include Time-Series Data with `session.Advanced.RawQuery`

---

{NOTE: }

You can include time-series data during a raw RQL query 
via `session.Advanced.RawQuery`.  

* [Include Time-Series Data with `Advanced.RawQuery`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries#include-time-series-data-with-advanced.rawquery)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries#syntax)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries#usage-sample)  

{NOTE/}

---

{PANEL: Include Time-Series Data with `Advanced.RawQuery`}

To include time-series data while querying via `Advanced.RawQuery`, 
use the `include timeseries` expression in your RQL query.  

{PANEL/}

{PANEL: Syntax}

* **`Advanced.RawQuery`**  
   * **Definition**  
      {CODE RawQuery-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `query` | string | Raw RQL Query |

   * **Return Value**: **`IRawDocumentQuery`**  
       {CODE-BLOCK: JSON}
public interface IRawDocumentQuery<T> :
    IQueryBase<T, IRawDocumentQuery<T>>,
    IDocumentQueryBase<T>
{
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.RawQuery`  
  Use `include timeseries` in your query  
* Pass `include timeseries` its arguments:  
   * **Time-series name**  
   * **Range start**  
   * **Range end**  

{PANEL/}

{PANEL: Usage Sample}

In this sample, we use a raw query to retrieve a document 
and include data from the documkent's "Heartrate" time-series.  

{CODE timeseries_region_Raw-Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

The entries we Get after the query, are retrieved 
**from the session's cache**.  

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
