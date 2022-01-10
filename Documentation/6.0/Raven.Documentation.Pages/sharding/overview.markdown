# Sharding: Overview

* **Sharding** is the division of database contents between autonomous **Shards**.  
  In most cases, it is implemented to allow an efficient management of exceptionally large databases.  

* Each **shard** is responsible for the storage and management of a **unique subset** 
  of the entire database contents.  
  E.g. -  
   * Documents can be allocated to 3 shards: **A**, **B** and **C**, by the document name.  
     Documents starting with `A..` to `I..` will be allocated to shard **A**,  
     documents starting with `J..` to `R..` to shard **B**,  
     and documents starting with `S..` to `Z..` to shard **C**.  
   * If a client requests a document whose name starts with J, the cluster will direct the request to shard **B**.  

* **RavenDB shards** are comprised of **cluster nodes**.  
  The number of cluster nodes that comprise each shard, is determined by the **Sharding Replication Factor**.  
  E.g., a replication factor of 2 determines that each shard is comprised of 2 nodes.  

* Clients access to sharded databases is the same as to unsharded databases, 
  requiring no changes on the client side.  

* The client API is generally unchanged under a sharded database.  
   * An obvious exception to this is the database creation API, which 
     now allows you to create a sharded database and set its options.  

* A sharded database can be created via API or using Studio.  

* Changes in RavenDB features under a sharded databases are addressed in general 
  lines [below]() and documented in detail in feature-specific articles.  

---

{NOTE: }

* In this page:  
  * [Sharding](../)  
  * Design  
     * Terminology  
        * $ - Database$Shard  
        * Buckets  
        * Linked documents - DocumentA$DocumentB
     * Unique subsets  
     * Sharding Factor  
     * DatabaseReplication: Database groups  
  * Enabling Sharding  
  * Sharding and Other Features  
     * Indexing  
     * Data Subscriptions  

{NOTE/}

---

{PANEL: Sharding}

In an **unsharded** database, each node holds a replica of the entire database 
and a client request can therefore extract any part of the data from any node.  
In a **sharded** database, however, each node is responsible for a selected dataset 
that constitutes only a part of the entire database contents.  


Clients remain **unaware** of the database being sharded

{PANEL/}

{PANEL: Design}

### Buckets

{PANEL/}

{PANEL: Enabling Sharding}

Enabling sharding is done simply by creating a sharded database, by API or from Studio.  

Via API, use []() to create a database, as follows:  



{PANEL/}

{PANEL: Sharding and Other Features}

### Sharding and Indexing

### Sharding and Data Subscriptions

### Sharding and Including Documents

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



