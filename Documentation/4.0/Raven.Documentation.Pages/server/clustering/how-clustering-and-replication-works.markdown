# Clustering : How Clustering and Replication Works

In RavenDB 4.x clustering and replication are two parts of one feature - this means setting up replication without having a cluster is not possible.

{PANEL:The Big Picture}

[RavenDB Cluster](../../glossary/ravendb-cluster) is a two or more RavenDB Instances which are called [cluster nodes](../../glossary/cluster-node). The cluster state is managed through [Raft algorithm](../../glossary/raft-algorithm) commands.  
There are two kinds of operations which are possible to use on a cluster.  

* Node level operation - an operation that happens locally, on a [cluster node](../../glossary/cluster-node). This includes:  
  * CRUD and query operations  
  * Local administration operations like starting/stopping indexes, client certificate management and runnig maintenance operations on JS Console  
*  Cluster level operation - an operation that is guaranteed to run on majority of cluster nodes.  
   Once executed, it will run through [Raft](../../glossary/raft-algorithm), and the Raft algorithm will be responsible to propagate [Raft commands](../../glossary/raft-command) to the rest of the nodes.  
   If there is no issues in the cluster, the [command](../../glossary/raft-command) will be propagated to all nodes, but if there are split-brain issues or some nodes are down, the command will be executed only if it propagates successfully on the majority of the nodes.  

{INFO:Raft Algorithm}
Raft is a "[distributed consensus](https://en.wikipedia.org/wiki/Consensus_(computer_science)) algorithm".  
This means that [Raft](../../glossary/raft-algorithm) is designed to allow the cluster to agree over the order of events that happen on different nodes.  
Those events are called [Raft commands](../../glossary/raft-command).  
{INFO/}

{INFO:How Raft commands work?}

The Raft commands are entries in the Raft state machine, which is local to each [cluster node](../../glossary/cluster-node).  

* When a cluster-level operation is sent from the client, the following sequence of events occurs:  
  * Client sends a command to the cluster node.  
  * If the node that the client sent command to is not a leader, it redirects the command to a leader  
  * The leader appends the command to leader log and propagates the command to other nodes  
  * If the leader receives acknowledge from majority of nodes, the command is actually executed.  
  * If the command is executed at the leader, it is comitted to the leader log, and notification is sent to other nodes. Once the nod receives the notification, it executes the command as well.  
  * If a non-leader node executes the command, it is added to the node log as well.  

{INFO/}

{PANEL/}

{PANEL:What happens when we create a cluster?}

The starting state is two or more RavenDB instances.  

### Server-side
Once the cluster is created (via Stdio, code or script), the node on which we executed the "join" command, becomes a cluster leader. After the cluster command is propagated successfully, the nodes will continue with regular Raft protocol.  
Each time a node is added or deleted, a node which was the origin of the change (on which we did the change), would propagate the changes on all cluster nodes. Each topology change event has an etag, which increments after each change to topology.  

### Client-side
Once a client is initialized, it will query the server for current topology. The response will initialize failover functionality with the list of fallback nodes.  
If a topology is changed after the client was initialized, the client would have old topology etag, and this would make the client fetch the updated topology from the server.  

{PANEL/}

{PANEL:Databases}

The idea is that the cluster will have databases with specified [replication factor](../../glossary/replication-factor) spread out over multiple [cluster nodes](../../glossary/cluster-node).  
If a [cluster node](cluster-node) that contains one of replicas goes offline or is [not reachable](https://en.wikipedia.org/wiki/Split-brain_(computing)), the cluster will relocate the replica to another node (if there is one available), and maintain it there.  
A group of nodes that contain the database is called [database group](../../glossary/database-group).  

{INFO:For Example}
Let us assume a five node cluster, with servers A,B,C,D,E.  
Then, we create a database with replication factor of 3.  

The newly created database will be distributed either manually or automatically to three of the cluster nodes. Let's assume it is distributed to B, C and E.  
If node C is offline or is not reachable, the cluster will relocate the database to any available node.  
{INFO/}

### Replication
Each [database group](../../glossary/database-group) has a master-master replication between the replicas.  

#### Failover
When initialized, the client gets a list of nodes that are in each [database group](../../glossary/database-group), so when one of the nods is down (or there are network issues), the client will automatically and transparently try contacting other nodes.  
How the failover nodes are selected, depends on the configuration of *read balance behavior*, which can be configured either in the [studio](../../studio/server/client-configuration) or in the client.  

#### Load-balance

* The load-balance behavior can be of three types:  
  * None - no load balance behavior at all  
  * Round Robin - Each request from the client will address another node  
  * Fastest Node - Each request will go to the fastest node. The fastest node will be recalculated.  

{PANEL/}

### What happens if there are issues?
There are two types of cluster issues which can happen:

  * [Split brain](https://en.wikipedia.org/wiki/Split-brain_(computing)) - 
  any network issue which causes one or more nodes to be inacessible. 
  In this case, each group of nodes that still can communicate with each other, 
  would form a new cluster, each with its own leader. 
  Once the split is "healed", and all nodes can communicate again, 
  one of the leaders will step down, and the cluster will return to normal. 
  Such scenario may generate document conflicts.
  * Too frequent leader elections - If a client sends a cluster-level operation to a 
  non-leader node, it will redirect the operation to the leader. 
  If in the meantime an election was held and the leader has changed, 
  the former leader will throw `NoLeaderException` in order to prevent infinite loop.
