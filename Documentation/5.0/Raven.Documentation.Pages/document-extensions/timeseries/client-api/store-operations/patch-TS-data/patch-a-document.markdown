# `PatchOperation`  
## patch Time-Series Entries To a Document  

---

{NOTE: }

* Use `PatchOperation` to patch time-series data to a single document 
  loaded by its ID.  

* In this page:  
  * [`PatchOperation`](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-a-document#patchoperation)  
     * [Syntax](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-a-document#syntax)  
     * [Usage Flow](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-a-document#usage-flow)  
     * [Usage Samples](../../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data/patch-a-document#usage-samples)  
{NOTE/}

---

{PANEL: `PatchOperation`}

* `PatchOperation` uses a custom Java Script to -  
  * Patch time-series entries to a document.  
  * Remove time-series entries from a document.  

{PANEL/}

{PANEL: Syntax}

* **`PatchOperation`**  
   * **Definition**  
     {CODE PatchOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `id` | `string` | Patched document ID |
        | `changeVector` | `string` | Change vector, to verify that the document hasn't been modified. <br> Can be `null`. |
        | `patch` | `PatchRequest` | The patching Java Script |
        | `patchIfMissing` | `PatchRequest` | Patching Java Script to be used if the document isn't found |
        | `skipPatchIfChangeVectorMismatch` | `bool` | If true, do not patch if the document has been modified <br> default: **false** |

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

* Create an instance of `PatchOperation` and pass its constructor -  
   * the document ID  
   * the change vector (or `null`)  
   * a new `PatchRequest` instance  
* Use the `PatchRequest` instance to pass `PatchOperation` 
  a Java Script.  
  Use the script to append or remove time-series entries.  
* Call `store.Operations.Send` to execute the operation.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample, we use `PatchOperation` to patch a document a single 
  time-series entry.  
  The script draws its arguments from its "Values" section.  
  {CODE TS_region-Operation_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we provide `PatchOperation`with a script that patches 
  100 time-series entries to a document.  
  Timestamps and values are drawn from an array, and other 
  arguments are provided in the "Values" section.  
  {CODE TS_region-Operation_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use `PatchOperation` to remove a range of 50 time-series 
  entries from a document.  
  {CODE TS_region-Operation_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
