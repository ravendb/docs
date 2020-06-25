## `IncludeTimeSeries`
# Include TS Data With `session.Load`

---

{NOTE: }

You can include a time series or a part of it while loading a document 
via `session.Load`.  

* [`session.Load` and `IncludeTimeSeries`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load#session.load-and-includetimeseries)  
   * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load#syntax)  
   * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load#usage-flow)  
   * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load#usage-sample)  


{NOTE/}

---

{PANEL: `session.Load` and `IncludeTimeSeries`}

To include time series data while retrieving documents via `session.Load`, 
pass `session.Load` the `IncludeTimeSeries` method of the `IIncludeBuilder` 
interface as an argument.  

{PANEL/}

{PANEL: Syntax}

* **`session.Load`**  
   * **Definition**  
     {CODE Load-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `id` | `string` | Document ID |
        | `includes` | `Action<IIncludeBuilder<T>>` | Include Object |

* **`IncludeTimeSeries`**  
   * **Definition**  
     {CODE IncludeTimeSeries-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `name` | `string` | Time series Name |
        | `from` | `DateTime?` | Time series range start <br> when null, `from` will default to `DateTime.MinValue`. |
        | `to` | `DateTime?` | Time series range end <br>  <br> when null, `to` will default to `DateTime.MaxValue`. |

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Load` and pass it -  
   * The **Document ID**  
   * The **`IncludeTimeSeries`** method with its argumrnts:  
     **Time series name**  
     **Range start**  
     **Range end**  

{PANEL/}

{PANEL: Usage Sample}

In this sample, we load a document using `session.Load` and include 
a selected range of entries from a time series named "Heartrate".  
{CODE timeseries_region_Load-Document-And-Include-TimeSeries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
