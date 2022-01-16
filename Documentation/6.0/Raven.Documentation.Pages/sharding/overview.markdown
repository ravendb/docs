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

{PANEL: Shards}

While each cluster node of an Unsharded database handles a full replica 
of the entire database, each node of a Sharded database, aka **Shard**, 
is assigned with a **subset** of the entire database contents.  
{NOTE: }
Take for example a 3-shards database, in which shard **A** is populated with 
documents `Users/1` to `Users/2000`, shard **B** with documents `Users/2001` 
to `Users/4000`, and shard **C** with documents `Users/4001` to `Users/6000`.  
A client requesting this database for `Users/3000`, will be routed by 
the cluster to shard **B** and served by it.  
{NOTE/}

* **Shards and Huge Databases**  
  The **size of the overall database**, comprised of all shards, can thus 
  grow to dozens and even hundreds of terabytes, while keeping each shard's 
  storage in a manageable size and maintaining its high performance and throughput.  

* **Shards and Indexing**  
  Indexing is one of the most demanding tasks that a database is required 
  to perform, and one of the most rewarding. As the database grows bigger, 
  both ends of the equation become more apparent: the price for indexing 
  in CPU usage and its reward in instant replies to client requests.  
  Implementing sharding with an exceptionally large database splits 
  indexing tasks between shards, minimizing each shard's indexing effort 
  and accelerating the service for a smaller number of clients.  

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
The number of documents occupying each shard and their sizes may vary.  

---

### Buckets Allocation

The number of buckets allocated for the whole database is fixed, always remaining 
**1,048,576** (1024 times 1024). Each shard is assigned with a partial range of 
buckets from this overall portion, in which documents can be stored.  

---

### Buckets Population

Buckets are populated with documents automatically by the cluster.  
A hash algorithm is executed over each document ID. The resulting 
hash code, a number between 0 and 1,048,576, is the number of the 
bucket in which the document is stored.  

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
Original document ID: `Users/1`  
The document you want Users/1 to share a bucket with: `Users/2`  
Rename `Users/1` to: `Users/1$Users/2`
{NOTE/}

{WARNING: }
Be careful not to force the storage of too many documents in the same bucket 
(and shard), to prevent the creation of an imbalanced database in which one 
of the shards is overpopulated and the others are underpopulated.  
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

To create a sharded database via API, use `CreateDatabaseOperation` as follows.  

{CODE-BLOCK:csharp}
store.Maintenance.Server.Send(
    new CreateDatabaseOperation(
        new DatabaseRecord(database), 
        replicationFactor: 2, // Sharding Replication Factor
        shardFactor: 3)); // Sharding Factor
{CODE-BLOCK/}

{PANEL/}

## Related articles

**Integrations**  
[Integrations: Power BI](../../integrations/postgresql-protocol/power-bi)  

**Studio**  
[Studio: Integrations and Credentials](../../studio/database/settings/integrations)  

**Security**  
[Setup Wizard](../../start/installation/setup-wizard)  
[Security Overview](../../server/security/overview)  

**Settings**  
[settings.json](../../server/configuration/configuration-options#json)  

**Additional Links**  
[Microsoft Power BI Download Page](https://powerbi.microsoft.com/en-us/downloads)  



