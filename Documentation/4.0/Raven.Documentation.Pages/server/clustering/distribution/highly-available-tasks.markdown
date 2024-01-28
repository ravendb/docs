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
  * [Constraints](../../../server/clustering/distribution/highly-available-tasks#constraints)  
  * [Responsible Node](../../../server/clustering/distribution/highly-available-tasks#responsible-node)  
  * [Tasks Relocation](../../../server/clustering/distribution/highly-available-tasks#tasks-relocation)  
{NOTE/}

---

{PANEL: Constraints}

1. Task is defined per [Database Group](../../../server/clustering/distribution/distributed-database).  

2. Task is executed by a single `Database Node` only.  
   With Backup Task being an exception in case of a cluster partition, see [Backup Task - When Cluster or Node are Down](../../../studio/database/tasks/ongoing-tasks/backup-task#backup-task---when-cluster-or-node-are-down).  

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

{NOTE: Additional Reading}
Learn more [here](../../../server/clustering/distribution/distributed-database#database-topology) 
about database nodes' relations and states. 
{NOTE/}

{PANEL/}

{PANEL: Tasks Relocation}

Upon a `Database Topology` change, _all_ existing tasks will be re-evaluated and 
re-distributed among the functional nodes.   

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
