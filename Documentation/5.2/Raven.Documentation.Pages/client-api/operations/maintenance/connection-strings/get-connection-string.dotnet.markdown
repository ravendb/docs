# Operations: How to Get Connection String

* You can get connection strings by using the [**GetConnectionStringsOperation** method](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#GetConnectionStringsOperation).

* This article explains how to configure the properties for the `GetConnectionStringOperation` method  
  To see how to define the properties of a connection string (e.g. password, database name, etc.), see [Add Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)


In this page:

* [GetConnectionStringsOperation](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#GetConnectionStringsOperation)  
* [RavenConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#ravenconnectionstring-configuration)  
* [SqlConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#sqlconnectionstring-configuration)  
* [OlapConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#olapconnectionstring-configuration)  
* [Generic ConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#generic-connectionstring)  
* [Example - Get all Connection Strings](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-all-connection-strings)  
* [Example - Get Connection String By Name and Type](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-connection-string-by-name-and-type)  

## GetConnectionStringsOperation

{CODE get_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionStringName** | `string` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `Raven`, `Sql`, or `Olap`|


| Return Value | | |
| ------------- | ----- | --- |
| `SqlConnectionStrings` | Dictionary<string, SqlConnectionString> | Dictionary that maps sql connection string name to definition |
| `RavenConnectionStrings` | Dictionary<string, RavenConnectionString> | Dictionary that maps raven connection string name to definition |
| `OlapConnectionStrings` |  Dictionary<string, OlapConnectionString> | Dictionary that maps olap connection string name to definition |


####RavenConnectionString Configuration

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####SqlConnectionString Configuration

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####OlapConnectionString Configuration

{CODE:csharp olap_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

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
