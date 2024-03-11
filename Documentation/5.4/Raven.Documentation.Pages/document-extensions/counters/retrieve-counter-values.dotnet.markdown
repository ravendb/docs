# Get Counter Values  
---

{NOTE: }

* Use `CountersFor.Get` to retrieve the value of a __single Counter__,  
  or `CountersFor.GetAll` to retrieve the names and values of __all Counters__ associated with a document.

* For all other `CountersFor` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

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

<a id="get-usage" /> __Get usage__:  

* Open a session
* Create an instance of `CountersFor`.
    * Either pass `CountersFor` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).
* Call `CountersFor.Get` to retrieve the current value of a single Counter.

{NOTE/}
{NOTE: }

<a id="get-example" /> __Get example__:  

{CODE counters_region_Get@DocumentExtensions\Counters\Counters.cs /}

{NOTE/}
{NOTE: }

<a id="get-syntax" /> __Get syntax__:  

{CODE Get-definition@DocumentExtensions\Counters\Counters.cs /}

| Parameter     | Type   | Description    |
|---------------|--------|----------------|
| `counterName` | string | Counter's name |

| Return Type  | Description             |
|--------------|-------------------------|
| `long`       | Counter's current value |

{NOTE/}
{PANEL/}

{PANEL: Get all Counters of a document}

{NOTE: }

<a id="getall-usage" /> __GetAll usage__: 

* Open a session.  
* Create an instance of `CountersFor`.
    * Either pass `CountersFor` an explicit document ID, -or-
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).
* Call `CountersFor.GetAll` to retrieve the names and values of all counters associated with the document.

{NOTE/}
{NOTE: }

<a id="getall-example" /> __GetAll example__:  

{CODE counters_region_GetAll@DocumentExtensions\Counters\Counters.cs /}

{NOTE/}
{NOTE: }

<a id="getall-syntax" /> __GetAll syntax__:  

{CODE GetAll-definition@DocumentExtensions\Counters\Counters.cs /}

| Return Type              | Description                     |
|--------------------------|---------------------------------|
| Dictionary<string, long> | Map of Counter names and values |

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
