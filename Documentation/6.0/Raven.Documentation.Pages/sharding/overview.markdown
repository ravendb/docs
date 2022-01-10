# Sharding: Overview

* **Sharding**, as implemented by RavenDB, is the distribution of a database's 
  contents between cluster nodes so each node, aka **Shard**, is responsible 
  for the storage and management of a **unique subset of the entire dataset**.  
  E.g., Shard A may be responsible for documents of the Users collection, and 
  Shard B for documents of the Orders collection.  

* Sharding is implemented in most cases to allow the efficient management of an 
  exceptionally large database.  

* Clients access to the database is unaffected, and the client API remains in 
  most part unchanged.  

* Changes in the behavior and usage of features like Indexing, Data Subscriptions, 
  and others, under a sharded databases, are documented in this article and in 
  articles specific to each feature.  

* A sharded database can be created via API or Studio.  

---

{NOTE: }

* In this page:  
  * [Sharding](../)  
  * Design  
     * Buckets  
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



