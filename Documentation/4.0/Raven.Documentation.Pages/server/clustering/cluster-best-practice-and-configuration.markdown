# Cluster: Best Practices

---

{NOTE: }

* In this page:
   * [Clusters should have an odd number of at least 3 nodes](../../)
   * [Nodes usually must have identical cluster configurations](../../)

{NOTE/}

---

{PANEL: }

### Clusters should have an odd number of at least 3 nodes

We recommend setting up clusters with an **odd number of nodes equal to or greater than 3**.

A _single node_ server will not have the ability to automatically failover to another node in the cluster if it goes down.  
This means that it is not highly available.  

A _two_ nodes cluster is also not recommended, since the cluster must have a majority of nodes to operate, 
and in this case, if one of the nodes is down or partitioned, no raft command will be committed, although any _database_ on the surviving node will still be responsive to the user.  

For ACID guarantees, a majority of the nodes must agree on every [cluster-wide transaction](../../server/clustering/cluster-transactions), 
so having an odd number of nodes makes achieving the majority easier.

{PANEL/}

{PANEL: }

### Nodes usually must have identical cluster configurations

Configuration mismatches tend to cause interaction problems between nodes.

If you must set cluster configurations differently in separate nodes, we recommend first testing it 
in a development environment to see that each node interacts properly with the others.
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

