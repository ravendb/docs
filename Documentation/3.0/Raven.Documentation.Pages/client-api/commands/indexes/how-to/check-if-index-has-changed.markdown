# Commands : Indexes : How to check if index has changed?

**IndexHasChanged** will let you check if the given index definition differs from the one on server. This might be useful when you want to check prior index deployment if index will be overwritten and indexing data will be lost.

## Syntax

{CODE index_has_changed_1@ClientApi\Commands\Indexes\HowTo\IndexHasChanged.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to check |
| **indexDef** | [IndexDefinition](../../../../glossary/indexes/index-definition) | index definition |

| Return Value | |
| ------------- | ----- |
| true | if index **does not exist** on server |
| true | if index definition **does not match** the one from **indexDef** parameter |
| false | if there are no differences between index definition on server and the one from **indexDef** parameter |

## Example

{CODE reset_index_2@ClientApi\Commands\Indexes\HowTo\ResetIndex.cs /}

## Related articles

- [GetIndex](../../../../client-api/commands/indexes/get)  
- [PutIndex](../../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../../client-api/commands/indexes/delete)  