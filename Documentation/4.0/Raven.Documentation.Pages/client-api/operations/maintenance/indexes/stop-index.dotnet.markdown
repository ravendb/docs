# Operations : How to stop index?

**StopIndexOperation** is used to stop indexing for index 

{NOTE Indexing will be resumed automatically after server restart. /}

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to stop indexing |

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndex.cs /}

## Related articles

- [How to **enable index**?](../../../client-api/operations/maintenance/enable-index-operation)
- [How to **stop indexing** until restart?](../../../client-api/operations/maintenance/stop-indexing-operation)
