# Get Counter Values  
---

{NOTE: }

* Use `counters_for.get` to retrieve the value of a **single Counter**,  
  or `counters_for.get_all` to retrieve the names and values of **all Counters** associated with a document.

* For all other `counters_for` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:  

  * [Get a single Counter's value](../../document-extensions/counters/retrieve-counter-values#get-a-single-counter)  
     * [`get` usage](../../document-extensions/counters/retrieve-counter-values#usage)  
     * [`get` example](../../document-extensions/counters/retrieve-counter-values#example)  
     * [`get` syntax](../../document-extensions/counters/retrieve-counter-values#syntax)

  * [Get all Counters of a document](../../document-extensions/counters/retrieve-counter-values#get-all-counters-of-a-document)  
     * [`get_all` usage](../../document-extensions/counters/retrieve-counter-values#usage-1)  
     * [`get_all` example](../../document-extensions/counters/retrieve-counter-values#example-1)  
     * [`get_all` Syntax](../../document-extensions/counters/retrieve-counter-values#syntax-1)  

{NOTE/}

---

{PANEL: Get a single Counter's value}

#### `get` usage:  

* Open a session
* Create an instance of `counters_for`.
    * Either pass `counters_for` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `counters_for.get` to retrieve the current value of a single Counter.

---

#### `get` example:  

{CODE:python counters_region_Get@DocumentExtensions\Counters\Counters.py /}

---

#### `get` syntax:  

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

#### `get_all` usage: 

* Open a session.  
* Create an instance of `counters_for`.
    * Either pass `counters_for` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `counters_for.get_all` to retrieve the names and values of all counters associated with the document.

---

#### `get_all` example:  

{CODE:python counters_region_GetAll@DocumentExtensions\Counters\Counters.py /}

---

#### `get_all` syntax:  

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
