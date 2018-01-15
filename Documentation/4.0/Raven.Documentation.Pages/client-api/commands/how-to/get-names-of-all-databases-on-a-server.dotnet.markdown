# Commands : How to get names of all databases on a server?

To download available database names from a server, use the `GetDatabaseNames` operation.

## Syntax

{CODE get_db_names_interface@ClientApi\Commands\HowTo\GetDatabaseNames.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **pageSize** | int | Maximum number of records that will be downloaded |
| **start** | int | Number of records that should be skipped. |

| Return Value | |
| ------------- | ----- |
| string[] | Names of databases on a server |

## Example

{CODE get_db_names_sample@ClientApi\Commands\HowTo\GetDatabaseNames.cs /}
