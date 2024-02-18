# Disable Index

 ---

{NOTE: }

* Use the `DisableIndexOperation` store operation to **disable a specific index**.  
* In rare occasions, an index can also be disabled manually, through the file system.  

* In this page:
    * [`DisableIndexOperation`](../../../../client-api/operations/maintenance/indexes/disable-index#disableindexoperation)
        * [Overview](../../../../client-api/operations/maintenance/indexes/disable-index#overview)
        * [Disable index - single node](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index---single-node)
        * [Disable index - cluster wide](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index---cluster-wide)
        * [Syntax](../../../../client-api/operations/maintenance/indexes/disable-index#syntax)
    * [Disabling an Index Manually](../../../../client-api/operations/maintenance/indexes/disable-index#disabling-an-index-manually)

{NOTE/}

---

{PANEL: `DisableIndexOperation`}

## Overview

{NOTE: }

__On which node the index is disabled__:  

* The index can be disabled either:  
    * On a single node, or  
    * Cluster wide - on all database-group nodes.  

* When disabling the index from the __client__ on a single node:  
  The index will be disabled on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.  

* When disabling an index from the __Studio__ (from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)),  
  The index will be disabled on the local node the browser is opened on, even if it is Not the preferred node.  


{NOTE/}

{NOTE: }

__When index is disabled__:  
 
* No indexing will be done by a disabled index on the node where index is disabled.  
  However, new data will be indexed by the index on other database-group nodes where it is not disabled.

* You can still query the index,  
  but results may be stale when querying a node on which the index was disabled.  

* Disabling an index is a __persistent operation__:  
  * The index will remain disabled even after restarting the server or after [disabling/enabling](../../../../client-api/operations/server-wide/toggle-databases-state) the database.  
  * To only pause the index and resume after a restart see: [pause index operation](../../../../client-api/operations/maintenance/indexes/stop-index).  

{NOTE/}

{NOTE: }

__How to enable the index__:  

* To enable the index from the client - see [enable index operation](../../../../client-api/operations/maintenance/indexes/enable-index).  

* To enable the index from the Studio - go to the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions).  

* [Resetting](../../../../client-api/operations/maintenance/indexes/reset-index) a disabled index will enable the index back  
  on the local node where the reset action was performed.

* Modifying the index definition will also enable back the normal operation of the index.  

{NOTE/}

## Disable index - single node

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_1@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}
{CODE-TAB:csharp:Async disable_1_async@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}
{CODE-TABS/}

## Disable index - cluster wide

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_2@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}
{CODE-TAB:csharp:Async disable_2_async@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}
{CODE-TABS/}

## Syntax

{CODE:csharp syntax@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | string | Name of index to disable |
| **clusterWide** | bool | `true` - Disable index on all database-group nodes<br>`false` - Disable index only on a single node (the preferred node) |

{PANEL/}

{PANEL: Disabling an Index Manually}

Special circumstances may rarely require that an index would be disabled manually, 
through the file system.  
For example, a faulty index may be loaded before 
[DisableIndexOperation](../../../../client-api/operations/maintenance/indexes/disable-index#disableindexoperation) 
gets a chance to disable it. Disabling the index through the file system can assure 
that its loading would be prevented.  

To disable an index manually, keep a file named `disable.marker` in the index path:  
{CODE:csharp disable-index-via-file-system@ClientApi\Operations\Maintenance\Indexes\DisableIndex.cs /}

{NOTE: }
To enable an index disabled this way, delete `disable.marker` from the index path.  
{NOTE/}

{PANEL/}


## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
