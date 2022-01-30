# Operations: How to Add a Connection String

* You can add a connection string by using **PutConnectionStringOperation**.

* This article explains how to define the properties needed to connect to an external database.  

* Standard properties (name, database location, password) are explained here.  

In this page:

* [Syntax](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#syntax)  
* [Configurations](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#configurations)  
* [Example - Add Raven Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#example---add-raven-connection-string)  
* [Example - Add Sql Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#example---add-sql-connection-string)  
* [Example - Add Olap Connection String - Local Machine](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#example---add-olap-connection-string---local-machine)  
 * [Example - Add Olap Connection String - AWS Cloud](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#example---add-olap-connection-string---aws-cloud)  

## Syntax

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |

###Configurations

####RavenConnectionString Configuration

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####SqlConnectionString Configuration

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

####OlapConnectionString Configuration

{CODE:csharp olap_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

####ConnectionString

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Raven Connection String

**Secure servers**  
To connect a secure RavenDB server you need to export the certificate from the source server and install it into the destination server.  
The `.pfx` certificate can be found in your server installation folder.  
Be sure that the node definition in the connection string has the "s" in https:  

{CODE add_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Sql Connection String

{CODE add_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Olap Connection String - Local Machine

{CODE olap_Etl_Connection_String@ClientApi\Operations\ConnectionStrings.cs /}

## Example - Add Olap Connection String - AWS Cloud

{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\ConnectionStrings.cs /}


## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
