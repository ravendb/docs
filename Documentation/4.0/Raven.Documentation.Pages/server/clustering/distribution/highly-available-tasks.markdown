# Highly Available Tasks

RavenDB Task is one of the following:

* An [Ongoing Tasks](../../ongoing-tasks/general-info). 
* Updating a `Rehab` or `Promotable` [Database Node](distributed-database#database-topology).

There is no single coordinator handing out tasks to a specific node. Rather, every node decides for himself if its his responsibility to execute the task.  
Each node will reevaluate his responsibilities with every change made to the [Database Record](../../../client-api/operations/server-wide/create-database) such as defining new `Index`, configuring or modifying an `Outgoing Task`, `Database Topology` change, etc.

{NOTE: Constraints}

1. Task is defined per [Database Group](../../../server/clustering/distribution/distributed-database).
2. Task is executed by a single `Database Node` only.
3. A `Database Node` can be assigned with many tasks.
4. The node must be in a [Member](../../../server/clustering/distribution/distributed-database#database-topology) state in the `Database Group` in order to perform a task.
5. Cluster must be in a [functional](../../../server/clustering/rachis/what-is-rachis#normal-operations) state.

**Note**: There are some exceptions to the above constraints in the behavior of the [Backup](../../../studio/database/tasks/ongoing-tasks/backup-task#backup-task---when-cluster-or-node-are-down) task.

{NOTE/} 

## Responsible Node

`Responsible Node` is the node that is responsible to perform a specific task.  

Each node checks whether he is the `Responsible Node` for the task by executing a local function that is based on the current [Database Topology](../../../server/clustering/distribution/distributed-database#database-topology) and the unique hash value of the task.
Since the `Database Topology` is _eventually consistent_ across the cluster, there will be an _eventually consistent_ `Responsible Node`, which will answer the above constraints.

### Mentor Node
When node has the task to update another node. The updating node is called `Mentor Node`.

## Tasks Relocation

* Upon `Database Topology` change, _all_ existing tasks will be reevaluated and redistributed among the functional nodes.   
* Upon modification of an `Outgoing Task` the unique task hash value is changed so the `Responsible Node` for that task is reevaluated.

For example:  
Let's assume that we have a 5 nodes cluster [A, B, C, D, E] with a database on [A, B, E] and a task on node B.  
Node B has network issues and is separated from the cluster. So on one side we have nodes [A, C, D, E] and [B] on the other.
The [Cluster Observer](../../../server/clustering/distribution/cluster-observer) will note that he can't reach node B and issue [Raft Command](../../../server/clustering/rachis/consensus-operations) to move node B to `Rehab` state. 
Once this change has propagated, it will trigger a reassessment of _all_ tasks in all reachable nodes. In our example the task will move to either A or E.   
In the meanwhile, node B which has no communication with the [Cluster Leader](../../../server/clustering/rachis/cluster-topology), moves itself to be a `Candidate` and removes all tasks. 





