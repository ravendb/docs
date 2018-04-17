# Consensus Operations

Any operation that is made at the cluster level and needs a consensus is named Raft Command or Raft Log.
This operation guaranteed to run on majority of cluster nodes. Once executed, it will run through Rachis and will propagate to the rest of the nodes.  
If there are no issues in the cluster, the operation will be propagated to all nodes, but if there are split-brain issues or some nodes are down, the command will be executed only if it propagates successfully to the majority of the nodes.

Since getting a consensus is an expensive operation it is limited only to the following:

* Creating / Deleting a database.
* Adding / Removing node to / from a database group.
* Changing database settings.
* Creating / Deleting Indexes (both static an auto).
* Configuring and running ongoing tasks: [Backups](../../ongoing-tasks/backups/basic), [External Replication](../../ongoing-tasks/external-replication/basic), [ETL](../../ongoing-tasks/etl/basics), [Subscriptions](../../ongoing-tasks/subscriptions/basic). 

{INFO: Check the Client-API}
In case that the majority of the nodes is down and the cluster not functional, every ongoing task has its own behavior.
So, check out the [Ongoing Tasks](../../ongoing-tasks/general-info) documentation for how to address each of those commands.
{INFO/} 

Does not require Consensus
---
It is important to understand that any document related operation **does not** require a consensus.
Any CRUD operation or query on an _existing_ index is executed against a specific node, regardless if there is a functional cluster. 
RavenDB keeps the documents synchronized by [Replication](../replication/how-replication-works), so documents are available for read, write and query even if there is no majority of nodes in the cluster.  

Raft Index
---
Every Raft command is assigned with a Raft Index, which corresponds to the raft operation sequence. For example an operation with the index of 7 is executed only after _all_ the operations with the smaller indexes has been executed.  
It is possible for any client with [Valid User] privileges to wait for a certain index to be executed on a specific cluster node by issuing the following  `GET` request:  
> http://`node-url`/rachis/waitfor?index=`index`

The request will return after a successful apply of the corresponding raft command or it will return `timeout` after `Cluster.OperationTimeoutInSec` has passed (default: 15 seconds). 

## Sending Raft Command
In order for the client to send a raft command he must have at least the [Operator](../../security/authorization/security-clearance-and-permissions#operator) privileges.  
When raft command is sent, the following sequence of events occurs: 

  1. Client sends the command to a cluster node.  
  2. If the node that the client sends the command to, is not the leader, it redirects the command to the leader.  
  3. The leader appends the command to its log and propagates the command to other nodes.  
  4. If the leader receives acknowledgment from majority of nodes, the command is actually executed.  
  5. If the command is executed at the leader, it is committed to the leader log, and notification is sent to other nodes. Once the node receives the notification, it executes the command as well.  
  6. If a non-leader node executes the command, it is added to the node log as well.  
  7. The client receives the Raft Index of that command, so he can wait upon it. 
