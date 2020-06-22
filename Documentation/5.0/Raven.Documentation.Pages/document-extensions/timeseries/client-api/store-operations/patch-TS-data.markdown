## `PatchOperation`, `PatchByQueryOperation`  
# Patch Time-Series Data  

---

{NOTE: }

* Time-series data can be patched -  
   * To a single document, located by its ID.  
   * To multiple documents located by a query.  
* Patching operations can be used to append, retrieve and remove time-series entries.  

* In this page:  
  * [Patching Operations](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patching-operations)  
  * [`PatchOperation`](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#syntax)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-samples)  
  * [`PatchByQueryOperation`](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#patchbyqueryoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#syntax-1)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-flow-1)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/patch-ts-data#usage-samples-1)  

{NOTE/}

---

{PANEL: Patching Operations}

* To **load a document by its ID and patch it time-series data**, use [PatchOperation](../../../../../client-api/operations/patching/single-document#patching-how-to-perform-single-document-patch-operations).  
  `PatchOperation` can be used to **append** and **remove** time-series entries.  

    Here, for example, we use `PatchOperation` to patch a document a single time-series entry.
    {CODE TS_region-Operation_Patch-Append-Single-TS-Entry@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

    {INFO: }
    This is the ordinary `PatchOperation` method we use for single-document patch 
    operations, with its custom script using the time-series JS API.  
    {INFO/}

* To **run a document query and patch time-series data to documents you find**, use [PatchByQueryOperation](../../../../../client-api/operations/patching/set-based).  
  `PatchByQueryOperation` can be used to **append**, **get** and **remove** time-series entries.  

    Here, we use `PatchByQueryOperation` to append a time-series entry to all 
    documents in the User collection.
    {CODE TS_region-PatchByQueryOperation-Append-To-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

    {INFO: }
    This is the ordinary `PatchByQueryOperation` method we use for set-based document 
    operations, with its custom script using the time-series JS API.  
    {INFO/}

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

---

#### Usage Flow

* Create an instance of `PatchOperation` and pass its constructor -  
   * the document ID  
   * the change vector (or `null`)  
   * a new `PatchRequest` instance  
* Use the `PatchRequest` instance to pass `PatchOperation` 
  a Java Script that appends or removes time-series entries.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples

* In this sample, we provide `PatchOperation`with a script that patches 
  100 time-series entries to a document.  
  Timestamps and values are drawn from an array, and other 
  arguments are provided in the "Values" section.  
  {CODE TS_region-Operation_Patch-Append-100-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we use `PatchOperation` to remove a range of 50 time-series 
  entries from a document.  
  {CODE TS_region-Operation_Patch-Remove-50-TS-Entries@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
        | `queryToUpdate` | `IndexQuery` | The query, including the Java Script |
        | `QueryOperationOptions` | `options` | Additional options <br> Default: `null` |

* **Return Value**: `Operation`   

---

#### Usage Flow

* Create a `PatchByQueryOperation` operation.  
* Pass `PatchByQueryOperation` a new `IndexQuery` instance.  
* Add the `IndexQuery` instance a Java Script that specifies 
   the query you want to run.  
* Call `store.Operations.Send` to execute the operation.  

---

#### Usage Samples

* In this sample, we run a document query and remove the HeartRate time-series 
  from documents we find.  
   {CODE TS_region-PatchByQueryOperation-Remove-From-Multiple-Docs@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we run a query and **get** a range of entries from the documents we find.  
   {CODE TS_region-PatchByQueryOperation-Get@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
