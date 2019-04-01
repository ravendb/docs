# Commands: How to switch command to a different database?

By default, commands available directly in store are working on a default database that was setup for that store. To switch commands to a different database that is available on that server use **ForDatabase** method.

{PANEL:ForDatabase}

{CODE for_database_1@ClientApi\Commands\HowTo\SwitchCommandsToDifferentDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | name of a database for which you want to get new commands |

| Return Value | |
| ------------- | ----- |
| IDatabaseCommands | New instance of commands that is scoped to the requested database. |

### Example

{CODE for_database_3@ClientApi\Commands\HowTo\SwitchCommandsToDifferentDatabase.cs /}

{PANEL/}

{PANEL:ForSystemDatabase}

{CODE for_database_2@ClientApi\Commands\HowTo\SwitchCommandsToDifferentDatabase.cs /}

| Return Value | |
| ------------- | ----- |
| IDatabaseCommands | Method returns new instance of commands that is scoped to the system database. |

### Example

{CODE for_database_4@ClientApi\Commands\HowTo\SwitchCommandsToDifferentDatabase.cs /}

{PANEL/}

## Related articles

- [How to **switch** commands **credentials**?](../../../client-api/commands/how-to/switch-commands-credentials)   
