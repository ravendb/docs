# Clustering Overview

[RavenDB Cluster](../../glossary/ravendb-cluster) consists of one or more RavenDB Instances which are called [Cluster Nodes](../../glossary/cluster-node).
In RavenDB 4.x clustering and replication are complementary parts of the same feature. While [Rachis](../../../server/clustering/rachis/what-is-rachis) responsible for keeping the cluster nodes in consensus, the replication keeps the documents across nodes in sync.

{INFO:Rachis is the RavenDB Raft Implementation}
Raft is a "[distributed consensus](https://en.wikipedia.org/wiki/Consensus_(computer_science)) algorithm".  
[Raft](../../glossary/raft-algorithm) is designed to allow the cluster to agree over the order of events that happen on different nodes.  
Those events are called [Raft Commands](../../glossary/raft-command).  
{INFO/}

The act of creating a new database should have a consensus in the cluster, only if the majority of nodes agreed to it, the database will be created.
However the decision on which nodes to created it, defined either by the user giving the specific nodes or by providing a [Replication Factor](../../glossary/database-group) to set the desired number replicas of the database.
A group of nodes that contain the same database is called [Database Group](../../glossary/database-group) and there is a master-master replication between every member in that group.

Any document related change, such as CRUD operation or query is _not_ going through Raft, but will be replicated to the other database replicas to keep the data up-to-date.
However the operation of creating an index is a [Cluster Operation](../../server/clustering/rachis/consensus-operations) which requires majority in order to be applied.

Clustering introduce also [Task Distribution](../../server/clustering/distribution/highly-available-tasks), which allows for tasks such as [Backups](../../server/ongoing-tasks/backups/basic), [External Replication](../../ongoing-tasks/external-replication/basic), [ETL Replication](../../ongoing-tasks/etl/basics), [Subscriptions](../../ongoing-tasks/subscriptions/basic) to be operational even if the current node to which the client connect to is down. 

Another important component in the RavenDB Cluster is the [Observer](../../../server/clustering/distribution/cluster-observer), his job is to monitor and maintain the status of every database in the cluster.   

{INFO:For Example}
Let us assume a five node cluster, with servers A, B, C, D, E.  
Then, we create a database with replication factor of 3 and define a backup task.

The newly created database will be distributed automatically to three of the cluster nodes. Let's assume it is distributed to B, C and E (So the database group is [B,C,E]).  
And the cluster decides to preform the backup task from the database on node C.

If node C goes offline or is not reachable, the Observer will notice it and relocate the database to other available node. 
Meanwhile the backup task will failover to be perform by other available node from the Database Group. 
{INFO/}
