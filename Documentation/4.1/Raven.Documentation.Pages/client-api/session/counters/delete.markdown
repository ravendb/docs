# Deleting a Counter  
---

{NOTE: }

* Use the [CountersFor](../../../client-api/session/counters/overview#counter-methods-and-the--object).`Delete` method to remove a Counter from a document.  

* In this page:
    - [`Delete ` Syntax](../../../client-api/session/counters/delete#delete-syntax)
    - [`Delete ` Usage](../../../client-api/session/counters/delete#delete-usage)
    - [Code Sample](../../../client-api/session/counters/delete#code-sample)
{NOTE/}

---

{PANEL: `Delete ` Syntax}

{CODE Delete-definition@ClientApi\Session\Counters\Counters.cs /}

| Parameter | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  string | Counter's name |
{PANEL/}

{PANEL: `Delete ` Usage}

*  **Flow**:  
  - Open a session  
  - Create an instance of `CountersFor`.  
      - Either pass the `CountersFor` constructor an explicit document ID, -or-  
      - Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  - Execute `CountersFor.Delete`
  - Execute `session.SaveChanges` for the changes to take effect  

* **Note**:
    * A Counter you deleted will be removed only after the execution of `SaveChanges()`.  
    * Deleting a document deletes its Counters as well.  
    * `Delete` will **not** generate an error if the Counter doesn't exist.  

{PANEL/}

{PANEL: Code Sample}

{CODE counters_region_Delete@ClientApi\Session\Counters\Counters.cs /}
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../../client-api/session/counters/overview)  
[Creating and Modifying Counters](../../../client-api/session/counters/create-or-modify)  
[Retrieving Counter Values](../../../client-api/session/counters/retrieve-counter-values)  
[Counters and other features](../../../client-api/session/counters/counters-and-other-features)  
[Counters in a Cluster](../../../client-api/session/counters/counters-in-a-cluster)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
