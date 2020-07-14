# Session: Get Time Series Names

---

{NOTE: }

Get the names of a document's time series using 
`Advanced.GetTimeSeriesFor`.  

* In this page:  
   * [`Advanced.GetTimeSeriesFor`](../../../../../document-extensions/timeseries/client-api/session/get/get-names#advanced.gettimeseriesfor)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/session/get/get-names#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session/get/get-names#usage-flow)  
      * [Usage Sample](../../../../../document-extensions/timeseries/client-api/session/get/get-names#usage-sample)  

{NOTE/}

---

{PANEL: `Advanced.GetTimeSeriesFor`}

Use this method to get all the names of a document's time series.  

{PANEL/}

{PANEL: Syntax}

* **Definition**  
  {CODE GetTimeSeriesFor-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Description |
    |:-------------|:-------------|
    | `instance` | The document whose time series names you want to get |

* **Return Value**  
     
     **`List<string>`**  
     An array of the loaded document's time series names.  

* **Exceptions**  
  If the instance is not tracked by the session, an `ArgumentException` exception is thrown.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Load a document.  
* Pass `Advanced.GetTimeSeriesFor` the loaded document.  

{PANEL/}

{PANEL: Usage Sample}

In this sample, we load a user document and use `Advanced.GetTimeSeriesFor` 
to get a list of its time series' names.  
{CODE timeseries_region_Retrieve-TimeSeries-Names@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
