# Client API : Commands : How to create or delete database?

This article will describe following commands (and extensions) that enable you to manage databases on a server:   
- [CreateDatabase]()   
- [DeleteDatabase]()   
- [EnsureDatabaseExists]()   

## CreateDatabase

This method is used to create a new database on a server.

### Syntax

{CODE create_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

**Parameters**

databaseDocument
:   Type: [DatabaseDocument]()   
A document containing all configuration options for new database (e.g. active bundles, name/id, path to data)

### Example

{CODE create_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

## DeleteDatabase

This method is used to delete a database from a server, with a possibility to remove all data from hard drive.

### Syntax

{CODE delete_database_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

**Parameters**

dbName
:   Type: string   
Name of a database to delete

hardDelete
:   Type: bool   
Should all data be removed (data files, indexing files, etc.). Default: `false`

### Example

{CODE delete_database_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

## EnsureDatabaseExists - extension method

This extension method creates database on a server with **default configuration and indexes** if that database does not exist.

### Syntax

{CODE ensure_database_exists_2@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

{CODE ensure_database_exists_1@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

**Parameters**

name
:   Type: string   
Name of a database that will be created if it does not exist

ignoreFailures
:   Type: bool   
Ignore any exceptions that could occured during database creation. Default: `false`

### Example

{CODE ensure_database_exists_3@ClientApi\Commands\HowTo\CreateDeleteDatabase.cs /}

#### Related articles

- [How to **switch** commands to different **database**?](../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get database and server **statistics**?](../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../client-api/commands/how-to/start-backup-restore-operations)   