# Operations: How to Remove Connection String

You can remove a connection string by using **RemoveConnectionStringOperation**.

## Syntax

{CODE remove_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to remove: `RavenConnectionString` or `SqlConnectionString` |

## Example - Remove Raven Connection String

{CODE remove_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Remove Sql Connection String

{CODE remove_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
