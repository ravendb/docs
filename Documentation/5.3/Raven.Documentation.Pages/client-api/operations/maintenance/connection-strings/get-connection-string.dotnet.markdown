# Operations: How to Get Connection String

* You can get connection strings by using the [**GetConnectionStringsOperation** method](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#getconnectionstringsoperation).

* This article explains how to configure the properties for the `GetConnectionStringOperation` method  
  To see how to define the properties of a connection string (e.g. password, database name, etc.), see [Add Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)


In this page:

* [GetConnectionStringsOperation](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#getconnectionstringsoperation)  
* [RavenConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#ravenconnectionstring)  
* [SqlConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#sqlconnectionstring)  
* [OlapConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#olapconnectionstring)  
* [ElasticsearchConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#elasticsearchconnectionstring)  
* [Generic ConnectionString](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#generic-connectionstring)  
* [Example - Get all Connection Strings](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-all-connection-strings)  
* [Example - Get Connection String By Name and Type](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#example---get-connection-string-by-name-and-type)  

## GetConnectionStringsOperation

{CODE get_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | Data Type | Description |
| ------------- | ----- | ---- |
| **connectionStringName** | `string` | Connection string name |
| **type** | `ConnectionStringType` | Connection string type: `Raven`, `Sql`, `Olap`, or `ElasticSearch`|


| Connection String Type | Return Value |  | Description |
| -- | ------------- | ----- | --- |
| Sql | `SqlConnectionStrings` | Dictionary<string, SqlConnectionString> | Dictionary that maps sql connection string name to definition |
| Raven | `RavenConnectionStrings` | Dictionary<string, RavenConnectionString> | Dictionary that maps raven connection string name to definition |
| Olap | `OlapConnectionStrings` |  Dictionary<string, OlapConnectionString> | Dictionary that maps olap connection string name to definition |
| ElasticSearch | `ElasticsearchConnectionStrings` |  Dictionary<string, ElasticsearchConnectionString> | Dictionary that maps elasticsearch connection string name to definition |


#### `RavenConnectionString`

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### `SqlConnectionString`

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### `OlapConnectionString`

{CODE:csharp olap_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

#### `ElasticsearchConnectionString`

{CODE:csharp elasticsearch_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

#### Generic `ConnectionString`

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### Get all Connection Strings

{CODE get_all_connection_strings@ClientApi\Operations\ConnectionStrings.cs /}

#### Get Connection String By Name and Type

{CODE get_connection_string_by_name@ClientApi\Operations\ConnectionStrings.cs /}


## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
