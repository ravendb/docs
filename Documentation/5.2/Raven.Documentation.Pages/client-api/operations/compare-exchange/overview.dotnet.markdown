# Compare Exchange Overview 
---

{NOTE: }

* Compare Exchange items are key/value pairs where the key serves a unique value across your database. 

* Compare-exchange operations require cluster consensus to ensure consistency across all nodes.  
  Once a consensus is reached, the compare-exchange items are distributed through the Raft algorithm to all nodes in the database group

* When creating documents using a cluster-wide session RavenDB automatically creates [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards),  
   which are compare-exchange items that guarantee ACID transactions.  
   Cluster-wide transactions prioritize consistency over performance -  

* Compare-exchange items are [not replicated externally](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-databases) to other databases.

* In this page:  
 * [What Compare Exchange Items Are](../../../client-api/operations/compare-exchange/overview#what-compare-exchange-items-are)  
 * [How to Create and Manage Compare-Exchange Items](../../../client-api/operations/compare-exchange/overview#how-to-create-and-manage-compare-exchange-items)  
 * [Using Compare-Exchange Items](../../../client-api/operations/compare-exchange/overview#using-compare-exchange-items)  
 * [When to Use Cluster-Wide Sessions or Node-Local Sessions](../../../client-api/operations/compare-exchange/overview#when-to-use-cluster-wide-sessions-or-node-local-sessions)  
 * [Why Compare-Exchange Items are Not Replicated to External Databases](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-databases)  
 * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
 * [Example II - Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  
 * [Example III - Ensuring Unique Values without Using Compare Exchange](../../../client-api/operations/compare-exchange/overview#example-iii---ensuring-unique-values-without-using-compare-exchange)  

{NOTE/}

---

{PANEL: What Compare Exchange Items Are}

Compare Exchange items are key/value pairs where the key servers a unique value across your database.

* Each compare-exchange item contains: 
  * **A key** - A unique string across the cluster.  
  * **A value** - Can be numbers, strings, arrays, or objects.  
    Any value that can be represented as JSON is valid.
  * **Metadata**  
  * **Raft index** - A version number that is modified on each change.  
    Any change to the value or metadata changes the Raft index.  

* Creating and modifying a compare-exchange item is an atomic, thread-safe [compare-and-swap](https://en.wikipedia.org/wiki/Compare-and-swap) interlocked 
  compare-exchange operation.

{PANEL/}

{PANEL: How to Create and Manage Compare-Exchange Items}
  
Compare exchange items are created and managed with any of the following approaches:

* **Atomic Guards**  
  To guarantee ACIDity across the cluster, RavenDB automatically creates and maintains [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  in cluster-wide sessions.  (As of RavenDB version 5.2)

* **Document Store Operations**  
  You can manage a compare-exchange item as an [Operation on the document store](../../../client-api/operations/compare-exchange/put-compare-exchange-value).  
  This can be done within or outside of a session (cluster-wide or single-node session).
   * When inside a Cluster-wide session:  
     If the session fails, the item is not created on any of the nodes.
   * When inside a Single-node session:  
     If the session fails the compare-exchange operation can still succeed  
     because store Operations do not rely on the success of the session.  
     You will need to delete the compare-exchange item explicitly if you don't want it to persist.

* **Cluster-Wide Sessions**  
  You can manage a compare-exchange item from inside a [Cluster-Wide session](../../../client-api/session/cluster-transaction).  
  If the session fails, the compare-exchange item creation also fails.  
  None of the nodes in the group will have the new compare-exchange item.

* **Studio**  
  Compare-exchange items can be created from the [Studio](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view) as well.

{PANEL/}

{PANEL: Using Compare-Exchange Items}

#### Why use compare-exchange items

* Compare-exchange items can be used to coordinate work between sessions that are 
  trying to modify a shared resource (such as a document) at the same time.  

* You can use compare-exchange items in various situations to reserve a unique resource.  
   * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
   * [Example II - Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  

* If you create a compare-exchange item, you can decide what actions to implement when the compare-exchange [value changes](../client-api/operations/compare-exchange/put-compare-exchange-value#example-ii---update-an-existing-key).  
  

---

#### How Atomic Guard items work when protecting shared documents in cluster-wide sessions

* Every time a document in a cluster-wide session is modified, RavenDB automatically creates 
  or uses an associated compare-exchange item called [Atomic Guard](../../../client-api/operations/compare-exchange/atomic-guards).  
  * Whenever one session changes and saves a document, the Atomic Guard version changes.  
    If other sessions load the document before the version changed, they will not be able to modify it.  
  * A `ConcurrencyException` will be thrown if the Atomic Guard version was modified by another session.  
* **If a ConcurrencyException is thrown**:  
  To ensure [atomicity](https://en.wikipedia.org/wiki/ACID#Atomicity), if even one action within a session fails, the entire [session will roll back](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching).  
  Be sure that your business logic is written so that if a concurrency exception is thrown, your code will re-execute the entire session.  


{INFO: }
#### Performance cost of cluster-wide sessions  

Cluster-wide transactions are more expensive than node-local transactions due to Raft consensus checks. 
People prefer a cluster-wide transaction when they prioritize consistency over performance and availability.  
It ensures ACIDity across the cluster.  

* One way to protect transactions **without** using cluster-wide sessions is to ensure that one node 
  is responsible for writing on a specific database. Conflicts can occur rarely, but performance is greatly improved. 
    * RavenDB single-node transactions are usually ACID on the local node, but if two nodes were to write concurrently on the same document, [conflicts](../../../server/clustering/replication/replication-conflicts)
      can occur. 
    * By default to prevent conflicts, one primary node is responsible for all reads and writes.  
      You can configure [load balancing](../../../client-api/session/configuration/use-session-context-for-load-balancing)
      to fine-tune the settings to your needs.  
    * To distribute work, you can set different nodes to be responsible for different sets of data.  
      Learn more in the article [Scaling Distributed Work in RavenDB](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/7-scaling-distributed-work-in-ravendb). 
{INFO/}

{PANEL/}

{PANEL: When to Use Cluster-Wide Sessions or Node-Local Sessions}

[Node-local (non-cluster-wide) sessions](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
are much faster and less expensive than cluster-wide sessions.  

**If conflict-free, safe ACID transactions are a must**, [cluster-wide sessions](../../../client-api/session/cluster-transaction) 
guarantee ACID transactions via Atomic Guards and their Raft consensus checks.

{NOTE: }
Your business logic can run both types of sessions as the situation requires.  
{NOTE/}

* **Node-local sessions** are significantly faster and have a default setting that one node is responsible for all reads/writes on a particular database. 
  Node-local transactions are almost always consistent, though if the primary node has to failover, there is a chance of a conflict occurring.  
   * Many situations can handle conflicts and you can program [conflict resolution](../../../server/clustering/replication/replication-conflicts) 
     scripts or use other conflict resolution strategies via [Studio](../../../studio/database/settings/conflict-resolution) 
     and also [in the client](../../../client-api/cluster/document-conflicts-in-client-side#modifying-conflict-resolution-from-the-client-side).
   * Because of the higher performance and lower cost of node-local sessions, we recommend using them in situations where 
     occasional conflicts, and the conflict resolution process, are acceptable.  

* **Cluster-wide sessions** have ACID guarantees for conflict-free transactions.
   * Because of the performance cost of raft consensus checks, we recommend using cluster-wide sessions  
     for transactions where immediate consistency is crucial.  
   * You can also set the session transaction mode to cluster-wide if you need every node to be able to read and write on the same database, 
     instead of the primary node handling all reads and writes in node-local sessions. 

{NOTE: }
There are also tools that provide flexibility such as [revisions](../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration) 
and the ability to [model your documents](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling) 
so that conflicts are prevented.
{NOTE/}

{PANEL/}

{PANEL: Why Compare-Exchange Items are Not Replicated to External Databases }

* Each cluster defines its policies and configurations, and should ideally have sole responsibility for managing its own documents. 
  Read [Consistency in a Globally Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system) 
  to learn more about why global database modeling is more efficient this way.
   
* When creating a compare-exchange item a Raft consensus is required from the nodes in the database group.
  Externally replicating such data is problematic as the target database may reside within a cluster that is in an
  unstable state where Raft decisions cannot be made. In such a state, the compare-exchange item will not be persisted in the target database.

* Conflicts between documents that occur between two databases are solved with the help of the documents
  Change-Vector. Compare-exchange conflicts cannot be handled properly as they do not have a similar
  mechanism to resolve conflicts.

* To ensure unique values between two databases without using compare-exchange items see [Example III](../../../client-api/operations/compare-exchange/overview#example-iii---ensuring-unique-values-without-using-compare-exchange) 
  for similar functionality that can be externally replicated.

{PANEL/}

{PANEL: Example I - Email Address Reservation}  

The following example shows how to use compare-exchange to create documents with unique values.  
The scope is within the database group on a single cluster. 

Compare-exchange items are not externally replicated to other databases.  
To establish uniqueness without using compare-exchange see [example III](../../../client-api/operations/compare-exchange/overview#example-iii---ensuring-unique-values-without-using-compare-exchange).

{CODE email@Server\CompareExchange.cs /}  

**Implications**:

* The `User` object is saved as a document, hence it can be indexed, queried, etc.  

* This compare-exchange item was [created as an operation](../../../client-api/operations/compare-exchange/overview#how-to-create-and-manage-compare-exchange-items)
  rather than with a [cluster-wide session](../../../client-api/session/cluster-transaction).  
  Thus, if `session.SaveChanges` fails, then the email reservation is _not_ rolled back automatically.  
  It is your responsibility to do so.  

* The compare-exchange value that was saved can be accessed in a query using CmpXchg:  
    {CODE-TABS}
    {CODE-TAB:csharp:Query-LINQ query_cmpxchg@Server\CompareExchange.cs /}  
    {CODE-TAB:csharp:Document-Query document_query_cmpxchg@Server\CompareExchange.cs /}  
    {CODE-TAB-BLOCK:sql:RQL}
    from Users as s where id() == cmpxchg("emails/ayende@ayende.com")  
    {CODE-TAB-BLOCK/}
    {CODE-TABS/}
    {PANEL/}

{PANEL: Example II - Reserve a Shared Resource}  

In the following example, we use compare-exchange to reserve a shared resource.  
The scope is within the database group on a single cluster.

The code also checks for clients who never release resources (i.e. due to failure) by using timeout.  

{CODE shared_resource@Server\CompareExchange.cs /}

{PANEL/}

{PANEL: Example III - Ensuring Unique Values without Using Compare Exchange}  

Unique values can also be ensured without using compare-exchange.

The below example shows how to achieve that by using **reference documents**.  
The reference documents' IDs will contain the unique values instead of the compare-exchange items.

Using reference documents is especially useful when [External Replication](../../../server/ongoing-tasks/external-replication) 
is defined between two databases that need to be synced with unique values.  
The reference documents will replicate to the destination database, 
as opposed to compare-exchange items, which are not externally replicated.

{NOTE: }
Sessions which process fields that must be unique should be set to [TransactionMode.ClusterWide](../../../client-api/session/cluster-transaction).  
{NOTE/}

{CODE create_uniqueness_control_documents@Server\CompareExchange.cs /}
{PANEL/}

## Related Articles

### Client API

- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards)
- [Resolving Document Conflicts](../../../client-api/cluster/document-conflicts-in-client-side)


### Studio

- [Compare Exchange View](../../../studio/database/documents/compare-exchange-view)  

### Server

- [Conflict Resolution](../../../server/clustering/replication/replication-conflicts)
- [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions)

---

## Code Walkthrough

- [Create CmpXchg Item](https://demo.ravendb.net/demos/csharp/compare-exchange/create-compare-exchange)  
- [Index CmpXchg Values](https://demo.ravendb.net/demos/csharp/compare-exchange/index-compare-exchange)  

---

## Ayende @ Rahien Blog

- [Consistency in a Globally Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system)


