# Reset Index Operation

---

{NOTE: }

* Use `ResetIndexOperation` to rebuild an index:  
  * All existing indexed data will be removed.  
  * All items matched by the index definition will be re-indexed.

* **Indexes scope**:  
  * Both static and auto indexes can be reset.

* **Nodes scope**:  
  * When resetting an index from the **client**:  
    The index is reset only on the preferred node only, and Not on all the database-group nodes.  
  * When resetting an index from the **Studio** [indexes list](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) view:  
    The index is reset on the local node the browser is opened on, even if it is Not the preferred node.  

* If the index is [disabled](../../../../client-api/operations/maintenance/indexes/disable-index) 
  or [paused](../../../../client-api/operations/maintenance/indexes/stop-index), resetting the index 
  will put it back to the **normal** running state on the local node where the action was performed.

* In this page:
    * [Reset index](../../../../client-api/operations/maintenance/indexes/reset-index#reset-index)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/reset-index#syntax)

{NOTE/}

---

{PANEL: Reset index}

{CODE:php reset@ClientApi\Operations\Maintenance\Indexes\ResetIndex.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\ResetIndex.php /}

| Parameters | Type | Description |
| - | - | - |
| **$indexName** | `?string` | Name of an index to reset |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Get Index](../../../../client-api/operations/maintenance/indexes/get-index)  
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)  
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
