# Operations : How to disable index?

**DisableIndexOperation** is used to turn the indexing off for given index. Querying `disabled` index is allowed, but may return stale results.

{NOTE Unlike [StopIndex](../../../client-api/operations/maintenance/stop-index-operation) or [StopIndexing](../../../client-api/operations/maintenance/stop-indexing-operation) disable index is persistent operation, so index remains disabled even after server restart. /}


## Syntax

{CODE disable_1@ClientApi\Operations\Indexes\DisableIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to disable indexing |

## Example

{CODE disable_2@ClientApi\Operations\Indexes\DisableIndex.cs /}

## Related articles

- [How to **enable index**?](../../../client-api/operations/maintenance/enable-index-operation)
- [How to **stop index** until restart?](../../../client-api/operations/maintenance/stop-index-operation)
