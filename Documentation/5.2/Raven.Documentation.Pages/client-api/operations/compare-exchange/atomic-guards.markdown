# Compare Exchange: Atomic Guards
---

{NOTE: }

* **Atomic guards** are 
  [compare exchange](../../../client-api/operations/compare-exchange/overview) 
  keys that RavenDB creates and manages automatically to guarantee the 
  [ACID](../../../server/clustering/cluster-transactions#cluster-transaction-properties) 
  properties of cluster-wide transactions.  

* Prior to the introduction of this feature (in RavenDB 5.2), client code had to 
  administer compare exchange keys explicitly. You can still do that if you wish, by 
  [disabling](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards) 
  the automatic usage of atomic guards in a session and defining and managing compare exchange keys 
  [yourself](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation) 
  where needed.  

* Atomic guards **expire** when (or shortly after) the documents that use them expire.  

* In this page:
  * [Syntax and Usage](../../../client-api/operations/compare-exchange/atomic-guards#syntax-and-usage)  
     * [Disabling Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards)  
  * [Expiration](../../../client-api/operations/compare-exchange/atomic-guards#expiration)  

{NOTE/}

---

{PANEL: Syntax and Usage}

Atomic guards are used **by default** when a session's transaction mode is set to 
[ClusterWide](../../../client-api/session/cluster-transaction#open-cluster-wide-session).  

The code sample below, for example, inexplicitly sets and uses an atomic guard 
for a cluster-wide transaction that stores a new document.  
{CODE:csharp atomic-guards-enabled@ClientApi/Operations/CompareExchange.cs /}

[Examine](../../../studio/database/documents/documents-and-collections#the-documents-view) 
the compare exchange keys list after running the above sample, to see the atomic guard that 
was automatically created and used (or just used, is it already existed) during this session.  
![Atomic Guard](images/atomic-guard.png "Atomic Guard")

{WARNING: }
**Do not remove** atomic guards manually, as nodes that attempt to use them 
may assume they are occupied and prevent the modification of documents whose 
transactions they mediate.  
If you accidentally removed an active atomic guard (that is, the atomic guard 
of a live document), re-create the document it served.  
{WARNING/}

---

### Disabling Atomic Guards

To **disable** the automatic usage of atomic guards in a session, set the session 
`DisableAtomicDocumentWritesInClusterWideTransaction` configuration option to `true`.  

In the sample below, the session uses **no atomic guards**.  
{CODE:csharp atomic-guards-disabled@ClientApi/Operations/CompareExchange.cs /}

{WARNING: }
To **guarantee ACIDity in cluster-wide transactions** when atomic guards are disabled, 
explicitly [set and use](../../../client-api/operations/compare-exchange/overview) 
the required compare exchange keys.  
{WARNING/}

{PANEL/}

{PANEL: Expiration}

Atomic guards expire on the expiration of the documents they server, and are automatically 
removed by a RavenDB cleanup task. You do not need to handle the cleanup yourself.  

{NOTE: }
Since different cleanup tasks handle the removal of deleted and expired documents 
and the removal of expired atomic guards, it may heppen that atomic guards of removed 
documents would linger in the compare exchange keys list a short while longer before 
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
