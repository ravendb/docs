# Compare Exchange Overview 
---

{NOTE: }

**What are Compare Exchange Items?**  

* CmpXchg are cluster-wide key/value pair items where the key is a unique identifier in the database,
  across all your database group nodes.
* Creating & modifying a CmpXchg item is an atomic, thread-safe, compare-and-swap interlocked compare-exchange operation.
  * The CmpXchg item is distributed to all nodes in a [cluster-wide transaction](../../../server/clustering/cluster-transactions)
    so that a consistent, unique key is guaranteed cluster-wide.  

**How are they useful?**  

* The CmpXchg items can be used to coordinate work between threads, clients, nodes, or sessions that are 
  trying to access a shared resource (such as a document) at the same time.  
  * They're useful when you want to do highly consistent operations at the cluster level, not just the individual node.  
  * RavenDB automatically creates Atomic Guards to ensure consistency in cluster-wide transactions.  
* This singular key can also be used to reserve a resource in various other situations  
  (see [API Compare-exchange examples](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)).  

**How can I manage them?**  

* Compare exchange items are created and managed by either of the following:
  * RavenDB [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards)  
    To guarantee ACIDity across the cluster, 
    as of RavenDB 5.2, we automatically create and maintain Atomic Guard CmpXchg items in cluster-wide sessions.  
  * [API Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations) See below.
  * [Session - Cluster Transaction](../../../client-api/session/cluster-transaction)
  * Using the [RavenDB Studio](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view)

* Once defined, the Compare Exchange Values can be accessed via [GetCompareExchangeValuesOperation](../../../client-api/operations/compare-exchange/get-compare-exchange-values).  

**In this page:**  

  * [Transaction Scope for Compare-Exchange Operations](../../../client-api/operations/compare-exchange/overview#transaction-scope-for-compare-exchange-operations)  
  * [Creating a Key](../../../client-api/operations/compare-exchange/overview#creating-a-key)  
  * [Updating a Key](../../../client-api/operations/compare-exchange/overview#updating-a-key)  
  * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
  * [Example II- Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  

{NOTE/}

---

{PANEL: Transaction Scope for Compare-Exchange Operations}


* A compare-exchange [operation](../../../client-api/operations/what-are-operations) 
  is done on the [document store](../../../client-api/what-is-a-document-store).  
  It is therefore not part of the session transactions.  

* Even if written inside the session scope, a compare exchange operation will be executed regardless 
  of whether the session `SaveChanges( )` succeeds or fails.  

* Thus, upon a [session transaction failure](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching), 
  if you had a successful cmpXchg operation inside the session block scope, 
  it will **not** be rolled back automatically.  

{PANEL/}

{PANEL: Creating a Key}

* Provide the following when saving a **key**:

| Parameter | Description |
| ------------- | ---- |
| **Key** | A string under which _Value_ is saved, unique in the database scope across the cluster. This string can be up to 512 bytes. |
| **Value** | The Value that is associated with the _Key_. <br/>Can be a number, string, boolean, array, or any JSON formatted object. |
| **Index** | The _Index_ number is indicating the version of _Value_.<br/>The _Index_ is used for the concurrency control, similar to documents Etags. |

* When creating a _new_ 'Compare Exchange Key', the index should be set to `0`.  

* The [Put](../../../client-api/operations/compare-exchange/put-compare-exchange-value) operation will succeed only if this key doesn't exist yet.  

* Note: Two different keys _can_ have the same values as long as the keys are unique.  
{PANEL/}

{PANEL: Updating a Key}

* Updating a 'Compare Exchange' key can be divided into 2 phases:

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

