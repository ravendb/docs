# Delete Counter  
---

{NOTE: }

* Use the `CountersFor.Delete` method to remove a specific Counter from a document.

* All the document's Counters are deleted when the document itself is deleted.  

* For all other `CountersFor` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:
    * [`Delete ` usage](../../document-extensions/counters/delete#delete-usage)
    * [Example](../../document-extensions/counters/delete#example)
    * [Syntax](../../document-extensions/counters/delete#syntax)

{NOTE/}

---

{PANEL: `Delete ` usage}

**Flow**:  

* Open a session.  
* Create an instance of `CountersFor`.  
    * Either pass `CountersFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).  
* Call `CountersFor.Delete`.
* Call `session.SaveChanges` for the changes to take effect.  

**Note**:

* A Counter you deleted will be removed only after the execution of `SaveChanges()`.  
* `Delete` will **not** generate an error if the Counter doesn't exist.
* Deleting a document deletes all its Counters as well.

{PANEL/}

{PANEL: Example}

{CODE counters_region_Delete@DocumentExtensions\Counters\Counters.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE Delete-definition@DocumentExtensions\Counters\Counters.cs /}

| Parameter     | Type   | Description    |
|---------------|--------|----------------|
| `counterName` | string | Counter's name |

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
