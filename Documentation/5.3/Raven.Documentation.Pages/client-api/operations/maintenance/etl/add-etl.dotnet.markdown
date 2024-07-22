# Operations: How to Add ETL

This API article demonstrates how to create various ETL tasks and the connection strings required.  

* You can add an ETL task by using the [**AddEtlOperation** method](../../../../client-api/operations/maintenance/etl/add-etl#addetloperation).  

* ETL tasks are ongoing tasks that:
  1. Extract selected data from your source database when changes are made or new data is added.
  2. Apply a transform script on the data.
  3. Load the transformed data to a destination that you designate.  

* To learn more about ETL (Extract, Transfer, Load) ongoing tasks, see this article on [ETL Basics](../../../../server/ongoing-tasks/etl/basics).  

* To learn how to manage ETL tasks using the Studio see: [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

---


In this page:

* [AddEtlOperation](../../../../client-api/operations/maintenance/etl/add-etl#addetloperation)  
* [Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-raven-etl)  
    * [Creating a Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-raven-etl)  
    * [Code Sample to Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-raven-etl)  

* [Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-sql-etl)  
    * [Creating a Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-sql-etl)  
    * [Code Sample to Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-sql-etl)  

* [Add OLAP ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-olap-etl)  
    * [Creating a Connection String for Olap ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-olap-etl)  
    * [Code Sample to Add Olap ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-olap-etl)  

* [Add Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-elasticsearch-etl-task)  
    * [Creating a Connection String for Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#creating-a-connection-string-for-elasticsearch-etl)  
    * [Code Sample to Add Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-for-elasticsearch-etl)  


## AddEtlOperation

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

{PANEL: }


## Add Raven ETL

[Raven ETL](../../../../server/ongoing-tasks/etl/raven) tasks enable ongoing Extract, Transform, Load functionality from a RavenDB source database to a RavenDB destination.  


### Creating a Connection String for Raven ETL


{NOTE: Secure servers}
 To [connect secure RavenDB servers](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates) 
 you need to 

  1. Export the server certificate from the source server. 
  2. Install it as a client certificate on the destination server.  

 This can be done in the RavenDB Studio -> Server Management -> [Certificates view](../../../../server/security/authentication/certificate-management#studio-certificates-management-view).
{NOTE/}

* After passing the certificate, you can either create an ETL with a connection string and transformation script [via the studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task) 
  or with the following API.  
  
{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

{PANEL/}


{PANEL: }


## Add Sql ETL

[SQL ETL](../../../../server/ongoing-tasks/etl/sql) tasks enable ongoing Extract, Transform, Load functionality from RavenDB to SQL databases. 
 
### Creating a Connection String for Sql ETL

{CODE sql_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Sql ETL

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

{PANEL/}


{PANEL: }


## Add OLAP ETL
[Olap ETL](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task) is an ETL process that converts RavenDB data to the Apache Parquet file format  
and sends it to local storage, cloud servers or File Transfer Protocol.

### Creating a Connection String for Olap ETL

The following code sample is for a connection string to a local machine.  
  
{CODE olap_Etl_Connection_String@ClientApi\Operations\AddEtl.cs /}
  
To connect to a cloud instance, see the [Olap ETL article](../../../../server/ongoing-tasks/etl/olap#section-1).  
  
The following code sample is for a connection string to Amazon AWS. If you use Google or Microsoft cloud servers, change the parameters accordingly.   
{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Olap ETL

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

{PANEL/}



{PANEL: }

## Add Elasticsearch ETL Task
[Elasticsearch ETL Task](../../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task) tasks enable ongoing 
Extract, Transform, Load functionality from RavenDB to Elasticsearch destinations.  

### Creating a Connection String for Elasticsearch ETL
Add an [Elasticsearch Connection String](../../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)  
  {CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Elasticsearch ETL
  {CODE add_elasticsearch_etl@ClientApi\Operations\AddEtl.cs /}

{PANEL/}


## Related Articles

### Server

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)
- [Elasticsearch ETL](../../../../server/ongoing-tasks/etl/elasticsearch)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
