# Set-Based Patch Operations
---

{NOTE: }

* Set-based patch operations allow you to apply changes to a set of documents that match specific criteria instead of separately targeting each document.

* To perform patch operations on a single document see [Single Document Patch Operations](../../../client-api/operations/patching/single-document).  
  Set-based patching can also be done from the [Studio](../../../studio/database/documents/patch-view).

* In this page: 
  * [Overview](../../../client-api/operations/patching/set-based#overview)
      * [Defining set-based patching](../../../client-api/operations/patching/set-based#defining-set-based-patching)
      * [Important characteristics](../../../client-api/operations/patching/set-based#important-characteristics)
  * [Examples](../../../client-api/operations/patching/set-based#examples)
      * [Update by collection query](../../../client-api/operations/patching/set-based#update-by-collection-query)
      * [Update by collection query - access metadata](../../../client-api/operations/patching/set-based#update-by-collection-query---access-metadata)
      * [Update by dynamic query](../../../client-api/operations/patching/set-based#update-by-dynamic-query)
      * [Update by static index query](../../../client-api/operations/patching/set-based#update-by-static-index-query)
      * [Update all documents](../../../client-api/operations/patching/set-based#update-all-documents)
      * [Update by document ID](../../../client-api/operations/patching/set-based#update-by-document-id)
      * [Update by document ID using parameters](../../../client-api/operations/patching/set-based#update-by-document-id-using-parameters)
      * [Allow updating stale results](../../../client-api/operations/patching/set-based#allow-updating-stale-results)
  * [Syntax](../../../client-api/operations/patching/set-based#syntax)  
      * [Send syntax](../../../client-api/operations/patching/set-based#send-syntax)  
      * [PatchByQueryOperation syntax](../../../client-api/operations/patching/set-based#syntax)  

{NOTE/}

---

{PANEL: Overview}

{NOTE: }

<a id="defining-set-based-patching" /> __Defining set-based patching__:  

---

  * In other databases, a simple SQL query that updates a set of documents can look like this:  
    `UPDATE Users SET IsActive = 0 WHERE LastLogin < '2020-01-01'`  

  * To achieve that in RavenDB, define the following two components within a `PatchByQueryOperation`:  
  
      1. __The query__:  
         An [RQL](../../../client-api/session/querying/what-is-rql) query that defines the set of documents to update.  
         Use the exact same syntax as you would when querying the database/indexes for usual data retrieval.  
    
      2. __The update__:  
         A JavaScript clause that defines the updates to perform on the documents resulting from the query.  

  * When sending the `PatchByQueryOperation` to the server, the server will run the query and perform the requested update on the query results.
  
      {CODE-BLOCK:sql}
// A "query & update" sample
// Update the set of documents from the Orders collection that match the query criteria:
// =====================================================================================

// The RQL part:
from Orders where Freight < 10

// The UPDATE part:
update  {
    this.Freight += 10;
} 
      {CODE-BLOCK/}

{NOTE/}
{NOTE: }

<a id="important-characteristics" /> __Important characteristics__:

---

* __Transactional batches__:  
  The patching of documents matching a specified query is run in batches of size 1024.  
  Each batch is handled in a separate write transaction.

* __Dynamic behavior__:  
  During the patching process, documents that are added/modified after the patching operation has started  
  may also be patched if they match the query criteria.

* __Concurrency__:  
  RavenDB doesn't perform concurrency checks during the patching process so it can happen that a document  
  has been modified or deleted while patching is in progress.

* __Patching stale indexes__:  
  By default, set-based patch operations will only succeed if the index is Not [stale](../../../indexes/stale-indexes).  
  For indexes that are frequently updated, you can explicitly allow patching on stale results if needed.  
  An example can be seen in the [Allow updating stale results](../../../client-api/operations/patching/set-based#allow-updating-stale-results) example.

* __Manage lengthy patch operations__:  
  The set-based patch operation (`PatchByQueryOperation`) runs in the server background may take a long time to complete.  
  Executing the operation via the `Send` method return an object that can be __awaited for completion__ or __aborted__ (killed). 
  Learn more about this and see dedicated examples in [Manage length operations](../../../client-api/operations/what-are-operations#manage-lengthy-operations).

{NOTE/}

{PANEL/}

{PANEL: Examples}

{NOTE: }

<a id="update-by-collection-query" /> __Update by collection query__:

---

{CODE:nodejs update_whole_collection@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="update-by-collection-query---access-metadata" /> __Update by collection query - access metadata__:

---

{CODE:nodejs update_collection_name@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="update-by-dynamic-query" /> __Update by dynamic query__:

---

{CODE:nodejs update_by_dynamic_query@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="update-by-static-index-query" /> __Update by static index query__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Update_by_static_index_query update_by_index_query@client-api\operations\patches\setBasedPatchRequests.js /}
{CODE-TAB:nodejs:Index index@client-api\operations\patches\setBasedPatchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="update-all-documents" /> __Update all documents__:

---

{CODE:nodejs update_all_documents@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="update-by-document-id" /> __Update by document ID__:

---

{CODE:nodejs update_by_id@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="update-by-document-id-using-parameters" /> __Update by document ID using parameters__:

---

{CODE:nodejs update_by_id_using_parameters@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{NOTE: }

<a id="allow-updating-stale-results" /> __Allow updating stale results__:

---

* Set `allowStale` to _true_ to allow patching of stale results.

* The RQL in this example is using an auto-index.  
  Use _allowStale_ in exactly the same way when querying a static-index.

{CODE:nodejs update_stale_results@client-api\operations\patches\setBasedPatchRequests.js /}

{NOTE/}
{PANEL/}

{PANEL: Syntax}

---

#### Send syntax

{CODE:nodejs syntax_1@client-api\operations\patches\setBasedPatchRequests.js /}

| Parameter     | Type                    | Description                                                         |
|---------------|-------------------------|---------------------------------------------------------------------|
| __operation__ | `PatchByQueryOperation` | The operation object describing the query and the patch to perform. |

| Return value                          |                                                                                         |
|---------------------------------------|-----------------------------------------------------------------------------------------|
| `Promise<OperationCompletionAwaiter>` | A promise that resolves to an object that allows waiting for the operation to complete. |

---

#### PatchByQueryOperation syntax

{CODE:nodejs syntax_2@client-api\operations\patches\setBasedPatchRequests.js /}

| Parameter         | Type         | Description                                                                                                                                                                               |
|-------------------|--------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __queryToUpdate__ | `string`     | The query & patch definition.<br>The RQL query starts as any other RQL query with a "from" statement.<br>It continues with an "update" clause that contains the Javascript patching code. | 
| __queryToUpdate__ | `IndexQuery` | Object containing the query & the patching string,<br>with the option to use parameters.                                                                                                  | 
| __options__       | `object`     | Options for the _PatchByQueryOperation_.                                                                                                                                                  |


{CODE:nodejs syntax_3@client-api\operations\patches\setBasedPatchRequests.js /}

{CODE:nodejs syntax_4@client-api\operations\patches\setBasedPatchRequests.js /}

{PANEL/}

## Related Articles

### Client API

- [What are operations](../../../client-api/operations/what-are-operations)

### Patching

- [Single document patch operations](../../../client-api/operations/patching/single-document)

### Knowledge Base

- [JavaScript engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
