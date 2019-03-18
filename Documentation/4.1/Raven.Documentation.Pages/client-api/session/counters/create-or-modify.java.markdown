# Creating and Modifying Counters
---

{NOTE: }

* Use the [countersFor](../../../client-api/session/counters/overview#counter-methods-and-the--object). `increment` method to **create** a new Counter or **modify** an existing Counter's value.  

*  If the Counter exists, `increment` will add a number to the Counter's current value.  
   If the Counter doesn't exist, `increment` will create it and set its initial value.  

* In this page:
  - [`increment` Syntax](../../../client-api/session/counters/create-or-modify#increment-syntax)
  - [`increment` Usage](../../../client-api/session/counters/create-or-modify#increment-usage)
  - [Code Sample](../../../client-api/session/counters/create-or-modify#code-sample)
{NOTE/}

---

{PANEL: `increment` Syntax}

{CODE:java Increment-definition@ClientApi\Session\Counters\Counters.java /}

| Parameters | Type | Description |
|:-------------:|:-------------:|:-------------|
| `counterName` |  String | Counter's name |
|`incrementValue` | Long | Increase Counter by this value. Default value is 1. <br> For a new Counter, this will be its initial value. |
{PANEL/}

{PANEL: `increment` Usage}

*  **Flow**:  
  - Open a session  
  - Create an instance of `countersFor`.  
      * Either pass `countersFor` an explicit document ID, -or-  
      - Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.load](../../../client-api/session/loading-entities#load).  
  - Execute `countersFor.increment`
  - Execute `session.saveChanges` for the changes to take effect  

* **Note**:
    * Modifying a Counter using `increment` only takes effect when `session.saveChanges()` is executed.  
    * To decrease a Counter's value, pass the method a negative number to `increment`.  
{PANEL/}

{PANEL: Code Sample}

{CODE:java counters_region_Increment@ClientApi\Session\Counters\Counters.java /}
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
