# Operations : Server : How to create a database?

This operation is used to create a new database on a server.

## Syntax

{CODE:csharp create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseRecord** | string | instance of `DatabaseRecord` containing database configuration |
| **replicationFactor** | bool | indicates how many nodes should contain the database |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}
