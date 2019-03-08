# Operations : How to Add Connection String

You can add connection string by using **PutConnectionStringOperation**.

## Syntax

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |

## Example - Add Raven Connection String

{CODE add_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Sql Connection String

{CODE add_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
