# Cluster Topology
---

{NOTE: }

* The cluster topology is defined by the type and state of each node in the cluster and the relation between them.  

* Clusters are typically composed of any odd number of nodes equal or greater than 3, see [Clusters Best Practice](../../../server/clustering/cluster-best-practice-and-configuration).  

* In this page:
    * [Fetching the topology](../../../server/clustering/rachis/cluster-topology#fetching-the-topology)
    * [Modifying the topology](../../../server/clustering/rachis/cluster-topology#modifying-the-topology)
    * [Nodes states and types](../../../server/clustering/rachis/cluster-topology#nodes-states-and-types)
    * [Changing node type](../../../server/clustering/rachis/cluster-topology#changing-node-type)  

{NOTE/}

---

{PANEL: Fetching the topology}

Any client with [Valid User](../../security/authorization/security-clearance-and-permissions#user) privileges can fetch the topology from the server
by sending the following REST API call:

| Action         | Method   | URL               |
|----------------|----------|-------------------|
| Fetch Topology | `GET`    | /cluster/topology |

{PANEL/}

{PANEL: Modifying the topology}

With [Cluster Admin](../../../server/security/authorization/security-clearance-and-permissions#cluster-admin) privileges, 
the client can modify the cluster through the [Studio](../../../studio/cluster/cluster-view#cluster-view-operations),  
or by using the following REST API calls:  

| Action      | Method   | URL                                      |
|-------------|----------|------------------------------------------|
| Add Node    | `PUT`    | /admin/cluster/node?url=`<node-url>`     |
| Remove Node | `DELETE` | /admin/cluster/node?nodeTag=`<node-tag>` |

Optional parameters (for the Add Node endpoint):

| Name             | Value type   | Description                                                                                                  |
|------------------|--------------|--------------------------------------------------------------------------------------------------------------|
| tag              | `string`     | The node tag. 1-4 uppercase unicode letters.<br>Default: 'A' - 'Z' assigned by order of addition.            |
| watcher          | `bool`       | Add the node as a [Watcher](../../../server/clustering/rachis/cluster-topology#watcher).<br>Default: `false` |
| maxUtilizedCores | `uint`       | Maximum number of cores that can be assigned to this node.<br>Default: number of processors.                 |

See the [Cluster API page](../../../server/clustering/cluster-api) for usage examples.

{PANEL/}

{PANEL: Nodes states and types}

In Rachis every cluster node has a [state](../../../server/clustering/rachis/cluster-topology#state) and a [type](../../../server/clustering/rachis/cluster-topology#type).  

{NOTE: }

##### Type
---

The type defines the node's ability to **vote** on accepting a new raft command in the cluster,  
and **elect** a new leader when needed.  

| Node type      | Description                                                                                                                                                                                |
|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Member**     | A fully functional voting node in the cluster.                                                                                                                                             |
| **Promotable** | A non-voting node. <br/>This is an intermediate stage until node is promoted by the Leader to a Member.                                                                                    |
| **Watcher**    | A non-voting node that is still fully managed by the cluster. <br/>Can be assigned with databases and tasks. See more [below](../../../server/clustering/rachis/cluster-topology#watcher). |

{NOTE/}

{NOTE: }

##### State
---

The Rachis state defines the **current role** of the node in the cluster.

| Rachis state     | Description                                                                                                                                                                                                                                                                                                                      |
|------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Passive**      | Not a part of _any_ cluster.<br/><br/>Every newly created RavenDB server starts at the initial **Passive** state.<br/>A passive node can become a Leader of a single-node-cluster,<br/>or it can be added to an already existing cluster.<br/><br/>A node that is removed from the topology moves back to the **Passive** state. |
| **Candidate**    | Has no Leader and tries to get elected for leadership.                                                                                                                                                                                                                                                                           |
| **Follower**     | Has a Leader and operates normally.                                                                                                                                                                                                                                                                                              |
| **Leader-Elect** | Just elected for leadership, but will take office and become the leader only after the dummy `noop` raft command was successfully applied in the cluster.                                                                                                                                                                        |
| **Leader**       | Leader of the cluster. See more [below](../../../server/clustering/rachis/cluster-topology#leader).                                                                                                                                                                                                                              |

{NOTE/}

{NOTE: }

![Figure 3. States Transitions](images/cluster-states.png "Cluster Nodes States Flow")

**1.** New server/node will start as **Passive**, meaning it is _not_ part of any cluster yet.  
**2.** When a node is added to a cluster, it immediately becomes the **Leader** if it is the only node in the cluster.  
**3.** When a node is added to a cluster and a Leader already exists, it will become **Promotable** (on its way to either becoming a Member or a Watcher - depending on what was specified when created.)  
**4.** Node will become a **Member** of the cluster if not specified otherwise.  
**5.** Node will become a **Watcher** if specified when adding the node.  
**6.** Member can be `Demoted` to a Watcher.  
**7.** Watcher can be `Promoted` to a Member. It will first become Promotable and a Member thereafter.  
**8.** Member (a regular Member or a Leader - but not a Watcher) can become a **Candidate** when a voting process for a new Leader takes place.  
**9.** When voting is over and a new Leader is elected, one candidate becomes the Leader and the rest go back to being his Followers.  

{NOTE/}

{NOTE: }

##### Leader
---
 
* The Leader makes sure that decisions are consistent at the cluster level, as long as a majority of the nodes are functioning and can talk to the Leader.  

* For example, the decision to add a database to a node will be either accepted by the entire cluster (eventually) or fail to register altogether.  

* [Raft Commands](../../../server/clustering/rachis/consensus-operations#raft-commands-implementation-details) can't be accepted while there is no Leader or if the Leader is down.  

{NOTE/}

{NOTE: }

##### Watcher
---

Increase your RavenDB cluster by adding Watchers with the advantage of _not_ suffering from large voting majorities and the latencies they can incur, 
as watchers don't take part in majority calculations.  

* A Watcher can be assigned databases and tasks as a regular Member but is not included in the decisions making flow,
  so cluster decisions can be made with a small majority of nodes while the actual size of the cluster can be much higher.  

* Any number of Watchers can be added to handle the workload.  

{NOTE/}

{PANEL/}

{PANEL: Changing Node Type}

Node types can be altered on the fly by using the following REST API calls:

| Action                         | Method  | URL                    | Clearance    |
|--------------------------------|---------|------------------------|--------------|
| Promote Watcher to be a Member | `POST`  | /admin/cluster/promote | ClusterAdmin |
| Demote Member to be a Watcher  | `POST`  | /admin/cluster/demote  | ClusterAdmin |
| Force Elections                | `POST`  | /admin/cluster/reelect | Operator     |

See the [Cluster API page](../../../server/clustering/cluster-api) for usage examples.

{PANEL/}
