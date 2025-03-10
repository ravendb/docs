# Get Counter Values  
---

{NOTE: }

* Use `countersFor.get` to retrieve the values of **specific counters**,  
  or `countersFor.getAll` to retrieve the names and values of **all counters** associated with a document.

* For all other `countersFor` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:  

  * [Get values of specific counters](../../document-extensions/counters/retrieve-counter-values#get-values-of-specific-counters)  
     * [`get` usage](../../document-extensions/counters/retrieve-counter-values#usage)  
     * [`get` example](../../document-extensions/counters/retrieve-counter-values#example)  
     * [`get` syntax](../../document-extensions/counters/retrieve-counter-values#syntax)

  * [Get all Counters of a document](../../document-extensions/counters/retrieve-counter-values#get-all-counters-of-a-document)  
     * [`getAll` usage](../../document-extensions/counters/retrieve-counter-values#usage-1)  
     * [`getAll` example](../../document-extensions/counters/retrieve-counter-values#example-1)  
     * [`getAll` Syntax](../../document-extensions/counters/retrieve-counter-values#syntax-1)  

{NOTE/}

---

{PANEL: Get values of specific counters}

#### `get` usage:  

* Open a session
* Create an instance of `countersFor`.
   * Either pass `countersFor` an explicit document ID, -or-
   * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
     e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `countersFor.get` to retrieve counter value/s.
   * `get("CounterName")` will return a single `int` value for the specified counter.  
   * `get(["counter1", "counter2"]` will return an array with values for all listed counters.  
      E.g., `[ "counter1" => 1, "counter2" => 5 ]`

---

#### `get` example:  

{CODE:php counters_region_Get@DocumentExtensions\Counters\Counters.php /}

---

#### `get` syntax:  

{CODE:php Get-definition@DocumentExtensions\Counters\Counters.php /}

| Parameter     | Type   | Description    |
|---------------|--------|----------------|
| `counters` | `string` or `StringList` or `array` | Counter names |

| Return Type  | Description             |
|--------------|-------------------------|
| `int`        | Counter's current value |

{PANEL/}

{PANEL: Get all Counters of a document}

---

#### `getAll` usage: 

* Open a session.  
* Create an instance of `countersFor`.
    * Either pass `countersFor` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `countersFor.getAll` to retrieve the names and values of all counters associated with the document.

---

#### `getAll` example:  

{CODE:php counters_region_GetAll@DocumentExtensions\Counters\Counters.php /}

---

#### `getAll` syntax:  

{CODE:php GetAll-definition@DocumentExtensions\Counters\Counters.php /}

| Return Type      | Description                 |
|------------------|-----------------------------|
| `array` | An array of Counter names and values |

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
