# Operations: How to Get Connection String

You can get connection strings by using **GetConnectionStringsOperation**.

## Syntax

{CODE:java get_connection_strings@ClientApi\Operations\ConnectionStrings.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionStringName** | `String` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `RAVEN` or `SQL` |


| Return Value | | |
| ------------- | ----- | --- |
| `sqlConnectionStrings` | Map<String, SqlConnectionString> | Map which maps sql connection string name to definition |
| `ravenConnectionStrings` | Map<String, RavenConnectionString> | Map which maps raven connection string name to definition |


####RavenConnectionString 

{CODE:java raven_connection_string@ClientApi\Operations\ConnectionStrings.java /}

####SqlConnectionString

{CODE:java sql_connection_string@ClientApi\Operations\ConnectionStrings.java /}

####ConnectionString

{CODE:java connection_string@ClientApi\Operations\ConnectionStrings.java /}

## Example - Get all Connection Strings

{CODE:java get_all_connection_strings@ClientApi\Operations\ConnectionStrings.java /}

## Example - Get Connection String By Name and Type

{CODE:java get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.java /}

## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
