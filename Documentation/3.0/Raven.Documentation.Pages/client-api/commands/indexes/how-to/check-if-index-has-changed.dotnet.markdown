# Commands: Indexes: How to check if an index has changed?

**IndexHasChanged** will let you check if the given index definition differs from the one on a server. This might be useful when you want to check the prior index deployment, if index will be overwritten, and if indexing data will be lost.

## Syntax

{CODE index_has_changed_1@ClientApi\Commands\Indexes\HowTo\IndexHasChanged.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to check |
| **indexDef** | [IndexDefinition](../../../../glossary/index-definition) | index definition |

| Return Value | |
| ------------- | ----- |
| true | if an index **does not exist** on a server |
| true | if an index definition **does not match** the one from the **indexDef** parameter |
| false | if there are no differences between an index definition on server and the one from the **indexDef** parameter |

## Example

{CODE index_has_changed_2@ClientApi\Commands\Indexes\HowTo\IndexHasChanged.cs /}

## Related articles

- [GetIndex](../../../../client-api/commands/indexes/get)  
- [PutIndex](../../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../../client-api/commands/indexes/delete)  
