# Replication
---

{NOTE: }

* Replication in RavenDB is the ongoing process of transferring data from one database to another.  

* Replication can be used:
   * As a failover in case a server goes down.
   * To distribute work across servers.
   * To create a two way updating system between a database hub and it's branches ("sinks") 
     where the sinks may only need to host part of the database.  

* [External Replication](../../../server/ongoing-tasks/external-replication) and [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication) 
  replicate data **from one cluster to another**.
   * External Replication is a one-way process.
   * Hub/Sink Replication can be a two-way process with various filters set.  
   * Some cluster-level features (such as compare-exchange) are not replicated across clusters to maintain [consistency boundaries](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)

* Unless otherwise specified, "Replication" refers to in-cluster replication **between the nodes of a single cluster**.
  This article is about single-cluster replication.

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
that sets which nodes will host this database and keep each other updated via replication. 

{INFO: When nodes are down}
When a node goes down, the other nodes take over normal data updates.  

When it comes back online, it will automatically update any changes made while down. These changes 
will then trigger the ongoing tasks that it is responsible for, such as ETL or Backups.

It is highly recommended to have a cluster of at least 3 nodes on separate machines. If one 
goes down, having at least two running nodes will maintain high availability and ensure that the 
third node will be updated properly via replication when it is back online. 
{INFO/}


## How Replication works

The replication process sends data over a TCP connection by the modification order, from the oldest to the newest.   

Every database has an [ETag](../../../glossary/etag), which is incremented on _every_ modification in the database's storage.   

Each replication process has a _source_, _destination_, and a last confirmed `ETag` which is the cursor to where the replication process is.   

The data is sent in batches. When the _destination_ confirms getting the data, the last accepted `ETag` is then advanced and the next batch of data is sent. 

{NOTE: Replication Failure} 
In case of failure it will re-start with the [Initial Handshake Procedure](../../../server/clustering/replication/advanced-replication), which will make sure we will start replicating from the last accepted `ETag`.
{NOTE/}

{PANEL/}

{PANEL: Maintaining Consistency Between Nodes}

### Replication Transaction Boundary

The boundary of a transaction is extended across multiple nodes.  
If there are several documents in the same transaction they will be sent in the same replication 
batch to keep the data consistent.  

However this doesn't always ensure the data consistency, since the same document can be modified in a different 
transaction and be sent in a different batch.  

### Replication consistency can be achieved by -  

* Using [Write Assurance](../../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
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
