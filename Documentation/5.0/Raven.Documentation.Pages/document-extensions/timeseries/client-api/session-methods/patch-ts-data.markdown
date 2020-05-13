# Patch Time-Series Data
---

{NOTE: }

To patch time-series data to a document, use `session.Advanced.Defer`.  

* [Patching Time-Series Data Using `session.Advanced.Defer`](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#patching-time-series-data-using-session.advanced.defer)  
   * [Syntax](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#syntax)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/patch-ts-data#usage-samples)  

{NOTE/}

---

{PANEL: Patching Time-Series Data Using `session.Advanced.Defer`}

* `Advanced.Defer` uses a custom Java Script to -  
  * Patch time-series entries to a document.  
  * Removes time-series entries from a document.  
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
   * the change vector, if needed (or `null` if not)  
   * a `PatchRequest` instance  
* Fill the `PatchRequest` instance with your script and its values.  
  The script can be used to append and remove time-series entries.  
* Call `session.SaveChanges()` to perform the patch.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we use `session.Advanced.Defer` to patch 
  a single time-series entry to a document .  
  The script draws its arguments from its "Values" section.  
  {CODE TS_region-Session_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample, we provide `session.Advanced.Defer`with 
  a script that patches 100 time-series entries to a document.  
  Timestamps and values are drawn from an array, and other 
  arguments are provided in the "Values" section.  
  {CODE TS_region-Session_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample, we remove a range of 50 time-series entries 
  from a document.  
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
