# Distributed Database

In RavenDB a database can be spanned across multiple nodes.  
A node where a database resides is referred as `Database Node`. The group of `Database Nodes` that assemble the distributed database, is called `Database Group`. 

{INFO: Important}
Each database node is a **full copy**, so it contains _all_ the database documents and index them locally. This greatly simplifies executing query requests, since there is no need to orchestrate an aggregation from various nodes. 
{INFO/}

When creating a database it is possible to specify the exact nodes of the `Database Group`, which implicitly will set the `Replication Factor` to the amount of the nodes. Or it is possible to pass the `Replication Factor` explicitly and let RavenDB to choose on which nodes to place the database.  
Either way, once the database is created by getting a [Consensus](../rachis/consensus-operations), the [Cluster Observer](cluster-observer) begins to monitor the `Database Group` to maintains the `Replication Factor`.

## Database Topology
The `Database Topology` describes the relations inside the `Database Group` between the `Database Nodes`.
Each `Database Node` can be in one of the following states:

| State | Description |
| - | - |
| **Member** | Fully updated and functional database node. |
| **Promotable** | Recently added node to the group, which is being updated. |
| **Rehab** | A former Member node that assumed to be _not_ up-to-dated due to partition. |

{INFO: Replication}
All `Members` have master-master [Replication](../replication/replication) in order to keep the documents in sync across the nodes.
{INFO/}

In general, all nodes in a newly created database are in the state of `Member`.  
When adding new `Database Node` to an already existing database group, a [Mentor Node](to-do) will be selected by the server in order to update it. The new node will be in a `Promotable` state until it will receive _and_ index all of the documents from the mentor node.

### Dynamic Database Distribution
If any of the `Database Nodes` is down or partitioned, the [Cluster Observer](cluster-observer) will recognize it and act as following:

1. If `Cluster.TimeBeforeMovingToRehabInSec` time has passed and the node is still unreachable, the node will be moved to a `Rehab` state.
2. If the node is for `Cluster.TimeBeforeAddingReplicaInSec` still in `Rehab`, a new database node will be automatically added to the database group to replace the `Rehab` node.
3. If the `Rehab` node is online again, it will be assigned with a [Mentor Node](to-do) to update him with the recent changes.
4. The first node to be up-to-date stays, while the other is deleted.


It is possible to toggle this feature on and off with the following `HTTP` request:

| URL | Method | URL Params |
| - | - | - |
| /admin/databases/dynamic-node-distribution | `POST` | name=[`database-name`], enable=[`bool`] |

## Sharding

RavenDB 4.x doesn't offer a **server side** sharding as an out of the box solution, although it is on the development roadmap to be implemented in a future version (Track it [here](http://issues.hibernatingrhinos.com/issue/RavenDB-8115)).  
However it is possible to implement sharding from the client side.

## Related articles 
- [Client API - Database Operations](../../../client-api/operations/server-wide)
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)
