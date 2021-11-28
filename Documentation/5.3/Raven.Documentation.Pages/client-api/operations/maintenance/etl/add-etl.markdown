# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL Task

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

### Connection String for Raven ETL

{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

## Example - Add Sql ETL Task

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

{CODE sql_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

## Example - Add OLAP ETL Task

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

### Connection String for Olap ETL

The following code sample is for a connection string to a local machine. Click to see connection keys required for [various levels of security](https://www.connectionstrings.com/olap-analysis-services/).
  
{CODE olap_Etl_Connection_String@ClientApi\Operations\AddEtl.cs /}
  
To connect to a cloud instance, see the [Olap ETL article about ongoing tasks](../../../../server/ongoing-tasks/etl/olap#ongoing-tasks-olap-etl).  
  
The following code sample is for a connection string to Amazon AWS. If you use Google or Microsoft cloud servers, change the parameters accordingly.   
{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\AddEtl.cs /}

## Example - Add Elasticsearch ETL Task

* **Add an [Elasticsearch Connection String](../../../../server/ongoing-tasks/etl/elasticsearch#add-an-elasticsearch-connection-string)**  
  {CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}

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
