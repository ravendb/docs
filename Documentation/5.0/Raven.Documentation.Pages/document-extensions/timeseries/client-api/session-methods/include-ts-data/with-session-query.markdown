## `IncludeTimeSeries`
# Include TS Data With `session.Query`

---

{NOTE: }

You can include time series data while retrieving a document via `session.Query`.  

* [`session.Query` and `IncludeTimeSeries`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query#session.query-and-includetimeseries)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query#syntax)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query#usage-samples)  

{NOTE/}

---

{PANEL: `session.Query` and `IncludeTimeSeries`}

To include time series data via `session.Query`, use `session.Query` 
with the `Include` LINQ expression and pass it the `IncludeTimeSeries` 
method of the `IQueryIncludeBuilder` interface as an argument.  

{PANEL/}

{PANEL: Syntax}

* **`IncludeTimeSeries`**  
   * **Definition**  
      {CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `name` | `string` | Time series Name |
        | `from` | `DateTime?` | Time series range start |
        | `to` | `DateTime?` | Time series range end |

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Query`with the `Include` Linq expression  
  Pass it the `IncludeTimeSeries` method as an argument  
* Pass `IncludeTimeSeries` its arguments:  
   * **Time series name**  
   * **Range start**  
   * **Range end**  

{PANEL/}

{PANEL: Usage Samples}

In this sample, we retrieve a document using `session.Query` and 
**include** data from the time series "Heartrate".  
{CODE timeseries_region_Query-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

The entries we Get after including the time series, are retrieved 
**from the session's cache**.  

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
