# Adding a Database Node

---

{NOTE: }

* When creating a database, you specify the number of replicas for that database.  
  This determines the number of database instances in the database-group.

* __The number of replicas can be dynamically increased__ even after the database is up and running,  
  by adding more nodes to the database-group.  

* The nodes added must already exist in the [cluster topology](../../../server/clustering/rachis/cluster-topology).

* Once the new node is added to the group,  
  the cluster assigns a mentor node (from the existing database-group nodes) to update the new node.

* In this page:
    * [Add database node - random](../../../client-api/operations/server-wide/add-database-node#add-database-node---random)
    * [Add database node - specific](../../../client-api/operations/server-wide/add-database-node#add-database-node---specific)
    * [Syntax](../../../client-api/operations/server-wide/add-database-node#syntax)  
{NOTE/}

---

{PANEL: Add database node - random}

* Use `AddDatabaseNodeOperation` to add another database-instance to the database-group.
* The node added will be a random node from the existing cluster nodes.   

{CODE:nodejs add_1@ClientApi\Operations\Server\addDatabaseNode.js /}

{PANEL/}

{PANEL: Add database node - specific}

* You can specify the node tag to add.  
* This node must already exist in the cluster topology.

{CODE:nodejs add_2@ClientApi\Operations\Server\addDatabaseNode.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Server\addDatabaseNode.js /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of a database for which to add the node. |
| **nodeTag** | string | Tag of node to add.<br>Default: If not passed then a random node from the existing cluster topology will be added. |

| Return Value<br>`DatabasePutResult` | | |
| - | - | - |
| raftCommandIndex | number | Index of raft command that was executed |
| name | string | Database name |
| topology | `DatabaseTopology` | The database topology |
| nodesAddedTo | string[] | New nodes added to the cluster topology.<br>Will be 0 for this operation. |

{PANEL/}





