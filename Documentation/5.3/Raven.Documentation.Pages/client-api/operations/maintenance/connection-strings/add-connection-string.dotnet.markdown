# Operations: How to Add a Connection String

* You can add a connection string by using the [**PutConnectionStringOperation** method](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation).

* This article demonstrates how to define the properties needed to connect to an external database.  

* Standard properties (name, database location, password) are explained here.  

In this page:

* [PutConnectionStringOperation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation)  
* [Add a Raven Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-raven-connection-string)  
* [Add a Sql Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-sql-connection-string)  
* [Add an Olap Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-olap-connection-string)  
* [Add an Elasticsearch Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-elasticsearch-connection-string)  

## PutConnectionStringOperation

{PANEL: }

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |


#### ConnectionString

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

## Add a Raven Connection String

{PANEL: }

{NOTE: Secure servers}
 To connect a secure RavenDB server you need to [export the certificate](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates) from the source server and install it into the destination server.  
 This can be done easily in the RavenDB Studio -> Server Management -> Certificates view.
{NOTE/}

{CODE add_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### RavenConnectionString Configuration

{CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

## Add a Sql Connection String

{PANEL: }

{CODE add_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### SqlConnectionString Configuration

{CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

## Add an Olap Connection String

{PANEL: }

#### To a Local Machine

{CODE olap_Etl_Connection_String@ClientApi\Operations\ConnectionStrings.cs /}

#### To a Cloud-Based Server

To connect to a cloud instance, see the [Olap ETL article](../../../../server/ongoing-tasks/etl/olap#section-1).  
  
The following code sample is for a connection string to Amazon AWS.  
If you use Google or Microsoft cloud servers, change the parameters accordingly.   

{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

#### OlapConnectionString Configuration

{CODE:csharp olap_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}


## Add an Elasticsearch Connection String  

{PANEL: }

{CODE create-connection-string@ClientApi\Operations\ConnectionStrings.cs /}

#### Elasticsearch ConnectionString Configuration

{CODE:csharp elasticsearch_connection_string_config@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)

### ETL (Extract, Transform, Load) Tasks

- [Operations: How to Add ETL](../../../../client-api/operations/maintenance/etl/add-etl)
- [Ongoing Tasks: ETL Basics](../../../../server/ongoing-tasks/etl/basics)

### External Replication

- [External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)
- [How Replication Works](../../../../server/clustering/replication/replication)

