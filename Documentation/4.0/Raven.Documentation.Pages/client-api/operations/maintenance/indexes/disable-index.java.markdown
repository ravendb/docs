# Operations : How to Disable an Index

The **DisableIndexOperation** is used to turn the indexing off for a given index. Querying a `disabled` index is allowed, but it may return stale results.

{NOTE Unlike [StopIndex](../../../../client-api/operations/maintenance/indexes/stop-index) or [StopIndexing](../../../../client-api/operations/maintenance/indexes/stop-indexing) disable index is a persistent operation, so the index remains disabled even after a server restart. /}


## Syntax

{CODE:java disable_1@ClientApi\Operations\Indexes\DisableIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | name of an index to disable indexing |

## Example

{CODE:java disable_2@ClientApi\Operations\Indexes\DisableIndex.java /}

## Related Articles

- [How to **enable index**?](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to **stop index** until restart?](../../../../client-api/operations/maintenance/indexes/stop-index)
