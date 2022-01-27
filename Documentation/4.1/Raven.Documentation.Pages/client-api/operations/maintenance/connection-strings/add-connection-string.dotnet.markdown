# Operations: How to Add a Connection String

* You can add a connection string by using the [**PutConnectionStringOperation**](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation) method.

* This article demonstrates how to connect to an external database.  

In this page:

* [PutConnectionStringOperation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation)  
* [Add a Raven Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-raven-connection-string)  
* [Add a Sql Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-sql-connection-string)  

{PANEL: `PutConnectionStringOperation`}

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | Connection String Type | Description |
| ------------- | ----- | ---- |
| **connectionString** | `T` | Connection string to create: `RavenConnectionString` or `SqlConnectionString` |


#### `ConnectionString`

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}



{PANEL: Add a Raven Connection String}

{NOTE: Secure servers}
 To connect a secure RavenDB server you need to export the certificate from the source server and install it into the destination server.  
 This can be done easily in the RavenDB Studio -> Server Management -> [Certificates view](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates).
{NOTE/}

{CODE add_raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

* `RavenConnectionString` 
  {CODE:csharp raven_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}



{PANEL: Add an Sql Connection String}

{CODE add_sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

* `SqlConnectionString` 
  {CODE:csharp sql_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

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

