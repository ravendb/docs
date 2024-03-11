# Deleting a Counter  
---

{NOTE: }

* All the Counters for a document are deleted when the document is deleted.  

* You can also use the [CountersFor](../../document-extensions/counters/overview#counter-methods-and-the--object).`Delete` method to remove a specific Counter from a document.  

* In this page:
    - [`Delete ` Syntax](../../document-extensions/counters/delete#delete-syntax)
    - [`Delete ` Usage](../../document-extensions/counters/delete#delete-usage)
    - [Code Sample](../../document-extensions/counters/delete#code-sample)
{NOTE/}

---

{PANEL: `Delete ` Syntax}

{CODE Delete-definition@DocumentExtensions\Counters\Counters.cs /}

| Parameter | Type | Description |
|:-------------:|:-------------:|:-------------:|
| `counterName` |  string | Counter's name |
{PANEL/}

{PANEL: `Delete ` Usage}

*  **Flow**:  
  * Open a session  
  * Create an instance of `CountersFor`.  
      * Either pass `CountersFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).  
  * Execute `CountersFor.Delete`
  * Execute `session.SaveChanges` for the changes to take effect  

* **Note**:
    * A Counter you deleted will be removed only after the execution of `SaveChanges()`.  
    * Deleting a document deletes its Counters as well.  
    * `Delete` will **not** generate an error if the Counter doesn't exist.  

{PANEL/}

{PANEL: Code Sample}

{CODE counters_region_Delete@DocumentExtensions\Counters\Counters.cs /}
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../studio/database/document-extensions/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Creating and Modifying Counters](../../document-extensions/counters/create-or-modify)  
[Retrieving Counter Values](../../document-extensions/counters/retrieve-counter-values)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
