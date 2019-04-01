# Operations: How to Get Collection Statistics

**GetCollectionStatisticsOperation** is used to return total count of documents, conflicts and document count in each collection.

## Syntax

{CODE:java stats_1@ClientApi\Operations\CollectionStats.java /}

{CODE:java stats_2@ClientApi\Operations\CollectionStats.java /}

| Return Value | | |
| ------------- | ----- | ---- |
| **CountOfDocuments** | int | Total documents count |
| **CountOfConflicts** | int | Total conflicts count |
| **Collections** | Map&lt;String, long&gt; | Maps collection name to document count |

## Example

{CODE:java stats_3@ClientApi\Operations\CollectionStats.java /}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [How to Get Database Statistics](../../../client-api/operations/maintenance/get-statistics)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)
