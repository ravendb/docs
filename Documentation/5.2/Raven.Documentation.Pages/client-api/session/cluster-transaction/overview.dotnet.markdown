# Cluster Transaction - Overview

---

{NOTE: }

* A session represents a single business transaction.  
  When opening a session, the session's mode can be set to either:  
    * __Single-Node__ - transaction is executed on a specific node and then replicated
    * __Cluster-Wide__ - transaction is registered for execution on all nodes in an atomic fashion

* In this page:  
    * [Open a cluster transaction](../../../client-api/session/cluster-transaction/overview#open-a-cluster-transaction)
    * [Cluster-wide transaction vs. Single-node transaction](../../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction)
     
{NOTE/}

---

{PANEL: Open a cluster transaction}

* To work with a cluster transaction open a __cluster-wide session__,  
  by explicitly setting the `TransactionMode` to `TransactionMode.ClusterWide`.

{CODE-TABS}
{CODE-TAB:csharp:Sync open_cluster_session_sync@ClientApi\Session\ClusterTransaction\Overview.cs /}
{CODE-TAB:csharp:Async open_cluster_session_async@ClientApi\Session\ClusterTransaction\Overview.cs /}
{CODE-TABS/}

* Similar to the single-node session,  
  any CRUD operations can be made on the cluster-wide session and the session will track them as usual.

{PANEL/}

{PANEL: Cluster-wide transaction vs. Single-node transaction}

{NOTE: }
#### Cluster-Wide
---

* Cluster-wide transactions are __fully ACID__ transactions across all the database-group nodes.  
  Implemented by the Raft algorithm, the cluster must first reach a consensus.  
  Once the majority of the nodes have approved the transaction,  
  the transaction is registered for execution in the transaction queue of all nodes in an atomic fashion.  

---

* The transaction will either __succeed on all nodes or be rolled-back__.
    * The transaction is considered successful only when successfully registered on all the database-group nodes.
      Once executed on all nodes, the data is consistent and available on all nodes.  
    * A failure to register the transaction on any node will cause the transaction to roll-back on all nodes and changes will Not be applied.

---

* The only __actions available__ are:
    * PUT / DELETE a document
    * PUT / DELETE a compare-exchange item

---

* To prevent from concurrent documents modifications,  
  the server creates [Atomic-Guards](../../../client-api/session/cluster-transaction/atomic-guards) that will be associated with the documents.  
  An Atomic-Guard will be created when:
    * A new document is created
    * Modifying an existing document that doesn't have yet an Atomic-Guard

---

* Cluster-wide transactions are __conflict-free__.

---

* The cluster-wide transaction is considered __more expensive and less performant__  
  since a cluster consensus is required prior to execution.  

---

* __Prefer a cluster-wide transaction when__:
    * Prioritizing consistency over performance & availability
    * When you would rather fail if a successful operation on all nodes cannot be ensured

{NOTE/}

{NOTE: }
#### Single-Node
---

* A single-node transaction is considered successful once executed successfully on the node the client is communicating with.
  The data is __immediately available__ on that node, and it will be __eventually-consistent__ across all the other database nodes when the replication process takes place soon after.

---

* __Any action is available__ except for PUT / DELETE a compare-exchange item.  
  No Atomic-Guards are created by the server.

---

* __Conflicts__ may occur when two concurrent transactions modify the same document on different nodes at the same time.
  They are resolved according to the defined conflict settings, either by using the latest version (default) or by following a conflict resolution script.  
  Revisions are created for the conflicting documents so that any document can be recovered.

---

* The single-node transaction is considered __faster and less expensive__,  
  as no cluster consensus is required for its execution.

---

* __Prefer a single-node transaction when__:  
    * Prioritizing performance & availability over consistency
    * When immediate data persistence is crucial
    * When you must ensure data is written even when other nodes are not reachable at the moment
    * And - when resolving occasional conflicts is acceptable

{NOTE/}
{PANEL/}

## Related Articles

### Server Clustering

- [cluster-Wide Transactions](../../../server/clustering/cluster-transactions)


