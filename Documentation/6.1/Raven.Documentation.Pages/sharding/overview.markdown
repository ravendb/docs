# Sharding Overview
---

{NOTE: }

* Sharding, supported by RavenDB from version 6.0 onward, is the distribution of a database's content among autonomous **Shards**,
  where each Shard hosts and manages a **unique subset** of the database content.

* In most cases, sharding allows the efficient usage and management of exceptionally large databases  
  (e.g., a 10-terabyte DB).

* Shards can be replicated across multiple nodes to enhance data accessibility.  
  Therefore, each RavenDB shard is hosted on at least one cluster node.

* Sharding is managed by the RavenDB server;  
  clients require no special adaptation when accessing a sharded database.
     
    * The client API remains unchanged when using a sharded database.       
    * Clients using RavenDB versions older than 6.0, which lack sharding support,  
      can seamlessly connect to a sharded database without any adaptations or even realizing it is sharded.     
    * Specific modifications to RavenDB features in a sharded environment are documented in detail in feature-specific articles.

* In this page:  
  * [Sharding](../sharding/overview#sharding)  
     * [Licensing](../sharding/overview#licensing)
     * [Client-Server communication](../sharding/overview#client-server-communication)  
     * [When should sharding be used?](../sharding/overview#when-should-sharding-be-used)  
  * [Shards](../sharding/overview#shards)  
     * [Shard replication](../sharding/overview#shard-replication)  
  * [How documents are distributed among shards](../sharding/overview#how-documents-are-distributed-among-shards)  
     * [Buckets allocation](../sharding/overview#buckets-allocation)  
     * [Buckets population](../sharding/overview#buckets-population)  
     * [Document extensions storage](../sharding/overview#document-extensions-storage)  
  * [Resharding](../sharding/overview#resharding)  
  * [Paging](../sharding/overview#paging)  
  * [Using local IP addresses](../sharding/overview#using-local-ip-addresses)  
  * [Creating a sharded database](../sharding/overview#creating-a-sharded-database)  

{NOTE/}

---

{PANEL: Sharding}

As a database grows [very large](https://en.wikipedia.org/wiki/Very_large_database), storing and managing it may become too demanding for any single node.  
System performance may suffer as resources like RAM, CPU, and storage are exhausted, routine chores like indexing and backup become massive tasks, 
responsiveness to client requests and queries slows down, and the system's throughput spreads thin serving an ever-growing number of clients.  

With sharding, as the volume of stored data grows, the database can be scaled out by splitting it into [shards](../sharding/overview#shards).  
This allows the database to be managed by multiple nodes and effectively removes most limits on its growth.  
In this manner, the size of the overall database, comprised of all shards, can reach dozens of terabytes and more,  
while keeping the resources of each shard in check and maintaining high performance and throughput.  

---

#### Licensing

{INFO: }
Sharding is fully available with the **Enterprise** license.  
{INFO/}

* On a **Developer** license, the replication factor is restricted to 1.
* On **Community** and **Professional** licenses, all shards must be on the same node.  
* Learn more about licensing [here](../start/licensing/licensing-overview).  

---

#### Client-Server communication

When a client connects to a sharded database, it is appointed a RavenDB server that functions as an **orchestrator**,  
mediating all the communication between the client and the database shards.  
The client remains unaware of this process and uses the same API used by non-sharded databases to load documents, query, and perform other operations.  

Note that this additional communication between the client and the orchestrator, as well as between the orchestrator and the shards,
introduces some overhead compared to using a non-sharded database.  

---

#### When should sharding be used?

While sharding solves many issues related to the storage and management of high-volume databases,  
the overhead it introduces can outweigh its benefits when the database size still poses no problem.  

You can postpone the transit to a sharded database when, for example, the database size is 100 GB,  
the server is well-equipped and can comfortably handle a much larger volume,  
and no dramatic increase in the number of potential users is expected any time soon.  

{NOTE: }

* We recommend that you plan ahead for a transition to a sharded database when your database size  
  is in the vicinity of 250 GB, so the transition is already well established when it reaches 500 GB.  

* RavenDB 6.0 and above can **migrate** a non-sharded database to a sharded database via  
  [external replication](../server/ongoing-tasks/external-replication) or [export & import](../studio/database/tasks/export-database) operations.

* To upgrade a non-sharded database from an earlier version of RavenDB to a sharded one,  
  you need to first upgrade the server to version 6.0 (or later), create a new sharded database,  
  and then replicate or export the data into it.

{NOTE/}
{PANEL/}

{PANEL: Shards}

While each cluster node of a non-sharded database handles a full replica of the entire database,  
each **shard** is assigned a **subset** of the entire database content.  

{NOTE: }
For example:  

Take a 3-shard database, in which shard **1** is populated with documents `Users/1`..`Users/2000`,  
shard **2** contains documents `Users/2001`..`Users/4000`, and shard **3** contains `Users/4001`..`Users/6000`.

A client that connects to this database to retrieve `Users/3000` and `Users/5000` would be served by an
automatically-appointed [orchestrator node](../sharding/overview#client-server-communication)
that would seamlessly retrieve `Users/3000` from shard **2**  
and `Users/5000` from shard **3** and hand them to the client.
{NOTE/}

As far as clients are concerned, a sharded database is still a single entity:  
clients are not required to detect whether the database is sharded or not, 
and clients of RavenDB versions prior to 6.0, which had no sharding support, 
can access a sharded database without any alterations.  

That said, shard-specific operations are also available:  
a client can, for example, track the shard where a document is stored and query that shard.  
The Studio can be used to relocate ([reshard](../sharding/resharding)) documents from one shard to another.  

!["Studio Document View"](images/overview_document-view.png "Studio Document View")

---

#### Shard replication 

Similar to non-sharded databases, shards can be **replicated** across cluster nodes to ensure the continuous availability
of all shards in case of a node failure, provide multiple access points, and load-balance the traffic between shard replicas.  

The number of nodes a shard is replicated to is determined by the **Shard Replication Factor**.  

!["Shard Replication"](images/overview_sharding-replication-factor.png "Shard Replication")

* In the image above, a 3-shard database is hosted by a 5-node cluster  
  (where two of the nodes, **D** and **E**, are not used by this database).  

* The Shard Replication Factor is set to 2, maintaining two replicas of each shard.  

{PANEL/}

{PANEL: How documents are distributed among shards}

#### Buckets
Documents in a sharded database are stored within virtual containers called **buckets**.  
The number of documents and the amount of data stored in each bucket may vary.  

---

#### Buckets allocation

Upon creating a sharded database, the cluster reserves **1,048,576** (1024 x 1024) buckets for the entire database.  
Each shard is assigned a range of buckets from this overall set, where documents can be stored.  
(Note: This default reservation method differs when using prefixed sharding. Learn more in [todo..](../todo..)).

!["Buckets Allocation"](images/overview_buckets-allocation.png "A range of buckets is assigned to each shard")

---

#### Buckets population

The cluster automatically populates the buckets with documents in the following way:

A hashing algorithm is applied to each document ID, generating a number between **0** and **1,048,575**.  
The resulting number determines the bucket number where the document is stored.  
(Note: This default hashing method differs when using prefixed sharding. Learn more in [todo..](../todo..).)  

Since the buckets are pre-assigned to the shards,  
the bucket number assigned to a document also determines which shard the document will reside on.

!["Buckets Population"](images/overview_buckets-population.png "Buckets Population")

{NOTE: }

* **Anchoring documents to a bucket**:  
  You can make documents share a bucket (and therefore a shard) based on their document ID suffix.  
  RavenDB uses this suffix to calculate the bucket number for the document.  
  Learn more in [Anchoring documents to a bucket](../sharding/administration/anchoring-documents).  

* **Anchoring documents to a shard**:   
  You can make documents reside on a specific shard based on their document ID prefix.  
  RavenDB uses this prefix to calculate a bucket number that resides on the requested shard.  
  Learn more in [Sharding by prefix](../sharding/administration/sharding-by-prefix).

{NOTE/}

---

#### Document extensions storage

Document extensions (i.e. Attachments, Time series, Counters, and Revisions) are stored in the same bucket as the document they belong to.
To achieve this, the bucket number (hash code) for these extensions is calculated using the ID of the document that owns them.

{PANEL/}

{PANEL: Resharding}

[Resharding](../sharding/resharding) is the relocation of data from one shard to another to maintain a balanced database,  
where all shards handle approximately the same volume of data.

The resharding process moves all data related to a certain bucket, including documents, document extensions,  
tombstones, etc., to a different shard and then associates the bucket with the new shard.  

{NOTE: }

For example:  

  1. Bucket `100,000` was initially associated with shard **1**.  
     Therefore, all data added to this bucket has been stored in shard **1**.  
  2. Relocating bucket `100,000` to shard **2** will:  
      * Move all the data that belongs to this bucket to shard **2**.  
      * Associate bucket `100,000` with shard **2**.  
      * From now on, any data added to this bucket will be stored in shard **2**.  
{NOTE/}

{PANEL/}

{PANEL: Paging}

From the client's perspective, [paging](../indexes/querying/paging) is conducted similarly in both sharded and non-sharded databases,  
using the same API.  

However, paging in a sharded database is more costly because the orchestrator must load data **from each shard**  
and sort the retrieved results before handing the selected page to the client.  

Read more about paging [here](../sharding/querying#paging).  

{PANEL/}

{PANEL: Using local IP addresses}

The local IP address of a cluster node can be exposed, allowing other cluster nodes to prioritize it over the public IP address when accessing the node.
Using a node's local IP address for inter-cluster communications can speed up the service and offer substantial cost savings over time.

Using this method can be particularly helpful in a sharded cluster, where each client request is handled by an orchestrator
that may need to communicate with all other shards to process the request and its results. 

Use [this configuration option](../server/configuration/core-configuration#serverurl.cluster) to expose a node's local IP address to other nodes.  

{PANEL/}

{PANEL: Creating a sharded database}

* When a database is created, the user can choose whether it will be sharded or not.  
  RavenDB (version 6.0 and later) provides this option by default, with no further steps required to enable the feature.

* A sharded database can be created via the [Studio](../sharding/administration/studio-admin#creating-a-sharded-database) or the [Client API](../sharding/administration/api-admin).

* A RavenDB cluster can run both sharded and non-sharded databases in parallel.  

{PANEL/}

## Related articles

### Sharding

- [Administration: Studio](../sharding/administration/studio-admin)  
- [Administration: API](../sharding/administration/api-admin)  
- [Unsupported Features](../sharding/unsupported)  
- [Migration](../sharding/migration)  
