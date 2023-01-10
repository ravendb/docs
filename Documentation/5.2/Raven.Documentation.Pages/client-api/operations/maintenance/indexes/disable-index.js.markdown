# Disable Index Operation

 ---

{NOTE: }

* Use `DisableIndexOperation` to disable a specific index.  

* The index can be disabled either:  
  * On a single node, or  
  * Cluster wide - on all database-group nodes.  

* __When index is disabled__:  
  * No indexing will take place, new data will not be indexed.  
  * You can still query the index,  
    but results may be stale when querying a node on which the index was disabled.  

* Disabling an index is a __persistent operation__:  
  * The index will remain disabled even after restarting the server or after disabling/enabling the database.  
  * To enable back the index use [enable index operation](../../../../client-api/operations/maintenance/indexes/enable-index).  
  * To only pause the index and resume after a restart go to: [pause index operation](../../../../client-api/operations/maintenance/indexes/stop-index).   

* Disabling/enabling an index can also be done from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) in the Studio. 

* In this page:
    * [Disable index - single node](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index---single-node)
    * [Disable index - cluster wide](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index---cluster-wide)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/disable-index#syntax)

{NOTE/}

---

{PANEL: Disable index - single node}

* With this option, the index will be disabled on the [preferred node](../../../../client-api/configuration/load-balance-and-failover#preferred-node) only.  
  The preferred node is simply the first node in the [database group topology](../../../../studio/database/settings/manage-database-group).

* Note: When disabling an index from the [Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions),  
  the index will be disabled on the local node the browser is opened on, even if it is Not the preferred node.

{CODE:nodejs disable_1@ClientApi\Operations\Maintenance\Indexes\disableIndex.js /}
{PANEL/}

{PANEL: Disable index - cluster wide}

{CODE:nodejs disable_2@ClientApi\Operations\Maintenance\Indexes\disableIndex.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\disableIndex.js /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | string | Name of index to disable |
| **clusterWide** | boolean | `true` - Disable index on all database-group nodes<br>`false` - Disable index only on a single node (the preferred node) |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
