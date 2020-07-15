# Operations: Patch Time Series  

---

{NOTE: }

* Time series data can be patched -  
   * to a single document located by its ID, using [PatchOperation](../../../../client-api/operations/patching/single-document#patching-how-to-perform-single-document-patch-operations).  
   * to multiple documents located by a query, using [PatchByQueryOperation](../../../../client-api/operations/patching/set-based).  
* Both patching operations can be used to Append, Get and Delete time series entries.  

* In this page:  
  * [Patching Operations](../../../../document-extensions/timeseries/client-api/operations/patch#patching-operations)  
  * [`PatchOperation`](../../../../document-extensions/timeseries/client-api/operations/patch#patchoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/operations/patch#syntax)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/operations/patch#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/operations/patch#usage-samples)  
  * [`PatchByQueryOperation`](../../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/operations/patch#syntax-1)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/operations/patch#usage-flow-1)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/operations/patch#usage-samples-1)  

{NOTE/}

---

{PANEL: Patching Operations}

{INFO: }

* To patch time series data, use `PatchOperation` or `PatchByQueryOperation`.  
* `PatchOperation` is RavenDB's operation for single-document patching, and 
  `PatchByQueryOperation` is used for set-based document operations. 
  You can use both to patch time series data, by 
  [customizing the JavaScript](../../../../document-extensions/timeseries/client-api/javascript-support) 
  they are using.  

{INFO/}

* Use `PatchOperation` to **load a document by its ID and patch it time series entries**.  

    Here, for example, we use `PatchOperation` to patch a document a single time series entry.
    {CODE TS_region-Operation_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Use `PatchByQueryOperation` to **run a document query and patch time series entries to matching documents**.  

    Here, we use `PatchByQueryOperation` to append a time series entry to all 
    documents of the User collection.
    {CODE TS_region-PatchByQueryOperation-Append-To-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: `PatchOperation`}

---

#### Syntax

* **`PatchOperation`**  
   * **Definition**  
     {CODE PatchOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `id` | `string` | Patched document ID |
        | `changeVector` | `string` | Change vector, to verify that the document hasn't been modified. <br> Can be `null`. |
        | `patch` | `PatchRequest` | The patching JavaScript |
        | `patchIfMissing` | `PatchRequest` | Patching JavaScript to be used if the document isn't found |
        | `skipPatchIfChangeVectorMismatch` | `bool` | If true, do not patch if the document has been modified <br> default: **false** |

* **`PatchRequest`**  
   * **Definition**  
     {CODE PatchRequest-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `Script` | `string` | Patching script |
        | `Values` | `Dictionary<string, object>` | Values that the patching script can use |

---

#### Usage Flow

* Create an instance of `PatchOperation` and pass its constructor -  
   * the document ID  
   * the change vector (or `null`)  
   * a new `PatchRequest` instance  
* Use the `PatchRequest` instance to pass `PatchOperation` 
  a JavaScript that appends or deletes time series entries.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples

* In this sample, we provide `PatchOperation`with a script that appends 
  100 time series entries to a document.  
  Timestamps and values are drawn from an array, and other 
  arguments are provided in the "Values" section.  
  {CODE TS_region-Operation_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use `PatchOperation` to delete a range of 50 time series 
  entries from a document.  
  {CODE TS_region-Operation_Patch-Delete-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

{PANEL: `PatchByQueryOperation`}

---

#### Syntax

* **`store.Operations.Send` Definition**  
  {CODE Store-Operations-send-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **`PatchByQueryOperation` Definition**  
  {CODE PatchByQueryOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `queryToUpdate` | `IndexQuery` | The query, including the JavaScript |
        | `QueryOperationOptions` | `options` | Additional options <br> Default: `null` |

* **Return Value**: `Operation`   

---

#### Usage Flow

* Create a `PatchByQueryOperation` operation.  
* Pass `PatchByQueryOperation` a new `IndexQuery` instance.  
* Add the `IndexQuery` instance a JavaScript that specifies 
   the query you want to run.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples

* In this sample, we run a document query and delete the HeartRate time series 
  from documents we find.  
   {CODE TS_region-PatchByQueryOperation-Delete-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we patch each User document a "NumberOfUniqueTagsInTS" field with 
  the number of different tags in the user's "ExerciseHeartRate" time series.  
  To do this, we use the JavaScript `get` method to get each time series' entries (the 
  range we choose includes them all), and check each entry's tag.  
   {CODE TS_region-PatchByQueryOperation-Get@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles

**Time Series and JavaScript**  
[The Time Series JavaScript API](../../../../document-extensions/timeseries/client-api/javascript-support)  

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
