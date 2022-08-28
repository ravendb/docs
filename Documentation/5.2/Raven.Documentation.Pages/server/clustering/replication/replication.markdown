# Replication
---

{NOTE: }

* Replication in RavenDB is the ongoing process of transferring data from one database to another.  

* Replication can be used:
   * As a failover, if a server goes down.  
   * To distribute work across nodes or clusters.

* This article discusses replication between databases **within the same cluster**.

* Read more about **replication to other clusters here**:
   * [External Replication](../../../server/ongoing-tasks/external-replication)
   * [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication)

* In this page: 
   * [Replication Within the Cluster](../../../server/clustering/replication/replication#replication-within-the-cluster)
      * [What is replicated](../../../server/clustering/replication/replication#what-is-replicated)
      * [To distribute work across nodes](../../../server/clustering/replication/replication#to-distribute-work-across-nodes)
      * [How Replication works](../../../server/clustering/replication/replication#how-replication-works)
   * [Maintaining Consistency Between Nodes](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes)

{NOTE/}

---

{PANEL: Replication Within the Cluster}

{NOTE: }

#### What is replicated:

  * Documents 
  * Revisions 
  * Attachments 
  * Conflicts  

{NOTE/}

Every database created in a cluster has a [Replication Factor](../../../server/clustering/distribution/distributed-database) 
that determines how many replicas will exist. 
While creating the database, you can also define which nodes will host this database.  

A topology of nodes that host the same database with master-master replication is called a [database group](../../../studio/database/settings/manage-database-group). 

{INFO: }
#### To distribute work across nodes
* To [maintain consistency](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes) 
  a [primary node](../../../client-api/session/configuration/use-session-context-for-load-balancing) 
  is responsible for writes and reads as a default setting.  
   * You can configure the [ReadBalanceBehavior](../../../client-api/configuration/load-balance-and-failover#conventions-load-balance--failover), 
     which determines to which nodes the client will send read requests. 
   * You can also set session-specific behavior for writes with [UseSessionContext](../../../client-api/session/configuration/use-session-context-for-load-balancing#loadbalancebehavior-usage), 
     which allows you to set read and write request behavior.  
     **Changing the default write request behavior can cause frequent conflicts** whenever two nodes write on the same document concurrently.  
* Distribution of work offers guaranteed consistency **when each node is responsible for a different database** 
  while updating the other nodes in a database group for failover purposes.  
  e.g. There won't be conflicts if only node A writes on the "Customers" database, while only node B writes on the "Invoices" database. 
{INFO/}

#### When nodes are down

When a node goes down, the [Cluster Oberver](../../../server/clustering/distribution/cluster-observer)
selects another node to fulfill all of the tasks which were assigned to the node that went down. 
This includes reads, writes, and ongoing tasks such ETL, Backup, and/or External Replication.  

When back online, the node will be updated by the Responsible Node with any changes made to the data while it was down. 
The now online node will update its indexes with any changes in the data.  
Any ongoing tasks for which it was explicitly set as the preferred node 
will be returned to the now-online node. 

If [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) 
was not explicitly toggled off, and if the fallen node does not come online (in 15 minutes by default), 
the Cluster Observer will select another available node to replace the fallen node in the database group to preserve the Replication Factor.

{INFO: }
We highly recommend setting each production database in a [group of 3, 5, or more odd-numbered nodes on separate machines](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#an-overview-of-a-ravendb-cluster)
to ensure [high availability](https://en.wikipedia.org/wiki/High-availability_cluster), clean [Raft consensus](../../../glossary/raft-algorithm), 
and enough available nodes to distribute the workload.  
{INFO/}

## How Replication works

The replication process sends data over a TCP connection in the order in which the documents were modified, from the oldest to the newest.   

Every document has an [ETag](../../../glossary/etag), 
which is local to each node and is incremented on _every_ document modification.  
An ETag is the local-node portion of the cluster's [Change Vector](../../../server/clustering/replication/change-vector).   

Each replication process has a _source_, a _destination_, and a last confirmed `Change Vector` which is used by the cluster for [concurrency control](../../../server/clustering/replication/change-vector#concurrency-control-at-the-cluster).   

The data is sent in batches. When the _destination_ node confirms getting the data, each document's last accepted `ETag` 
is then incremented and the next batch of data is sent.  
When the documents' local ETags are modified, the Change Vectors are modified accordingly.  

{NOTE: Replication Failure} 
The [Handshake Procedure](../../../server/clustering/replication/advanced-replication) ensures that the source and destination 
are in sync as to the current state of a document while it is being modified. 
In case of failure, replication will re-start with the Initial Handshake Procedure, 
which will make sure we will start replicating from the last accepted `ETag`.
{NOTE/}

{PANEL/}

{PANEL: Maintaining Consistency Between Nodes}

### Replication Transaction Boundary

The boundary of a transaction is extended across multiple nodes.  
If there are several documents in the same transaction they will be sent in the same replication 
batch to keep the data consistent.  

However, this doesn't always ensure data consistency since the same document can be modified in a different 
transaction and be sent in a different batch.  

### Replication consistency can be achieved by -  

* Using a [Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance)
  which waits until the transactions are replicated before completing the `session.SaveChanges` procedure.  
* Not disabling the default [primary-node responsibility](../../../server/clustering/replication/replication#to-distribute-work-across-nodes) 
  for writes and reads.  
* Using [cluster-wide sessions](../../../server/clustering/cluster-transactions) 
  if you need every node to be able to read and write.  
  Cluster-wide transactions ensure strong consistency, but the required Raft consensus checks for each transaction slow performance.  
* Enabling [Revisions](../../../server/extensions/revisions).  
  When documents that own revisions are replicated, their revisions will be replicated with them.  
     {INFO: Let's see how the replication of revisions helps data consistency.}
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
