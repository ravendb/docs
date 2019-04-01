# Commands: How to create or delete database?

This article will describe the following commands (and extensions) that enable you to manage databases on a server:   
- [CreateDatabase](../../../client-api/commands/how-to/create-delete-database#createdatabase)   
- [DeleteDatabase](../../../client-api/commands/how-to/create-delete-database#deletedatabase)   
- [EnsureDatabaseExists](../../../client-api/commands/how-to/create-delete-database#ensuredatabaseexists---extension-method)   

{PANEL:**CreateDatabase**}

This method is used to create a new database on a server.

### Syntax

{CODE create_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseDocument** | [DatabaseDocument](../../../glossary/database-document) | A document containing all configuration options for new database (e.g. active bundles, name/id, path to data) |

### Example

{CODE create_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

{PANEL/}

{PANEL:**DeleteDatabase**}

This method is used to delete a database from a server, with a possibility to remove all the data from hard drive.

### Syntax

{CODE delete_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **dbName** | string | Name of a database to delete |
| **hardDelete** | bool | Should all data be removed (data files, indexing files, etc.). Default: `false` |

### Example

{CODE delete_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

{PANEL/}

{PANEL:**EnsureDatabaseExists - extension method**}

This extension method creates database on a server with **default configuration and indexes** if that database does not exist.

### Syntax

{CODE ensure_database_exists_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

{CODE ensure_database_exists_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | Name of a database that will be created if it does not exist |
| **ignoreFailures** | bool | Ignore any exceptions that could heve occurred during database creation. Default: `false` |

### Example

{CODE ensure_database_exists_3@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

{PANEL/}

## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   
