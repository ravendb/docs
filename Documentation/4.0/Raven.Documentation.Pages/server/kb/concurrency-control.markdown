# Concurrency control & change vectors

RavenDB defines some simple rules to determine how to handle concurrent operations on the same document across the cluster. It uses the document's [change vector](../../glossary/change-vector).
This allows RavenDB to detect concurrency issues when writing to a document and conflicts when the document has been concurrently modified on different nodes during network partition.

Every document in RavenDB has a corresponding change vector. This change vector is updated by RavenDB every time the document is changed. This happens when on document creation and 
any modification (such as `PUT`, `PATCH`, etc). A delete operation will also cause RavenDB to update the document change vector, however, at that point the change vector will belong to
the document tombstone (since the document itself has already been deleted).

A change vector is present in the document's metadata (`@metadata.@change-vector`) and typically looks like this: `A:1-0tIXNUeUckSe73dUR6rjrA, B:7-kSXfVRAkKEmffZpyfkd+Zw`. Each time
a document is updated, the server will update the change vector. This is mostly used internally inside RavenDB for many purposes (conflict detection, deciding what documents a particular
subscription already seen, what send to an ETL destination, etc) but can also be very useful for clients.

In particular, the change vector is _guarnateed_ to change whenever the document changes and can be used as part of optimistic concurrency checks. A document modification (`PUT`, `PATCH` 
or `DELETE`) can all specify an expected change vector for a document (with an empty change vector signifying that the document does not exists). In such a case, all operations in the 
transaction will be aborted and no changes will be applied to any of the documents modified in the transaction.

## Concurrency control at the cluster

RavenDB implements a multi master strategy for handling database writes. This means that it will _never_ reject a valid write to a document (under the assumption that if you tried to write
data to the database, you probably are interested in keeping it). That behavior can lead to certain edge cases. In particular, under network partition scenario, it is possible for two clients
to talk to two RavenDB nodes and to update a document with optimistic concurrency check. 

The concurrency check is done at the _local node_ level, to ensure that we can still process writes in the case of a network partition or partial failure scenario. That can mean that two
writes to separate servers will both succeed, even if each write specified the same original change vector, because each server did the check indepdendently. Under such a scenario, the 
generated change vectors for the document on each server will be different, and as soon as replication between these nodes will run the databases will detect this conflict and resolve
it according to your conflict resolution strategy.

In practice, this kind of scenario is rare, since RavenDB attempts to direct all writes to the same node for each database under normal conditions to
ensure that optimistic concurrency checks will always happen on the same machine.

## Related Articles

- [Glossary: Change Vector](../../glossary/change-vector)
- [Client API: How to enable optimistic concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency)
