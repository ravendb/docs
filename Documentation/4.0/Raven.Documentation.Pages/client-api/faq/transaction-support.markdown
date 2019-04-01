# FAQ: Transaction Support in RavenDB

{PANEL:ACID storage}

All storage operations performed in RavenDB are fully ACID (Atomicity, Consistency, Isolation, Durability), this is because internally RavenDB used a custom made storage engine called *Voron*, which guarantees all the properties of the ACID, no matter if those are executed on document, index or cluster storage data.

{PANEL/}

{PANEL:ACID for document operations}

In RavenDB all actions performed on documents are fully ACID. An each document operation or a batch of operations applied to a set of documents sent in a single HTTP request will execute in a single transaction. The ACID properties of RavenDB are:

* _Atomicity_  - All operations are atomic. Either they succeed or fail, not midway operation. In particular, operations on multiple documents will all happen atomically, all the way or none at all.

* _Consistency and Isolation / Consistency of Scans_ - In a single transaction, all operations operate under snapshot isolation. Even if you access multiple documents, you'll get all of their state as it was in the beginning of the request.

* _Visibility_ - All transactions are immediately made available on commit. Thus, if a transaction is commit after updating two docs, you'll always see the updates to those two docs at the same time. (That is, you either see the updates to both, or you don't see the update to either one).

* _Durability_ - If an operation has completed successfully, it was fsync'ed to disk. Reads will never return any data that hasn't been flushed to disk.

All of these constraints are ensured when you use [a session](../session/what-is-a-session-and-how-does-it-work) and call [`SaveChanges`](../session/saving-changes).

{PANEL/}

{PANEL:BASE for query operations}

The transaction model is different when indexes are involved, because indexes are BASE (Basically Available, Soft state, Eventual consistency), not ACID. Then the following constraints are applied to query operations:

* _Basically Available_ - Index query results will be always available but they might be stale.

* _Soft state_ - The state of the system could change over the time because some amount of time is needed to perform the indexing. This is an incremental operation the less documents remains to index, the more accurate index results we have.

* _Eventual consistency_ - The database will eventually become consistent once it stops receiving new documents and the indexing operation finishes.

{PANEL/}

## Related Articles

### Server

- [Storage Engine](../../server/storage/storage-engine)
