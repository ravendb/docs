# Replication : Conflicts

### What is a Conflict?

A conflict occurs when the same document is updated on two nodes independently of one another.  
This can happen because of a network split or because of several client updates that each talked to different 
nodes faster than we could replicate the information between the nodes.  

In a distributed system, we can either choose to run a consensus (which requires consulting a majority on every decision)
or accept the potential for conflicts. 
For document writes, RavenDB chooses to accept conflicts as a tradeoff of always being able to accept writes on any node.

### Conflict Detection
Each document in a RavenDB cluster has a [change vector](../../../server/clustering/replication/change-vector) which is used for conflict detection.

### Conflict Resolution
When there is a conflict between two or more document versions, it will need to be resolved. RavenDB must decide which of the versions should be kept.
  
There are three options of resolving the conflict:

  * Using "resolve to latest", which is the built-in resolution strategy
  * Providing javascript code which would implement a resolution strategy
  * Manual resolution

#### Conflict Resolution Script
The conflict resolution script receives the `docs` object as a parameter, which is an array of conflicting documents.  
It is expected that the script returns an object which will resolve the conflict and gets written as a new object.

{PANEL: Conflict Resolution}

### Configuring Conflict Resolution Using the Client  
Setting up conflict resolution strategy in the client is done via sending cluster-level operation - [ModifyConflictSolverOperation](../../../client-api/operations/server-wide/modify-conflict-solver), which is a [Raft command](../../../glossary/raft-command).
  
### Configuring Conflict Resolution Using the Management Studio
Conflict resolution scripts can be set up also via the Management Studio as well.   
Using resolution scripts, we can implement custom logic to be executed when a conflict occurs.  
For example, given multiple conflicting Northwind's _Order_ document variants, the following script will merge the ordered items so there will be no missing items and the amount of items ordered will be maximal.

{CODE-BLOCK:csharp}
var final = docs[0];

for(var i = 1; i < docs.length; i++)
{
	var currentOrder = docs[i];
	for(var j = 0; j < currentOrder.Lines.length; j++)
	{
		var item = currentOrder.Lines[j];
		var match = final.Lines
			             .find(i => i.ProductId == item.ProductId);
		if(!match)
		{
			// not in Order, add
			final.Lines.push(item);
		}
		else
		{
			match.Quantity = Math.max(
				item.Quantity,
				match.Quantity);
		}
	}
}

return final;
{CODE-BLOCK/}

On this screenshot, we can see the conflict resolution screen in which we would write the above script.
![Conflict Resolution Script in Management Studio](images/conflict-resolution-script-in-studio.jpg)  

{PANEL/}  
  
### What Happens at Server-Side?
When the server receives an incoming replication batch, it compares the [change vector](../../../server/clustering/replication/change-vector) 
of the incoming document with the [change vector](../../../server/clustering/replication/change-vector) of the local document. 
If there is no local document, i.e. document replicates for the first time, an empty change vector is assumed.
  
Let's assume _remote_cv_ to be the change vector of a remote document, and _local_cv_ to be a change vector of a local document.
The comparison of the [change vectors](../../../server/clustering/replication/change-vector) may yield three possible results:  
  
* _remote_cv_ <= _local_cv_ --> Nothing to do in this case
* _remote_cv_ > _local_cv_ -->  Remote document is more recent than local, replace local document with remote
* _remote_cv_ **conflicts** with _local_cv_ --> Try to resolve conflict by using defined conflict resolvers
  
{NOTE: Change Vector comparisons}
[Change vectors](../../../server/clustering/replication/change-vector) is essentially a collection of **<[database ID](../../../glossary/database-id)/[Node Tag](../../glossary/node-tag)/[Etag](../../../glossary/etag)>** three-part tuples.
Conceptually, comparing two change vectors means answering a question - which change vector refers to an earlier event.  

The comparison is defined as follows:  
  
* Two change vectors are equal when and only when all etags _equal_ between corresponding node and database IDs
* Change vector A is larger than change vector B if and only if all etags are _larger or equal_ between corresponding node and database IDs
* Change vector A conflicts with change vector B if and only if at least one etags is _larger, equal or has node etag (and the other don't)_ and at least one etags is _smaller_ between corresponding node and database IDs
  
Note that the change vectors are unsortable for two reasons:

* Change vectors are unsorted collections of node tags/etag tuples, they can be sorted in multiple ways
* Conflicting change vectors cannot be compared
  
#### Example 1
Let us assume two change vectors, v1 = [A:8, B:10, C:34], v2 = [A:23, B:12, C:65]  
When we compare v1 and v2, we will do three comparisons:

* A --> 8 (v1) < 23 (v2)
* B --> 10 (v1) < 12 (v2)
* C --> 34 (v1) < 65 (v2)
  
Corresponding etags in v2 are greater than the ones in v1. This means that v1 < v2

#### Example 2
Let us assume two change vectors, v1 = [A:18, B:12, C:51], v2 = [A:23, B:12, C:65]  
When we compare v1 and v2, we will do three comparisons:

* A --> 18 (v1) < 23 (v2)
* B --> 12 (v1) = 12 (v2)
* C --> 51 (v1) < 65 (v2)
  
Corresponding etags in v2 are greater than the ones in v1. This means that v1 < v2


#### Example 3
Let us assume two change vectors, v1 = [A:18, B:12, C:65], v2 = [A:58, B:12, C:51]  
When we compare v1 and v2, we will do three comparisons:

* A --> 18 (v1) < 58 (v2)
* B --> 12 (v1) = 12 (v2)
* C --> 65 (v1) > 51 (v2)
  
Etag 'A' in v1 is smaller than in v2, and Etag 'C' is larger in v1 than in v2. This means that v1 conflicts with v2.  

{NOTE/}

### The Resolution Algorithm
When trying to resolve a conflict, RavenDB will try multiple steps to resolve it. If any of the steps succeeds, the conflict resolution will end and resolved document will be written to the storage.
  
* Step 1: If `ResolveToLatest` flag is set (by default it is set to true), resolve the conflict to the document variant where the "latest modified" is the latest.
* Step 2: Check whether the document contents are identical, if yes, then there is no conflict. In this case, the change vector of the two documents is merged, and will retain the maximum entries for the corresponding **node Tag/Database IDs**.
{NOTE The identity check applies to [tombstone](../../../glossary/tombstone) as well, and it will always resolve to the local one, since the tombstones are always considered equal. /}
* Step 3: Try to resolve the conflict by using a script, which is set up by [configuring a conflict resolver](../../../server/clustering/replication/replication-conflicts#conflict-resolution).  
A resolution script is set-up per document collection.
  
If all else fails, record conflicting document variants as "Conflicted Documents" which will have to be resolved manually. 

### Manually Resolving Conflicts
In case one or more documents are in a "conflicted" state, we need to resolve the conflict manually.  
  
This can be done in two ways:

* A **PUT** operation with the "conflicted" document Id will cause the conflict to be resolved to contain the value from the **PUT** operation
* The conflict can be resolved via the Management Studio where a document which will resolve the conflict can be picked manually from all conflicting versions. The Studio will attempt at merging the documents as much as possible, and where it is not possible, the studio will leave merge tags that will mark the merged areas

  
![Conflict Resolution Script in Management Studio](images/resolve-conflicted-document-screen.jpg)  

{NOTE In case any document is conflicted and the conflict is not resolved, the conflicting document variants are stored as revisions to their original document while the document itself is removed. /}
  
In the example we have in this conflict, once we remove merging tags in the document and press "resolve and save",  
the resulting document will look like the following:

![Document after resolved conflict](images/resolve-conflicted-document-screen2.jpg)  
