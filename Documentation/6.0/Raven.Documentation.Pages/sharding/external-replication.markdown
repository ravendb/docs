# Sharding: External Replication
---

{NOTE: }

* [External Replication](../server/ongoing-tasks/external-replication) is an 
  ongoing RavenDB task that you can create and run to maintain a live replica 
  of your database on another RavenDB server.  
* Defining an external replication task via [Studio](../studio/database/tasks/ongoing-tasks/external-replication-task#definition) 
  or [API](../server/ongoing-tasks/external-replication#code-sample) under a 
  sharded database is similar to defining such tasks under a non-sharded database.  
* Sharded and non-sharded databases can replicate data to each other, providing 
  their version is at least 6.0.  

* In this page:  
  * [Supported Versions](../sharding/external-replication#supported-versions)  
  * [External Replication Types](../sharding/external-replication#external-replication-types)
     * [Non-Sharded Database to Sharded Database](../sharding/external-replication#non-sharded-database-to-sharded-database)  
     * [Sharded Database to Sharded Database](../sharding/external-replication#sharded-database-to-sharded-database)  

{NOTE/}

---

{PANEL: Supported Versions}

* A sharded database and a non-sharded database **can** replicate data to each 
  other, providing their versions are 6.0 or higher.  
* Replicating data between a sharded database and a RavenDB version earlier 
  than 5.4 is **not supported**.  
* Non-sharded databases **can** replicate data to each other regardless of 
  their version. E.g., a non-sharded 6.0 database can replicate data to a 5.2 
  database and vice versa.  

{PANEL/}

{PANEL: External Replication Types}

{INFO: Internal -vs- External Replication}

* **Internal replication** is applied automatically when the replication 
  factor is larger than 1, to make the shard database more available by 
  maintaining multiple accessible copies of it.  
  Learn more about shards internal replication in the [overview](../sharding/overview#shard-replication) 
  article and administration [Studio](../sharding/administration/studio-admin) 
  and [API](../sharding/administration/api-admin) articles.  
  
* **External replication** is applied when a dedicated task is defined for it.  
  Read more about it and follow a step-by-step guide 
  [here](../studio/database/tasks/ongoing-tasks/external-replication-task#step-by-step-guide).  
{INFO/}

All data replicated by or to a sharded database is mediated via 
[orchestrators](../sharding/overview#client-server-communication). 
The shards themselves are oblivious to their being shards: from 
a shard's perspective, it is just a regular RavenDB database that 
can, among its other ordinary RavenDB features, replicate data.  

External replication from and to non-sharded databases requires 
no special syntax or preparations. It does, however, cost the server 
some additional work, that, especially when the database is large 
and every extra operation counts, should be taken into account by 
the administrator. Here is how external replication works behind 
the scenes.  

## Non-Sharded Database to Sharded Database

The image below depicts a non-sharded database replicating data to a 5-shard database.  

![Non-Sharded Database to Sharded Database](images/external-replication_non-sharded-to-sharded.png "Non-Sharded Database to Sharded Database")

1. **Non-Sharded Database**  
2. **Replication to Sharded Database**  
   The database is unaware that the destination database is sharded, 
   no special syntax or preparation is needed.  
3. **Orchestrator**  
   The orchestrator receives and prepares the replicated data, 
   grouping documents and document extensions by document IDs so each 
   entity can be stored in the correct shard.  
4. **Transfer to Shard**  
   The orchestrator transfers each destination shard its data.  
   Optimization routines are applied to make the process as 
   effective as possible.  
5. **Shard**  
   Document and document extensions are assigned to buckets by document ID.  
   Shard replies to replicated data and replication attempts are similar 
   to replies made by non-sharded databases.  

## Sharded Database to Sharded Database

* The image below depicts a 3-shard database replicating data to a 5-shard database.  
* Each shard replicates its data as an autonomous database.  

![Sharded Database to Sharded Database](images/external-replication_sharded-to-sharded.png "Sharded Database to Sharded Database")

1. **DB 1 Shard**  
   The shard is unaware that the destination database is sharded.  
2. **Replication to DB 2**  
   The database is unaware that the destination database is sharded, 
   no special syntax or preparation is needed.  
3. **DB 2 Orchestrator**  
   The orchestrator receives and prepares the replicated data, 
   grouping documents and document extensions by document IDs so each 
   entity can be stored in the correct shard.  
4. **Transfer to Shard**  
   The orchestrator transfers each destination shard its data.  
   Optimization routines are applied to make the process as 
   effective as possible.  
5. **DB 2 Shard**  
   Documents and document extensions are assigned to buckets by document ID.  
   Shard replies to replicated data and replication attempts are similar 
   to replies made by non-sharded databases.  

{PANEL/}

## Related articles

### External Replication
[Overview](../server/ongoing-tasks/external-replication)  
[Studio Admin](../studio/database/tasks/ongoing-tasks/external-replication-task#definition)  
[API Admin](../server/ongoing-tasks/external-replication#code-sample)
[Step-by-Step Guide](../studio/database/tasks/ongoing-tasks/external-replication-task#step-by-step-guide)  

### Sharding
[Overview](../sharding/overview#shard-replication)  
[Studio Administration](../sharding/administration/studio-admin) 
[API Administration](../sharding/administration/api-admin)  
[Orchestrators](../sharding/overview#client-server-communication)  
