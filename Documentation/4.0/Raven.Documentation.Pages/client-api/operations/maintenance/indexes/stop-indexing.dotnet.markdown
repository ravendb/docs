# Operations : How to Stop Indexing

**StopIndexingOperation** is used to stop indexing for the entire database.

Use [StopIndexOperation](../../../../client-api/operations/maintenance/indexes/stop-index) to stop single index.

{NOTE Indexing will be resumed automatically after a server restart or after using [start indexing operation](../../../../client-api/operations/maintenance/indexes/start-indexing)./}

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndexing.cs /}

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndexing.cs /}

## Related Articles

- [How to **disable index**?](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to **stop index** until restart?](../../../../client-api/operations/maintenance/indexes/stop-index)
- [How to **resume indexing**?](../../../../client-api/operations/maintenance/indexes/start-indexing)
