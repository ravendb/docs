## `session.Advanced.Defer`
# Patch Time Series Data

---

{NOTE: }

* To patch time series data to a document, use `session.Advanced.Defer`.  
   * You can pass `Defer` a script to Append, Get, and Remove time series entries.  
   * You can handle a single document at a time.  

* [Patching Time Series Data Using `session.Advanced.Defer`](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#patching-time-series-data-using-session.advanced.defer)  
   * [Syntax](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-samples)  

{NOTE/}

---

{PANEL: Patching Time Series Data Using `session.Advanced.Defer`}

{INFO: }

* [Defer](../../../../client-api/operations/patching/single-document#non-typed-session-api) 
  is used for patching in general, not necessarily for time series data patching.  
* To patch time series data, you need to customize the Javascript `Defer` uses.  
* Learn about customizable Javascripts and the JS time series API [here](../../../../document-extensions/timeseries/client-api/ts-javascript-api).  

{INFO/}

{PANEL/}

{PANEL: Syntax}

* **`PatchCommandData`**  
   * **Definition**  
     {CODE PatchCommandData-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `id` | `string` | Patched document ID |
        | `changeVector` | `string` | Change vector, to verify that the document hasn't been modified. <br> Can be `null`. |
        | `patch` | `PatchRequest` | The patching Java Script |
        | `patchIfMissing` | `PatchRequest` | Patching Java Script to be used if the document isn't found |

* **`PatchRequest`**  
   * **Definition**  
     {CODE PatchRequest-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `Script` | `string` | Patching script |
        | `Values` | `Dictionary<string, object>` | Values that the patching script can use |

{PANEL/}

{PANEL: Usage Flow}

* Open a session  
* Call `session.Advanced.Defer` and pass it a `PatchCommandData` instance.  
* Pass the `PatchCommandData` constructor method -  
   * the document ID  
   * the change vector (or `null`)  
   * a `PatchRequest` instance with a Java Script that appends or removes time series entries.  
* Call `session.SaveChanges()` to perform the patch.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we pass `Defer`a script that patches a document 100 time series entries with random heartrate values.  
  {CODE TS_region-Session_Patch-Append-100-Random-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample, we remove a range of 50 time series entries from a document.  
  {CODE TS_region-Session_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

## Related articles

**Time Series and JavaScript**  
[The Time Series JavaScript API](../../../../document-extensions/timeseries/client-api/ts-javascript-api)  

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/api-overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/queries-overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
