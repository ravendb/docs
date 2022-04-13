# Compare Exchange: Atomic Guards
---

{NOTE: }

* **Atomic guards** are 
  [compare exchange](../../../client-api/operations/compare-exchange/overview) 
  key/value pairs that RavenDB creates and manages automatically to guarantee 
  [ACID](../../../server/clustering/cluster-transactions#cluster-transaction-properties) 
  transactions in cluster-wide sessions.  

* Atomic Guards coordinate work between sessions that try to write on a document at the same time. 
  Saving a document is prevented if another session has incremented the Atomic Guard Raft index, 
  which is triggered by changing the document.

* Prior to the introduction of this feature (in **RavenDB 5.2**), client code had to 
  administer compare-exchange entries explicitly. You can still do that if you wish by 
  [disabling](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards) 
  the automatic usage of atomic guards in a session and defining and managing compare exchange 
  key/value pairs 
  [yourself](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation) 
  where needed.  
  * **We strongly recommend not managing atomic guards manually** unless you _truly_ know what you're doing. 
    Doing so could cancel RavenDB's ACID transaction guarantees.  

* In this page:
  * [When are Atomic Guards Created](../../../client-api/operations/compare-exchange/atomic-guards#when-are-atomic-guards-created)  
  * [Atomic Guard Transaction Scope](../../../client-api/operations/compare-exchange/atomic-guards#atomic-guard-transaction-scope)  
  * [Syntax and Usage](../../../client-api/operations/compare-exchange/atomic-guards#syntax-and-usage)  
  * [Disabling Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards)  
  * [When are Atomic Guards Removed](../../../client-api/operations/compare-exchange/atomic-guards#when-are-atomic-guards-removed)  

{NOTE/}

---

{PANEL: When are Atomic Guards Created}

* Atomic Guards are created by default only when the session's transaction mode is set to 
  [ClusterWide](../../../client-api/session/cluster-transaction#open-cluster-wide-session):
  * New documents:  
    A new Atomic Guard is created when successfully saving a new document in the cluster-wide session.  
  * Existing documents that don't have Atomic Guards:  
    A new Atomic Guard is created when modifying an existing document in the cluster-wide session.  
* If a document already has an associated Atomic Guard:  
  Upon modifying a document in a new session, the Raft Index of its related cmpXchg item will increment.

{PANEL/}

{PANEL: Atomic Guard Transaction Scope}

* When working with a [cluster-wide session](../../../client-api/session/cluster-transaction), 
  an Atomic Guard is created upon a successful creation of a document.  
  * To set a session as cluster-wide, define its `TransactionMode` as `ClusterWide` when opening the session.

* If the session's `SaveChanges()` fails, the entire session is rolled-back and the Atomic Guard is not created.  

{INFO: }

* Atomic Guards are local to the cluster they were defined on.  

* Atomic Guards will not be replicated to another cluster in any ongoing replication task.  
  We've designed them this way so that [data ownership remains at the cluster level](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).  


{INFO/}

{PANEL/}

{PANEL: Syntax and Usage}

In the code sample below, for example, an atomic guard is automatically 
created upon the creation of a new document, and then used when two sessions 
compete on changing the document.  

{CODE:csharp atomic-guards-enabled@ClientApi/Operations/CompareExchange.cs /}

If you [examine](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view) 
the compare exchange entries list after running the above sample, you'll see the atomic guard that 
was automatically created.  
![Atomic Guard](images/atomic-guard.png "Atomic Guard")

   * The generated Atomic Guard key name is composed of the prefix `rvn-atomic`
     and name of the document that it is associated with.


{PANEL/}

{PANEL: Disabling Atomic Guards}

To **disable** the automatic usage of atomic guards in a session, set the session 
`DisableAtomicDocumentWritesInClusterWideTransaction` configuration option to `true`.  

In the sample below, the session uses **no atomic guards**.  
{CODE:csharp atomic-guards-disabled@ClientApi/Operations/CompareExchange.cs /}

{WARNING: Warning}
* To enable **ACIDity in cluster-wide transactions** when atomic guards are disabled,  
  you have to explicitly [set and use](../../../client-api/operations/compare-exchange/overview) 
  the required compare exchange key/value pairs.  

* Only disable and edit Atomic Guards if you truly know what you're doing as it can negatively 
  impact ACID transaction guarantees.  

* **Do not remove or edit** atomic guards manually while a session is using them. 
  A session that is currently using these removed atomic guards will not be able to save 
  their related documents resulting in an error.  
  * If you accidentally remove an active atomic guard that is associated with an existing document, 
    recreate or save the document again in a cluster-wide session which will re-create the Atomic Guard.  
{WARNING/}

{PANEL/}

{PANEL: When are Atomic Guards Removed}

Atomic guards expire on the expiration of the documents they are used for and are automatically 
removed by a RavenDB cleanup task. You do not need to handle the cleanup yourself.  

{NOTE: }
Since different cleanup tasks handle the removal of deleted and expired documents 
and the removal of expired atomic guards, it may happen that atomic guards of removed 
documents would linger in the compare exchange entries list a short while longer before 
they are removed.  

* You do **not** need to remove such atomic guards yourself, they will be removed by 
  the cleanup task.  
* You **can** re-create expired documents whose atomic guards haven't been removed yet.  
  RavenDB will create new atomic guards for such documents, and expire the old ones.  
{NOTE/}

{PANEL/}

## Related Articles

### Client API
- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)

### Session
- [Cluster-Wide Session](../../../client-api/session/cluster-transaction#open-cluster-wide-session)

### Server
- [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions)
- [Cluster-Wide Transactions ACIDity](../../../server/clustering/cluster-transactions#cluster-transaction-properties)
- [Document Expiration](../../../server/extensions/expiration)

### Additional Resources
- [Simplifying Atomic Cluster-Wide Transactions" / Oren Eini](https://ayende.com/blog/194405-A/ravendb-5-2-simplifying-atomic-cluster-wide-transactions)
