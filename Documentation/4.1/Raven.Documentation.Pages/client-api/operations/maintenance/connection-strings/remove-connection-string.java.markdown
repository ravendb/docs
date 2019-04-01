# Operations: How to Remove Connection String

You can remove a connection string by using **RemoveConnectionStringOperation**.

## Syntax

{CODE:java remove_connection_string@ClientApi\Operations\ConnectionStrings.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to remove: `RavenConnectionString` or `SqlConnectionString` |

## Example - Remove Raven Connection String

{CODE:java remove_raven_connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Example - Remove Sql Connection String

{CODE:java remove_sql_connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
