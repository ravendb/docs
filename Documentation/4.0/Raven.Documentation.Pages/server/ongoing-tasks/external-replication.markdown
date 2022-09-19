# Ongoing Tasks: External Replication

---

{NOTE: }

* [Replication within the cluster](../../server/clustering/replication/replication) automatically syncs data between nodes in a database group.  
  
* **External Replication** is the replication of data between different clusters or between two databases in the same cluster.

* In this page: 
   * [About External Replication](../../server/ongoing-tasks/external-replication#about-external-replication)
   * [Preventing Conflicts Between Clusters](../../server/ongoing-tasks/external-replication#preventing-conflicts-between-clusters)
   * [Delayed Replication](../../server/ongoing-tasks/external-replication#delayed-replication)

{NOTE/}

---

{PANEL: About External Replication}

**What is being replicated:**  

 * All database documents and related data:  
   * Attachments
   * Revisions
   * Counters
   * Time Series

**What is _not_ being replicated:**  

  * Server and cluster level features:  
    * Indexes
    * Conflict resolver definitions
    * Compare-Exchange
    * Subscriptions
    * Identities
    * Ongoing tasks
      * ETL
      * Backup
      * Hub/Sink Replication

{NOTE: Why are cluster-level features not replicated?}
To provide for architecture that prevents conflicts between clusters, especially when ACID transactions are important, 
RavenDB is designed so that data ownership is at the cluster level.  
To learn more, see [Data Ownership in a Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).
{NOTE/}

---

**Conflicts:**  

  * Two databases that have an External Replication task defined between them will detect and resolve document 
    [conflicts](../../server/clustering/replication/replication-conflicts) according to each database conflict resolution policy.  
  * It is recommended to have the same [policy configuration](../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client) on both the source and the target databases.  

We call the task of replicating between two different database groups _external replication_.  
There is actually no difference in the implementation of an _external replication_ and a regular replication process.  
The reason we define them differently is because of the default behavior of a cluster to setup well connected _database groups_.  
This may be limiting if you wish to design your own replication topology and _external replication_ is the solution for those unique cases.  

{PANEL/}

{PANEL: Preventing Conflicts Between Clusters}

[Consistency boundaries](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)
between clusters are crucial to preserve data integrity and model an efficient global system.  

One way to prevent conflicts is to ensure that only one server modifies a document.  

#### To ensure that two clusters don't write on the same document  

Following are a few ways to provide unique IDs across all of your clusters.  

Some of these approaches also make the source cluster clear in the ID so that you can set up logic 
that prevents multiple servers from modifying the same document.

You can ensure document ID uniqueness by:

* Ensuring that the node-tags are all unique.  
  e.g. (NYC-nodes A,B,C), (LDN-nodes D,E,F)  
* Including the cluster names in the [identifiers](../../client-api/document-identifiers/working-with-document-identifiers).  
  e.g. (NYC/Customers/12345), (LDN/Customers/12345)  
* Using a Globally Unique Identifier ([GUID](../../server/kb/document-identifier-generation#guid)).  
* [Using a unique field](../../../client-api/operations/compare-exchange/overview#example-iii---ensuring-unique-values-without-using-compare-exchange) 
  (such as an email address) in the ID is a global way to prevent duplication.  

{PANEL/}

{PANEL: Delayed Replication}

In RavenDB we introduced a new kind of replication, _delayed replication_, what it does is replicating data that is delayed by `X` amount of time.  
The _delayed replication_ works just like normal replication but instead of sending data right away it waits `X` amount of time.  
Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data due to a faulty patch script or other human errors.  
While you can and should always use backup for those cases, having a live database makes it super fast to failover to and prevent business lose while you take down the faulty databases and restore them.  

{PANEL/}

## Related articles

### Replication

- [How Replication Works](../../server/clustering/replication/replication)
