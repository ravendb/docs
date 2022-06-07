# Replication
---

{NOTE: }

* Replication in RavenDB is the ongoing process of transferring data from one database to another.  

* Replication can be used:
   * As a failover in case a server goes down.  
   * To distribute work across servers.  
      * To [maintain consistency](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes), 
        RavenDB uses single-node responsiblity for writes and reads as default in non-cluster-wide transactions.  
      * Distribution of work guarantees consistency when each node is responsible for a different database, 
        but still updates the others for failover purposes.  
        Also, geo-distributed databases use [External Replication](../../../server/ongoing-tasks/external-replication) 
        or [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication) to distribute work and reduce latency 
        but should maintain [consistency boundaries](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).  

* The rest of this article will focus on **single-cluster replication**.  
  If you want to learn more about **replication between clusters**, see the articles about one-way [External Replication](../../../server/ongoing-tasks/external-replication) 
  and two-way [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication) where various filters can also be set.  

* In this page: 
   * [About Single-Cluster Replication](../../../server/clustering/replication/replication#about-single-cluster-replication)
      * [How Replication works](../../../server/clustering/replication/replication#how-replication-works)
   * [Maintaining Consistency Between Nodes](../../../server/clustering/replication/replication#maintaining-consistency-between-nodes)

{NOTE/}

{PANEL: About Single-Cluster Replication}

{NOTE: }

* What is replicated:
   * Documents 
   * Revisions 
   * Attachments 
   * Conflicts  

{NOTE/}

Every database created in a cluster has a [Replication Factor](../../../server/clustering/distribution/distributed-database) 
that sets which nodes will host this database and will keep each other updated via replication. 

#### When nodes are down

When a node goes down, the other nodes take over normal data updates.  

When it comes back online, it will automatically be updated by the other nodes with any changes made while it was down.  

These changes will then trigger the ongoing tasks that the now-refreshed node is responsible for, 
such as indexing, ETL or Backups, to further process the updated data.

{INFO: Why clusters should have at least 3 nodes} 

It is highly recommended to have a cluster of at least 3 nodes on separate machines. If one 
goes down, having at least two running nodes will maintain high availability and ensure that the 
third node will be updated properly via replication when it is back online.  

To ensure consistency across the cluster, [cluster-wide transactions](../../../server/clustering/cluster-transactions) 
work most efficiently with odd numbers of nodes (3, 5, 7...) because they must get confirmations 
from a majority of the nodes to complete every transaction.  
If one goes down temporarily, the cluster must have at least 2 functional nodes to complete cluster-wide transactions.  
{INFO/}


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

However, this doesn't always ensure the data consistency since the same document can be modified in a different 
transaction and be sent in a different batch.  

### Replication consistency can be achieved by -  

* Using [Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
* Not changing the default single-node-responsibility for writes and reads on each database. For example, node A can be responsible for writes on a database called 
  "Receipts", while node B can be responsible for "CustomerInformation", and so on.
  By default, one node is responsible for all reads and writes on any database.
  You can configure [load balancing](../../../client-api/session/configuration/use-session-context-for-load-balancing) 
  to fine-tune the settings to your needs.
     * Learn more about [Scaling Distributed Work In RavenDB](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/7-scaling-distributed-work-in-ravendb) 
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
        `Users/1` will be stored, and so will the `Users/2` revision, that will become 
        a live `Users/2` document.  

{PANEL/}

## Related Articles  

### Single-Cluster

- [Replication Conflicts](../../../server/clustering/replication/replication-conflicts)
- [Change Vector](../../../server/clustering/replication/change-vector)
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
