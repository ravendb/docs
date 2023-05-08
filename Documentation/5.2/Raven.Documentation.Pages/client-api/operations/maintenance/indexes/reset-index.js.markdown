# Reset Index Operation

---

{NOTE: }

* Use `ResetIndexOperation` to rebuild an index:  
  * All existing indexed data will be removed.  
  * All items matched by the index definition will be re-indexed.  

* __Indexes scope__:  
  * Both static and auto indexes can be reset.

* __Nodes scope__:  
  * When resetting an index from the __client__:  
    The index is reset only on the preferred node only, and Not on all the database-group nodes.  
  * When resetting an index from the __Studio__ (from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)):  
    The index is reset on the local node the browser is opened on, even if it is Not the preferred node.  

* If index is [disabled](../../../../client-api/operations/maintenance/indexes/disable-index) or [paused](../../../../client-api/operations/maintenance/indexes/stop-index) 
  then resetting the index will put it back to the __normal__ running state  
  on the local node where action was performed.

* In this page:
    * [Reset index](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---single-index)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/set-index-priority#syntax)

{NOTE/}

---

{PANEL: Reset index}

{CODE:nodejs reset@ClientApi\Operations\Maintenance\Indexes\resetIndex.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\resetIndex.js /}

| Parameters | | |
| - | - | - |
| **indexName** | string | Name of an index to reset |

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
