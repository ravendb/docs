# Commands: How to get names of all databases on a server?

To download all available database names from a server, use the `GetDatabaseNames` command from `GlobalAdmin`.

## Syntax

{CODE get_database_names_1@ClientApi\Commands\HowTo\GetDatabaseNames.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **pageSize** | int | Maximum number of records that will be downloaded |
| **start** | int | Number of records that should be skipped. Default: `0` |

| Return Value | |
| ------------- | ----- |
| string[] | Names of all databases on a server |

## Example

{CODE get_database_names_2@ClientApi\Commands\HowTo\GetDatabaseNames.cs /}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
