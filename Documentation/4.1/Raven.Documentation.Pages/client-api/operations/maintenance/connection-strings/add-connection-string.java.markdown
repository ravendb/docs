# Operations: How to Add Connection String

You can add a connection string by using **PutConnectionStringOperation**.

## Syntax

{CODE:java add_connection_string@ClientApi\Operations\ConnectionStrings.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |

####RavenConnectionString 

{CODE:java raven_connection_string@ClientApi\Operations\ConnectionStrings.java /}

####SqlConnectionString

{CODE:java sql_connection_string@ClientApi\Operations\ConnectionStrings.java /}

####ConnectionString

{CODE:java connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Example - Add Raven Connection String

{CODE:java add_raven_connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Example - Add Sql Connection String

{CODE:java add_sql_connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
