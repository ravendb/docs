# Operations: How to Get Collection Statistics

**GetCollectionStatisticsOperation** is used to return total count of documents, conflicts and document count in each collection.

## Syntax

{CODE stats_1@ClientApi\Operations\CollectionStats.cs /}

{CODE stats_2@ClientApi\Operations\CollectionStats.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **CountOfDocuments** | int | Total documents count |
| **CountOfConflicts** | int | Total conflicts count |
| **Collections** | Dictionary&lt;string, long&gt; | Dictionary which maps collection name to document count |

## Example

{CODE stats_3@ClientApi\Operations\CollectionStats.cs /}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [How to Get Database Statistics](../../../client-api/operations/maintenance/get-statistics)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)
