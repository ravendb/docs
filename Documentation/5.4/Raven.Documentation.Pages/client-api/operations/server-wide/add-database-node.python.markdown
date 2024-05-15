# Adding a Database Node

---

{NOTE: }

* When creating a database, you can specify the number of replicas for that database.  
  This determines the number of database instances in the database-group.

* **The number of replicas can be dynamically increased** even after the database is up and running,  
  by adding more nodes to the database-group.  

* The nodes added must already exist in the [cluster topology](../../../server/clustering/rachis/cluster-topology).

* Once a new node is added to the database-group,  
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

{CODE:python add_1@ClientApi\Operations\Server\AddDatabaseNode.py /}

{PANEL/}

{PANEL: Add database node - specific}

* You can specify the node tag to add.  
* This node must already exist in the cluster topology.

{CODE:python add_2@ClientApi\Operations\Server\AddDatabaseNode.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax@ClientApi\Operations\Server\AddDatabaseNode.py /}

| Parameters | Type | Description |
| - | - | - |
| **database_name** | `str` | Name of a database for which to add the node. |
| **node_tag** | `str` | Tag of node to add.<br>Default: a random node from the existing cluster topology will be added. |

| Object returned by Send operation:<br>`DatabasePutResult` | Type | Description |
| - | - | - |
| raft_command_index | `int` | Index of the raft command that was executed |
| name | `str` | Database name |
| topology | `DatabaseTopology` | The database topology |
| nodes_added_to | `list<str>` | New nodes added to the cluster topology.<br>Will be 0 for this operation. |

{PANEL/}

