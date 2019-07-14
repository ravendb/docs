# Compare Exchange Overview 
---

{NOTE: }

* The **Compare Exchange** feature allows you to perform cluster-wide _interlocked_ distributed operations.  

* Unique **Keys** can be reserved in the [Database Group](../../../studio/database/settings/manage-database-group) accross the cluster.  
  Each key has an associated **Value**.  

* Modifying these values is an ***interlocked compare exchange*** operation.  

* Once defined, the Compare Exchange Values can be accessed via [GetCompareExchangeValuesOperation](../../../client-api/operations/compare-exchange/get-compare-exchange-values),  
  or by using RQL in a query (see example-I below)  

* In this page:  
  * [Compare Exchange Transaction Scope](../../../client-api/operations/compare-exchange/overview#compare-exchange-transaction-scope)  
  * [Creating a Key](../../../client-api/operations/compare-exchange/overview#creating-a-key)  
  * [Updating a Key](../../../client-api/operations/compare-exchange/overview#updating-a-key)  
  * [Example I - Email Address Reservation](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)  
  * [Example II- Reserve a Shared Resource](../../../client-api/operations/compare-exchange/overview#example-ii---reserve-a-shared-resource)  
{NOTE/}

---

{PANEL: Compare Exchange Transaction Scope}

* Since the compare-exchange operations guarantee atomicity across the entire cluster, 
  the feature is **not** using the transaction associated with a session object, as a session transaction spans only a single node.  

* So if a compare-exchange operation has failed when used inside a session block, it will **not** be rolled back automatically upon a session transaction failure.  
{PANEL/}

{PANEL: Creating a Key}

* Provide the following when saving a **key**:

| Parameter | Description |
| ------------- | ---- |
| **Key** | A string under which _Value_ is saved, unique in the database scope across the cluster. This string can be up to 512 bytes. |
| **Value** | The Value that is associated with the _Key_. <br/>Can be a number, string, boolean, array or any JSON formatted object. |
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

* Compare Exchange can be used to maintain uniqueness across users emails accounts.  

* First try to reserve a new user email.  
  If the email is successfully reserved then save the user account document.  

{CODE:java email@Server\CompareExchange.java /}  

**Implications**:

* The `User` object is saved as a document, hence it can be indexed, queried, etc.  

* If `session.saveChanges` fails, the email reservation is _not_ rolled back automatically. It is your responsibility to do so.  

* The compare exchange value that was saved can be accessed from `RQL` in a query:  

{CODE-TABS}
{CODE-TAB:java:Sync query_cmpxchg@Server\CompareExchange.java /}  
{CODE-TAB-BLOCK:sql:RQL}
from Users as s where id() == cmpxchg("emails/ayende@ayende.com")  
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{PANEL/}

{PANEL: Example II - Reserve a Shared Resource}  

* Use compare exchange for a shared resource reservation.  

* The code also checks for clients who never release resources (i.e. due to failure) by using timeout.  

{CODE:java shared_resource@Server\CompareExchange.java /}
{PANEL/}

## Related Articles

### Compare Exchange

- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
