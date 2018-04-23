# Document conflict in client-side
  
## What are conflicts?
When two or more changes of a single document are done concurrently in two separate nodes, 
RavenDB cannot know which one of the changes is the correct one. This is called document conflict.  
For more information about conflicts and their resolution, see [article about conflicts](../../server/clustering/replication/replication-conflicts).  

{NOTE By default, RavenDB will solve conflicts using "resolve to latest" strategy, thus the conflict will be resolved to a document with the latest 'modified date'./}
  
## When is conflict exception thrown?
DocumentConflictException will be thrown for any access of a conflicted document.

### Additionally
 Fetching attachments of a conflicted document will throw `InvalidOperationException` on the server.

## How the conflict can be resolved from the client side?
 * PUT of a document with ID that belongs to conflicted document will resolve the conflict.

{CODE PUT_Sample@ClientApi\Cluster\DocumentConflictsInClientSide.cs /}

 * DELETE of a conflicted document will resolve its conflict.  

{CODE DELETE_Sample@ClientApi\Cluster\DocumentConflictsInClientSide.cs /}

 * Incoming replication will resolve conflict if the incoming document has a larger [change vector](../../server/clustering/change-vector).

## Modifying conflict resolution from the client-side
In RavenDB we can resolve conflicts either by resolving to latest or by using conflict resolution script to decide which one of the conflicted document variants are the ones that need to be kept. The following is an example how we can set conflict resolution script from the client-side.
{CODE Modify_conflict_resolution_sample@ClientApi\Cluster\DocumentConflictsInClientSide.cs /}
