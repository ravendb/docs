# Enable Index Operation

 ---

{NOTE: }

* When an index is enabled, indexing will take place, and new data will be indexed.

* To learn how to disable an index see article [disable index](../../../../client-api/operations/maintenance/indexes/disable-index).

* In this page:
    * [How to enable an index](../../../../client-api/operations/maintenance/indexes/enable-index#how-to-enable-an-index)
    * [Enable index from the Client API](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index-from-the-client-api)
      * [Enable index - single node](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index---single-node)
      * [Enable index - cluster wide](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index---cluster-wide)
      * [Syntax](../../../../client-api/operations/maintenance/indexes/enable-index#syntax)

{NOTE/}

---

{PANEL: How to enable an index}

* __From the Client API__:  
  Use `EnableIndexOperation` to enable the index from the Client API.  
  The index can be enabled either:  
    * On a single node, or  
    * Cluster wide - on all database-group nodes.  

* __From Studio__:  
  To enable the index from the Studio go to the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions).

* __Reset index__:  
  [Resetting](../../../../client-api/operations/maintenance/indexes/reset-index) a disabled index will enable the index back on the local node where the reset action was performed.

* __Modify index definition__:  
  Modifying the index definition will also enable back the normal operation of the index.

* An index that was disabled via the file system can also be enabled by either option from above  
  after removing the [disable.marker](../../../../todo..) file. 
  Learn more in [Disable index manually via the file system](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index-manually-via-the-file-system).
  
{PANEL/}

{PANEL: Enable index from the Client API}

{NOTE: }

<a id="enable-index---single-node" /> __Enable index - single node__:  

* With this option, the index will be enabled on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only.  
  The preferred node is simply the first node in the [database group topology](../../../../studio/database/settings/manage-database-group).

* Note: When enabling an index from the [Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions),  
  the index will be enabled on the local node the browser is opened on, even if it is Not the preferred node.

{CODE-TABS}
{CODE-TAB:csharp:Sync enable_1@ClientApi\Operations\Maintenance\Indexes\EnableIndex.cs /}
{CODE-TAB:csharp:Async enable_1_async@ClientApi\Operations\Maintenance\Indexes\EnableIndex.cs /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="enable-index---cluster-wide" /> __Enable index - cluster wide__:  

{CODE-TABS}
{CODE-TAB:csharp:Sync enable_2@ClientApi\Operations\Maintenance\Indexes\EnableIndex.cs /}
{CODE-TAB:csharp:Async enable_2_async@ClientApi\Operations\Maintenance\Indexes\EnableIndex.cs /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="syntax" /> __Syntax__: 

{CODE:csharp syntax@ClientApi\Operations\Maintenance\Indexes\EnableIndex.cs /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | string | Name of index to enable |
| **clusterWide** | bool | `true` - Enable index on all database-group nodes<br>`false` - Enable index only on a single node (the preferred node) |

{NOTE/}
{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)
- [Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
