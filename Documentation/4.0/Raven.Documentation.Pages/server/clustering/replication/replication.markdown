# Replication
---

{NOTE: }

* Replication in RavenDB is the ongoing process of transferring data from one database to another.  

* Replication can be used:
   * As a failover, if a server goes down.  
   * To distribute work across nodes or clusters.
      * Geo-distributed databases use one-way [External Replication](../../../server/ongoing-tasks/external-replication) 
        or two-way [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication) between different clusters 
        to distribute work and reduce latency but should maintain [consistency boundaries](../../../server/ongoing-tasks/external-replication#maintaining-consistency-boundaries-between-clusters). 
      * Read more about multiple geo-distributed clusters in [Inside RavenDB](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/7-scaling-distributed-work-in-ravendb#multiple-clusters-multiple-data-centers).

* The rest of this article will focus on **within-cluster replication** in a single cluster.  

* In this page: 
   * [About Within-Cluster Replication](../../../server/clustering/replication/replication#about-within-cluster-replication)
      * [What is replicated](../../../server/clustering/replication/replication#what-is-replicated)
      * [To distribute work across nodes](../../../server/clustering/replication/replication#to-distribute-work-across-nodes)
      * [How Replication works](../../../server/clustering/replication/replication#how-replication-works)
   * [Maintaining Consistency Between Nodes](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes)

{NOTE/}

---

{PANEL: About Within-Cluster Replication}

{NOTE: }

#### What is replicated:

   * Documents 
   * Revisions 
   * Attachments 
   * Conflicts  

{NOTE/}

Every database created in a cluster has a [Replication Factor](../../../server/clustering/distribution/distributed-database) 
in which you determine how many replicas will exist and will keep each other updated via replication. 
While creating it, you can also define which nodes will host this database.  

A topology of nodes that host the same database with master-master replication is called a [database group](../../../studio/database/settings/manage-database-group). 

{INFO: }
#### To distribute work across nodes
* To [maintain consistency](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes), 
  a [primary node](../../../client-api/session/configuration/use-session-context-for-load-balancing) is responsible for writes and reads as a default setting.  
   * You can configure the [ReadBalanceBehavior](../../../client-api/configuration/load-balance-and-failover#conventions-load-balance--failover). 
   * You can also set session-specific behavior for writes with [UseSessionContext](../../../client-api/session/configuration/use-session-context-for-load-balancing#loadbalancebehavior-usage).
* Distribution of work offers guaranteed consistency **when each node is responsible for a different database**, 
  while updating the other nodes in a database group for failover purposes.  
  * e.g. There won't be conflicts if only node A writes on the "Customers" database, while only node B writes on the "Invoices" database. 
{INFO/}

#### When nodes are down

When a node goes down, the other nodes take over normal data updates.  

When it comes back online, it will automatically be updated by the responsible node with any changes made while it was down.  

These changes will then trigger the tasks that the now-refreshed node is responsible for, 
such as indexing, ETL, or Backups, to further process the updated data.

We highly recommend setting each production database in a [group of 3, 5, or more odd-numbered nodes on separate machines](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#an-overview-of-a-ravendb-cluster)
to ensure [high availability](https://en.wikipedia.org/wiki/High-availability_cluster), clean [Raft consensus](../../../glossary/raft-algorithm), 
and enough available nodes to distribute the workload.  


## How Replication works

The replication process sends data over a TCP connection by the modification order, from the oldest to the newest.   

Every database has an [ETag](../../../glossary/etag), which is incremented on _every_ modification in the database's storage.   

Each replication process has a _source_, _destination_, and a last confirmed `ETag` which is the cursor to where the replication process is.   

The data is sent in batches. When the _destination_ confirms getting the data, the last accepted `ETag` is then advanced and the next batch of data is sent. 

{NOTE: Replication Failure} 
In case of failure, it will re-start with the [Initial Handshake Procedure](../../../server/clustering/replication/advanced-replication), 
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

* Using [Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance)
  which waits until the transactions are replicated before completing the `session.SaveChanges` procedure.  
* Not disabling the default [primary-node responsibility](../../../server/clustering/replication/replication#to-distribute-work-across-nodes) 
  for writes and reads.  
* Use [cluster-wide sessions](../../../server/clustering/cluster-transactions) 
  if you need every node to be able to read and write.  
  Cluster-wide transactions ensure strong consistency, but the required Raft consensus checks for each transaction slow performance.  
* Enabling [Revisions](../../../server/extensions/revisions).  
  When documents that own revisions are replicated, their revisions will be replicated with them.  
   * Let's see how the replication of revisions helps data consistency.
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
        In this case, the creation of `Users/1` and `Users/2` will also create revisions 
        for them both. These revisions will continue to carry the Etag given to them 
        at their creation and will be replicated in the same batch.  
        When the batch arrives at the destination, data consistency will be kept:  
        `Users/1` will be stored, and so will the `Users/2` revision, which will become 
        a live `Users/2` document.  

{PANEL/}

## Related Articles  

### Within-Cluster

- [Replication Conflicts](../../../server/clustering/replication/replication-conflicts)
- [Change Vector](../../../server/clustering/replication/change-vector)
- [Load Balancing](../../../client-api/session/configuration/use-session-context-for-load-balancing)
- [Advanced Replication Topics](../../../server/clustering/replication/advanced-replication)
- [Using Embedded Instance](../../../server/clustering/replication/replication-and-embedded-instance)
- [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions)

### Between Clusters

- [External Replication](../../../server/ongoing-tasks/external-replication)
- [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication)

---

### Inside RavenDB

- [RavenDB Clusters](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#an-overview-of-a-ravendb-cluster)
- [Replication of data in a database group](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/6-ravendb-clusters#replication-of-data-in-a-database-group)

### Ayende @ Rahien Blog

- [Data ownership in a distributed system](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)
