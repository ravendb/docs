# Operations: Server: How to delete a database?

This operation is used to delete databases from a server, with a possibility to remove all the data from hard drive.

## Syntax

{CODE:csharp delete_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Name of a database to delete |
| **HardDelete** | bool | Should all data be removed (data files, indexing files, etc.). |
| **FromNode** | string | Remove the database just from a specific node. Default: `null` which would delete from all |
| **TimeToWaitForConfirmation** | TimeSpan | Time to wait for confirmation. Default: `null` will user server default (15 seconds) |

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync DeleteDatabases@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async DeleteDatabasesAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

## Example II

In order to delete just one database from a server, you can also use this simplified constructor

{CODE-TABS}
{CODE-TAB:csharp:Sync DeleteDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async DeleteDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}
