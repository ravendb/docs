# Commands : How to get database and server statistics?

There are two types of statistics available. First type of statistics returns server-wide information, the second one returns statistics for particular database.

## Database statistics

Database statistics can be downloaded using `GetStatistics` method in commands.

### Syntax

{CODE database_statistics_1@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.cs /}

**Return Value**

Type: [DatabaseStatistics](../../../glossary/client-api/database-statistics)   
Current statistics for a database that commands work on

### Example

{CODE database_statistics_2@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.cs /}

## Server statistics

Server statistics can be downloaded using `GetStatistics` method in `GlobalAdmin` commands.

### Syntax

{CODE server_statistics_1@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.cs /}

**Return Value**

Type: [AdminStatistics](../../../glossary/client-api/admin-statistics)      
Current statistics for a server

### Example

{CODE server_statistics_2@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.cs /}

#### Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   