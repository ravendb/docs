# Operations: How to Get Connection String

* Get a connection string's properties using the [**GetConnectionStringsOperation**](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#getconnectionstringsoperation) method.

* To learn how to **Create** a connection string see: [Add Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)


In this page:

* [GetConnectionStringsOperation](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#GetConnectionStringsOperation)  
* [Definitions](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#definitions)
* [Code Samples](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#code-samples)

{PANEL: `GetConnectionStringsOperation`}

{CODE get_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | Data Type | Description |
| ------------- | ----- | ---- |
| **connectionStringName** | `string` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `Raven`, `Sql`, or `Olap`|


| Type | Return Value | Return Value Type | Description |
| ---- | ------------- | ----- | --- |
| `SqlConnectionStrings` | Dictionary<string, SqlConnectionString> | Dictionary that maps sql connection string name to definition |
| `RavenConnectionStrings` | Dictionary<string, RavenConnectionString> | Dictionary that maps raven connection string name to definition |
| `OlapConnectionStrings` |  Dictionary<string, OlapConnectionString> | Dictionary that maps olap connection string name to definition |

{PANEL/}

{PANEL: Definitions}

#### `RavenConnectionString`

A Raven connection string definition:

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### `SqlConnectionString`

An Sql connection string definition:

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### `OlapConnectionString`

An Olap connection string definition:

{CODE:csharp olap_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

#### `ConnectionString`

A generic connection string definition:

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

{PANEL: Code Samples}

#### Get all Connection Strings

{CODE get_all_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

#### Get Connection String By Name and Type

{CODE get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
