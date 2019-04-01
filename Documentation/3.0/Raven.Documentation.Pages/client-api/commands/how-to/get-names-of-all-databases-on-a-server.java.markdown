# Commands: How to get names of all databases on a server?

To download all available database names from a server, use the `getDatabaseNames` command from `GlobalAdmin`.

## Syntax

{CODE:java get_database_names_1@ClientApi\Commands\HowTo\GetDatabaseNames.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **pageSize** | int | Maximum number of records that will be downloaded |
| **start** | int | Number of records that should be skipped. Default: `0` |

| Return Value | |
| ------------- | ----- |
| String[] | Names of all databases on a server |

## Example

{CODE:java get_database_names_2@ClientApi\Commands\HowTo\GetDatabaseNames.java /}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
