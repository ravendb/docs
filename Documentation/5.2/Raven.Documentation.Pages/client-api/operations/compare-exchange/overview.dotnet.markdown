# Compare Exchange Overview 
---

{NOTE: }

* **Compare-exchange** items are cluster-wide key/value pairs where the key is a unique identifier. 

* Each compare-exchange item contains: 
  * A key which is a unique string across the cluster  
  * A value which can be any json value you choose  
  * Raft index which increments to enable comparison.  
    Any change to the value or metadata changes the Raft index.  
  * Metadata  

* Creating and modifying a compare-exchange item is an atomic, thread-safe [compare-and-swap](https://en.wikipedia.org/wiki/Compare-and-swap) interlocked 
  compare-exchange operation.
  * The compare-exchange item is distributed to all nodes in a [cluster-wide transaction](../../../server/clustering/cluster-transactions)
    so that a consistent, unique key is guaranteed cluster-wide.  

* **To Ensure ACID Transactions** RavenDB automatically creates [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  in cluster-wide transactions.  


**In this page:**  

  * [Using Compare-Exchange Items](../../../client-api/operations/compare-exchange/overview#using-compare-exchange-items)  
  * [Transaction Scope for Compare-Exchange Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations)  
  * [Creating a Key](../../../client-api/operations/compare-exchange/overview#creating-a-key)  
  * [Updating a Key](../../../client-api/operations/compare-exchange/overview#updating-a-key)  
  * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
  * [Example II- Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  

{NOTE/}

---

{PANEL: Using Compare-Exchange Items}

### Why use compare-exchange items

* Compare-exchange items can be used in various situations to protect or reserve a resource by checking changes in values.  
  (see [API Compare-exchange examples](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)).
  * If you create a compare-exchange key/value pair, you can decide what happens when the Raft index increments 
    as a result of a change in the value.

* Compare-exchange items can be used to coordinate work between threads, clients, nodes, or sessions that are 
  trying to modify a shared resource at the same time.  

---

### How to use them to guarantee ACID transactions  

* Create or modify documents in a cluster-wide session to create [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards):  
  * Atomic Guards are associated with a document ID across the cluster.  
  * Whenever one session holds the cluster-wide Atomic Guard associated with a document, other sessions will be able to read the document, 
    but they will not be able to modify it until the first session finishes.  
  * The value in an atomic guard is `null` because instead of looking for changes in the value, 
    it looks for changes in the associated document.
  * Every time a document is modified, the compare-exchange Raft Index is incremented. This ensures that if, for example, 
    a document is being read by node B while node A is writing on it 
      * The Raft index will have changed and won't match when node B tries to write. 
      * This will throw an exception. 
      * Node B is forced to re-read and thus have an updated version of the document.  
  * Cluster-wide transactions have a performance cost when compared to non-cluster-wide transactions, but they guarantee isolation.  
  * You can disable Atomic Guards and manually maintain them via code.  See the [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
    article to learn more.  

---

### How compare-exchange items are managed  

* Compare exchange items are created and managed with any of the following approaches:
  * RavenDB creates and maintains [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) automatically in 
    cluster-wide sessions to guarantee ACIDity across the cluster.  (As of RavenDB version 5.2)
  * Via [Client API Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations)  
  * In [Cluster-Wide Sessions](../../../client-api/session/cluster-transaction)
  * Using [Studio](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view)

{PANEL/}

{PANEL: Transaction Scope for Compare-Exchange Operations}


* A compare-exchange [operation](../../../client-api/operations/what-are-operations) 
  is performed on the [document store](../../../client-api/what-is-a-document-store) level.  
  It is therefore not part of the session transactions.  

* Even if written inside the session scope, a compare exchange operation will be executed regardless 
  of whether the session `SaveChanges( )` succeeds or fails.  

* Thus, upon a [session transaction failure](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching), 
  if you had a successful compare-exchange operation inside the session block scope, 
  it will **not** be rolled back automatically.  

{PANEL/}

{PANEL: Creating a Key}

Provide the following when saving a **key**:

| Parameter | Description |
| ------------- | ---- |
| **Key** | A string under which _Value_ is saved, unique in the database scope across the cluster. This string can be up to 512 bytes. |
| **Value** | The Value that is associated with the _Key_. <br/>Can be a number, string, boolean, array, or any JSON formatted object. |
| **Index** | The _Index_ number is indicating the version of _Value_.<br/>The _Index_ is used for the concurrency control, similar to documents Etags. |

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

* Compare Exchange can be used to maintain uniqueness across users' email accounts.  

* First try to reserve a new user email.  
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

### Studio

- [Compare Exchange View](../../../studio/database/documents/compare-exchange-view)

<br/>

## Code Walkthrough

- [Create CmpXchg Item](https://demo.ravendb.net/demos/csharp/compare-exchange/create-compare-exchange)  
- [Index CmpXchg Values](https://demo.ravendb.net/demos/csharp/compare-exchange/index-compare-exchange)  

