# Get Counter Values  
---

{NOTE: }

* Use `counters_for.get` to retrieve the value of a **single Counter**,  
  or `counters_for.get_all` to retrieve the names and values of **all Counters** associated with a document.

* For all other `counters_for` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:  

  * [Get a single Counter's value](../../document-extensions/counters/retrieve-counter-values#get-a-single-counter)  
     * [Get usage](../../document-extensions/counters/retrieve-counter-values#get-usage)  
     * [Get example](../../document-extensions/counters/retrieve-counter-values#get-example)  
     * [Get syntax](../../document-extensions/counters/retrieve-counter-values#get-syntax)

  * [Get all Counters of a document](../../document-extensions/counters/retrieve-counter-values#get-all-counters-of-a-document)  
     * [GetAll usage](../../document-extensions/counters/retrieve-counter-values#getall-usage)  
     * [GetAll example](../../document-extensions/counters/retrieve-counter-values#getall-example)  
     * [GetAll Syntax](../../document-extensions/counters/retrieve-counter-values#getall-syntax)  

{NOTE/}

---

{PANEL: Get a single Counter's value}

#### Get usage:  

* Open a session
* Create an instance of `counters_for`.
    * Either pass `counters_for` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).
* Call `counters_for.get` to retrieve the current value of a single Counter.

---

#### Get example:  

{CODE:python counters_region_Get@DocumentExtensions\Counters\Counters.py /}

---

#### Get syntax:  

{CODE:python Get-definition@DocumentExtensions\Counters\Counters.py /}

| Parameter     | Type   | Description    |
|---------------|--------|----------------|
| `counter` | str | Counter name |

| Return Type  | Description             |
|--------------|-------------------------|
| `int`        | Counter's current value |

{PANEL/}

{PANEL: Get all Counters of a document}

---

#### GetAll usage: 

* Open a session.  
* Create an instance of `counters_for`.
    * Either pass `counters_for` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).
* Call `counters_for.get_all` to retrieve the names and values of all counters associated with the document.

---

#### GetAll example:  

{CODE:python counters_region_GetAll@DocumentExtensions\Counters\Counters.py /}

---

#### GetAll syntax:  

{CODE:python GetAll-definition@DocumentExtensions\Counters\Counters.py /}

| Return Type      | Description                     |
|------------------|---------------------------------|
| `Dict[str, int]` | Map of Counter names and values |

{PANEL/}

## Related articles

**Studio Articles**:  
[Studio Counters Management](../../studio/database/document-extensions/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Creating and Modifying Counters](../../document-extensions/counters/create-or-modify)  
[Deleting a Counter](../../document-extensions/counters/delete)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
