# Rachis - RavenDB's Raft Implementation

Definition of Rachis: Spinal column, also the distal part of the shaft of a feather that bears the web.   
![Figure 1. Clustering. Rachis.](images/cluster-rachis.png)

Rachis is the name we picked for RavenDB's Raft implementation. Raft is an easy-to-understand consensus protocol. 
You can read all about it [here](https://raft.github.io/), and there's also a nice visualization.   

###Why Raft?   
Raft is a distributed consensus protocol. It allows you to reach an order set of operations across your entire cluster. 
This means that you can apply a set of operations on a state machine, and have the same final state machine in all 
nodes in the cluster. It is also drastically simpler to understand than Paxos, which is the more known alternative.   

###What is Rachis?   
It is a Raft implementation. To be rather more exact, it is a Raft implementation with the following features:

* The ability to manage a distributed set of state machine and to reliably commit updates to said state machines.   
* Dynamic topology (nodes can join and leave the cluster on the fly, including state sync).   
* Large state machines (snapshots, efficient transfers, non voting members).   
* ACID local log using Voron Storage Engine.   
* Support for in memory and persistent state machines.   
* Support for voting & non voting members.   
* A lot of small tweaks for best behavior in strange situations (forced step down, leader timeout and many more).

###How does it work?
You can read the [Raft paper](http://web.stanford.edu/~ouster/cgi-bin/papers/raft-atc14) and the full thesis, of 
course, but there are some subtleties, so it is worth going into a bit more detail.   

Clusters are typically composed of odd number of servers (3,5 or 7), which can communicate freely with one another. 
The startup process for a cluster requires us to designate a single server as the seed. This is the only server 
that can become the cluster leader during the cluster bootstrap process. It is done to make sure that during startup, 
before we had the chance to tell the servers about the cluster topology, they won't consider themselves as single 
node clusters and start accepting requests before we add them to the correct cluster.   

Only the seed server will become the leader and the others will wait for instructions. We can then let the seed 
server know about the other nodes in the cluster. It will initiate a join operation which will reach to the other 
nodes and setup the appropriate cluster topology. At that point, all the other servers are on equal footing, and 
there is no longer any meaningful distinction between them. The notion of a seed node is relevant **only** for 
cluster bootstrap, once that is done, all servers have the same configuration, and there is no difference between 
them.   

###Normal operations
During normal operations, there is going to be a leader that is going to be accepting all the [Raft Commands](../../../server/clustering/rachis/consensus-operations) requests for the cluster, 
and handle committing them cluster wide. During those operations, you can spread reads across members in the cluster, 
for better performance.    