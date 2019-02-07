# Rachis - RavenDB's Raft Implementation
---

{NOTE: }

* **Rachis** is the RavenDB's **Raft** implementation.  

* Definition of Rachis: Spinal column, also the distal part of the shaft of a feather that bears the web.  
{NOTE/}

---

{PANEL: What is Raft ?}

* **Raft** is a simple and easy-to-understand distributed consensus protocol.  

* It allows you to execute an ordered set of operations across your entire cluster.  
  This means that you can apply a set of operations on a state machine, and have the _same_ final state machine in all the cluster's nodes.  
  So any series of events (called a [Raft Commands](../../../server/clustering/rachis/consensus-operations#implementation-details)) will be executed in the _same_ order on each node.  

* The [Leader Node](../../../server/clustering/rachis/cluster-topology#leader) accepts all the Raft Commands requests for the cluster and handles committing them cluster-wide.  
  These Raft Commands are done only if the majority of the nodes in the cluster agreed to it !  

{INFO: Learn More about Raft}

* Nice visualizations can be found in [the secret life of data](http://thesecretlivesofdata.com/raft/) & [raft.github](https://raft.github.io/).  
* The full Raft thesis can be found in: [Raft paper](http://web.stanford.edu/~ouster/cgi-bin/papers/raft-atc14).  
{INFO/}
{PANEL/}

{PANEL: What is Rachis ?}

**Rachis** is RavenDB's Raft implementation with the following added features:  

* Support for in memory and persistent large multi-tasks state machines.  
* Reliably committing updates to a distributed set of state machines.  
* Support for voting & non-voting cluster members, see [Cluster Toplogy](../../../server/clustering/rachis/cluster-topology).   
* Dynamic topology, nodes can be added and removed from the cluster on the fly.  
* Managing situations such as handling a Leader timeout and forcing a leader to step down.  
* ACID local log using the Voron Storage Engine.   
{PANEL/}
