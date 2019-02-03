# Counters In Clusters
---

{NOTE: }

* This section describes Counter behaviors in a Cluster.
  * How Counters are modified and replicated.
  * How Counters are allowed to be modified concurrently without conflict.  
  * In which rare cases do Counter modifications "race" against each other.  

* In this page:  
  * [Counter-Value Modification](../../../client-api/session/counters/counters-in-clusters#counter-value-modification)  
     * [Value modification Flow](../../../client-api/session/counters/counters-in-clusters#value-modification-flow)  
     * [Value Replication Flow](../../../client-api/session/counters/counters-in-clusters#value-replication-flow)  
     * [Server Reply to Client Request](../../../client-api/session/counters/counters-in-clusters#server-reply-to-client-request)  
  * [Counter Name modification](../../../client-api/session/counters/counters-in-clusters#counter-name-modification)  
  * [Concurrent Modification](../../../client-api/session/counters/counters-in-clusters#concurrent-modification)  
  * [Concurrent `Delete` and `Increment`](../../../client-api/session/counters/counters-in-clusters#concurrent-delete-and-increment)  
     * [In a single-server system](../../../client-api/session/counters/counters-in-clusters#in-a-single-server-system)  
     * [In a multi-node cluster](../../../client-api/session/counters/counters-in-clusters#in-a-multi-node-cluster)  
{NOTE/}

---

{PANEL: Counter-Value Modification}

####Value Modification Flow
Each node **manages its own portion** of a Counter's total value, independently from other nodes.  

  > In the following 3-nodes-cluster example:  
  > - The total value of the "ProductLikes" Counter is 80.  
  > - Each node independently manages a portion of this total.  
  >  
  > | Counter Name | Node Tag  | Counter Value on this node |
  > |:---:|:---:|:---:|
  > | ProductLikes | A | 42 |
  > | ProductLikes | B | 28 |
  > | ProductLikes | C | 10 |

When a client modifies a Counter's value, only the portion of the Counter's value 
**on the node this client writes to** is modified. The Counter's values on the other nodes do not change. 

  > In the following example:  
  > - A client has used node B to increment ProductLikes by 5.  
  > - Only node B's portion of the value is incremented.  
  >  
  > | Counter Name | Node Tag  | Counter Value per node |
  > |:---:|:---:|:---:|
  > | ProductLikes | A | 42 |
  > | ProductLikes | **B** | **33** |
  > | ProductLikes | C | 10 |

---

####Value Replication Flow
After modifying the Counter's value locally, a node [replicates](../../../client-api/session/counters/counters-in-clusters#value-modification-and-replication) the new value to all other nodes.  
This way each node is always kept updated with the values set for each Counter **by all nodes**.  

  > In the following example:  
  > - The `ProductLikes` Counter is incremented by 2 on node C.  
  > - Node C sends the new node-C value (12) to nodes A and B.  
  > - All nodes are updated with the same three values.
  > 
  > | Counter Name | Node Tag  | Counter Value per node |
  > |:---:|:---:|:---:|
  > | ProductLikes | A | 42 |
  > | ProductLikes | B | 33 |
  > | ProductLikes | **C** | **12** |

  > {NOTE: }
  > Note that **only the Counter's value** is replicated.  
  > The document itself hasn't been modified and needs no replication.  
  > {NOTE/}

---

####Server Reply to Client Request  
When a client requests a Counter's value, it gets a single accumulated sum.  

  > In the following example, a request for ProductLikes's value will yield **85**.  
  > 
  > | Counter Name | Node Tag  | Counter Value per node |
  > |:---:|:---:|:---:|
  > | ProductLikes | A | **42** |
  > | ProductLikes | B | **33** |
  > | ProductLikes | C | **10** |
  > | | | **Total Value: 42+33+10 = 85** |
{PANEL/}

{PANEL: Counter Name modification}

**Replication due to a Counter Name modification**:

* As described in the [Overview](../../../client-api/session/counters/overview#overview) section, creating or deleting a Counter triggers a document change.  
* As a result, the whole document, including the new Counter value, is replicated to all nodes in the database group.  
  Existing Counters aren't replicated, since they already exist on the other nodes and their values haven't changed.  


{PANEL/}

{PANEL: Concurrent Modification}

**The same Counter can be concurrently modified by multiple clients**.  

As described in the [Counters in a multi-node cluster](../../../client-api/session/counters/counters-in-clusters#counters-in-a-multi-node-cluster) section, each node manages its own portion of a Counter's value.  
As a result:  

* Clients can modify the same Counter concurrently.  
* Nodes do not need to coordinate Counter modification with each other.  
* Concurrent value modification cannot cause conflicts.
   * Find more about Counters and conflicts in the [Overview](../../../client-api/session/counters/overview#overview) section.  

{PANEL/}

{PANEL: Concurrent `Delete` and `Increment`}

A sequence of Counter actions is [cumulative](../../../client-api/session/counters/overview#overview), as long as it doesn't [Delete](../../../client-api/session/counters/delete) the Counter.  
When Delete **is** executed, the order of execution **does** matter.  

* When [Increment](../../../client-api/session/counters/create-or-modify) and 
  [Delete](../../../client-api/session/counters/delete) are called concurrently, 
  their order of execution is unknown and the outcome becomes uncertain.  
  We can identify this behavior in two different scenarios:  
   * In a single-server system  
   * In a multi-node cluster  

---

####In a Single-Server System

Different clients may simultaneously request to Delete and Increment the same Counter.  

* The result depends upon the server's **order of execution**.  
   * If Delete is executed _last_, the Counter will be permanently deleted.  
   * If Delete is executed _before_ Increment, the Counter will be deleted but then re-created with the value of the Increment action as its initial value.  

---

####In a Multi-Node Cluster

Different ***nodes*** may concurrently Delete and Increment the same Counter.  

* The outcome depends upon the order of ***replication***.  
   * If the node that deleted the counter replicates the change _last_, the Counter will be permanently deleted.  
   * If the node that incremented the counter replicates the change _last_, the Counter will be deleted but then re-created with the value of the Increment action as its initial value.  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../../client-api/session/counters/overview)  
[Creating and Modifying Counters](../../../client-api/session/counters/create-or-modify)  
[Deleting a Counter](../../../client-api/session/counters/delete)  
[Retrieving Counter Values](../../../client-api/session/counters/retrieve-counter-values)  
[Counters and other features](../../../client-api/session/counters/counters-and-other-features)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
