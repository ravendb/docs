# Operations: How to Get Connection String

You can get connection strings by using **GetConnectionStringsOperation**.

## Syntax

{CODE get_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionStringName** | `string` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `Raven` or `Sql` |


| Return Value | | |
| ------------- | ----- | --- |
| `SqlConnectionStrings` | Dictionary<string, SqlConnectionString> | Dictionary which maps sql connection string name to definition |
| `RavenConnectionStrings` | Dictionary<string, RavenConnectionString> | Dictionary which maps raven connection string name to definition |


####RavenConnectionString 

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####SqlConnectionString

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####ConnectionString

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Get all Connection Strings

{CODE get_all_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Get Connection String By Name and Type

{CODE get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.cs /}

## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
