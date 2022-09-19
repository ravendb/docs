# Replication
---

{NOTE: }

* Replication in RavenDB is the continuous process of transferring data from one node to another within a database group.  

* Replication is a default behavior between nodes in a database group.  
  It is done to provide high availability.

* This article discusses internal data replication within a [Database Group](../../../studio/database/settings/manage-database-group), 
  among nodes of the same cluster.

* For replication between different databases or clusters, refer to [External Replication](../../../server/ongoing-tasks/external-replication)
  and [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication).

* In this page: 
   * [Overview](../../../server/clustering/replication/replication#overview)
   * [What is Replicated](../../../server/clustering/replication/replication#what-is-replicated)
   * [How Replication Works](../../../server/clustering/replication/replication#how-replication-works)
   * [Maintaining Data Consistency](../../../server/clustering/replication/replication#maintaining-data-consistency)
   * [Maintaining High Availability If Nodes Go Down](../../../server/clustering/replication/replication#maintaining-data-consistency)

{NOTE/}

---

{PANEL: Overview}

* You can keep multiple instances of your database on different nodes in the cluster.  
  The group of nodes in the cluster that contains the same database is called a [Database Group](../../../studio/database/settings/manage-database-group).

* The number of nodes in the Database Group is referred to as the database [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor).

* RavenDB uses master-to-master replication to keep the database data in-sync across the Database Group nodes.  
  This guarantees the availability of your cluster as reads & writes can be done on any of the nodes.  
  By default, each database group has a designated [primary node](../../../server/clustering/replication/replication#distributing-workload-among-nodes) 
  that handles all reads and writes to preserve ACID transactions. [If this primary node goes down](../../../server/clustering/replication/replication#maintaining-high-availability-if-nodes-go-down), 
  another node instantly and automatically takes over the workload.

* Data that is modified on one node is automatically replicated to all other database instances.  

* Replication may involve occasional conflicts if a node does not wait for cluster consensus via [cluster-wide sessions](../../../server/clustering/cluster-transactions).  
  Without cluster-wide sessions, a write is always accepted, even if it will create a conflict.  
   * Conflicts are resolved according to the defined [Conflict Resolution](../../../server/clustering/replication/replication-conflicts) policy.

{PANEL/}

{PANEL: What is Replicated:}

  * Documents 
  * Revisions 
  * Attachments 
  * Conflicts  
  * Tombstones
  * Counters
  * Time Series

**Server level features are not replicated.** 
Consistency between nodes is achieved on the cluster by using the [Raft Protocol](../../../server/clustering/rachis/what-is-rachis).

* Index definitions and index data
* Ongoing tasks definitions
* Compare-exchange items
* Identities
* Conflict resolution scripts

{PANEL/}

{PANEL: How Replication Works}

* Each database instance holds a TCP connection to each of the other database instances in the group.  
  Whenever there is a 'write' on one instance, it will be sent to all the others immediately over this connection.

* This is done in an async manner.  
  If the database instance is unable to replicate the data, it will still accept that 'write action' and send it later.

* The replication process sends the data over a TCP connection in the order in which the documents were modified, from the oldest to the newest.   

* Each replication process has a _source_, a _destination_, and a last confirmed `Change Vector` which is used by the cluster for [concurrency control](../../../server/clustering/replication/change-vector#concurrency-control-at-the-cluster).   

* Every database item has an [ETag](../../../glossary/etag), 
  which is local to the database instance on each node.  
  This ETag is incremented on _every_ item modification. The modified item receives the next consecutive number.  
  The order by which the documents are replicated is set by this Etag, from low to high.  
  The ETag is also part of the item's [Change Vector](../../../server/clustering/replication/change-vector).   

* The data is sent in batches from the source to the destination.  
  The destination database records the ETag of the last item that it has received in the batch.  
  Upon the next [replication-handshake](../../../server/clustering/replication/advanced-replication) 
  that last-accepted ETag is passed to the source so that the source will
  know from where to continue sending the next batch.

* In case of a replication failure, when sending the next batch, replication will start from the item 
  that has this last-accepted Etag, which is known from the previous successful batch.


{PANEL/}

{PANEL: Maintaining Data Consistency}

#### Transactions Atomicity

RavenDB guarantees that modifications made in the same transaction will always be replicated to the other database
instances in a single batch and won't be broken into separate batches.

#### Cluster or Local Transactions Can Be Set In Each Session As Needed

You can set each session as either a [cluster-wide or local-node session](LINKTONEWRDOC-2271CWv.LNarticle) as needed.  
In some transactions, the performance cost of the cluster-wide Raft check is worthwhile.  
Local node transactions are much faster, but can have rare conflicts. The [conflict resolution](../../../server/clustering/replication/replication-conflicts) 
logic can be defined per collection to ensure consistency according to the type of transaction done with documents in that collection.

### Cluster-Wide Transactions

Using [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions), which is implemented by the [Raft Protocol](../../../server/clustering/rachis/consensus-operations#consensus-operations), 
ensures strong consistency and has a performance cost.  
Such a transaction will either be persisted on all database group nodes or will be rolled back on all upon failure.

After a cluster consensus is reached, the Raft command to be executed is propagated to all nodes.
When the command is executed locally on a node, if the items that are persisted are of the [type that replicates](../../../server/clustering/replication/replication#what-is-replicated), 
then the node will replicate them to the other nodes by the replication process.

A node that receives such replication will accept this write unless it already committed it through a raft
command it received before as well.

### Single Node Transactions

In local node sessions, the primary node modifies documents.  
It then replicates the modifications to the other nodes in the database group for high availability.  

#### Write Assurance

With Single-Node sessions you can use the Write-Assurance method
which will ensure that the data will be replicated to the specified number of nodes.

#### Single-Writer

By default to prevent conflicts, all of the clients will 'talk' (send read/write requests) to the first node in the database group topology,
which is called the preferred/primary node.
If this configuration is not modified, then all write requests will go through this node.
Data will be consistent as the same copies will replicate to all other nodes.
To modify this default read/write topology set [LoadBalanceBehavior](../../../client-api/session/configuration/use-session-context-for-load-balancing).

#### Keeping Transaction Boundaries in Local Node Transactions

If there are several documents modifications in the same transaction they will be sent in the same replication
batch, keeping the transaction boundary on the destination as well.

However, when a document is modified in two separate transactions 
and if replication of the 1st transaction has not yet occurred, 
then that document will not be sent when the 1st transaction is replicated, 
it will be sent with the 2nd transaction.

To fix that, either:

* Use the same transaction  
  Ensure that documents you care about are updated in the same transaction, and so will be sent in the same batch.
* Enable Revisions  
  When a revision is created for a document it is written as part of the same transaction as the document.  
  The revision is then replicated along with the document in the same indivisible batch.  
     {INFO: How revisions replication can help data consistency}
     Consider a scenario in which two documents, `Users/1` and `Users/2`, 
     are **created in the same transaction**, and then `Users/2` is **modified 
     in a different transaction**.  

     * **How will `Users/1` and `Users/2` be replicated?**  
       When RavenDB creates replication batches, it keeps the 
       [transaction boundary](../../../server/clustering/replication/replication#replication-transaction-boundary) 
       by always sending documents that were modified in the same transaction, 
       **in the same batch**.  
       In our scenario, however, `Users/2` was modified after its creation, it 
       is now recognized by its Etag as a part of a different transaction than 
       that of `Users/1`, and the two documents may be replicated in two different 
       batches, `Users/1` first and `Users/2` later.  
       If this happens, `Users/1` will be replicated to the destination without `Users/2` 
       though they were created in the same transaction, causing a data inconsistency that 
       will persist until the arrival of `Users/2`.  

     * **The scenario will be different if revisions are enabled.**  
       In this case the creation of `Users/1` and `Users/2` will also create revisions 
       for them both. These revisions will continue to carry the Etag given to them 
       at their creation, and will be replicated in the same batch.  
       When the batch arrives at the destination, data consistency will be kept:  
       `Users/1` will be stored, and so will the `Users/2` revision, that will become 
       a live `Users/2` document.  

     {INFO/}

{PANEL/}

{PANEL: Maintaining High Availability If Nodes Go Down}

When a node goes down, the [Cluster Oberver](../../../server/clustering/distribution/cluster-observer)
selects another node to fulfill all of the tasks which were assigned to the node that went down. 
This includes reads, writes, and ongoing tasks such ETL, Backup, and/or External Replication.  

The section on  [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) 
explains the different steps and default timing for handling nodes that go down.

{PANEL/}

## Related Articles  

### Replication Within the Cluster

- [Replication Conflicts](../../../server/clustering/replication/replication-conflicts)
- [Change Vector](../../../server/clustering/replication/change-vector)
- [Load Balancing](../../../client-api/session/configuration/use-session-context-for-load-balancing)
- [Advanced Replication Topics](../../../server/clustering/replication/advanced-replication)
- [Using Embedded Instance](../../../server/clustering/replication/replication-and-embedded-instance)
- [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions)

### Replication Between Clusters

- [External Replication](../../../server/ongoing-tasks/external-replication)
- [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication)

---

### Inside RavenDB

- [RavenDB Clusters](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#an-overview-of-a-ravendb-cluster)
- [Replication of data in a database group](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#replication-of-data-in-a-database-group)

### Ayende @ Rahien Blog

- [Data ownership in a geo-distributed system](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)
