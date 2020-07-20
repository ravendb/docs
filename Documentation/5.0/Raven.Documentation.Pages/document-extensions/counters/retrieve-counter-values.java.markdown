# Retrieving Counter Values  
---

{NOTE: }

* Use [countersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`get` to retrieve the value of **a single Counter**,  
  or [countersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`getAll` to retrieve the values of **all the Counters of a document**.  

* In this page:  

  * [get Method - Retrieve a single Counter's value](../../document-extensions/counters/retrieve-counter-values#get-method---retrieve-a-single-counter)  
      - [get Syntax](../../document-extensions/counters/retrieve-counter-values#get-syntax)  
      - [get Usage](../../document-extensions/counters/retrieve-counter-values#get-usage-flow)  
      - [get Code Sample](../../document-extensions/counters/retrieve-counter-values#get-code-sample)  

  * [getAll Method - Retrieve all Counters of a document](../../document-extensions/counters/retrieve-counter-values#getall-method---retrieve-all-counters-of-a-document)  
      - [getAll Syntax](../../document-extensions/counters/retrieve-counter-values#getall-syntax)  
      - [getAll Usage](../../document-extensions/counters/retrieve-counter-values#getall-usage-flow)  
      - [getAll Code Sample](../../document-extensions/counters/retrieve-counter-values#getall-code-sample)
{NOTE/}

---

{PANEL: Get Method - Retrieve a single Counter's value}

#### Get Syntax:

* Use `get` to retrieve the current value of a single Counter.  

{CODE:java Get-definition@DocumentExtensions\Counters\Counters.java /}

| Parameters | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  String | Counter's name |

| Return Type | Description |
|:-------------:|:-------------:|
| `long` | Counter's current value |

#### Get Usage Flow:

  * Open a session  
  * Create an instance of `countersFor`.  
      * Either pass `countersFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  * Execute `countersFor.get`

{NOTE: }

#### Get Code Sample:

{CODE:java counters_region_Get@DocumentExtensions\Counters\Counters.java /}
{NOTE/}
{PANEL/}

---

{PANEL: getAll Method - Retrieve ALL Counters of a document}

#### getAll Syntax:

* Use `getAll` to retrieve all names and values of a document's Counters.  

{CODE:java getAll-definition@DocumentExtensions\Counters\Counters.java /}

| Return Type |Description |
|:-------------:|:-------------:|
| Map<String, Long> | Map of Counter names and values |

####getAll Usage Flow:

* Open a session.
* Create an instance of `countersFor`.  
   * Either pass `countersFor` an explicit document ID, -or-  
   * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Execute `countersFor.getAll`.

{NOTE: }

####getAll Code Sample:
{CODE:java counters_region_GetAll@DocumentExtensions\Counters\Counters.java /}
{NOTE/}

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/document-extensions/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Creating and Modifying Counters](../../document-extensions/counters/create-or-modify)  
[Deleting a Counter](../../document-extensions/counters/delete)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
