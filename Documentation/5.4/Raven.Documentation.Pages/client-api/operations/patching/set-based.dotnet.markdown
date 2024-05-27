# Set-Based Patch Operations

{NOTE: }
Sometimes we need to update a large number of documents matching certain criteria. A simple SQL query doing that will look like this:

`UPDATE Users SET IsActive = 0 WHERE LastLogin < '2020-01-01'`   

This is usually not the case for NoSQL databases where set based operations are not supported. RavenDB does support them by passing it a query and an operation definition. It will run the query and perform that operation on its results.

The same queries and indexes that are used for data retrieval are used for the set based operations. The syntax defining which documents to work on is exactly the same as you'd specified for those documents to be pulled from the store.

In this page:  
[Syntax overview](../../../client-api/operations/patching/set-based#syntax-overview)  
[Examples](../../../client-api/operations/patching/set-based#examples)  
[Additional notes](../../../client-api/operations/patching/set-based#additional-notes)  
{NOTE/}


{PANEL: Syntax overview}

### Sending a Patch Request

{CODE sendingSetBasedPatchRequest@Common.cs /}

| Parameter | | |
| ------------- | ------------- | ----- |
| **operation** | `PatchByQueryOperation` | PatchByQueryOperation object, describing the query and the patch that will be performed |

| Return Value | |
| ------------- | ----- |
| `Operation` | Object that allows waiting for operation to complete. It also may return information about a performed patch: see examples below. |

### PatchByQueryOperation

{CODE patchBeQueryOperationCtor1@Common.cs /}

{CODE patchBeQueryOperationCtor2@Common.cs /}

| Parameter         | Type                    | Description                                                                                                                                                                               |
|-------------------|-------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **queryToUpdate** | `string`                | The query & patch definition.<br>The RQL query starts as any other RQL query with a "from" statement.<br>It continues with an "update" clause that contains the Javascript patching code. |
| **queryToUpdate** | `IndexQuery`            | Object containing the query & the patching string,<br>with the option to use parameters.                                                                                                  |
| **options**       | `QueryOperationOptions` | Options defining how the operation will be performed and various constraints on how it is performed.<br>Default: `null`                                                                   |

{PANEL/}

{PANEL: Examples}

### Update whole collection
{CODE update_value_in_whole_collection@ClientApi\Operations\Patches\PatchRequests.cs /}

### Update by dynamic query
{CODE update-value-by-dynamic-query@ClientApi\Operations\Patches\PatchRequests.cs /}

### Update by static index query result
{CODE update-value-by-index-query@ClientApi\Operations\Patches\PatchRequests.cs /}

### Updating a collection name
{CODE change-collection-name@ClientApi\Operations\Patches\PatchRequests.cs /}

### Updating by document ID
{CODE patch-by-id@ClientApi\Operations\Patches\PatchRequests.cs /}

### Updating by document ID using parameters
{CODE patch-by-id-using-parameters@ClientApi\Operations\Patches\PatchRequests.cs /}

### Updating all documents
{CODE change-all-documents@ClientApi\Operations\Patches\PatchRequests.cs /}

### Patch on stale results
{CODE update-on-stale-results@ClientApi\Operations\Patches\PatchRequests.cs /}

### Report progress on patch
{CODE report_progress_on_patch@ClientApi\Operations\Patches\PatchRequests.cs /}

### Process patch results details
{CODE patch-request-with-details@ClientApi\Operations\Patches\PatchRequests.cs /}

{PANEL/}

{PANEL: Additional notes}

{SAFE:Safe By Default}

By default, set based operations will **not work** on indexes that are stale. The operations will **only succeed** if the specified **index is not stale**. This is to make sure you only delete what you intended to delete. 

For indexes that are updated all the time, you can set the AllowStale field of QueryOperationOptions to true if you want to patch on stale results. 

{SAFE/}

{WARNING: Patching and Concurrency} 

The patching of documents matching a specified query is run in batches of size 1024. RavenDB doesn't do concurrency checks during the operation so it can happen than a document has been updated or deleted meanwhile.

{WARNING/}

{WARNING: Patching and Transaction} 

The patching of documents matching a specified query is run in batches of size 1024.  
Each batch is handled in a separate write transaction.

{WARNING/}

{PANEL/}

## Related Articles

### Client API

- [What are operations](../../../client-api/operations/what-are-operations)

### Patching

- [Single document patch operations](../../../client-api/operations/patching/single-document)

### Knowledge Base

- [JavaScript engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
