# Operations : How to Stop an Index

The **StopIndexOperation** is used to stop indexing for an index. 

{NOTE Indexing will be resumed automatically after server restart. /}

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to stop indexing |

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndex.cs /}

## Related Articles

- [How to **enable index**?](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to **stop indexing** until restart?](../../../../client-api/operations/maintenance/indexes/stop-indexing)
