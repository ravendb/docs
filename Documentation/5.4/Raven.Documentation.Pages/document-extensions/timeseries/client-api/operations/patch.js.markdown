# Patch Time Series Operations  

---

{NOTE: }
   

* Patching time series data (Append or Delete entries) can be performed via [Operations](../../../../client-api/operations/what-are-operations).
  * Use [PatchOperation](../../../../client-api/operations/patching/single-document) to patch data on a **single** document.
  * Use [PatchByQueryOperation](../../../../client-api/operations/patching/set-based) to patch data on **multiple** documents.

* Patching time series entries on a single document can also be performed via the [session](../../../../document-extensions/timeseries/client-api/session/patch).

* In this page:  
  * [Patch time series data - single document](../../../../document-extensions/timeseries/client-api/operations/patch#patch-time-series-data---single-document)
     * [Usage](../../../../document-extensions/timeseries/client-api/operations/patch#usage)
     * [Examples](../../../../document-extensions/timeseries/client-api/operations/patch#examples)
     * [Syntax](../../../../document-extensions/timeseries/client-api/operations/patch#syntax)
  * [Patch time series data - multiple documents](../../../../document-extensions/timeseries/client-api/operations/patch#patch-time-series-data---multiple-documents)
     * [Usage](../../../../document-extensions/timeseries/client-api/operations/patch#usage-1)
     * [Examples](../../../../document-extensions/timeseries/client-api/operations/patch#examples-1)
     * [Syntax](../../../../document-extensions/timeseries/client-api/operations/patch#syntax-1)

{NOTE/}

---

{PANEL: Patch time series data - single document}

---

### Usage

* Create a `PatchRequest` instance:
    * Define the Append or Delete action using the [JavaScript time series API](../../../../document-extensions/timeseries/client-api/javascript-support).

* Create a `PatchOperation` instance and pass it:
    * The ID of the document to patch
    * The document change vector (or `null`)
    * The `PatchRequest` object

* Execute the `PatchOperation` operation by calling `store.operations.send`

* NOTE:
    * The server treats timestamps passed in the patch request script as **UTC**, no conversion is applied by the client to local time.
    * Appending entries to a time series that doesn't yet exist yet will create the time series.
    * No exception is thrown if the specified document does not exist.

---

### Examples

{NOTE: }

In this example, we **append** a single entry to time series "HeartRates" on the specified document.
  
{CODE:nodejs patch_1@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

{NOTE: }

In this example, we **append** 100 entries to time series "HeartRates" on the specified document.  
Timestamps and values are drawn from an array and other arguments are provided in the "values" property.  
 
{CODE:nodejs patch_2@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

{NOTE: }

In this example, we **delete** a range of 50 entries from time series "HeartRates" on the specified document.  

{CODE:nodejs patch_3@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

---

### Syntax

* The detailed syntax of `PatchOperation` is listed under this [syntax section](../../../../client-api/operations/patching/single-document#operations-api-syntax).

* The detailed syntax of `PatchRequest` is listed under this [syntax section](../../../../client-api/operations/patching/single-document#session-api-using-defer-syntax).

* The available JavaScript API methods are detailed in the [time series JavaScript support](../../../../document-extensions/timeseries/client-api/javascript-support) article.

{PANEL/}

{PANEL: Patch time series data - multiple documents}

---

### Usage

* In order to patch time series data on multiple documents, you need to:
  * Define a query that retrieves the set of documents to be patched (can be a dynamic or an index query).
  * Define the patching action that will be executed on the matching documents.

* This is achieved by defining a string, or creating an instance of `IndexQuery` that contains such string,  
  with the following two parts:
  * **The query**: provide an [RQL](../../../../client-api/session/querying/what-is-rql) code snippet to filter the documents you want to patch.
  * **The patching script**: use the [JavaScript time series API](../../../../document-extensions/timeseries/client-api/javascript-support) to define the patching action.
    
* Create a `PatchByQueryOperation` instance and pass it the `IndexQuery` object, or the defined string.

* Execute the `PatchByQueryOperation` by calling `store.operations.send`.  
  * The patch operation will be executed only on documents that match the query.
  * This type of operation can be awaited for completion. Learn more in [Manage length operations](../../../../client-api/operations/what-are-operations#manage-lengthy-operations).

* NOTE:
    * The server treats timestamps passed in the patch request script as **UTC**, no conversion is applied.
    * No exception is thrown if any of the documents no longer exist during patching.

---

### Examples

{NOTE: }

In this example, we **append** an entry to time series "HeartRates" on ALL documents in the "Users" collection.

{CODE:nodejs patch_4@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

{NOTE: }

In this example, we **delete** the "HeartRates" time series from documents that match the query criteria.  

{CODE:nodejs patch_5@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

{NOTE: }

* In this example, for each document in the "Users" collection, we patch a document field with data retrieved from its time series entries.
  The document's time series data itself is Not patched.

* The document field "numberOfUniqueTagsInTS" will be updated with the number of unique tags in the user's "HeartRates" time series.

* To do this, we use the JavaScript [get](../../../../document-extensions/timeseries/client-api/javascript-support#section-3) method to get all the time series entries for each document  
  and extract each entry's tag.  
  
{CODE:nodejs patch_6@documentExtensions\timeSeries\client-api\patchOperations.js /}

{NOTE/}

---

### Syntax

* The detailed syntax of `PatchByQueryOperation` is listed under this [syntax section](../../../../client-api/operations/patching/set-based#patchbyqueryoperation-syntax).

* The available JavaScript API methods are detailed in the [time series JavaScript support](../../../../document-extensions/timeseries/client-api/javascript-support) article.

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
