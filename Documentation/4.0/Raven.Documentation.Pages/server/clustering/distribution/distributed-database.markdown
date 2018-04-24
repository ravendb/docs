# Distributed Database

In RavenDB a database can be replicated across multiple nodes.  
A node where a database resides is referred as `Database Node`. The group of `Database Nodes` that assemble the distributed database, is called `Database Group`.   
Every database keeps its configuration (e.g. Index definitions, Database topology) on the raft as [Database Record](../../../client-api/operations/server-wide/create-database) object.

{INFO: Important}
Each database node is a **full copy**, so it contains _all_ the database documents and index them locally. This greatly simplifies executing query requests, since there is no need to orchestrate an aggregation from various nodes. 
{INFO/}

When creating a database it is possible to specify the exact nodes of the `Database Group`, which implicitly will set the `Replication Factor` to the amount of the nodes. Or it is possible to pass the `Replication Factor` explicitly and let RavenDB to choose on which nodes to place the database.  
Either way, once the database is created by getting a [Consensus](./../../server/clustering/rachis/consensus-operations), the [Cluster Observer](../../../server/distribution/cluster-observer) begins to monitor the `Database Group` to maintains the `Replication Factor`.

## Database Topology
The `Database Topology` describes the relations inside the `Database Group` between the `Database Nodes`.
Each `Database Node` can be in one of the following states:

| State | Description |
| - | - |
| **Member** | Fully updated and functional database node. |
| **Promotable** | Recently added node to the group, which is being updated. |
| **Rehab** | A former Member node that assumed to be _not_ up-to-dated due to partition. |

{INFO: Replication}
All `Members` have master-master [Replication](../../../server/clustering/replication/replication) in order to keep the documents in sync across the nodes.
{INFO/}

In general, all nodes in a newly created database are in the state of `Member`.  
When adding new `Database Node` to an already existing database group, a [Mentor Node](../../../server/clustering/distribution/database-tasks#mentor-node) will be selected by the server in order to update it. The new node will be in a `Promotable` state until it will receive _and_ index all of the documents from the mentor node.

### Nodes Order

The database topology is always ordered by `Member` nodes to appear first, then `Rehabs` and `Promotables` are last. 
The order is important, since it defines the client's order of access into the `Database Group`.  
The order can be changed with through the [Client-API](../../../client-api/operations/server-wide/reorder-database-members-operation) or via the [Studio](../../../studio/database/settings/manage-database-group#database-group-topology---actions).

## Dynamic Database Distribution
If any of the `Database Nodes` is down or partitioned, the [Cluster Observer](../../../server/distribution/cluster-observer) will recognize it and act as following:

1. If [Cluster.TimeBeforeMovingToRehabInSec](../../../server/configuration/cluster-configuration#cluster.timebeforemovingtorehabinsec) (default: 60 seconds) time has passed and the node is still unreachable, the node will be moved to a `Rehab` state.
2. If the node is for [Cluster.TimeBeforeAddingReplicaInSec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec) (default: 900 seconds) still in `Rehab`, a new database node will be automatically added to the database group to replace the `Rehab` node.
3. If the `Rehab` node is online again, it will be assigned with a [Mentor Node](../../../server/clustering/distribution/database-tasks#mentor-node) to update him with the recent changes.
4. The first node to be up-to-date stays, while the other is deleted.

{WARNING: Deletion}
The `Rehab` is actually deleted only after he is reconnected to the cluster and sent any new document that it may have. 
{WARNING/}

It is possible to toggle this feature on and off with the following `HTTP` request:

| URL | Method | URL Params |
| - | - | - |
| /admin/databases/dynamic-node-distribution | `POST` | name=[`database-name`], enable=[`bool`] |
 

## Sharding

RavenDB 4.x doesn't offer sharding as an out of the box solution, although it is on the development roadmap to be implemented in a future version (Track it [here](http://issues.hibernatingrhinos.com/issue/RavenDB-8115)).  

## Related articles 
- [Client API - Database Operations](../../../client-api/operations/server-wide)
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)
