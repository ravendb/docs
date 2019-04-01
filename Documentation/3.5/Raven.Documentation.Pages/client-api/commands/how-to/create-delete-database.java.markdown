# Commands: How to create or delete database?

This article will describe the following commands that enable you to manage databases on a server:   
- [CreateDatabase](../../../client-api/commands/how-to/create-delete-database#createdatabase)   
- [DeleteDatabase](../../../client-api/commands/how-to/create-delete-database#deletedatabase)   
- [EnsureDatabaseExists](../../../client-api/commands/how-to/create-delete-database#ensuredatabaseexists---extension-method)   

{PANEL:**CreateDatabase**}

This method is used to create a new database on a server.

### Syntax

{CODE:java create_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseDocument** | [DatabaseDocument](../../../glossary/database-document) | A document containing all configuration options for new database (e.g. active bundles, name/id, path to data) |

### Example

{CODE:java create_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

{PANEL/}

{PANEL:**DeleteDatabase**}

This method is used to delete a database from a server, with a possibility to remove all the data from hard drive.

### Syntax

{CODE:java delete_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **dbName** | String | Name of a database to delete |
| **hardDelete** | bool | Should all data be removed (data files, indexing files, etc.). Default: `false` |

### Example

{CODE:java delete_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

{PANEL/}

{PANEL:**EnsureDatabaseExists**}

This extension method creates database on a server with **default configuration and indexes** if that database does not exist.

### Syntax

{CODE:java ensure_database_exists_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | Name of a database that will be created if it does not exist |
| **ignoreFailures** | boolean | Ignore any exceptions that could have occurred during database creation. Default: `false` |

### Example

{CODE:java ensure_database_exists_3@ClientApi\Commands\HowTo\CreateDeleteDatabase.java /}

{PANEL/}

## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   
