# Set Index Lock Mode Operation

---

{NOTE: }

* The lock mode controls the behavior of index modifications.  
  Use `SetIndexesLockOperation` to modify the **lock mode** for a single index or multiple indexes.

* **Indexes scope**:  
  The lock mode can be set only for static-indexes, not for auto-indexes.

* **Nodes scope**:  
  The lock mode will be updated on all nodes in the database group.

* Setting the lock mode can also be done in the **Studio** [indexes list](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) view.  
  Locking an index is not a security measure, the index can be unlocked at any time.  

* In this page:
    * [Lock modes](../../../../client-api/operations/maintenance/indexes/set-index-lock#lock-modes)
    * [Sample usage flow](../../../../client-api/operations/maintenance/indexes/set-index-lock#sample-usage-flow)
    * [Set lock mode - single index](../../../../client-api/operations/maintenance/indexes/set-index-lock#set-lock-mode---single-index)
    * [Set lock mode - multiple indexes](../../../../client-api/operations/maintenance/indexes/set-index-lock#set-lock-mode---multiple-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/set-index-lock#syntax)

{NOTE/}

---

{PANEL: Lock modes}

* **Unlocked** - when lock mode is set to `Unlock`:  
  * Any change to the index definition will be applied.  
  * If the new index definition differs from the one stored on the server,  
    the index will be updated and the data will be re-indexed using the new index definition.  
 
* **Locked (ignore)** - when lock mode is set to `LockedIgnore`:  
  * Index definition changes will Not be applied.  
  * Modifying the index definition will return successfully and no error will be raised,  
    however, no change will be made to the index definition on the server.
 
* **Locked (error)** - when lock mode is set to `LockedError`:  
  * Index definitions changes will Not be applied.  
  * An exception will be thrown upon trying to modify the index.  

{PANEL/}

{PANEL: Sample usage flow}

Consider the following scenario:

* Your client application defines and [deploys a static-index](../../../../client-api/operations/maintenance/indexes/put-indexes) upon application startup.
  
* After the application has started, you make a change to your index definition and re-indexing occurs.   
  However, if the index lock mode is _'Unlock'_, the next time your application will start,  
  it will reset the index definition back to the original version.

* Locking the index allows to make changes to the running index and prevents the application  
  from setting it back to the previous definition upon startup. See the following steps:  
<br>

  1. Run your application  
  2. Modify the index definition on the server (from Studio, or from another application),  
     and then set this index lock mode to `LockedIgnore`.  
  3. A side-by-side replacement index is created on the server.  
     It will index your dataset according to the **new** definition.  
  4. At this point, if any instance of your original application is started,  
     the code that defines and deploys the index upon startup will have no effect  
     since the index is 'locked'.  
  5. Once the replacement index is done indexing, it will replace the original index.  

{PANEL/}

{PANEL: Set lock mode - single index}

{CODE-TABS}
{CODE-TAB:csharp:Sync set_lock_single@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}
{CODE-TAB:csharp:Async set_lock_single_async@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Set lock mode - multiple indexes}

{CODE-TABS}
{CODE-TAB:csharp:Sync set_lock_multiple@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}
{CODE-TAB:csharp:Async set_lock_multiple_async@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}

| Parameters | Type | Description |
|- | - | - |
| **indexName** | string | Index name for which to set lock mode |
| **mode** | `IndexLockMode` | Lock mode to set |
| **parameters** | `SetIndexesLockOperation.Parameters` | List of indexes + Lock mode to set.<br>An exception is thrown if any of the specified indexes do not exist. |

{CODE syntax_2@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}

{CODE syntax_3@ClientApi\Operations\Maintenance\Indexes\SetLockMode.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Change Index Priority](../../../../client-api/operations/maintenance/indexes/set-index-priority)
