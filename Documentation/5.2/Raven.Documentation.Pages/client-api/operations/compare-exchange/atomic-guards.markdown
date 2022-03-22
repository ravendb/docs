# Compare Exchange: Atomic Guards
---

{NOTE: }

* **Atomic guards** are 
  [compare exchange](../../../client-api/operations/compare-exchange/overview) 
  key/value pairs that RavenDB creates and manages automatically in cluster-wide sessions to guarantee the 
  atomicity and overall 
  [ACID](../../../server/clustering/cluster-transactions#cluster-transaction-properties) 
  properties of cluster-wide transactions.  

* Prior to the introduction of this feature (in **RavenDB 5.2**), client code had to 
  administer compare-exchange entries explicitly. You can still do that if you wish, by 
  [disabling](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards) 
  the automatic usage of atomic guards in a session and defining and managing compare exchange 
  key/value pairs 
  [yourself](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation) 
  where needed.  
  * **We strongly recommend not managing compare-exchange manually** unless you _truly_ know what you're doing. 
    Doing so could cancel RavenDB's ACID transaction guarantees.  

* Atomic guards [expire](../../../client-api/operations/compare-exchange/atomic-guards#expiration) 
  on (or shortly after) the expiration or removal of the documents they serve.  

* In this page:
  * [When are Atomic Guards Created](../../../client-api/operations/compare-exchange/atomic-guards#when-are-atomic-guards-created)  
  * [Syntax and Usage](../../../client-api/operations/compare-exchange/atomic-guards#syntax-and-usage)  
  * [Disabling Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards)  
  * [Expiration](../../../client-api/operations/compare-exchange/atomic-guards#expiration)  

{NOTE/}

---

{PANEL: When are Atomic Guards Created}

* Atomic guards are created **by default** when a session's transaction mode is 
  [ClusterWide](../../../client-api/session/cluster-transaction#open-cluster-wide-session).  

* They are only created when creating a document with a fully successful session SaveChanges.  

* If you disable Atomic Guards, you remove RavenDB's guarantee of ACID transactions across the cluster.  

{PANEL/}

{PANEL: Atomic Guard Transaction Scope}

* RavenDB uses automatically created compare exchange items, called [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards), 
  to ensure cluster-wide ACID transactions as of version 5.2.  

* When working with a [cluster-wide session](../../../client-api/session/cluster-transaction), 
  an Atomic Guard is created upon a successful creation of a document.  
  * To set the session as cluster-wide the `TransactionMode` must be defined as `ClusterWide`.

* If `session.SaveChanges()` fails, the entire session is rolled-back and the Atomic Guard is not created.  

{INFO: }

* Atomic Guards are local to the cluster they were defined on.  

* Atomic Guards will not be replicated across separate clusters in any replication task.  
  We've designed them this way so that [data ownership remains the cluster level](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).  


{INFO/}

{PANEL/}

{PANEL: Syntax and Usage}

In the code sample below, for example, an atomic guard is automatically 
created upon the creation of a new document, and then used when two sessions 
compete on changing the document.  

{CODE:csharp atomic-guards-enabled@ClientApi/Operations/CompareExchange.cs /}

If you [examine](../../../studio/database/documents/documents-and-collections#the-documents-view) 
the compare exchange entries list after running the above sample, you'll see the atomic guard that 
was automatically created and used by it.  
![Atomic Guard](images/atomic-guard.png "Atomic Guard")


{PANEL/}

{PANEL: Disabling Atomic Guards}

{WARNING: }
**Do not remove or edit** atomic guards manually, as sessions that use them 
may consider them occupied and prevent their modification.  
If you accidentally remove an active atomic guard (that is, the atomic guard 
of a live document), re-create the document.  
{WARNING/}

To **disable** the automatic usage of atomic guards in a session, set the session 
`DisableAtomicDocumentWritesInClusterWideTransaction` configuration option to `true`.  

In the sample below, the session uses **no atomic guards**.  
{CODE:csharp atomic-guards-disabled@ClientApi/Operations/CompareExchange.cs /}

{WARNING: Warning}
To **guarantee ACIDity in cluster-wide transactions** when atomic guards are disabled,  
you have to explicitly [set and use](../../../client-api/operations/compare-exchange/overview) 
the required compare exchange key/value pairs.  

Only disable and edit Atomic Guards if you truly know what you're doing as it can negatively impact ACID transaction guarantees.  
{WARNING/}

{PANEL/}

{PANEL: Expiration}

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
