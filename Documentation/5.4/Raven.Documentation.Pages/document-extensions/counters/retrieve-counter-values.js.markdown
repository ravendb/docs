# Get Counter Values  
---

{NOTE: }

* Use `countersFor.get` to retrieve the value of a **single Counter**,  
  or `countersFor.getAll` to retrieve the names and values of **all Counters** associated with a document.

* For all other `countersFor` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:  

  * [Get a single Counter's value](../../document-extensions/counters/retrieve-counter-values#get-a-single-counter)  
     * [Get usage](../../document-extensions/counters/retrieve-counter-values#get-usage)  
     * [Get example](../../document-extensions/counters/retrieve-counter-values#get-example)  
     * [Get syntax](../../document-extensions/counters/retrieve-counter-values#get-syntax)

  * [Get all Counters of a document](../../document-extensions/counters/retrieve-counter-values#get-all-counters-of-a-document)  
     * [GetAll usage](../../document-extensions/counters/retrieve-counter-values#getall-usage)  
     * [GetAll example](../../document-extensions/counters/retrieve-counter-values#getall-exmaple)  
     * [GetAll Syntax](../../document-extensions/counters/retrieve-counter-values#getall-syntax)  

{NOTE/}

---

{PANEL: Get a single Counter's value}

{NOTE: }

<a id="get-usage" /> **Get usage**:  

* Open a session
* Create an instance of `countersFor`.
    * Either pass `countersFor` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `countersFor.get` to retrieve the current value of a single Counter.

{NOTE/}
{NOTE: }

<a id="get-example" /> **Get example**:  

{CODE:nodejs getCounter@documentExtensions\counters\get.js /}

{NOTE/}
{NOTE: }

<a id="get-syntax" /> **Get syntax**:  

{CODE:nodejs syntax_1@documentExtensions\counters\get.js /}

| Parameter  | Type   | Description    |
|------------|--------|----------------|
| `counter`  | string | Counter's name |

| Return Type                    | Description                                                                                  |
|--------------------------------|----------------------------------------------------------------------------------------------|
| `Promise<number>`              | A `Promise` resolving to the Counter's current value, or to `null` if counter doesn't exist. |

{NOTE/}
{PANEL/}

{PANEL: Get all Counters of a document}

{NOTE: }

<a id="getall-usage" /> **GetAll usage**: 

* Open a session.  
* Create an instance of `countersFor`.
    * Either pass `countersFor` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.load](../../client-api/session/loading-entities#load).
* Call `countersFor.getAll` to retrieve the names and values of all counters associated with the document.

{NOTE/}
{NOTE: }

<a id="getall-example" /> **GetAll example**:  

{CODE:nodejs getAllCounters@documentExtensions\counters\get.js /}

{NOTE/}
{NOTE: }

<a id="getall-syntax" /> **GetAll syntax**:  

{CODE:nodejs syntax_2@documentExtensions\counters\get.js /}

| Return Type       | Description                                                              |
|-------------------|--------------------------------------------------------------------------|
| `Promise<object>` | A `Promise` resolving to a dictionary of counter values by counter names |

{NOTE/}
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
