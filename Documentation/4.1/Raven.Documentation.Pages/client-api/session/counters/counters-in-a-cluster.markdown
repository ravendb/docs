# Counters in a Cluster
---

{NOTE: }

* This section describes Counter behaviors in a Cluster.
  * How Counters are modified and replicated.
  * How Counters are allowed to be modified concurrently without conflict.  
  * In which rare cases do Counter modifications "race" against each other.  

* In this page:  
  * [Counters in a multi-node Cluster](../../../client-api/session/counters/counters-in-a-cluster#counters-in-a-multi-node-cluster)  
      * [Value Modification and Replication](../../../client-api/session/counters/counters-in-a-cluster#value-modification-and-replication)  
          * [Value modification Flow](../../../client-api/session/counters/counters-in-a-cluster#value-modification-flow)  
          * [Value Replication Flow](../../../client-api/session/counters/counters-in-a-cluster#value-replication-flow)  
          * [Replication due to a Counter Name modification](../../../client-api/session/counters/counters-in-a-cluster#replication-due-to-a-counter-name-modification)  
      * [Concurrent Modification](../../../client-api/session/counters/counters-in-a-cluster#concurrent-modification)  
  * [Concurrent Delete and Increment](../../../client-api/session/counters/counters-in-a-cluster#concurrent-delete-and-increment)  
      * In a single-server system  
      * In a multi-node cluster  
{NOTE/}

---

{PANEL: Counters in a Multi-Node Cluster}

###Value Modification and Replication

* A Counter's value is distributed between different nodes.  
   * Each node manages its own portion of the Counter's total value, independently from other nodes.  
   * When a client modifies a Counter's value -  
       * Only the portion of the Counter's value **on the node the client writes to** is modified.  
       * The Counter values on the other nodes do not change.
   * The modified portion of the value is **distributed to all other nodes** that contain a replica of this database.  
   * This way, each node is always kept updated with the values set for each Counter **by all nodes**.  
   * When a client requests a Counter's value, it receives **a single accumulated sum**.  

{NOTE: }

####Value modification Flow
* Each node **manages its own portion** of a Counter's total value, independently from other nodes.  

      In the following example, the total value of "ProductLikes" is 80.  
      Each node independently manages a portion of this total.  

      | Counter Name | Node Tag  | Counter Value per node |
      |:---:|:---:|:---:|
      | ProductLikes | A | 42 |
      | ProductLikes | B | 28 |
      | ProductLikes | C | 10 |

* When a client modifies a Counter's value, only the portion of the Counter's value **on the node the client writes to** is modified.  
  The counter values on the other nodes do not change.

      In the following example, a client used node B to increment ProductLikes by 5.  
      Only node B's portion of the value has been incremented.  

      | Counter Name | Node Tag  | Counter Value per node |
      |:---:|:---:|:---:|
      | ProductLikes | A | 42 |
      | ProductLikes | **B** | **33** |
      | ProductLikes | C | 10 |

      After modifying the Counter's value locally, a node [replicates](../../../client-api/session/counters/counters-in-a-cluster#counters-and-replication) the new value to all other nodes.  
      This way, each node is always informed with the values set for each Counter **by all nodes**

* When a client requests a Counter's value, it gets a single accumulated sum.  

      In the following example, a request for ProductLikes's value will yield **85**.  

      | Counter Name | Node Tag  | Counter Value per node |
      |:---:|:---:|:---:|
      | ProductLikes | A | **42** |
      | ProductLikes | B | **33** |
      | ProductLikes | C | **10** |
      | | | **Total Value: 42+33+10 = 85** |

####Value Replication Flow
* **Replication due to a Counter _Value_ modification**  
   A Counter value modification is followed by a replication of the new value to all other nodes that contain a replica of this database.  
   This way, each node is always informed of the values all nodes gave each Counter.  
   * Note that **only the Counter's value** is replicated.  
     The document itself hasn't been modified and needs no replication.  

      E.g.,  
      **1**. The `ProductLikes` Counter is incremented by 2 on node C.  
      **2**. Node C sends the new node-C value (12) to nodes A and B.  
      **3**. All nodes are updated with the same three values:

      | Counter Name | Node Tag  | Counter Value per node |
      |:---:|:---:|:---:|
      | ProductLikes | A | 42 |
      | ProductLikes | B | 33 |
      | ProductLikes | **C** | **12** |

####Replication due to a Counter Name modification
* As described in the [Overview](../../../client-api/session/counters/overview#overview) section, creating or deleting a Counter triggers a document change.  
* As a result, the whole document, including its Counters and their values, is replicated to all nodes in the database group.  
{NOTE/}

###Concurrent Modification

* **The same Counter can be concurrently modified by multiple clients**  
    As described in the [Counters in a multi-node cluster](../../../client-api/session/counters/counters-in-a-cluster#counters-in-a-multi-node-cluster) section, each node manages its own portion of a Counter's value.  
    Resulting from that -
    * Clients can modify the same Counter concurrently.  
    * Nodes do not need to coordinate Counter modification with each other.  
    * Concurrent value modification cannot cause conflicts.
      * Find more about Counters and conflicts in the [Overview](../../../client-api/session/counters/overview#overview) section.  

{PANEL/}

{PANEL: Concurrent `Delete` and `Increment`}

* A sequence of Counter actions is [cumulative](../../../client-api/session/counters/overview#overview), as long as it doesn't [Delete](../../../client-api/session/counters/delete) the Counter.  
   When Delete **is** called, the order of execution **does** matter.  

* When [Increment](../../../client-api/session/counters/increment) and [Delete](../../../client-api/session/counters/delete) are called concurrently, their order of execution is unknown and the outcome becomes uncertain.  
  We can identify this behavior in two different scenarios:  
  * **In a single-server system**  
      * Different ***clients*** may simultaneously request to Delete and Increment the same Counter.  
        * The result depends upon the server's order of ***execution***.  
          * If Delete is executed _last_, the Counter will be permanently deleted.  
          * If Delete is executed _before_ Increment, the Counter will be deleted but then re-created with the value of the Increment action as its initial value.  
  * **In a multi-node cluster**  
      * Different ***nodes*** may concurrently Delete and Increment the same Counter.  
        * The outcome depends upon the order of ***replication***.  
          * If the node that deleted the counter replicates the change _last_, the Counter will be permanently deleted.  
          * If the node that incremented the counter replicates the change _last_, the Counter will be deleted but then re-created 
            with the value of the Increment action as its initial value.  

* In either case, you may find it useful to check a Counter's existence before modifying it.  
  You can do this using [Get](../../../client-api/session/counters/retrieve-counter-values) - a nonexistent Counter will return `NULL`.  

{PANEL/}

## Related articles
### Studio
- [Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

###Client-API - Session
- [Counters Overview](../../../client-api/session/counters/overview)
- [Create or Modify Counter](../../../client-api/session/counters/create-or-modify)
- [Delete Counter](../../../client-api/session/counters/delete)
- [Retrieve Counter Values](../../../client-api/session/counters/retrieve-counter-values)
- [Counters Interoperability](../../../client-api/session/counters/interoperability)

###Client-API - Operations
- [Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)
