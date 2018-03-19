# Clustering : Replication

Replication in RavenDB is the process of transferring _data_ from one database to another.  

* We consider those elements to be _data_:  
  * Documents  
  * Revisions  
  * Attachments  
  * Conflicts  

{PANEL: How does replication works}
The replication process sends _data_ over a _TCP connection_ by the modification order, meaning that the least modified data gets sent first.  
Internally we keep an [ETag](../../glossary/etag) per data element, which is an incremental integer value, and this is how we are able to determine what is the order of modifications.  
Each replication process has a _source_ , _destination_ and a last confirmed _ETag_ which is basically a cursor to where we have gotten to in the replication process.  
The _data_ is sent from the _source_, in batches, to the _destination_.  
When the _destination_ confirms getting the _data_ the _ETag_ is then advanced and the next batch of _data_ is sent.  
In case of failure of the process, it will re-start with the [initial handshake procedure](../clustering/replication#replication-handshake-procedure).
{PANEL/}

{PANEL: Replication Handshake Procedure}
Whenever the replication process between two databases start it has to determine the process state, the same logic applies for new or re-established replication.  
The first message has been sent is a request to establish a TCP connection of type _replication_ with a _protocol version_.  
At this point, the _destination server_ verifies that the protocol version matches and that the request is authorized.  
Once the _source_ gets the OK message it queries the _destination_ about the lastest ETag it got from him.  
The _destination_ sends back a _heartbeat_ message with both the latest _ETag_ he got from the _source_ and the current [change vector](../clustering/change-vector)of the database.  
The _ETag_ is used as a starting point for the replication process but it is then been filtered by the _destination's_ current _change vector_,
meaning we will skip documents with higher _Etag_ and lower _change vecotr_, this is done so we won't send the same data from multiple sources, you can read more about this [below](../clustering/replication#preventing-the-ripple-effect).  
{PANEL/}

{PANEL: Preventing the ripple effect}
RavenDB [_database group_](../../glossary/database-group) is a well connected graph of replication channels, meaning that if there are `n` nodes in a _database group_ there are `n*(n-1)` replication channels.  
We wanted to prevent the case where inserting data into one database will cause the data to propagate multiple times through multiple paths to all the other nodes.  
We have managed to do so by delaying the propagation of data coming from the replication logic itself.  
If the sole source of incoming data is replication we will not replicate it right away, we will wait up to `15 seconds` before sending data.  
This will allow the destination to inform us about his current change vector and most of the time the data will get filtered at the source.  
On a stable system the steady state will have a _spanning tree_ of replication channels, `n-1` of the fastest channels available that are doing the actual work and the rest are just sending heartbeats.  
{PANEL/}

{PANEL: Replication transaction boundary}
In RavenDB 4.x we extended the boundry of a transaction across multiple nodes.  
Let say that you want to modify two documents `X` and `Y` in the same transaction and the commit happens on node `A`, what would happen on node `B`?  
In previous versions of RavenDB, the modifications of `X` and `Y` could have ended up in two different replication batches.  
This may cause data in-consistency issues if `X` and `Y` are somehow related (which they probably are since they were modified in the same transaction).  
Let's say that `X` is a _Bitcoin_ wallet and `Y` is another _Bitcoin_ wallet and we want to move 1 _Bitcoin_ from `X` to `Y`.  
What will happen on the server side, node `A`, transaction #17 will contain modification to `X` with ETag 1024 and modification to `Y` with ETag 1025.  
Let's say that our replication batch sends all documents from ETag 1 to 1024, in the previous version the modification of `Y` would not have been replicated to node `B`.  
While node `A` is preparing to send the next replication batch server `B` shows that `X` has one fewer _Bitcoin_ while `Y` remains the same so the total _Bitcoin_ between `X` and `Y` just droped by one.  
Luckly in RavenDB 4.x this temporary data inconsistency is now prevented by extending the replication batch to include all documents that were modified in the same transaction as the last document, 
meaning that in our case etag 1025 will also be sent in the batch since it was modified in transaction #17 like etag 1024 which was suppose to be last.  

This sounds great but there is one caveat what will happen if `Y` is modified by another transaction, transaction #18, before the replication sends it over to node `B`?  
RavenDB replication process only sends the latest state of the data meaning that even though `X` and `Y` are modified in the same transaction they may be replicated in diffrent batches if one of them was modified in the meanwhile.  
Consider the case before with the _Bitcoin_ wallet, let's say that your govrement decide to put a 10% tax on _Bitcoin_ transaction and the tax can only apply after the transaction is confirmed.  
We can not calculate the tax in transaction #17 and modify `Y` accordingly since the law forbids it and so we must send another transaction #18 deducting 0.1 _Bitcoin_ from `Y`.  
Now what could happen is that the replication batch will send `X` without `Y` since it was not modified in the same transaction and node `B` will have data inconsistency issues again.  
Luckily not all is lost there are a couple of solutions this problem:  

1. You can use [write assurence](../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
2. You could enable [revisions](../revisions) and thus we will replicate the revisions along side the documents and this way `X` will be sent with `Y'` which is the revision of `Y` after we added one _Bitcoin_ to it but before we took the 0.1 _Bitcoin_ tax off.  

{PANEL/}

{PANEL: Rehab state and database group observer}

In a [database group](../../glossary/database-group)  we might have a node that is behind the other nodes, and currently being updated by one of the other nodes in the _database group_.  
This may happen for a few reason, it was just added, it was down for a while or it was just restored from a backup snapshot.  
We consider such node to be in a _rehab_ state untill it caught up with the rest of the _database group_.  
While a node is in a _rehab_ state it may not be assigned any tasks such as _backup_, _ETL_ or _subscriptions_.  

Every _database group_ has an _observer_, the _observer_ is a node that is assigned to monitor a _database group_.  
The _observer_ periodically pings all members of a _database group_ and checks that they are both up and that their data is up to date.  
If the _observer_ notice that a node is down or lagging behind it will mark it as in _rehab_ state and thus it will not get assigned new tasks.  
Moreover, if tasks were assigned to a node and it just got updated to a _rehand_ state his assigned task will get re-assgined to other nodes in the _database group_.   
Note that although the _observer_ is making the decision to mark a node as in _rehab_ the actual modification of the state is done by the _cluster leader_.  
If an _observer_  is not responding the cluster leader will assign a different node to become the _database group_ _observer_.
{PANEL/}

{PANEL: External Replication}
We call the task of replicating between two different database groups _external replication_.  
There is actually no difference in the implementation of an _external replication_ and a regular replication process.  
The reason we define them differently is because of the default behavior of a cluster to setup well conected _database groups_.  
This may be limiting if you wish to design your own replication topology and _external replication_ is the solution for those unique cases.  
{PANEL/}

{PANEL: Delayed Replication}
In RavenDB v4.x we introduced a new kind of replication, _delayed replication_, what it does is replicating data that is delayed by `X` amount of time.  
The _delayed replication_ works just like notmal replication but instand of sending data right away it waits `X` amount of time.  
Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data due to a faulty patch script or other human errors.  
While you can and should always use backup for those cases, having a live database makes it super fast to failover to and prevent business lose while you take down the faulty databases and restore them.  
{PANEL/}
