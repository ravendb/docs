# Cluster Topology

Every newly created RavenDB server starts at the initial Passive [Rachis State](cluster-topology#state).  
A passive node can become a leader of a single-node-cluster, or it can be added to an already existed cluster.
{PANEL:Changing the Topology}
Any client with [Valid User](../../security/authorization/security-clearance-and-permissions#user) privileges can fetch the topology by sending the `HTTP` request `http://leader-url/cluster/topology` with the `GET` method.  
With [Cluster Operator](../../security/authorization/security-clearance-and-permissions#operator) privileges, the client can modify the cluster using the below REST API or simply through the [Studio](to-do).

| Action | Method | URL |
| - | - | - |
| Add Node | `PUT` | http://`leader-url`/admin/cluster/node?url=`node-url` |
| Remove Node | `DELETE` | http://`leader-url`/admin/cluster/node?url=`node-url` |

Optional parameters:

| Name | Value Type | Description |
| - | - | - |
| watcher | `bool` | The node can be added as a [Watcher](to-do) (default: `false`) |
| assignedCores | `unit` | Number of cores to assign to this node (default: number of processors) |
{PANEL/}

{INFO:Nodes States and Types}
In [Rachis](what-is-rachis) every cluster node has a state and a type.
Type
---
The type defines the node's ability to vote on new raft commands in the cluster

| Node Type | Description |
| - | - |
| **Member** | Fully functional voting member in the cluster |
| **Promotable** | Non-voting, intermediate stage until promoted by the Leader to a Member |
| **Watcher** | Non-voting node that is still fully managed by the cluster. Can be assigned with databases and tasks. |

State
---
The Rachis state defines the current role of the node in the cluster.

| Rachis State | Description |
| - | - |
| **Passive** | Not a part of _any_ cluster. |
| **Candidate** | Has no leader and tries to get elected for leadership. |
| **Follower** | Has a leader and operates normally. |
| **Leader Elect** | Just elected for leadership, but will take office and became the leader only after the dummy `noop` raft command was successfully applied in the cluster. |
| **Leader** | Leader of the cluster. |

![Figure 3. States Transitions](images/cluster-states.png)

**1.** New server/node will start as **Passive**, meaning it is _not_ part of any cluster yet.  
**2.** When a node is added to a cluster, it immediately becomes the **Leader** if it is the only node in the cluster.  
**3.** When a node is added to a cluster and a Leader already exists, it will become **Promotable** (on its way to either becoming a Member or a Watcher - depending on what was specified when created.)  
**4.** Node will become a **Member** of the cluster if not specified otherwise.  
**5.** Node will become a **Watcher** if specified when adding the node.  
**6.** Member can be `Demoted` to a Watcher.  
**7.** Watcher can be `Promoted` to a Member. It will first become Promotable and a Member thereafter. <br>
**8.** Member (a regular Member or a Leader - but not a Watcher) can become a **Candidate** when a voting process for a new Leader takes place. <br>
**9.** Candidate notices an already reigning leader and moves to become his Follower.

Note : If node is removed from topology it moves back to **Passive**.  
{INFO/}

Leader
---
 The Leader makes sure that decisions are consistent at the cluster level, as long as a majority of the nodes are functioning and can talk to one another.
 For example, the decision to add a database to a node will be either accepted by the entire cluster (eventually) or fail to register altogether.
[Raft Commands](raft-commands) can't be accepted while there is no Leader or the Leader is down.  

Watcher
---
Grow your RavenDB cluster by adding Watchers without suffering from large voting majorities and the latencies they can incur,
as these nodes don’t take part in majority calculations and are only there to watch what’s going on in the cluster.  
So cluster decisions can be made with a small majority of nodes while the actual size of the cluster can be much higher.

{PANEL:Change Node Type}
Node types can be altered on the fly by using the following REST API calls:

| Action | Method | URL |
| - | - | - |
| Promote Watcher to be a Member | `POST` | http://`leader-url`/admin/cluster/promote |
| Demote Member to be a Watcher | `POST` | http://`leader-url`/admin/cluster/demote |
| Force Elections | `POST` | http://`leader-url`/admin/cluster/reelect |
{PANEL/}