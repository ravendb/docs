# Operations : How to Get Connection String

You can get connection strings by using **GetConnectionStringsOperation**.

## Syntax

{CODE get_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionStringName** | `string` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `Raven` or `Sql` |

## Example - Get all Connection Strings

{CODE get_all_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Get Connection String By Name and Type

{CODE get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.cs /}

## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
