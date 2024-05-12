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

* **From the Client API**:  
  Use `EnableIndexOperation` to enable the index from the Client API.  
  The index can be enabled either:
    * On a single node, or
    * Cluster wide - on all database-group nodes.

* **From Studio**:  
  To enable the index from the Studio go to the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions).

* **Reset index**:  
  [Resetting](../../../../client-api/operations/maintenance/indexes/reset-index) a disabled index will enable the index back on the local node where the reset action was performed.

* **Modify index definition**:  
  Modifying the index definition will also enable back the normal operation of the index.

* An index that was disabled via the file system can also be enabled by either option from above  
  after removing the [disable.marker](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index-manually-via-the-file-system) file.
  Learn more in [Disable index manually via the file system](../../../../client-api/operations/maintenance/indexes/disable-index#disable-index-manually-via-the-file-system).

{PANEL/}

{PANEL: Enable index from the Client API}

{NOTE: }

<a id="enable-index---single-node" /> **Enable index - single node**:  

* With this option, the index will be enabled on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only.  
  The preferred node is simply the first node in the [database group topology](../../../../studio/database/settings/manage-database-group).

* Note: When enabling an index from the [Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions),  
  the index will be enabled on the local node the browser is opened on, even if it is Not the preferred node.

{CODE:nodejs enable_1@client-api\operations\maintenance\indexes\enableIndex.js /}

{NOTE/}
{NOTE: }

<a id="enable-index---cluster-wide" /> **Enable index - cluster wide**:  

{CODE:nodejs enable_2@client-api\operations\maintenance\indexes\enableIndex.js /}

{NOTE/}
{NOTE: }

<a id="syntax" /> **Syntax**: 

{CODE:nodejs syntax@client-api\operations\maintenance\indexes\enableIndex.js /}

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

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
