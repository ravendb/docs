# Operations: How to Get Connection String

You can get connection strings by using **GetConnectionStringsOperation**.


* This article explains how to configure the properties for `GetConnectionStringOperation`  
  To see how to define the properties of a connection string (e.g. password, database name, etc.), see [Add Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)

* You can get connection strings by using **GetConnectionStringsOperation**.

In this page:

* [Syntax](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#syntax)  
* [RavenConnectionString Configuration](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#ravenconnectionstring-configuration)  
* [SqlConnectionString Configuration](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#sqlconnectionstring-configuration)  
* [OlapConnectionString Configuration](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#olapconnectionstring-configuration)  
* [Generic ConnectionString Configuration](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#generic-connectionstring)  
* [Example - Get all Connection Strings](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-all-connection-strings)  
* [Example - Get Connection String By Name and Type](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-connection-string-by-name-and-type)  

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


####RavenConnectionString Configuration

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####SqlConnectionString Configuration

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####OlapConnectionString Configuration

{CODE:csharp olap_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####Generic ConnectionString

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Get all Connection Strings

{CODE get_all_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Get Connection String By Name and Type

{CODE get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.cs /}





## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
