# Highly Available Tasks
---

{NOTE: }

* A **RavenDB Task** can be one of the following:  

  * An [Ongoing Task](../../../studio/database/tasks/ongoing-tasks/general-info)  
  * Updating a `Rehab` or a `Promotable` [Database Node](../../../server/clustering/distribution/distributed-database#database-topology)  

* There is no single coordinator handing out tasks to a specific node.  
  Instaed, each node decides on its own if it is the [Reponsible Node](../../../server/clustering/distribution/highly-available-tasks#responsible-node) of the task.  

* Each node will re-evaluate its responsibilities with every change made to the [Database Record](../../../client-api/operations/server-wide/create-database),  
  such as defining a new _index_, configuring or modifying an _Ongoing Task_, any _Database Topology_ change, etc.  

* In this page:  
  * [License](../../../server/clustering/distribution/highly-available-tasks#license)  
  * [Constraints](../../../server/clustering/distribution/highly-available-tasks#constraints)  
  * [Responsible Node](../../../server/clustering/distribution/highly-available-tasks#responsible-node)  
  * [Tasks Relocation](../../../server/clustering/distribution/highly-available-tasks#tasks-relocation)  
  * [Pinning a Task](../../../server/clustering/distribution/highly-available-tasks#pinning-a-task)  
{NOTE/}

---

{PANEL: License}

Please [check your license](https://ravendb.net/buy) to verify whether the Highly Available Tasks feature 
is activated in your database.  
  
 * If your license **provides** highly available tasks, the responsibilities of a failed node will be assigned 
   automatically and immediately to another, available, cluster node.  
   Reads, Writes, and the ongoing tasks SQL and Raven ETL, Backup, Data subscription, and External replication, 
   are supported.  
   If, for example, the node responsible for Raven ETL tasks fails, the cluster observer will assign an available 
   node with this responsibility and Raven ETL tasks will automatically resume.  
   The new responsible node will be provided with all the changes that occured since the original responsible node 
   failed so tasks are resumed without any data loss.  
 * If your license does **not** provide highly available tasks, the responsibilites of a failed node will be 
   resumed when the node returns online.  
 * Scenarios [below](../../../server/clustering/distribution/highly-available-tasks#tasks-relocation) 
   are meant to demonstrate the behavior of a system licensed for highly available tasks.  

{PANEL/}

{PANEL: Constraints}

1. Task is defined per [Database Group](../../../server/clustering/distribution/distributed-database).  

2. Task is executed by a single `Database Node` only.  
   With Backup Task being an exception in case of a cluster partition, see [Backup Task - When Cluster or Node are Down](../../../studio/database/tasks/backup-task#when-the-cluster-or-node-are-down).  

3. A `Database Node` can be assigned with many tasks.  

4. The node must be in a [Member](../../../server/clustering/distribution/distributed-database#database-topology) state in the `Database Group` in order to perform a task.  

5. Cluster must be in a functional state.  
{PANEL/}

{PANEL: Responsible Node}

* `Responsible Node` is the node that is responsible to perform a specific Ongoing Task.  

* Each node checks whether it is the `Responsible Node` for the task by executing a local function that is based on the  
  _unique hash value_ of the task and the current [Database Topology](../../../server/clustering/distribution/distributed-database#database-topology).  

* Since the `Database Topology` is _eventually consistent_ across the cluster,  
  there will be an **eventually consistent single Responsible Node**, which will answer the above constraints.  

{NOTE: Mentor Node}
The node is called a `Mentor Node` when its task is updating a _Rehab_ or a _Promotable_.  
{NOTE/}
{PANEL/}

{PANEL: Tasks Relocation}

* Upon a `Database Topology` change, _all_ existing tasks will be re-evaluated and 
  re-distributed among the functional nodes.   

* The responsible node for an `Ongoing Task` is also re-evalutated upon a change in the 
  unique hash value of the Ongoing Task.  

{NOTE: }

**For example**:  

Let's assume that we have a 5 nodes cluster [A, B, C, D, E] with a database on [A, B, E] and a task on node B.  

Node B has network issues and is separated from the cluster. 
So nodes [A, C, D, E] are on one side and node [B] is on the other side.  

The [Cluster Observer](../../../server/clustering/distribution/cluster-observer) will note that it can't reach node B 
and issue a [Raft Command](../../../server/clustering/rachis/consensus-operations) in order to move node B to a `Rehab` state.  

Once this change has propagated, it will trigger a re-assessment of _all_ tasks in _all_ reachable nodes.  
In our example the task will move to either A or E.  

In the meanwhile, node B which has no communication with the [Cluster Leader](../../../server/clustering/rachis/cluster-topology),  
moves itself to be a `Candidate` and removes all its tasks.  
{NOTE/}
{PANEL/}

{PANEL: Pinning a Task}

* It is sometimes better to **prevent** the failover of a task to another responsible node.  
* An example for such a case is an ETL task that transferres 
  [Artificial Documents](../../../studio/database/indexes/create-map-reduce-index#saving-map-reduce-results-in-a-collection-(artificial-documents)). 
  If a node that runs such a task would fail and transfer the responsibility for the task to 
  another node, the task will stop transferring the artificial documents.  
* The failover of a task to another responsible node can be prevented by 
  **pinning the task** to its original node.  
   * A task can be pinned to a selected node using Studio or code.  
     ![Pinning an ETL Task Using Studio](images/pinning-etl-task.png "Pinning an ETL Task Using Studio") 
   * A task pinned to a specific node is handled only by this node.  
   * If the node the task is pinned to goes offline, the task will **not** be 
     executed until the node is back online.  
   * If the node the task is pinned to is removed from the database group, the 
     responsibility for The task will be assigned by the cluster to another node.  

{PANEL/}

