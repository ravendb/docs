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
  
 * If your license **provides** highly available tasks, the responsibilities of a failed cluster node will 
   be assigned automatically and immediately to another, available, node.  
   Supported tasks include Reads, Writes, and the ongoing tasks SQL and Raven ETL, Backup, Data subscription, 
   and External replication.  
   If, for example, the node responsible for a Raven ETL task fails, the cluster observer will assign 
   an available node with the responsibility for the task and ETL transfers will automatically resume 
   from the failure point on.  
 * If your license does **not** provide highly available tasks, the tasks will resume their activity when 
   the original node returns online.  
   If the fallen node does not return online within a given time (set by 
   [cluster.timebeforeaddingreplicainsec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec), 
   the cluster observer will attempt to select an available node to replace it in the database group 
   and redistribute its tasks among available nodes.  
 * Scenarios [below](../../../server/clustering/distribution/highly-available-tasks#tasks-relocation) 
   demonstrate the behavior of a system that **is** licensed for highly available tasks.  

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

It is sometimes preferable to **prevent** the failover of tasks to different responsible nodes.  
An example for such a case is a heavy duty backup task, that better be left for the continuous care 
of its original node than reassigned during a backup operation.  
Another example is an ETL task that transfers 
[artificial documents](../../../studio/database/indexes/create-map-reduce-index#saving-map-reduce-results-in-a-collection-(artificial-documents)). 
In this case a reassigned task might skip some of the artificial documents that were created on 
the original node.  

The failover of a task to another responsible node can be prevented by **pinning the task** to 
its original node.  

* A pinned task will be handled only by the node it is pinned to as long as this node is a database 
  group member.  
* If the node the task is pinned to fails, the task will **not** be executed until the node is back online.  
  When the node awakes, the task will be resumed from the failure point on.  
* If a node remains offline for the period set by 
  [cluster.timebeforeaddingreplicainsec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec), 
  the cluster observer will attempt to select an available node to replace it in the database group 
  and redistribute the fallen node's tasks, including pinned ones, among database group members.  

---

A task can be pinned to a selected node via Studio or using code.  

####Pinning via Studio

![Pinning an ETL Task Using Studio](images/pinning-etl-task.png "Pinning an ETL Task Using Studio")

####Pinning using code
To pin a task to the node that runs it, set the task's `PinToMentorNode` configuration 
option to `true`.  
In the following example, a RavenDB ETL task is pinned.  

{CODE add_raven_etl_task@ClientApi\Operations\HighAvailabilityTasks.cs /}

{PANEL/}

