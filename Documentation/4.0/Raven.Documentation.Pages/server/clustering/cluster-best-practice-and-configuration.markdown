# Cluster: Best Practices

---

{NOTE: }

* In this page:
   * [Clusters should have an odd number of at least 3 nodes](../../server/clustering/cluster-best-practice-and-configuration#clusters-should-have-an-odd-number-of-at-least-3-nodes)
   * [Avoid different cluster configurations among the cluster's nodes](../../server/clustering/cluster-best-practice-and-configuration#avoid-different-cluster-configurations-among-the-clusters-nodes)

{NOTE/}

---

{PANEL: }

### Clusters should have an odd number of at least 3 nodes

We recommend setting up clusters with an odd number of nodes equal to or greater than 3.

**A single node cluster:**  
Will not have the ability to automatically failover to another node if it goes down.  
This means that it is not highly available.  

**A two nodes cluster:**  
Also not recommended since the cluster must have a consensus among the majority of nodes to operate.  
With a two-node cluster, if one of the nodes is down or partitioned, the other node is not considered a 'majority'
and thus no [Raft](../../glossary/raft-algorithm) 
command will be created, although any database on the surviving node will still be responsive to the user.  

**Odd number of 3 or more nodes:**  
For ACID guarantees, a majority of the nodes must agree on every [cluster-wide transaction](../../server/clustering/cluster-transactions), 
so having an odd number of nodes makes achieving the majority easier.

{PANEL/}

{PANEL: }

### Avoid different cluster configurations among the cluster's nodes

Configuration mismatches tend to cause interaction problems between nodes.

If you must set [cluster configurations](../../server/configuration/cluster-configuration) differently in separate nodes,  
**we recommend first testing it** in a development environment to see if each node interacts properly with the others.
{PANEL/}

## Related articles 

### Cluster

- [Getting Started](../../start/getting-started)
- [Clustering Overview](../../server/clustering/overview)

### Consistency

- [Cluster-Wide Transactions](../../server/clustering/cluster-transactions)
- [Session - Cluster-Wide](../../client-api/session/cluster-transaction)
- [Atomic Guards](../../client-api/operations/compare-exchange/atomic-guards)
- [Compare Exchange](../../client-api/operations/compare-exchange/overview)

### Availability

- [Highly Available Tasks](../../server/clustering/distribution/highly-available-tasks)

