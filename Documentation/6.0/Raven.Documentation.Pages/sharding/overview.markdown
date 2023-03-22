# Sharding: Overview
---

{NOTE: }

* **Sharding**, supported by RavenDB from version 6.0 and on, 
  is the distribution of a database's content between autonomous 
  **Shards**.  

* In most cases, sharding is implemented to allow efficient usage 
  and management of exceptionally large databases (i.e. a 10-terabyte DB).  

* Sharding is managed by the RavenDB server, no special adaptation 
  is required from clients when accessing a sharding-capable server 
  or a sharded database.  
    * The client API is **unchanged** under a sharded database.  
      Clients of RavenDB versions older than 6.0 (that do not 
      provide sharding) can seamlessly connect a sharded database,
      without making any adaptations or even knowing that the 
      database they connect is sharded.  
    * Particular modifications in RavenDB features under a sharded 
      database are documented in detail in feature-specific articles.  

* Each [Shard](../sharding/overview#shards) hosts and manages 
  a **unique subset** of the database content.  
  Documents are sorted between shards by their **document ID**.  

* Each RavenDB shard is hosted by at least one cluster node.  
  Shards can be replicated over multiple nodes to increase data 
  accessibility.  

* In this page:  
  * [Sharding](../sharding/overview#sharding)  
     * [Client-Server Communication](../sharding/overview#client-server-communication)  
     * [When Should Sharding Be Used?](../sharding/overview#when-should-sharding-be-used)  
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
throughput spreads thin serving an ever-growing number of clients.  

As the volume of stored data grows, the database can be scaled out by 
splitting it into [shards](../sharding/overview#shards), allowing it to be 
handled by multiple nodes and presenting practically no limit to its growth.  
The size of the overall database, comprised of all shards, can reach in 
this fashion dozens of terabytes and more while keeping the resources 
of each shard in check and maintaining its high performance and throughput.  

---

### Client-Server Communication

As a client connects a sharded database, it is appointed a RavenDB server 
that functions as an **orchestrator** and mediates all the communication 
between the client and the database shards.  
The client remains unaware of this process and uses the same API used by 
non-sharded databases to load documents, query, and so on.  
The additional communication between the client and the orchestrator and 
between the orchestrator and the shards does, however, present an overhead 
over the usage of a non-sharded database.  

---

### When Should Sharding Be Used?

While sharding solves many issues related to the storage and management 
of high-volume databases, the overhead it presents outweighs its benefits 
when the database size still poses no problem. We can postpone the transit 
to a sharded database when, for example, the database size is 100 GB, the 
server is well equipped and would comfortably handle a much larger volume, 
and no dramatic increase is expected in the number of potential users 
any time soon.  

{NOTE: }
We recommend that you plan ahead for a transition to a sharded database when 
your database size is in the vicinity of 250 GB, so the transition is already well 
established when it reaches 500 GB.  
{NOTE/}

{NOTE: }
RavenDB 6.0 and above can **migrate** its database to a sharded database 
via [external replication](../server/ongoing-tasks/external-replication) 
or [export & import](../studio/database/tasks/export-database) operations.  

You cannot, however, upgrade a non-sharded database into a sharded one.  
To upgrade RavenDB to 6.0 and migrate the database data you will need 
to upgrade the server, create a new, sharded database, and replicate or 
export the data into it.  
{NOTE/}

{PANEL/}

---

{PANEL: Shards}

While each cluster node of a non-sharded database handles a full replica 
of the entire database, each **shard** is assigned a **subset** of the 
entire database content.  
{NOTE: }
Take, for example, a 3-shards database, in which shard **1** is populated with 
documents `Users/1`..`Users/2000`, shard **2** with documents `Users/2001`..`Users/4000`, 
and shard **3** with documents `Users/4001`..`Users/6000`.  
A client that connects this database to retrieve `Users/3000` and `Users/5000` 
would be served by an automatically-appointed [orchestrator node](../sharding/overview#client-server-communication) 
that would seamlessly retrieve `Users/3000` from shard **2** and `Users/5000` from 
shard **3** and hand them to the client.  
{NOTE/}

---

### Shard Replication 

Similarly to non-sharded databases, shards can be **replicated** by cluster nodes 
to ensure the continuous availability of all shards in case of a node failure, 
provide multiple access points, and load-balance the traffic between shard replicas.  

The number of nodes a shard is replicated to is determined by 
the **Shard Replication Factor**.  

!["Shard Replication"](images/overview_sharding-replication-factor.png "Shard Replication")

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
Each shard is assigned a range of buckets from this overall portion, in which 
documents can be stored.  

!["Buckets Allocation"](images/overview_buckets-allocation.png "Buckets Allocation")

---

### Buckets Population

Buckets are populated with documents automatically by the cluster.  
A hash algorithm is executed over each document ID. The resulting 
hash code, a number between 0 and 1,048,576, is the number of the 
bucket in which the document is stored.  

!["Buckets Population"](images/overview_buckets-population.png "Buckets Population")

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
Rename `Users/70` to: `Users/70$Users/4`
{NOTE/}

!["Forcing Documents to Share a Bucket"](images/overview_force-docs-to-share-bucket.png "Forcing Documents to Share a Bucket")

{WARNING: }
Be careful not to force the storage of too many documents in the same bucket 
(and shard), to prevent the creation of an imbalanced database in which one 
of the shards is overpopulated and others are underpopulated.  
{WARNING/}

{PANEL/}

{PANEL: Resharding}

**Resharding** is the reallocation of data from one shard 
to another, to maintain a balanced database in which all shards 
handle about the same volume of data.  

The resharding process moves all the data related to a certain 
bucket, including documents, document extensions, tombstones, etc., 
to a different shard, and then associates the bucket with the new shard.  
{NOTE: }
  E.g.,

  1. Bucket `100,000` was initially associated with shard **1**.  
     Therefore, all data added to this bucket has been stored in shard **1**.  
  2. Resharding bucket `100,000` to shard **2** will:  
      * Move all the data that belong to this bucket to shard **2**.  
      * Associate bucket `100,000` with shard **2**.  
        From now on, any data added to this bucket will be stored in shard **2**.  
{NOTE/}

{PANEL/}

{PANEL: Creating a Sharded Database}

{NOTE: }

* A sharded database can be created via API or using Studio.  

* A RavenDB cluster can run sharded and non-sharded databases in parallel.  

* RavenDB (6.0 and on) supports sharding by default, no further steps are required to enable the feature.
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

**Server**  
[External Replication](../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../studio/database/tasks/export-database)  

