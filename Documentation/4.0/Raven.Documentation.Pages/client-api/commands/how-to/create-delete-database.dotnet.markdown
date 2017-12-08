# Commands : How to create or delete database?

This article will describe how to create or delete databases on a server using server maintenance opertaions:   
- [CreateDatabaseOperation](../../../client-api/commands/how-to/create-delete-database#createdatabaseoperation)   
- [DeleteDatabasesOperation](../../../client-api/commands/how-to/create-delete-database#deletedatabasesoperation)   

{PANEL:**CreateDatabaseOperation**}

This operation is used to create a new database on a server.

### Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync CreateDatabase@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateDatabaseAsync@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:**DeleteDatabasesOperation**}

This operation is used to delete databases from a server, with a possibility to remove all the data from hard drive.

### Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync DeleteDatabases@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async DeleteDatabasesAsync@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TABS/}

In order to delete just one database from a server, you can also use this simplified constructor:

{CODE-TABS}
{CODE-TAB:csharp:Sync DeleteDatabase@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async DeleteDatabaseAsync@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | string | Name of a database to delete |
| **hardDelete** | bool | Should all data be removed (data files, indexing files, etc.). Default: `false` |
| **fromNode** | string | Remove the database just from a specific node. Default: `null` which would delete from all |
| **timeToWaitForConfirmation** | TimeSpan | Time to wait for confirmation. Default: `15 seconds` |

{PANEL/}

## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   
