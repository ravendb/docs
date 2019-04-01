# Commands: How to get database and server statistics?

There are two types of statistics available. First type of statistics returns server-wide information, the second one returns statistics for particular database.

{PANEL:Database statistics}

Database statistics can be downloaded using `getStatistics` method in the commands.

### Syntax

{CODE:java database_statistics_1@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.java /}

| Return Value | |
| ------------- | ----- |
| [DatabaseStatistics](../../../glossary/database-statistics) | Current statistics for a database that commands work on |

### Example

{CODE:java database_statistics_2@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.java /}

{PANEL/}

{PANEL:Server statistics}

Server statistics can be downloaded using `getStatistics` method in `GlobalAdmin` commands.

### Syntax

{CODE:java server_statistics_1@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.java /}

| Return Value | |
| ------------- | ----- |
| [AdminStatistics](../../../glossary/admin-statistics) | Current statistics for a server |

### Example

{CODE:java server_statistics_2@ClientApi\Commands\HowTo\GetDatabaseAndServerStatistics.java /}

{PANEL/}

## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
