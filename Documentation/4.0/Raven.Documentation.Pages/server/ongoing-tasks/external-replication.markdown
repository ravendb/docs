# Ongoing Tasks: External Replication
---

{INFO: }

* [Single cluster replication](../../server/clustering/replication/replication) is default between nodes in a cluster.  
  Replication between different clusters is called **External Replication**. 

* In this page: 
   * [About External Replication](../../server/ongoing-tasks/external-replication#about-external-replication)
   * [Maintaining Consistency Boundaries Between Clusters](../../server/ongoing-tasks/external-replication#maintaining-consistency-boundaries-between-clusters)
   * [Delayed Replication](../../server/ongoing-tasks/external-replication#delayed-replication)

{INFO/}

{PANEL: About External Replication}

We call the task of replicating between two different database groups _external replication_.  
There is actually no difference in the implementation of an _external replication_ and a regular replication process.  
The reason we define them differently is because of the default behavior of a cluster to setup well connected _database groups_.  
This may be limiting if you wish to design your own replication topology and _external replication_ is the solution for those unique cases.  

{PANEL/}

{PANEL: Maintaining Consistency Boundaries Between Clusters}

[Consistency boundaries](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)
between clusters are crucial to preserve data integrity and model an efficient global system.  

**Be sure to create business logic which ensures that two clusters don't write on the same document.** 

{INFO: To maintain consistency boundaries between clusters}
You can establish document uniqueness by:

* Ensuring that the node-tags are all unique. 
   * e.g. (NYC-nodes A,B,C), (LDN-nodes D,E,F)  
* Including the cluster names in the [identifiers](../../client-api/document-identifiers/working-with-document-identifiers). 
   * e.g. (NYC/Customers/12345), (LDN/Customers/12345)  
* Using a Globally Unique Identifier ([GUID](../../server/kb/document-identifier-generation#guid)).  
* Using a unique field such as an email address.  
{INFO/}

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
