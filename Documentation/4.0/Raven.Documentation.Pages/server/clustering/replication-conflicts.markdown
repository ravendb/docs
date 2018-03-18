# Replication : Conflicts

### What is a conflict?
A conflict occurs when the same document is updated on two nodes independently of one another. 
This can happen because of a network split or because of several client updates that each talked to different 
nodes faster than we could replicate the information between the nodes.

In a distributed system, we can either choose to run a consensus (which requires consulting a majority on every decision)
or accept the potential for conflicts. 
For document writes, RavenDB chooses to accept conflicts as a tradeoff of always being able to accept writes.

### Conflict detection
Each document in RavenDB cluster has an [etag](../../glossary/etag) and a [change vector](../../server/clustering/change-vector).

### Conflict Resolution
When there is a conflict between two or more document versions, it will need to be resolved - 
i.e. decide which of the versions should be kept, because RavenDB has no idea how to merge conflicts.

There is two options of resolving the conflict
  * Using "resolve to latest", which is a built-in resolution strategy
  * Providing javascript code which would implement a resolution strategy.

Setting up conflict resolution strategy in the client is done via sending cluster-level operation - [ModifyConflictSolverOperation](../../client-api/operations/server-wide/modify-conflict-solver), which is a [Raft command](../../glossary/raft-command).

### Conflict Resolution At Server-Side
When the server receives incoming replication batch, it comparse the [change vector](../../server/clustering/change-vector) 
of the incoming document with the [change vector](../../server/clustering/change-vector) of the local document 
(if there is no local document, i.e. document replicates for the first time, an empty change vector is assumed).  
 
Lets assume _CVremote_ to be change vector of remote document, and _CVlocal_ to be a change vector of local document.
The comparison of the [change vectors](../../server/clustering/change-vector) may yield three possible results:  
  
* _CVremote_ <= _CVlocal_ --> Nothing to do in this case
* _CVremote_ > _CVlocal_ -->  Remote document is more recent than local, replace local document with remote
* _CVremote_ **conflicts** with _CVlocal_ --> Try to resolve conflict by using defined conflict resolvers
  
#### The resolution algorithm
When trying to resolve a conflict, RavenDB will try multiple steps to resolve it. If any of the steps succeeds, the conflict resolution will end and resolved document will be written to the storage.

* Step 1: Check whether the document contents are identical, if yes, then there is no conflict.
{NOTE Conflict between remote and local [tombstone](../../glossary/tombstone) will always resolve to the local one. /}
* Step 2: Try to resolve the conflict by using a script, which is set up by [configuring a conflict resolver](../../client-api/operations/server-wide/modify-conflict-solver).  
A resolution script is set-up per document collection.
* Step 3: If `ResolveToLatest` flag is set (by default it is set to true), resolve the conflict to the document variant where the "latest modified" is the latest.
  
If all else fails, record conflicting document variants as "Conflicted Documents".

