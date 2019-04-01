# Operations: Server: How to Enable or Disable a Database

To enable or disable a database, use the `ToggleDatabasesStateOperation`.

## Syntax

{CODE toggle_1@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

{CODE toggle_2@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | string | Name of database to toggle state |
| **disable** | bool | `True`, if database should be disabled, `False` is database should be enabled |

| Return Value | | |
| ------------- | -- | ----- |
| **Disabled** | bool | If database was disabled |
| **Name** | string | Name of the database |
| **Success** | bool | If request succeed |
| **Reason** | string | The reason for success or failure. |

## Example I

{CODE toggle_1_0@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

## Example II

{CODE toggle_2_0@ClientApi\Operations\Server\ToggleDatabasesState.cs /}
