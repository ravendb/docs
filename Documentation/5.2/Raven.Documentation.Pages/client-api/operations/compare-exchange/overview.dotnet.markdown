# Compare Exchange Overview 
---

{NOTE: }

* The **Compare Exchange** feature allows you to perform cluster-wide _interlocked distributed operations_.  

* Compare-exchange key/value pairs can be used to prevent writing on a document if another session is currently holding the 'key'.  

* Compare exchange can be created and managed explicitly in your code.  

* To ensure ACIDity across the cluster, since RavenDB 5.2 they are created automatically as [Atomic Guards](../../../client-api/operations/compare-exchange/overview#atomic-guards) 
  whenever the session is defined as `ClusterWide`.

* Once defined, the Compare Exchange Values can be accessed via [GetCompareExchangeValuesOperation](../../../client-api/operations/compare-exchange/get-compare-exchange-values),  
  or by using RQL in a query ([see example-I below](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)).  

In this page:  

  * [Compare Exchange Values in RavenDB](../../../client-api/operations/compare-exchange/overview#compare-exchange-values-in-ravendb)  
  * [Transaction Scope](../../../client-api/operations/compare-exchange/overview#transaction-scope)  
    * [Manually Created](../../../client-api/operations/compare-exchange/overview#manually-created)  
    * [Atomic Guards](../../../client-api/operations/compare-exchange/overview#atomic-guards)  
  * [Creating a Key](../../../client-api/operations/compare-exchange/overview#creating-a-key)  
  * [Updating a Key](../../../client-api/operations/compare-exchange/overview#updating-a-key)  
  * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
  * [Example II- Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  

{NOTE/}

---

{PANEL: Compare Exchange Values in RavenDB}

* Unique **Keys** can be reserved across the cluster in the [Database Group](../../../studio/database/settings/manage-database-group).  
  Each key has an associated **Value** which RavenDB uses to compare with the expected value.  

* The compare-exchange value must remain consistent throughout the cluster.  

* Modifying these values is an [interlocked compare exchange operation](https://ayende.com/blog/182948-C/distributed-compare-exchange-operations-with-ravendb),  
  meaning that to maintain consistency, a majority of the nodes need to agree before a change can be made.  

{PANEL/}

{PANEL: Transaction Scope}

#### Manually Created

* A manually created compare-exchange [operation](../../../client-api/operations/what-are-operations) 
  is done on the [document store](../../../client-api/what-is-a-document-store).  
  It is therefore not part of the session transactions.  

* Even if written inside the session scope, a compare exchange operation will be executed regardless 
  of whether the `session.SaveChanges()` succeeds or fails.  

* Thus, if a compare-exchange operation has failed when used inside a session block,  
  it will **not** be rolled back automatically upon a [session transaction failure](../../../client-api/session/what-is-a-session-and-how-does-it-work#batching).  

#### Atomic Guards

* RavenDB uses automatically created compare exchange items, called [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards), 
  to ensure cluster-wide ACID transactions as of version 5.2.  

* When working with a [cluster-wide session](../../../client-api/session/cluster-transaction), 
  an Atomic Guard is created upon a successful creation of a document.  
  * To set the session as cluster-wide the `TransactionMode` must be defined as `ClusterWide`.

* If `session.SaveChanges()` fails, the Atomic Guard is not created.  

{INFO: }

* Atomic Guards are local to the cluster they were defined on.  

* Atomic Guards will not be replicated across clusters in replication tasks.  


{INFO/}

{WARNING: Warning}
Do not remove or edit atomic guards manually as it will likely disable ACID guarantees for cluster-wide transactions.
{WARNING/}

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

