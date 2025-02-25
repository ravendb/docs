# Distributed Database
---

{NOTE: }

* In RavenDB, a database can be replicated across multiple nodes, depending on its [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor).  

* A node where a database resides is referred to as a `Database Node`.  
  The group of _Database Nodes_ that assemble the distributed database, is called a [Database Group](../../../studio/database/settings/manage-database-group).  

* Unless [Sharding](../../../server/clustering/distribution/distributed-database#sharding) is employed,
  each Database Node has a **full copy** of the database, containing **all** the database documents, and indexes them **locally**.
  This greatly simplifies executing query requests, as there is no need to aggregate data from multiple nodes.
  
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
Upon database creation, this object is propagated through Rachis to all nodes in the cluster.  

After that, each node updates its own `Database Record` independently upon receiving any new Raft command,  
e.g. when an index has changed.  

{PANEL/}

{PANEL: Replication Factor}

When creating a database, you can either:

* Explicitly specify the _exact_ nodes to use for the `Database Group`
* Or, specify only the number of nodes needed and let RavenDB automatically choose the nodes  
  on which the database will reside.

In either case, the number of nodes will represent the `Replication Factor`.

Once the database is created by getting a [Consensus](../../../server/clustering/rachis/consensus-operations),  
the [Cluster Observer](../../../server/clustering/distribution/cluster-observer) begins monitoring the _Database Group_ in order to maintain this Replication Factor.

{PANEL/}

{PANEL: Database Topology}

The `Database Topology` describes the relationships between the Database Nodes within the [Database Group](../../../studio/database/settings/manage-database-group).  
Each **Database Node** can be in one of the following states:  

| State          | Description                                                                     |
|----------------|---------------------------------------------------------------------------------|
| **Member**     | Fully updated and functional database node.                                     |
| **Promotable** | A node that has been recently added to the group and is being updated.          |
| **Rehab**      | A former Member node that is assumed to be _not_ up-to-date due to a partition. |

{NOTE: States Flow}

* In general, all nodes in a newly created database are in a `Member` state.  
* When adding a new Database Node to an already existing database group, a `Mentor Node` is selected in order to update it.  
  The new node will be in a `Promotable` state until it receives _and_ indexes all the documents from the mentor node.  
* Learn more in:  
  * [Cluster node types](../../../studio/cluster/cluster-view#cluster-nodes-types)
  * [Cluster node states and types flow](../../../studio/cluster/cluster-view#cluster-nodes-states-&-types-flow)

{NOTE/}

{NOTE: Nodes Order}

* The database topology is kept in a list that is always ordered with `Member` nodes first,  
  then `Rehabs` and `Promotables` are last. 
* The order is important since it defines the client's order of access into the Database Group,  
  (see [Load balancing client requests](../../../client-api/configuration/load-balance/overview)).  
* The order can be modified using the [Client-API](../../../client-api/operations/server-wide/reorder-database-members)
  or via the [Studio](../../../studio/database/settings/manage-database-group#database-group-topology---actions).  

{NOTE/}

{INFO: Replication}

All `Members` have master-master [Replication](../../../server/clustering/replication/replication) in order to keep the documents in sync across the nodes.

{INFO/}
{PANEL/}

{PANEL: Dynamic Database Distribution}

If any of the `Database Nodes` is down or partitioned, the [Cluster Observer](../../../server/clustering/distribution/cluster-observer) will recognize it and act as follows:  

1. If the time that is defined in [TimeBeforeMovingToRehabInSec](../../../server/configuration/cluster-configuration#cluster.timebeforemovingtorehabinsec) (default: 60 seconds) has passed  
   and the node is still unreachable, the node will be moved to a `Rehab` state.
   
2. If the node remains in `Rehab` for the time defined in [TimeBeforeAddingReplicaInSec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec) (default: 900 seconds),  
   a new database node will be automatically added to the database group to replace the `Rehab` node.

3. If the `Rehab` node is online again, it will be assigned a [Mentor Node](../../../server/clustering/distribution/highly-available-tasks#responsible-node) to update it with the recent changes.

4. The first node to be up-to-date stays, while the other is deleted.

{WARNING: Deletion}

The `Rehab` node is deleted only when it reconnects to the cluster,
and only AFTER it has finished sending all new documents it may have (while disconnected) to the other nodes in the _Database Group_.

{WARNING/}

The _Dynamic Database Distribution_ feature can be toggled on and off with the following request:

| URL                                        | Method   | URL Params                              |
|--------------------------------------------|----------|-----------------------------------------|
| /admin/databases/dynamic-node-distribution | `POST`   | name=[`database-name`], enable=[`bool`] |

{PANEL/}

{PANEL: Sharding}

* Sharding, supported by RavenDB as an out-of-the-box solution starting with version **6.0**,  
  is the distribution of a database's content across autonomous shards.

* Learn more about sharding in this dedicated [Sharding overview](../../../sharding/overview) article.

{PANEL/}

## Related articles 

- [Client API - Create Database](../../../client-api/operations/server-wide/create-database)  
- [Create Database via Studio](../../../studio/database/create-new-database/general-flow)  
