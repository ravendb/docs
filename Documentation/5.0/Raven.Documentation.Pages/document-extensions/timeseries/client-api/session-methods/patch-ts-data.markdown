## `session.Advanced.Defer`
# Patch Time-Series Data

---

{NOTE: }

To patch time-series data to a document, use `session.Advanced.Defer`.  

* This is the same [Defer](../../../../client-api/operations/patching/single-document#non-typed-session-api) 
  method we ordinarily use for patching, with additional Java Script API for the patching of time-series data.  

* [Patching Time-Series Data Using `session.Advanced.Defer`](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#patching-time-series-data-using-session.advanced.defer)  
   * [Syntax](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-samples)  

{NOTE/}

---

{PANEL: Patching Time-Series Data Using `session.Advanced.Defer`}

* The custom Java Script you pass [Defer](../../../../client-api/operations/patching/single-document#non-typed-session-api) 
  can patch time-series entries to a document or remove entries from a document.  
* You can handle a single document at a time.  

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
   * the change vector or `null`)  
   * a `PatchRequest` instance with a Java Script that appends or removes time-series entries.  
* Call `session.SaveChanges()` to perform the patch.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we pass `Defer`a script that patches a document 100 time-series entries with random heartrate values.  
  {CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample, we remove a range of 50 time-series entries from a document.  
  {CODE TS_region-Session_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
