# Operations : How to stop indexing?

**StopIndexingOperation** is used to stop indexing for entire database.

Use [StopIndexOperation](../../../client-api/operations/maintenance/stop-index-operation) to stop single index.

{NOTE Indexing will be resumed automatically after server restart or after using [start indexing operation](../../../client-api/operations/maintenance/start-indexing-operation)./}

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndexing.cs /}

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndexing.cs /}

## Related articles

- [How to **disable index**?](../../../client-api/operations/maintenance/disable-index-operation)
- [How to **stop index** until restart?](../../../client-api/operations/maintenance/stop-index-operation)
- [How to **resume indexing**?](../../../client-api/operations/maintenance/start-indexing-operation)
