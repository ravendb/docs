# Operations: How to Add a Connection String

You can add a connection string by using **PutConnectionStringOperation**.

## Syntax

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |

####RavenConnectionString 

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####SqlConnectionString

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####ConnectionString

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Raven Connection String

{CODE add_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Sql Connection String

{CODE add_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
