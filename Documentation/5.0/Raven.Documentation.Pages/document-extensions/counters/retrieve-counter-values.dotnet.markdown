# Retrieving Counter Values  
---

{NOTE: }

* Use [CountersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`Get` to retrieve the value of **a single Counter**,  
  or [CountersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`GetAll` to retrieve the values of **all the Counters of a document**.  

* In this page:  

  * [Get Method - Retrieve a single Counter's value](../../document-extensions/counters/retrieve-counter-values#get-method---retrieve-a-single-counter)  
      - [Get Syntax](../../document-extensions/counters/retrieve-counter-values#get-syntax)  
      - [Get Usage](../../document-extensions/counters/retrieve-counter-values#get-usage-flow)  
      - [Get Code Sample](../../document-extensions/counters/retrieve-counter-values#get-code-sample)  

  * [GetAll Method - Retrieve all Counters of a document](../../document-extensions/counters/retrieve-counter-values#getall-method---retrieve-all-counters-of-a-document)  
      - [GetAll Syntax](../../document-extensions/counters/retrieve-counter-values#getall-syntax)  
      - [GetAll Usage](../../document-extensions/counters/retrieve-counter-values#getall-usage-flow)  
      - [GetAll Code Sample](../../document-extensions/counters/retrieve-counter-values#getall-code-sample)
{NOTE/}

---

{PANEL: Get Method - Retrieve a single Counter's value}

#### Get Syntax:

* Use `Get` to retrieve the current value of a single Counter.  

{CODE Get-definition@DocumentExtensions\Counters\Counters.cs /}

| Parameters | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  string | Counter's name |

| Return Type | Description |
|:-------------:|:-------------:|
| `long` | Counter's current value |

#### Get Usage Flow:

  * Open a session  
  * Create an instance of `CountersFor`.  
      * Either pass `CountersFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).  
  * Execute `CountersFor.Get`

{NOTE: }

#### Get Code Sample:

{CODE counters_region_Get@DocumentExtensions\Counters\Counters.cs /}
{NOTE/}
{PANEL/}

---

{PANEL: GetAll Method - Retrieve ALL Counters of a document}

#### GetAll Syntax:

* Use `GetAll` to retrieve all names and values of a document's Counters.  

{CODE GetAll-definition@DocumentExtensions\Counters\Counters.cs /}

| Return Type |Description |
|:-------------:|:-------------:|
| Dictionary<string, long> | Map of Counter names and values |

####GetAll Usage Flow:

* Open a session.    - 
* Create an instance of `CountersFor`.  
   * Either pass `CountersFor` an explicit document ID, -or-  
   * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).  
* Execute `CountersFor.GetAll`.

{NOTE: }

####GetAll Code Sample:
{CODE counters_region_GetAll@DocumentExtensions\Counters\Counters.cs /}
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
