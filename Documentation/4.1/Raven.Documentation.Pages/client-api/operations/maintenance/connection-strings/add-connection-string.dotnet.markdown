# Operations: How to Add a Connection String

* You can add a connection string by using the [PutConnectionStringOperation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation) method.

* This article demonstrates how to connect to an external database.  

In this page:

* [PutConnectionStringOperation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#putconnectionstringoperation)  
* [Add a Raven Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-raven-connection-string)  
* [Add an Sql Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string)  

{PANEL: `PutConnectionStringOperation`}

{CODE add_connection_string@ClientApi\Operations\ConnectionStrings.cs /}

| Parameters | Connection String Type | Description |
| ------------- | ----- | ---- |
| **connectionString** | `Raven` | Connection string to create: `RavenConnectionString` |
| **connectionString** | `Sql` | Connection string to create: `SqlConnectionString` |


#### `ConnectionString`

{CODE:csharp connection_string@ClientApi\Operations\ConnectionStrings.cs /}

{PANEL/}



{PANEL: Add a Raven Connection String}

{NOTE: Secure servers}
 To [connect secure RavenDB servers](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates) 
 you need to 

  1. Export the server certificate from the source server. 
  2. Install it as a client certificate on the destination server.  

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

