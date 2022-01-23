# Sharding: Overview

* **Sharding** is the distribution of database contents between autonomous 
  [Shards](../sharding/overview#shards), each shard storing and managing 
  its **unique subset** of the entire database contents.  

* In most cases, sharding is implemented to enable efficient management of 
  exceptionally large databases.  

* Clients' access to sharded databases is similar to their access to non-sharded 
  databases, requiring **no special adaptation** on the client side.  
   * The client API is **unchanged** under a sharded database.  

* Changes in RavenDB features under a sharded database are documented 
  in detail in feature-specific articles.  

---

{NOTE: }

* In this page:  
  * [Sharding](../sharding/overview#sharding)  
     * [When Should Sharding be Used?](../sharding/overview#when-should-sharding-be-used)  
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
storing and managing it become too demanding for any single node: 
The system's **performance** suffers as its resources, i.e. RAM, CPU, 
and storage, are exhausted, **routine chores** like indexing and backup 
become massive tasks, **responsiveness** to client requests and queries 
drops, and the system's **throughput** spreads thin among an ever-growing 
number of users.  

**Sharding** means scaling out a database by splitting its contents 
between autonomous [shards](../sharding/overview#shards).  
As the database continues to grow, additional shards can be added 
to share the burden, setting a much higher limit to the database's 
potential capacity and growth.  
The overall database, comprised of all shards, can grow in this fashion 
to any size while keeping the resources of each shard in check and 
maintaining its high performance and throughput.  

---

### When Should Sharding be Used?

While sharding solves many issues related to the storage and management 
of high-volume databases, its implementation does come with an overhead 
that outweighs its benefits when the database is smaller than 250 GB or so.  

{NOTE: }
If you create a **new database** and expect it to contain hundreds of 
millions of records or more, e.g. if it is likely to reach a billion 
records within a year, we recommend that you create a sharded database 
for your contents.  

If your **existing database** is non-sharded, we recommend that you transit 
your data to a sharded database when its size is in the vicinity of 250 GB.  
You should probably be well after the transition when its size reaches 1 TB.  
{NOTE/}

{NOTE: }

* Sharding is available for RavenDB ver. 6.0 and higher.  
* Data can be migrated between sharded and non-sharded databases, 
  but the migration is an expensive process.  
{NOTE/}

{PANEL/}

---

{PANEL: Shards}

While each cluster node of a non-sharded database handles a full replica 
of the entire database, each **Shard** of a sharded database stores and 
administers a **subset** of the entire database contents.  

{NOTE: }
Take for example a 3-shards database, in which the document `Users/1` is 
stored in shard **A**, `Users/2001` is stored in shard **B**, and  `Users/4001` 
is stored in shard **C**.  
A client requesting this database for `Users/2001`, will be routed by 
the cluster to shard **B** and served by it.  
{NOTE/}

---

### Shard Replication 

Each shard is replicated by multiple cluster nodes that form a **Shard 
Database Group**, to ensure the continuous availability of the shard in case 
one of the member nodes fails, provide clients with multiple access points, 
and load-balance the traffic between shard replicas.  

The number of nodes a shard is replicated to is determined by 
the **Shard Replication Factor**.  

!["Shard Replication"](images/sharding-replication-factor.png "Shard Replication")

* In the image above, a 3-shards database is hosted by a 5-nodes cluster (where 
  the two remaining nodes, **D** and **E**, host a non-sharded database).  
  The Shard Replication Factor is set to 2, maintaining two replicas of each shard.  

{PANEL/}

{PANEL: Buckets}

Documents are stored in a sharded database within virtual containers named **Buckets**.  
The number of documents and the amount of data stored in each bucket may vary.  

---

### Buckets Allocation

The number of buckets allocated for the whole database is fixed, always remaining 
**2^10** (a little over a million).  
Each shard is assigned by the cluster with a range of buckets from this overall 
portion, in which documents [and their extensions](../sharding/overview#document-extensions-storage) 
can be stored.  

!["Buckets Allocation"](images/buckets-allocation.png "Buckets Allocation")

{NOTE: }
We expect that each bucket would contain about 1 MB of data per 1 TB of 
data stored in the entire database.  
{NOTE/}

---

### Buckets Population

Buckets are populated with documents automatically by the cluster.  

* The XXHash64 hash algorithm is executed over each document ID.  
* The resulting hash code, a number between 0 and [2^10](../sharding/overview#buckets-allocation), 
  is the number of the bucket in which the document will be stored.  

!["Buckets Population"](images/buckets-population.png "Buckets Population")

---

### Document Extensions Storage

Document extensions (i.e. Attachments, Time series, Counters, and Revisions) 
are stored **in the same bucket as the document they belong to**.  
To make this happen, the bucket number ([hash code](../sharding/overview#buckets-population)) 
that document extensions are given, is calculated by the ID of the document 
that owns them.  

---

### Forcing Documents to Share a Bucket

When the cluster assigns a buckets number for a document, this also 
determines in which shard the document will be stored (since the bucket 
may be stored in any of the shards).  
However, you can force the cluster to assign a chosen document 
to the same bucket (and shard) as another document you choose.  

* To make two documents share a bucket, add the document ID of one of 
  them the following suffix:  
  `$` + `ID`  
  Where `ID` is the ID of the document you want it to share a bucket with.  
    {NOTE: }
    E.g. -  
    Original ID: `Orders/120`  
    The document you want Orders/120 to share a bucket with: `Users/4`  
    New ID: `Orders/120$Users/4`
    {NOTE/}

* The cluster will assign the document to a bucket by its suffix` ID.  
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
database, in which different shards handle about the same volume of data 
as each other.  
Resharding is performed gradually, at a steady pace, to make sure it 
has no damaging effect on the shards' performance and availability.  

{PANEL/}

{PANEL: Creating a Sharded Database}

{NOTE: }

* A sharded database can be created via API or using Studio.  

* A RavenDB cluster can run sharded and non-sharded databases in parallel.  

* Sharding is enabled by the database license, no further steps are required.
{NOTE/}

To create a sharded database via API, use [CreateDatabaseOperation](../client-api/operations/server-wide/create-database) as follows.  

{CODE-BLOCK:csharp}
store.Maintenance.Server.Send(
    new CreateDatabaseOperation(
        new DatabaseRecord(database), 
        replicationFactor: 2, // Shard Replication Factor
        shardFactor: 3)); // Number of Shards
{CODE-BLOCK/}

* Read about **shard replication** and the Shard Replication Factor [here](../sharding/overview#shard-replication).  

{PANEL/}

## Related articles

**Client API**  
[Create Database](../client-api/operations/server-wide/create-database)  

**External Links**  
[very large Database](https://en.wikipedia.org/wiki/Very_large_database)  


