# Replication : Conflicts

### What is a conflict?
A conflict occurs when the same document is updated on two nodes independently of one another. 
This can happen because of a network split or because of several client updates that each talked to different 
nodes faster than we could replicate the information between the nodes.

In a distributed system, we can either choose to run a consensus (which requires consulting a majority on every decision)
or accept the potential for conflicts. 
For document writes, RavenDB chooses to accept conflicts as a tradeoff of always being able to accept writes on any node.

### Conflict detection
Each document in RavenDB cluster has a [change vector](../../server/clustering/change-vector), which is used for conflict detection.

### Conflict Resolution
When there is a conflict between two or more document versions, it will need to be resolved - 
i.e. decide which of the versions should be kept, because RavenDB has no idea how to merge conflicts.
  
There is three options of resolving the conflict
  * Using "resolve to latest", which is a built-in resolution strategy
  * Providing javascript code which would implement a resolution strategy
  * Manually resolution

#### Conflict resolution script
The conflict resolution script receives `docs` object as a parameter, which is an array of conflicting documents. 
It is expected that the script returns an object which will resolve the conflict and gets written as a new object.

### Configuring conflict resolution using the client  
Setting up conflict resolution strategy in the client is done via sending cluster-level operation - [ModifyConflictSolverOperation](../../client-api/operations/server-wide/modify-conflict-solver), which is a [Raft command](../../glossary/raft-command).
  
### Configuring conflict resolution using the Management Studio
Conflict resolution scripts can be set up also via the Management Studio as well. On the following screenshot, we can see an example of such configuration.
  
![Conflict Resolution Script in Management Studio](images/conflict-resolution-script-in-studio.jpg)  
  
  
### Conflict Resolution At Server-Side
When the server receives incoming replication batch, it compares the [change vector](../../server/clustering/change-vector) 
of the incoming document with the [change vector](../../server/clustering/change-vector) of the local document 
(if there is no local document, i.e. document replicates for the first time, an empty change vector is assumed).  
  
Lets assume _remote_cv_ to be change vector of remote document, and _local_cv_ to be a change vector of local document.
The comparison of the [change vectors](../../server/clustering/change-vector) may yield three possible results:  
  
* _remote_cv_ <= _local_cv_ --> Nothing to do in this case
* _remote_cv_ > _local_cv_ -->  Remote document is more recent than local, replace local document with remote
* _remote_cv_ **conflicts** with _local_cv_ --> Try to resolve conflict by using defined conflict resolvers

{NOTE: Change Vector comparisons}
[Change vectors](../../server/clustering/change-vector) is essentially a collection of **<[database ID](../../glossary/database-id)/[Node Tag](../../glossary/node-tag)/[Etag](../../glossary/etag)>** three-part tuples.
  
The comparison operations are defined as follows:  

* Two change vectors are equal when and only when all etags _equal_ between corresponding node and database Ids
* Change vector A is larger than change vector B if and only if all etags are _larger or equal_ between corresponding node and database Ids
* Change vector A conflicts with change vector B if and only if some etags are _larger or equal_ and some etags are _smaller_ between corresponding node and database Ids
  
_Conceptually, two conflicting change vectors means that RavenDB doesn't know which document change happened earlier._

{NOTE/}

### The resolution algorithm
When trying to resolve a conflict, RavenDB will try multiple steps to resolve it. If any of the steps succeeds, the conflict resolution will end and resolved document will be written to the storage.
  
* Step 1: Check whether the document contents are identical, if yes, then there is no conflict. In this case, the change vector of the two documents is merged, and will retain the maximum entries for the corresponding **node Tag/Database Ids**.
{NOTE Conflict between remote and local [tombstone](../../glossary/tombstone) will always resolve to the local one. /}
* Step 2: Try to resolve the conflict by using a script, which is set up by [configuring a conflict resolver](../../client-api/operations/server-wide/modify-conflict-solver).  
A resolution script is set-up per document collection.
* Step 3: If `ResolveToLatest` flag is set (by default it is set to true), resolve the conflict to the document variant where the "latest modified" is the latest.
  
If all else fails, record conflicting document variants as "Conflicted Documents" which will have to be resolved manually. 

### Manually resolving conflicts
In case one or more documents are in "conflicted" state, we need to resolve the conflict manually.  
This can be done in two ways:

* A **PUT** operation with the "conflicted" document Id will cause the conflict to be resolved to contain the value from the **PUT** operation.
* The conflict can be resolved via the Management Studio, where a document which will resolve the conflict can be picked manually from all conflicting versions.
  
![Conflict Resolution Script in Management Studio](images/resolve-conflicted-document-screen.jpg)  
