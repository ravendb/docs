﻿# Operations : How to perform set based operations on documents

Sometimes we need to update a large amount of documents answering certain criteria. With SQL this is a simple operation, and a query doing that will look like this:

`UPDATE Users SET IsActive = 0 WHERE LastLogin < '2010-01-01'`   

This is usually not the case for NoSQL databases, where set based operations are not supported. RavenDB does support them, and by passing it a query and an operation definition, it will run the query and perform that operation on its results.

The same queries and indexes that are used for data retrieval are used for the set based operations, therefore the syntax defining which documents to work on is exactly the same as you'd specified for those documents to be pulled from store.

## Syntax

{PANEL: Sending patch request}

{CODE sendingSetBasedPatchRequest@Common.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operation** | [PatchByQueryOperation](../../glossary/patchQueryOperation) | PatchByQueryOperation object, describing the query and the patch that will be performed |

| Return Value | |
| ------------- | ----- |
| [Operation](../../glossary/operation) | Object that allows waiting for operation to complete, it also may return information about performed patch, see examples below. |

{PANEL/}

{PANEL: PatchByQueryOperation} 

### Simple
    This is a simpler overload of the the PatchByQueryOperation ctor, it allows defining the RQL update statement, while all other paremeters get's their default values. Best for working with non-stale data.

{CODE patchBeQueryOperationCtor1@Common.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **queryToUpdate** | string | RQL query defining the update operation. The RQL query starts as any other RQL query with "from" and "udpate" statements, but later, it continues with an "update" clause, in which you describe the javascript pathc code

### Full

{CODE patchBeQueryOperationCtor2@Common.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **queryToUpdate** | [InexQuery](../../glossary/indexQuery) | RQL query defining the update operation. The RQL query starts as any other RQL query with "from" and "udpate" statements, but later, it continues with an "update" clause, in which you describe the javascript pathc code
| **options** | [QueryOperationOptions](../../glossary/queryOperationOptions) | Options defining how the operation will be performed and various constraints on how it is performed

{PANEL/}

## Remarks

{SAFE By default, Set based operations will **not work** on indexes that are stale, and the operation will **only succeed** if the specified **index is not stale**. This is to make sure you only delete what you intended to delete. /}

For indexes that are updated all the time, you can set the AllowStale field of QueryOperationOptions to true if you want to patch on stale results anyway.

## Examples

{PANEL:Update whole collection }
{CODE update_value_in_whole_collection@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

{PANEL:Update by dynamic query}
{CODE update-value-by-dynamic-query@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

{PANEL:Update by static index query result }
{CODE update-value-by-index-query@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

{PANEL:Patch on stale results }
{CODE update-on-stale-results@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

{PANEL:Report progress on patch }
{CODE report_progress_on_patch@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

{PANEL:Process patch results details }
{CODE patch-request-with-details@ClientApi\Operations\Patches\PatchRequests.cs /}
{PANEL/}

## Related articles

- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)  
- [Session: DeleteByIndex](../../../session/how-to/delete-documents-using-index-with-linq)   
