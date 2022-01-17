# Sharding: Overview

* **Sharding** is the distribution of database contents between autonomous **Shards**.  

* A [Shard](../sharding/overview#shards) is a RavenDB cluster node that is responsible 
  for the storage and management of a **unique subset** of the entire database contents.  

* In most cases, sharding is implemented to enable efficient management of 
  exceptionally large databases.  

* Clients' access to sharded databases is similar to their access to unsharded 
  databases, requiring no special adaptation on the client side.  
   * The client API is **unchanged** under a sharded database.  

* Changes in RavenDB features under a sharded database are documented 
  in detail in feature-specific articles.  

---

{NOTE: }

* In this page:  
  * [Sharding](../sharding/overview#sharding)  
     * [When Should I Use Sharding?](../sharding/overview#when-should-i-use-sharding)  
  * [Shards](../sharding/overview#shards)  
     * [Shard Replication](../sharding/overview#shard-replication)  
  * [Buckets](../sharding/overview#buckets)  
     * [Buckets Allocation](../sharding/overview#buckets-allocation)  
     * [Buckets Population](../sharding/overview#buckets-population)  
     * [Document Extensions Storage](../sharding/overview#document-extensions-storage)  
     * [Forcing Documents to Share a Bucket](../sharding/overview#forcing-documents-to-share-a-bucket)  
  * [Resharding](../sharding/overview#resharding)  
  * [Creating a Sharded Database](../sharding/overview#creating-a-sharded-database)  

{NOTE/}

---

{PANEL: Sharding}

As a database grows [very large](https://en.wikipedia.org/wiki/Very_large_database), 
storing and managing it may become too demanding for any single node.  
System performance may suffer as resources like RAM, CPU, and storage are 
exhausted, routine chores like indexing and backup become massive tasks, 
responsiveness to client requests and queries slows down, and the system's 
throughput spreads thin, serving an ever-growing number of clients.  

As the volume of stored data grows, the database can be scaled out by 
splitting it to [shards](../sharding/overview#shards), allowing it to be 
handled by multiple nodes and presenting practically no limit to its growth.  
The size of the overall database, comprised of all shards, can reach in 
this fashion dozens of terabytes and more while keeping the resources 
of each shard in check and maintaining its high performance and throughput.  

---

### When Should I Use Sharding?

While sharding solves many issues related to the storage and management 
of high-volume databases, its implementation does present an overhead that 
outweighs its benefits when the database is smaller than 250GB or so 
(assuming the node can still comfortably handle this volume).  

{NOTE: }
We recommend that you plan ahead for a transition to a sharded database when 
your database size is in the vicinity of 250GB. You should probably be well 
after the transition when it reaches 500GB.  
{NOTE/}

{NOTE: }

* A database of a version under 6.0 cannot currently be migrated to a sharded database.  
* An unsharded database cannot currently be turned to a sharded database,  
  and a sharded database cannot currently be turned to an unsharded database.
{NOTE/}

{PANEL/}

---

{PANEL: Shards}

While each cluster node of an Unsharded database handles a full replica 
of the entire database, each node of a Sharded database, aka **Shard**, 
is assigned with a **subset** of the entire database contents.  
{NOTE: }
Take for example a 3-shards database, in which shard **A** is populated with 
documents `Users/1`..`Users/2000`, shard **B** with documents `Users/2001`..`Users/4000`, 
and shard **C** with documents `Users/4001`..`Users/6000`.  
A client requesting this database for `Users/3000`, will be routed by 
the cluster to shard **B** and served by it.  
{NOTE/}

---

### Shard Replication 

Similarly to unsharded databases, shards can be **replicated** by cluster nodes 
to ensure the continuous availability of all shards in case of a node failure, 
provide multiple access points, and load-balance the traffic between shard replicas.  

The number of nodes a shard is replicated to is determined by 
the **Shard Replication Factor**.  

!["Shard Replication"](images/sharding-replication-factor.png "Shard Replication")

* In the image above, a 3-shards database is hosted by a 5-nodes cluster (where 
  two of the nodes, **D** and **E**, are unused by this database).  
  The Shard Replication Factor is set to 2, maintaining two replicas of each shard.  

{PANEL/}

{PANEL: Buckets}

Documents are stored in a sharded database within virtual containers named **Buckets**.  
The number of documents and the amount of data stored in each bucket may vary.  

---

### Buckets Allocation

The number of buckets allocated for the whole database is fixed, always remaining 
**1,048,576** (1024 times 1024).  
Each shard is assigned with a range of buckets from this overall portion, in which 
documents can be stored.  

!["Buckets Allocation"](images/buckets-allocation.png "Buckets Allocation")

---

### Buckets Population

Buckets are populated with documents automatically by the cluster.  
A hash algorithm is executed over each document ID. The resulting 
hash code, a number between 0 and 1,048,576, is the number of the 
bucket in which the document is stored.  

!["Buckets Population"](images/buckets-population.png "Buckets Population")

As buckets are spread among different shards, the bucket number 
allocated for a document also determines which shard the document 
will reside on.  

---

### Document Extensions Storage

Document extensions (i.e. Attachments, Time series, Counters, and 
Revisions) are stored in the same bucket as the document they belong to.  
To make this happen, the bucket number (hash code) they are given 
is calculated by the ID of the document that owns them.  

---

### Forcing Documents to Share a Bucket

The cluster can be forced to store a document in the same bucket 
(and therefore in the same shard) as another document.  
To do this, add the document name a suffix: `$` + <`ID`>  
The cluster will calculate the document's bucket number by 
the suffix ID, and store the document in this bucket.  

{NOTE: }
E.g. - 
Original document ID: `Users/70`  
The document you want Users/70 to share a bucket with: `Users/4`  
Rename `Users/1` to: `Users/70$Users/4`
{NOTE/}

!["Forcing Documents to Share a Bucket"](images/force-docs-to-share-bucket.png "Forcing Documents to Share a Bucket")

{WARNING: }
Be careful not to force the storage of too many documents in the same bucket 
(and shard), to prevent the creation of an imbalanced database in which one 
of the shards is overpopulated and others are underpopulated.  
{WARNING/}

{PANEL/}

{PANEL: Resharding}

**Resharding** is a continuous, automatic process in which buckets are 
reallocated by the cluster from one shard to another to maintain a balanced 
database, in which shards handle about the same volume of data.  
Resharding is performed gradually, at a steady pace, to make sure that 
the shards' performance is unhurt.  

{PANEL/}

{PANEL: Creating a Sharded Database}

{NOTE: }

* A sharded database can be created via API or using Studio.  

* A RavenDB cluster can run sharded and unsharded databases in parallel.  

* Sharding is enabled by the database license, no further steps are required.
{NOTE/}

To create a sharded database via API, use [CreateDatabaseOperation](../client-api/operations/server-wide/create-database) as follows.  

{CODE-BLOCK:csharp}
store.Maintenance.Server.Send(
    new CreateDatabaseOperation(
        new DatabaseRecord(database), 
        replicationFactor: 2, // Sharding Replication Factor
        shardFactor: 3)); // Sharding Factor
{CODE-BLOCK/}

{PANEL/}

## Related articles

**Client API**  
[Create Database](../client-api/operations/server-wide/create-database)  



