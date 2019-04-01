# Operations: Server: How to add database node?

**AddDatabaseNodeOperation** allows you to add extra node to database group.

## Syntax

{CODE add_1@ClientApi\Operations\Server\AddDatabaseNode.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | string | Name of a database to add node |
| **node** | string | Cluster node tag to extent database to. Default: random node tag. |

| Return Value | |
| ------------- | ----- |
| `DatabasePutResult` | Database put result |

## Example I

{CODE add_2@ClientApi\Operations\Server\AddDatabaseNode.cs /}

## Example II

{CODE add_3@ClientApi\Operations\Server\AddDatabaseNode.cs /}
