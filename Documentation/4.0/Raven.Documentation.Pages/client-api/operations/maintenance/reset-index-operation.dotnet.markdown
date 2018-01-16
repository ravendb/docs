# Operations : How to reset index?

**ResetIndexOperation** will remove all indexing data from a server for a given index so the indexation can start from scratch for that index.

## Syntax

{CODE reset_index_1@ClientApi\Operations\ResetIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to reset |


## Example

{CODE reset_index_2@ClientApi\Operations\ResetIndex.cs /}

## Related articles

- [GetIndex](../../../../client-api/operations/maintenance/get-index-operation)  
- [PutIndexes](../../../../client-api/operations/maintenance/put-indexes-operation)  
- [DeleteIndex](../../../../client-api/operations/maintenance/delete-index-operation)  
