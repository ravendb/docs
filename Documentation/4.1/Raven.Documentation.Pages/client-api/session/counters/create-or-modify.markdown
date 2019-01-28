# Creating and Modifying Counters
---

{NOTE: }

* Use the [CountersFor](../../../client-api/session/counters/overview#counter-methods-and-the--object).`Increment` method to **create** a new Counter or **modify** an existing Counter's value.  

*  If the Counter exists, `Increment` will add a number to the Counter's current value.  
   If the Counter doesn't exist, `Increment` will create it and set its initial value.  

* In this page:
  - [`Increment` Syntax](../../../client-api/session/counters/create-or-modify#increment-syntax)
  - [`Increment` Usage](../../../client-api/session/counters/create-or-modify#increment-usage)
  - [Code Sample](../../../client-api/session/counters/create-or-modify#code-sample)
{NOTE/}

---

{PANEL: `Increment` Syntax}

{CODE Increment-definition@ClientApi\Session\Counters\Counters.cs /}

| Parameters | Type | Description |
|:-------------:|:-------------:|:-------------|
| `counterName` |  string | Counter's name |
|`incrementValue` | long | Increase Counter by this value. Default value is 1. <br> For a new Counter, this will be its initial value. |
{PANEL/}

{PANEL: `Increment` Usage}

*  **Flow**:  
  - Open a session  
  - Create an instance of `CountersFor`.  
      * Either pass `CountersFor` an explicit document ID, -or-  
      - Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  - Execute `CountersFor.Increment`
  - Execute `session.SaveChanges` for the changes to take effect  

* **Note**:
    * Modifying a Counter using `Increment` only takes effect when `session.SaveChanges()` is executed.  
    * To decrease a Counter's value, pass the method a negative "incrementValue".  
{PANEL/}

{PANEL: Code Sample}

{CODE counters_region_Increment@ClientApi\Session\Counters\Counters.cs /}
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../../client-api/session/counters/overview)  
[Deleting a Counter](../../../client-api/session/counters/delete)  
[Retrieving Counter Values](../../../client-api/session/counters/retrieve-counter-values)  
[Counters and other features](../../../client-api/session/counters/counters-and-other-features)  
[Counters In Clusters](../../../client-api/session/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
