# Cluster: Overview
---

{INFO: }

RavenDB's clustering provides redundancy and an increased availability of data that is consistent  
across a fault-tolerant, [High-Availability](https://en.wikipedia.org/wiki/High-availability_cluster) cluster.  
{INFO/}

---

{NOTE: Cluster Topology}

* A [RavenDB Cluster](../../glossary/ravendb-cluster) consists of one or more RavenDB server instances which are called [Cluster Nodes](../../glossary/cluster-node).  
* Each node has a specific state and type, learn more in [Cluster Topology](../../server/clustering/rachis/cluster-topology).  
{NOTE/}

{NOTE: Cluster Consensus}

* Some actions, such as creating a new database or creating an index, require a [cluster consensus](../../server/clustering/rachis/consensus-operations) in order to occur.  
* The cluster nodes are kept in consensus by using [Rachis](../../server/clustering/rachis/what-is-rachis), 
  which is RavenDB's Raft Consensus Algorithm implementation for distributed systems.  
* **Rachis** algorithm ensures the following:  
  * These actions are done only if the majority of the nodes in the cluster agreed to it !  
  * Any such series of events (each called a [Raft Command](../../glossary/raft-command)) will be executed in the _same_ order on each node.  
{NOTE/}

{NOTE: Data Consistency}

* In RavenDB, the database is replicated to multiple nodes - see [Database Distribution](../../server/clustering/distribution/distributed-database).  
* A group of nodes in the cluster that contains the same database is called a [Database Group](../../studio/database/settings/manage-database-group).  
  (The number of nodes in the database group is set by the replication factor supplied when creating the database).  
* Documents are kept in sync across the _Database Group_ nodes with a [master to master replication](../../server/clustering/replication/replication).  
* Any document related change such as a CRUD operation doesn't go through Raft, 
  instead, it is automatically **replicated** to the other database instances to in order to keep the data up-to-date.  
{NOTE/}

{NOTE: Data Availability}

* Due to the consistency of the data, even if the majority of the cluster is down, as long as a single node is available, we can still process Reads and Writes.
* Read requests can be spread among the cluster's nodes for better performance.  
{NOTE/}

{NOTE: Distributed Work}

* Whenever there’s a [Work Task](../../server/clustering/distribution/highly-available-tasks) for a _Database Group_ to do (e.g. a Backup task), 
  the cluster will decide which node will actually be responsible for it.  
* These tasks are operational even if the node to which the client is connected to is down, as this nodes' tasks are **re-assigned** to other available nodes in the _Database Group_.  
{NOTE/}

{NOTE: Cluster's Health}

* The cluster's health is monitored by the [Cluster Observer](../../server/clustering/distribution/cluster-observer) which checks upon each node in the cluster.  
* The node state is recorded in the relevant database groups so that the cluster can maintain the database replication factor and re-distribute its work tasks if needed.  
{NOTE/}

## Related Articles

### Cluster in the Studio
- [Cluster View](../../studio/server/cluster/cluster-view)
- [Adding Node to Cluster](../../studio/server/cluster/add-node-to-cluster)  
- [Cluster Observer Log](../../studio/server/cluster/cluster-observer)  
- [Database Group View](../../studio/database/settings/manage-database-group)  
- [Ongoing Work Tasks](../../studio/database/tasks/ongoing-tasks/general-info)  
- [Cluster-Operations -vs- DB-Operations](../../studio/server/cluster/cluster-view#cluster-wide-operation--vs--database-operations)  
