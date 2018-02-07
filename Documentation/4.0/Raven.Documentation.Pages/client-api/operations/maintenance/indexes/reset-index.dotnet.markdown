# Operations : How to Reset an Index

**ResetIndexOperation** will remove all indexing data from a server for a given index so the indexation can start from scratch for that index.

## Syntax

{CODE reset_index_1@ClientApi\Operations\ResetIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to reset |


## Example

{CODE reset_index_2@ClientApi\Operations\ResetIndex.cs /}

## Related Articles

- [How to **get index**?](../../../../client-api/operations/maintenance/indexes/get-index)  
- [How to **put indexes**?](../../../../client-api/operations/maintenance/indexes/put-indexes)  
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
