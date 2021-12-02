# Operations: How to Add ETL

To learn more about ETL (Extract, Transfer, Load) ongoing tasks, see this article on [ETL Basics](../../../../server/ongoing-tasks/etl/basics).  

You can add ETL task by using **AddEtlOperation**.  

To manage them in the Studio read this article about [ETLs and how to add them via the GUI Studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task).  

---


In this page:

* [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)  
* [Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-raven-etl)  
    * [Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-raven-etl)  
    * [Code Sample to Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-raven-etl)  

* [Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-sql-etl)  
    * [Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-sql-etl)  
    * [Code Sample to Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-sql-etl)  

* [Add OLAP ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-olap-etl)  
    * [Connection String for Olap ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-olap-etl)  
    * [Code Sample to Add Olap ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-to-add-olap-etl)  

* [Add Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#add-elasticsearch-etl-task)  
    * [Connection String for Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-elasticsearch-etl)  
    * [Code Sample to Add Elasticsearch ETL](../../../../client-api/operations/maintenance/etl/add-etl#code-sample-for-elasticsearch-etl)  


## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Add Raven ETL


### Connection String for Raven ETL

**Secure servers**  
To connect a secure RavenDB server you need to export the certificate from the source server and install it into the destination server.  
The `.pfx` certificate can be found in your server installation folder.  
Be sure that the node definition in the connection string has the "s" in https:  

{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

## Add Sql ETL

### Connection String for Sql ETL

{CODE sql_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Sql ETL

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

## Add OLAP ETL

### Connection String for Olap ETL

The following code sample is for a connection string to a local machine.  
  
{CODE olap_Etl_Connection_String@ClientApi\Operations\AddEtl.cs /}
  
To connect to a cloud instance, see the [Olap ETL article](../../../../server/ongoing-tasks/etl/olap#section-1).  
  
The following code sample is for a connection string to Amazon AWS. If you use Google or Microsoft cloud servers, change the parameters accordingly.   
{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Olap ETL

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}



## Add Elasticsearch ETL Task

### Connection String for Elasticsearch ETL
Add an [Elasticsearch Connection String](../../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)  
  {CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}

### Code Sample to Add Elasticsearch ETL
* **Add an [Elasticsearch ETL Task](../../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-etl-task)**  
  {CODE add_elasticsearch_etl@ClientApi\Operations\AddEtl.cs /}

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
