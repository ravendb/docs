# Deleting a Counter  
---

{NOTE: }

* All the Counters for a document are deleted when the document is deleted.  

* You can also use the [countersFor](../../../client-api/session/counters/overview#counter-methods-and-the--object).`delete` method to remove a specific Counter from a document.  

* In this page:
    - [`delete ` Syntax](../../../client-api/session/counters/delete#delete-syntax)
    - [`delete ` Usage](../../../client-api/session/counters/delete#delete-usage)
    - [Code Sample](../../../client-api/session/counters/delete#code-sample)
{NOTE/}

---

{PANEL: `delete ` Syntax}

{CODE:java Delete-definition@ClientApi\Session\Counters\Counters.java /}

| Parameter | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  String | Counter's name |
{PANEL/}

{PANEL: `delete ` Usage}

*  **Flow**:  
  * Open a session  
  * Create an instance of `countersFor`.  
      * Either pass `countersFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.load](../../../client-api/session/loading-entities#load).  
  * Execute `countersFor.delete`
  * Execute `session.saveChanges` for the changes to take effect  

* **Note**:
    * A Counter you deleted will be removed only after the execution of `saveChanges()`.  
    * Deleting a document deletes its Counters as well.  
    * `delete` will **not** generate an error if the Counter doesn't exist.  

{PANEL/}

{PANEL: Code Sample}
{CODE:java counters_region_Delete@ClientApi\Session\Counters\Counters.java /}
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../../client-api/session/counters/overview)  
[Creating and Modifying Counters](../../../client-api/session/counters/create-or-modify)  
[Retrieving Counter Values](../../../client-api/session/counters/retrieve-counter-values)  
[Counters and other features](../../../client-api/session/counters/counters-and-other-features)  
[Counters In Clusters](../../../client-api/session/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
