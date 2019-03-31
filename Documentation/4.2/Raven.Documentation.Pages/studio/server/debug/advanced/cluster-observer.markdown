# Cluster Observer
---

{NOTE: }

* The Cluster Observer is a supervising component in the cluster.  
  It monitors the nodes health and adjusts the nodes state in the Database Group accordingly.  

* For each database, it sets the nodes state within the Database Group, thus determining the Database Group Topology.  

* The Observer is always running on the `Leader` node, it requires a consensus to operate.  
  If there is no Leader, no Database Group Topology decisions can be made.  

* In this page:  
  * [The Observer Log](cluster-observer#the-observer-log)  
  * [Observer Log Data](cluster-observer#observer-log-data)  
  * [States Flow](cluster-observer#states-flow)  
{NOTE/}

{NOTE: }
**Note:**  

* Don't confuse the Database Group Topology with the Cluster Topology  

* For more info about node types and states in the **Cluster** see [Cluster View](../../../studio/server/cluster/cluster-view)  
  For more info about node types and states in the **Database Group** see [Database Group](../../database/settings/manage-database-group)  
{NOTE/}

---

{PANEL: The Observer Log}

![Figure 1. The Observer Log](images/cluster-observer-1.png "The Observer Log")

{PANEL/}

{PANEL: Observer Log Data}

![Figure 2. Log Data](images/cluster-observer-2.png "Log Data")


### 1. Refreshing Data

{NOTE: }
Click `Refresh` to get latest actions performed by the Observer

**When a new Leader is elected:**  

* `Term` value will increase  
* The Observer will move and run on the new Leader  
* All previous log data will not be kept - clicking `Refresh` will start a new log  
{NOTE/}

### 2. Suspending the Cluster Observer

{NOTE: }
**Be careful with this option**  

* Use this only when you don't want the Observer to make any changes to the Database Group Topology  
  i.e. when taking nodes down for maintenance  
{NOTE/}

### 3. States Flow

{NOTE: }
####**When a new node is added to a Database Group:**  

1. The Observer will add the node as `Promotable`.  

2. Once the node has caught up with the state of the Database Group, fully updated with the database data and finished indexing the last data sent to it, 
   the Observer will promote it to a full `Member`.  

   For a detailed explanation of a Member actions and tasks within a database group see [Database Group](../../../studio/database/settings/manage-database-group)  
{NOTE/}

{NOTE: }
####**When the Observer notices that a node is down ,or unresponsive, it will:**  

1. Set the node to a `Rehab` state.  

2. If 'Allow Dynamic Database Distribution' is set (see [Create Database](../../../studio/server/databases/create-new-database/general-flow)),  
   then after a pre-configured time period, the Observer will add another node from the cluster to the Database Group in order to keep the replication factor.  
   The replacement node will be set to `Promotable` and will start catching up data from the other nodes in the group.  

3. The Observer will Update the Database Group Topology with these changes so that each node in the Database Group will re-calculate his work assignments.  
<br/>

    ![Figure 3. Node D down  - Node B replacing](images/cluster-observer-3.png "Node D is down - Node B is replacing")

{NOTE/}

{NOTE: }
####**When the `Rehab` node is responsive again:**  

**1.** If a replacement node was added when the node was down: (Dynamic Database distribution was set)  

   * If the replacing node is fully updated (in a `Member` state)  
     then the Observer will delete the node that came back from the Database Group Topology.  

   * If the replacing node is still Not fully updated (still in `Promotable` state)  
     then the Observer will set the `Rehab` node to `Promotable` so that it starts updating.  
     At this point the replication factor is greater than was set for the relevant database,  
     so the Observer will go ahead and delete the extra node, 
     either the replacement one or the Rehab one - whoever is the last to be fully updated.  

   * Either way, deletion of a node will _not_ occur before the Observer verifies that all 
     data to be deleted from the node was replicated to the Database Group members.  
<br/>

     ![Figure 4. Deleting Node D](images/cluster-observer-4.png "Node B is fully updated - Deleting Node D")

**2.** If no replacement node was added: (Dynamic Database Distribution was _not_ set)  

   * The `Rehab` node will start catching up data from the other members in the relevant Databases Groups.  
     (Note: the node can be part of multiple Databases Groups...)  

   * Once it is fully updated, the Observer will directly promote it to a full `Member` in the Database Group Topology.  
{NOTE/}
{PANEL/}

## Related articles 

- [Cluster Observer Details](../../../server/clustering/distribution/cluster-observer)  
