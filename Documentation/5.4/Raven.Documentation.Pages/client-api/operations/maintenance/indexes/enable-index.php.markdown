# Enable Index Operation

 ---

{NOTE: }

* When an index is enabled, indexing will take place, and new data will be indexed.

* To learn how to disable an index, see [disable index](../../../../client-api/operations/maintenance/indexes/disable-index).

* In this page:
    * [How to enable an index](../../../../client-api/operations/maintenance/indexes/enable-index#how-to-enable-an-index)
    * [Enable index from the Client API](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index-from-the-client-api)
      * [Enable index - single node](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index---single-node)
      * [Enable index - cluster wide](../../../../client-api/operations/maintenance/indexes/enable-index#enable-index---cluster-wide)
      * [Syntax](../../../../client-api/operations/maintenance/indexes/enable-index#syntax)

{NOTE/}

---

{PANEL: How to enable an index}

* **From the Client API**:  
  Use `EnableIndexOperation` to enable the index from the Client API.  
  The index can be enabled:  
    * On a single node.  
    * Cluster wide, on all database-group nodes.  

* **From Studio**:  
  To enable the index from Studio go to the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions).

* **Reset index**:  
  [Resetting](../../../../client-api/operations/maintenance/indexes/reset-index) a disabled index will re-enable the index 
  locally, on the node that the reset operation was performed on.

* **Modify index definition**:  
  Modifying the index definition will also re-enable the normal operation of the index.

* The above methods can also be used to enable an index that was 
  [disabled via the file system](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index-manually-via-the-file-system), 
  after removing the `disable.marker` file.  
  
{PANEL/}

{PANEL: Enable index from the Client API}

#### Enable index - single node:  

* With this option, the index will be enabled on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only.  
  The preferred node is simply the first node in the [database group topology](../../../../studio/database/settings/manage-database-group).

* Note: When enabling an index from [Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions),  
  the index will be enabled on the local node the browser is opened on, even if it is Not the preferred node.

{CODE:php enable_1@ClientApi\Operations\Maintenance\Indexes\EnableIndex.php /}

---

#### Enable index - cluster wide:  

{CODE:php enable_2@ClientApi\Operations\Maintenance\Indexes\EnableIndex.php /}

---

#### Syntax: 

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\EnableIndex.php /}

| Parameters | Type | Description |
| - | - | - |
| **$indexName** | `?string` | Name of index to enable |
| **clusterWide** | `bool` | `true` - Enable index on all database-group nodes<br>`false` - Enable index only on a single node (the preferred node) |

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
