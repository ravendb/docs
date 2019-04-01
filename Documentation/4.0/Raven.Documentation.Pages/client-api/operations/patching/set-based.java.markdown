# Patching: How to Perform Set Based Operations on Documents

{NOTE: }
Sometimes we need to update a large amount of documents answering certain criteria. A simple SQL query doing that will look like this:

`UPDATE Users SET IsActive = 0 WHERE LastLogin < '2010-01-01'`   

This is usually not the case for NoSQL databases where set based operations are not supported. RavenDB does support them by passing it a query and an operation definition. It will run the query and perform that operation on its results.

The same queries and indexes that are used for data retrieval are used for the set based operations. The syntax defining which documents to work on is exactly the same as you'd specified for those documents to be pulled from the store.

In this page:  
[Syntax overview](../../../client-api/operations/patching/set-based#syntax-overview)  
[Examples](../../../client-api/operations/patching/set-based#examples)  
[Additional notes](../../../client-api/operations/patching/set-based#additional-notes)  
{NOTE/}


{PANEL: Syntax overview}

### Sending a Patch Request

{CODE:java sendingSetBasedPatchRequest@Common.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operation** | `PatchByQueryOperation` | PatchByQueryOperation object, describing the query and the patch that will be performed |

| Return Value | |
| ------------- | ----- |
| `Operation` | Object that allows waiting for operation to complete. It also may return information about a performed patch: see examples below. |

### PatchByQueryOperation

{CODE:java patchBeQueryOperationCtor1@Common.java /}

{CODE:java patchBeQueryOperationCtor2@Common.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **queryToUpdate** | `String` or `IndexQuery` | RQL query defining the update operation. The RQL query starts as any other RQL query with "from" and "update" statements. Later, it continues with an "update" clause in which you describe the JavaScript patch code
| **options** | `QueryOperationOptions` | Options defining how the operation will be performed and various constraints on how it is performed

{PANEL/}

{PANEL:Examples}

### Update whole collection
{CODE:java update_value_in_whole_collection@ClientApi\Operations\Patches\PatchRequests.java /}

### Update by dynamic query
{CODE:java update-value-by-dynamic-query@ClientApi\Operations\Patches\PatchRequests.java /}

### Update by static index query result
{CODE:java update-value-by-index-query@ClientApi\Operations\Patches\PatchRequests.java /}

### Updating a collection name
{CODE:java change-collection-name@ClientApi\Operations\Patches\PatchRequests.java /}

### Updating by document ID
{CODE:java patch-by-id@ClientApi\Operations\Patches\PatchRequests.java /}

### Updating by document ID using parameters
{CODE:java patch-by-id-using-parameters@ClientApi\Operations\Patches\PatchRequests.java /}

### Updating all documents
{CODE:java change-all-documents@ClientApi\Operations\Patches\PatchRequests.java /}

### Patch on stale results
{CODE:java update-on-stale-results@ClientApi\Operations\Patches\PatchRequests.java /}

{PANEL/}

{PANEL: Additional notes}

{SAFE:Safe By Default}

By default, set based operations will **not work** on indexes that are stale. The operations will **only succeed** if the specified **index is not stale**. This is to make sure you only delete what you intended to delete. 

For indexes that are updated all the time, you can set the AllowStale field of QueryOperationOptions to true if you want to patch on stale results. 

{SAFE/}

{WARNING: Patching and Concurrency} 

The patching of documents matching a specified query is run in batches of size 1024. RavenDB doesn't do concurrency checks during the operation so it can happen than a document has been updated or deleted meanwhile.

{WARNING/}

{PANEL/}

## Related Articles

### Patching

- [Single Document Based Patch Operation](../../../client-api/operations/patching/single-document)

### Knowledge Base

- [JavaScript Engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript Engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
