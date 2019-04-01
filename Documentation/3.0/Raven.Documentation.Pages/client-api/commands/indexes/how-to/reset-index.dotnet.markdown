# Commands: Indexes: How to reset index?

**ResetIndex** will remove all indexing data from a server for a given index so the indexation can start from scratch for that index.

## Syntax

{CODE reset_index_1@ClientApi\Commands\Indexes\HowTo\ResetIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to reset |

## Example

{CODE reset_index_2@ClientApi\Commands\Indexes\HowTo\ResetIndex.cs /}

## Related articles

- [GetIndex](../../../../client-api/commands/indexes/get)  
- [PutIndex](../../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../../client-api/commands/indexes/delete)  
