# Operations: Server: How to Promote a Database Node?

This operation is used to promote a database node. After promotion, the node is considered as a `Member`. 

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
