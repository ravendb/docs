# Delete a Counter  
---

{NOTE: }

###`CountersFor.Delete`

* Use the `Delete` method to remove a Counter from a document.  

* `Delete` is a member of the [CountersFor Session object](../../../client-api/session/counters/overview#counter-methods-and-the--object).  

* `Delete` will not generate an error if the Counter doesn't exist.  

* In this page:
    - [Syntax](../../../client-api/session/counters/delete#syntax)
    - [Usage](../../../client-api/session/counters/delete#usage)
    - [Code Sample](../../../client-api/session/counters/delete#code-sample)
{NOTE/}

---

{PANEL: Syntax}

{CODE Delete-definition@ClientApi\Session\Counters\Counters.cs /}

| Parameters | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  string | Counter's name |
{PANEL/}

{PANEL: Usage}

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
{PANEL/}

{PANEL: Code Sample}

{CODE counters_region_Delete@ClientApi\Session\Counters\Counters.cs /}
{PANEL/}

## Related articles
### Studio
- [Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

###Client-API - Session
- [Counters Overview](../../../client-api/session/counters/overview)
- [Create or Modify Counter](../../../client-api/session/counters/create-or-modify)
- [Retrieve Counter Values](../../../client-api/session/counters/retrieve-counter-values)
- [Counters Interoperability](../../../client-api/session/counters/interoperability)
- [Counters in a Cluster](../../../client-api/session/counters/counters-in-a-cluster)

###Client-API - Operations
- [Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)
