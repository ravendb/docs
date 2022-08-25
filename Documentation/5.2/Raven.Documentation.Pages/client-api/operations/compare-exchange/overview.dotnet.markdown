# Compare Exchange Overview 
---

{NOTE: }

* **Compare-exchange** items are cluster-wide key/value pairs where the key is a unique identifier. 

* **To Ensure ACID Transactions** RavenDB automatically creates [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  in cluster-wide transactions. When using cluster-wide transactions, you do not need to create or maintain compare-exchange items to preserve consistency.  
  * Cluster-wide transactions present a [performance cost](../../../client-api/operations/compare-exchange/overview#performance-cost-of-cluster-wide-sessions) 
    when compared to non-cluster-wide transactions. 
    They prioritize consistency over performance to ensure ACIDity across the cluster.  

* Compare-exchange items are [not replicated externally](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-clusters) to other clusters.

* In this page:  
 * [What Compare Exchange Items Are](../../../client-api/operations/compare-exchange/overview#what-compare-exchange-items-are)  
 * [Using Compare-Exchange Items](../../../client-api/operations/compare-exchange/overview#using-compare-exchange-items)  
 * [When to Use Cluster-Wide Sessions or Node-Local Sessions](../../../client-api/operations/compare-exchange/overview#when-to-use-cluster-wide-sessions-or-node-local-sessions)  
 * [Why Compare-Exchange Items are Not Replicated to External Clusters](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-clusters)  
 * [Transaction Scope for Compare-Exchange Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations)  
 * [Creating a Key](../../../client-api/operations/compare-exchange/overview#creating-a-key)  
 * [Updating a Key](../../../client-api/operations/compare-exchange/overview#updating-a-key)  
 * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
 * [Example II- Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  

{NOTE/}

---

{PANEL: What Compare Exchange Items Are}

Compare Exchange items are key/value pairs that allow you to perform cluster-wide interlocked distributed operations.

* Each compare-exchange item contains: 
  * A key - A unique string across the cluster.  
  * A value - Can be numbers, strings, arrays, or objects.  
    Any value that can be represented as JSON is valid.
  * Metadata  
  * Raft index - A version number that is modified on each change.  
    Any change to the value or metadata changes the Raft index.  

* Creating and modifying a compare-exchange item is an atomic, thread-safe [compare-and-swap](https://en.wikipedia.org/wiki/Compare-and-swap) interlocked 
  compare-exchange operation.
  * The compare-exchange item is distributed to all nodes in a [cluster-wide transaction](../../../server/clustering/cluster-transactions)
    so that a consistent, unique key is guaranteed cluster-wide.  

{PANEL/}

{PANEL: Using Compare-Exchange Items}

#### Why use compare-exchange items

* Compare-exchange items can be used to coordinate work between sessions that are 
  trying to modify a shared resource (such as a document) at the same time.  

* You can use compare-exchange items in various situations to protect or reserve a resource.  
  (see [API Compare-exchange examples](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)).
  * If you create a compare-exchange key/value pair, you can decide what actions to implement when the Raft index increments. 
    The Raft index increments as a result of a change in the compare-exchange value or metadata.  

---

#### How compare-exchange items are managed  

* Compare exchange items are created and managed with any of the following approaches:
  * RavenDB creates and maintains [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) automatically in 
    cluster-wide sessions to guarantee ACIDity across the cluster.  (As of RavenDB version 5.2)
  * Via [Client API Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations)  
  * In [Cluster-Wide Sessions](../../../client-api/session/cluster-transaction)
  * Using [Studio](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view)

---

#### How Atomic Guard items work when protecting shared documents in cluster-wide sessions

* Every time a document in a cluster-wide session is modified, RavenDB automatically creates 
  or uses an associated compare-exchange item called [Atomic Guard](../../../client-api/operations/compare-exchange/atomic-guards).  
  * Whenever one session changes and saves a document, the Atomic Guard version changes. 
    If other sessions loaded the document before the version changed, they will not be able to modify it.  
  * A `ConcurrencyException` will be thrown if the Atomic Guard version was modified by another session.  
* **If a ConcurrencyException is thrown**:  
  To ensure [atomicity](https://en.wikipedia.org/wiki/ACID#Atomicity), if even one session transaction fails, the entire [session will roll back](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching).  
  Be sure that your business logic is written so that **if a concurrency exception is thrown**, your code will re-execute the entire session.  


{INFO: }
#### Performance cost of cluster-wide sessions  

Cluster-wide transactions are more expensive than node-local transactions due to Raft consensus checks. 
People prefer a cluster-wide transaction when they prioritize consistency over performance and availability.  
It ensures ACIDity across the cluster.  

* One way to protect transactions **without** using cluster-wide sessions is to ensure that one node 
  is responsible for writing on a specific database. Conflicts can occur rarely, but performance is greatly improved. 
    * RavenDB node-local transactions are usually ACID on the local node, but if two nodes write concurrently on the same document, [conflicts](../../../server/clustering/replication/replication-conflicts)
      can occur. 
    * By default to prevent conflicts, one primary node is responsible for all reads and writes.  
      You can configure [load balancing](../../../client-api/session/configuration/use-session-context-for-load-balancing)
      to fine-tune the settings to your needs.  
    * To distribute work, you can set different nodes to be responsible for different sets of data.  
      Learn more in the article [Scaling Distributed Work in RavenDB](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/7-scaling-distributed-work-in-ravendb). 
{INFO/}

---

#### When to Use Cluster-Wide Sessions or Node-Local Sessions

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
     scripts or use other conflict resolution strategies via Studio and also [in the client](../../../client-api/cluster/document-conflicts-in-client-side#modifying-conflict-resolution-from-the-client-side).
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

{PANEL: Why Compare-Exchange Items are Not Replicated to External Clusters }

[To prevent consistency conflicts between clusters](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system) 
and model an efficient global system, each cluster should have sole ownership of documents created in it.  

In geo-distributed systems, to avoid latency problems, a new cluster is usually set up in each region.  
But to achieve strong consistency in a distributed system, each transaction must achieve a majority consensus amongst the
involved servers.  Trying to achieve consensus on each transaction between different clusters is usually unrealistic, 
especially considering geographic latency.  

{WARNING: }
If multiple clusters are set to modify the same document and then replicate it to each other, 
there will likely be frequent conflicts.
{WARNING/}

To ensure consistency between clusters, documents created in each cluster are owned and operated only by that cluster.  
Learn how to protect document uniqueness and [local ownership in a global system](../../../server/ongoing-tasks/external-replication#maintaining-consistency-boundaries-between-clusters) 
in our article on External Replication. 

**The rule of thumb for documents created by another cluster - you can look, but don't touch.**

{PANEL/}

{PANEL: Transaction Scope for Compare-Exchange Operations}

Conventionally, compare-exchange **session operations** are used in [cluster-wide sessions](../../../client-api/session/cluster-transaction).  
Since RavenDB 5.2, we automatically create and maintain [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
to guarantee cluster-wide session ACID transactions.

This article is about non-session-specific compare-exchange operations.

* A non-session-specific compare-exchange [operation](../../../client-api/operations/what-are-operations) (described below) 
  is performed on the [document store](../../../client-api/what-is-a-document-store) level.  
  It is therefore not part of the session transactions.  

{WARNING: If a session fails}

* Even if written inside the session scope, a non-session compare exchange **operation** will be executed regardless 
  of whether the session `SaveChanges( )` succeeds or fails. 
  * This is not the case when using compare-exchange [session methods](../../../client-api/session/cluster-transaction)

* Thus, upon a [session transaction failure](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching), 
  if you had a successful compare-exchange operation (as described below) inside the failed session block, 
  it will **not** be rolled back automatically with the failed session.  
{WARNING/}

{PANEL/}

{PANEL: Creating a Key}

Provide the following when saving a **key**:

{CODE put_0@ClientApi\Operations\CompareExchange.cs /}

| Parameter | Description |
| ------------- | ---- |
| **Key** | A string under which _Value_ is saved, unique in the database scope across the cluster. This string can be up to 512 bytes. |
| **Value** | The Value that is associated with the _Key_. <br/>Can be a number, string, boolean, array, or any JSON formatted object. |
| **Index** | The _Index_ number is indicating the version of _Value_.<br/>The _Index_ is used for concurrency control, similar to documents Etags. |

* When creating a _new_ 'Compare Exchange **Key**', the index should be set to `0`.  

* The [Put](../../../client-api/operations/compare-exchange/put-compare-exchange-value) operation will succeed only if this key doesn't exist yet.  

* Note: Two different keys _can_ have the same values as long as the keys are unique.  
{PANEL/}

{PANEL: Updating a Key}

Updating a compare exchange key can be divided into 2 phases:

  1. [Get](../../../client-api/operations/compare-exchange/get-compare-exchange-value) the existing _Key_. The associated _Value_ and _Index_ are received.  

  2. The _Index_ obtained from the read operation is provided to the [Put](../../../client-api/operations/compare-exchange/put-compare-exchange-value) operation along with the new _Value_ to be saved.  
     This save will succeed only if the index that is provided to the 'Put' operation is the **same** as the index that was received from the server in the previous 'Get', 
     which means that the _Value_ was not modified by someone else between the read and write operations.
{PANEL/}

{PANEL: Example I - Email Address Reservation}  

{NOTE: }

* **To Ensure ACID Transactions** RavenDB automatically creates [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  in [cluster-wide transactions](../../../client-api/session/cluster-transaction).  
  There is no need to manually create or maintain Compare-Exchange items to ensure consistency across your cluster.

* [Compare-Exchange items are not replicated to other clusters](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-clusters)
  to preserve consistency between clusters.  
  In documents where immediate consistency is important, each cluster should be solely responsible for the documents created by it.

{NOTE/}

* Compare Exchange can be used to maintain uniqueness across users' email accounts.  

* First, try to reserve a new user email.  
  If the email is successfully reserved then save the user account document.  

{CODE email@Server\CompareExchange.cs /}  

**Implications**:

* The `User` object is saved as a document, hence it can be indexed, queried, etc.  

* If `session.SaveChanges` fails, the email reservation is _not_ rolled back automatically. It is your responsibility to do so.  

* The compare exchange value that was saved can be accessed from `RQL` in a query:  

{CODE-TABS}
{CODE-TAB:csharp:Sync query_cmpxchg@Server\CompareExchange.cs /}  
{CODE-TAB-BLOCK:sql:RQL}
from Users as s where id() == cmpxchg("emails/ayende@ayende.com")  
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{PANEL/}

{PANEL: Example II - Reserve a Shared Resource}  

{NOTE: }

* **To Ensure ACID Transactions** RavenDB automatically creates [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  in [cluster-wide transactions](../../../client-api/session/cluster-transaction).  
  There is no need to manually create or maintain Compare-Exchange items to ensure consistency across your cluster.

* [Compare-Exchange items are not replicated to other clusters](../../../client-api/operations/compare-exchange/overview#why-compare-exchange-items-are-not-replicated-to-external-clusters)
  to preserve consistency between clusters.  
  In documents where immediate consistency is important, each cluster should be solely responsible for the documents created by it.

{NOTE/}

* Use compare exchange for a shared resource reservation.  

* The code also checks for clients who never release resources (i.e. due to failure) by using timeout.  

{CODE shared_resource@Server\CompareExchange.cs /}
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


