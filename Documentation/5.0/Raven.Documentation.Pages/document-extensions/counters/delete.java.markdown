# Deleting a Counter  
---

{NOTE: }

* All the Counters for a document are deleted when the document is deleted.  

* You can also use the [countersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`delete` method to remove a specific Counter from a document.  

* In this page:
    - [`delete ` Syntax](../../document-extensions/counters/delete#delete-syntax)
    - [`delete ` Usage](../../document-extensions/counters/delete#delete-usage)
    - [Code Sample](../../document-extensions/counters/delete#code-sample)
{NOTE/}

---

{PANEL: `delete ` Syntax}

{CODE:java Delete-definition@DocumentExtensions\Counters\Counters.java /}

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
{CODE:java counters_region_Delete@DocumentExtensions\Counters\Counters.java /}
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/document-extensions/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Creating and Modifying Counters](../../document-extensions/counters/create-or-modify)  
[Retrieving Counter Values](../../document-extensions/counters/retrieve-counter-values)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
