# Operations : Server : How to promote database node?

This operation is used to promote database node. After promotion node is considered as `Member`. 

## Syntax

{CODE:csharp promote_1@ClientApi\Operations\Server\PromoteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | string | Name of a database |
| **node** | string | Node tag to promote into database group `Member` |

## Example

{CODE:csharp promote_2@ClientApi\Operations\Server\PromoteDatabase.cs /}


## Related Articles

- [Explanation of Node Types](../../../studio/server/cluster/cluster-view#nodes-types)
