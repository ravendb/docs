# Distributed Database
---

{NOTE: }

* In RavenDB, a database can be replicated across multiple nodes, depending on its [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor).  

* A node where a database resides is referred to as a `Database Node`.  
  The group of _Database Nodes_ that assemble the distributed database, is called a `Database Group`.  

* Each Database Node has a **full copy** of the database, it contains **all** the database documents and it indexes them **locally**.  
  This greatly simplifies executing query requests, since there is no need to orchestrate an aggregation of data from various nodes.  

* In this page:  
  * [The Database Record](../../../server/clustering/distribution/distributed-database#the-database-record)  
  * [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor)  
  * [Database Topology](../../../server/clustering/distribution/distributed-database#database-topology)  
  * [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution)  
  * [Sharding](../../../server/clustering/distribution/distributed-database#sharding)  
{NOTE/}

---

{PANEL: The Database Record}

Each database instance keeps its configuration (e.g. Index definitions, Database topology) in a [Database Record](../../../client-api/operations/server-wide/create-database) object.  
Upon database creation, this object is passed through Rachis to all nodes in the cluster.  

After that, each node updates its own `database record` on its own upon any new Raft command received,  
i.e. when an index has changed.  
{PANEL/}

{PANEL: Replication Factor}

When creating a database it is possible to specify the _exact_ nodes for the `Database Group`, or just the number of nodes needed.  
This will implicitly set the `Replication Factor` to the specified amount of nodes.  

It is possible to pass the `Replication Factor` explicitly and let RavenDB choose the nodes on which the database will reside.  

Either way, once the database is created by getting a [Consensus](../../../server/clustering/rachis/consensus-operations),  
the [Cluster Observer](../../../server/clustering/distribution/cluster-observer) begins monitoring the _Database Group_ in order to maintain the `Replication Factor`.  
{PANEL/}

{PANEL: Database Topology}

The `Database Topology` describes the relations inside the `Database Group` between the `Database Nodes`.  
Each `Database Node` can be in one of the following states:  

| State | Description |
| - | - |
| **Member** | Fully updated and functional database node. |
| **Promotable** | A node that has been recently added to the group and is being updated. |
| **Rehab** | A former Member node that is assumed to be _not_ up-to-date due to a partition. |

{NOTE: States Flow}
In general, all nodes in a newly created database are in a `Member` state.  
When adding a new `Database Node` to an already existing database group, a `Mentor Node`
is selected in order to update it.  
The new node will be in a `Promotable` state until it receives _and_ indexes all the documents from the mentor node.  
{NOTE/}

{NOTE: Nodes Order}
The database topology is kept in a list that is always ordered with `Member` nodes first, then `Rehabs` and `Promotables` are last. 
The order is important since it defines the client's order of access into the `Database Group`, (see [Client Request Configuration](../../../client-api/configuration/load-balance-and-failover#conventions--load-balance--failover)).  
The order can be changed with the client API.  
or via the [Studio](../../../studio/database/settings/manage-database-group#database-group-topology---actions).  
{NOTE/}

{INFO: Replication}
All `Members` have master-master [Replication](../../../server/clustering/replication/replication) in order to keep the documents in sync across the nodes.
{INFO/}
{PANEL/}

{PANEL: Dynamic Database Distribution}

If any of the `Database Nodes` is down or partitioned, the [Cluster Observer](../../../server/clustering/distribution/cluster-observer) will recognize it and act as following:  

1. If [Cluster.TimeBeforeMovingToRehabInSec](../../../server/configuration/cluster-configuration#cluster.timebeforemovingtorehabinsec) (default: 60 seconds) time has passed and the node is still unreachable,  
   the node will be moved to a `Rehab` state.

2. If the node is for [Cluster.TimeBeforeAddingReplicaInSec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec) (default: 900 seconds) still in `Rehab`,  
   a new database node will be automatically added to the database group to replace the `Rehab` node.

3. If the `Rehab` node is online again, it will be assigned with a [Mentor Node](../../../server/clustering/distribution/highly-available-tasks#responsible-node) to update him with the recent changes.

4. The first node to be up-to-date stays, while the other is deleted.

{WARNING: Deletion}
The `Rehab` node is actually deleted only when it is re-connected to the cluster,  
_and_ only after it has finished sending all its new documents that it may have (while it was disconnected) to the other nodes in the _Database Group_.  
{WARNING/}

The _Dynamic Database Distribution_ feature can be toggled on and off with the following request:

| URL | Method | URL Params |
| - | - | - |
| /admin/databases/dynamic-node-distribution | `POST` | name=[`database-name`], enable=[`bool`] |
{PANEL/}

{PANEL: Sharding}

Currently, RavenDB **4.x** does not offer sharding as an out-of-the-box solution.  
Sharding is supported starting from RavenDB **6.0**.
{PANEL/}

## Related articles 
- [Client API - Create Database](../../../client-api/operations/server-wide/create-database)  
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)  
