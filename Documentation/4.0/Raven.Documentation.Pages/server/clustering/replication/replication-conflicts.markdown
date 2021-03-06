﻿# Replication Conflicts

## What is a Conflict?

A conflict occurs when the same document is updated concurrently on two different nodes.  
This can happen because of a network split or because of several client updates that each talked to different 
nodes faster than we could replicate the information between the nodes.  

In a distributed system, we can either choose to run a consensus (which requires consulting a majority on every decision)
or accept the potential for conflicts. 

For document writes, RavenDB chooses to accept conflicts as a trade-off of always being able to accept writes on any node.

## Conflict Detection
Each document in a RavenDB cluster has a [Change Vector](../../../server/clustering/replication/change-vector) which is used for conflict detection.

When the server receives an incoming replication batch, it compares the [Change Vector](../../../server/clustering/replication/change-vector) 
of the incoming document with the [Change Vector](../../../server/clustering/replication/change-vector) of the local document. 
  
Let's assume _remote_cv_ to be the change vector of a remote document, and _local_cv_ to be a change vector of a local document.  
The [Comparison](../../../server/clustering/replication/change-vector#change-vector-comparisons) of the [Change Vectors](../../../server/clustering/replication/change-vector) may yield three possible results:  
  
* _remote_cv_ <= _local_cv_ -> Nothing to do, the local document is more up-to-date.
* _remote_cv_ > _local_cv_ ->  Remote document is more recent than local, replace local document with remote
* _remote_cv_ **conflicts** with _local_cv_ -> Try to resolve conflict.

When there is a conflict between two or more document versions, RavenDB will try multiple steps to resolve it. If any of the steps succeeds, the conflict resolution will end and resolved document will be written to the storage.
  
1. If `ResolveToLatest` flag is set (by default it is `true`), resolve the conflict to the document variant where the [Latest Modified](../../../client-api/session/how-to/get-entity-last-modified) property is the latest.
2. Check whether the document contents are identical, if so, then there is no conflict and the change vector of the two documents is merged.
{NOTE The identity check applies to [Tombstone](../../../glossary/tombstone) as well, and it will always resolve to the local one, since the tombstones are always considered equal. /}
3. Try to resolve the conflict by using a script, which is set up by configuring a [Conflict Resolver](../../../server/clustering/replication/replication-conflicts#conflict-resolution-script).  
4. If all else fails, record conflicting document variants as "Conflicted Documents" which will have to be resolved [Manually](../../../server/clustering/replication/replication-conflicts#manually-resolving-conflicts). 

{PANEL: Conflict Resolution Script} 
A resolution script is defined per collection and have the following input arguments:

| Name | Type | Description |
| - | - | - |
| `docs` | Array of Objects | An unsorted array of the conflicted documents, excluding the `Tombstones`. |
| `hasTomestone` | boolean | Indicate if there is a `Tombstone` among the conflicted documents. |
| `resolveToTombstone` | string | upon returning will resolve the conflict to `Tombstone`. |


The returned value from the script, can be:

* An object (any object), which will be the conflict resolution.
* `null` or simply `return;`, which will leave the conflict as is.
* The `resolveToTombstone` string, which will resolve the conflict to `Tombstone`.

{NOTE: Script Exception}
If the script will encounter an exception, the execution will aborted and the conflict will remain.
{NOTE/}

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
			             .find(i => i.Product == item.Product);
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

{PANEL:Manually Resolving Conflicts}
In case one or more documents are in a "conflicted" state, we need to resolve the conflict manually.  
  
This can be done in two ways:

* A **PUT** operation with the "conflicted" document Id will cause the conflict to be resolved to contain the value from the **PUT** operation
* The conflict can be resolved via the Management Studio where a document which will resolve the conflict can be picked manually from all conflicting versions. The Studio will attempt at merging the documents as much as possible, and where it is not possible, the studio will leave merge tags that will mark the merged areas

  
![Conflict Resolution Script in Management Studio](images/resolve-conflicted-document-screen.jpg)  

{NOTE In case any document is conflicted and the conflict is not resolved, the conflicting document variants are stored as revisions to their original document while the document itself is removed. /}
  
In the example we have in this conflict, once we remove merging tags in the document and press "resolve and save",  
the resulting document will look like the following:

![Document after resolved conflict](images/resolve-conflicted-document-screen2.jpg)  
{PANEL/}
