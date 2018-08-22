# Cluster Transactions

The Cluster transaction feature allows to perform consistent cluster wide ACID writes and can be composed from two optional parts:  

1. [Compare Exchange](../../client-api/operations/compare-exchange/overview) values, which will be validated and executed by the cluster.
2. Store/Delete operations on documents, which are executed by the database itself.

This page contains:

- [What is ACID in a RavenDB cluster transaction](../../server/clustering/cluster-transactions#what-is-acid-in-a-ravendb-cluster-transaction)
- [Concurrent Cluster and Single-Node Transactions](../../server/clustering/cluster-transactions#oncurrent-cluster-and-single-node-transactions)
- [Flow of a cluster transaction request](../../server/clustering/cluster-transactions#flow-of-a-cluster-transaction-request)

The client API to this feature can be found [here](../../client-api/session/saving-changes#transaction-mode---cluster-wide).

## What is ACID in a RavenDB Cluster Transaction

**Atomicity** - After having a quorum for the cluster transaction request by raft and successful concurrency check for the compare exchange values, it is guaranteed to be executed.
Failure during the quorum or the concurrency check will abort the transaction, while failure during the commit of the documents will halt any further cluster transactions execution on the database until that failure is remedied.

**Consistency** - Is _fully_ guaranteed against the requested node. The node will complete the request only when the transaction is completed and the documents are persistent.
It is however _eventually_ consistent on the other nodes of the cluster, since the transaction on the database is asynchronous to the consensus itself.
Raft ensures the order of execution and the valid state of the various cluster transactions.     

**Isolation** - Execution of the compare exchange part of the transaction has concurrency control to prevent writing to the same value in an unsafe way. 
Execution of the documents part is sequential.

**Durability** - On one hand the system rely on the [Voron](../../server/storage/storage-engine) engine to ensure persistent writes to the disk, on the other hand Raft is responsible for having a cluster wide persistent state.    

## Concurrent Cluster and Single-Node Transactions

### Case 1: Multiple concurrent cluster transactions  

There is a concurrency check only on the compare exchange commands, so updating the same key concurrently will result in success for a single transaction and aborting the rest.
However there is no concurrency check on the documents, so the last document wins.

{INFO If the concurrency check of the compare exchange has passed, the transaction can't be aborted and eventually will be committed. /}

### Case 2: Concurrent cluster and non-cluster transaction

It might be that a cluster transaction occurred on one node and a regular transaction on an other node. [Replication](../../server/clustering/replication/replication) will try to synchronize the data, so in order to avoid conflicts every document that was modified under the cluster transaction will receive the special `RAFT:cluster-wide-incrementing-number` [Change Vector](../../server/clustering/replication/change-vector) and will have a precedence over a regular change vector.

## Flow of a Cluster Transaction Request

1. A request sent from the client via [SaveChanges()]() will generate a [Raft Command](../../server/clustering/rachis/consensus-operations#implementation-details) and the server will wait for a consensus on it.
2. When consensus is achieved, every node will validate the compare exchange values first, if this fails the transaction is aborted.
From the nature of the raft algorithm this should either be _eventually_ accepted on _all_ nodes or fail on _all_ of them. 
3. Once the validation has passed the request will be stored on every node to be processed in an asynchronous manner by the relevant database.
4. The relevant database noticed that it have pending cluster transactions and will start to execute them. 
Since order matters a failure at this stage will halt the cluster transaction execution until it fixed.
5. Every document that has been added by the cluster transaction gets the `RAFT:cluster-wide-incrementing-number` [Change Vector](../../server/clustering/replication/change-vector) and will have a precedence if conflict arises between that document and a document from a regular transaction.   
6. After the database has executed the requested transaction a respond will be return to the client.
On success, the client receives the transaction's [Raft Index](../../server/clustering/rachis/consensus-operations#raft-index) which will be added to any future requests, so performing an operation against any other node will wait for that index to be applied first, ensuring order of operations.