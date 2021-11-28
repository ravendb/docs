# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

In this page:

* [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)  
* [Example - Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-raven-etl)  
    * [Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-raven-etl)  
* [Example - Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-sql-etl)  
    * [Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-sql-etl)  
* [Example - Add OLAP ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-olap-etl)  
    * [Connection String for Olap ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-olap-etl)  



## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

### Connection String for Raven ETL

{CODE raven_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

## Example - Add Sql ETL

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

### Connection String for Sql ETL

{CODE sql_etl_connection_string@ClientApi\Operations\AddEtl.cs /}

## Example - Add OLAP ETL

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

### Connection String for Olap ETL

The following code sample is for a connection string to a local machine. Click to see connection keys required for [various levels of security](https://www.connectionstrings.com/olap-analysis-services/).
  
{CODE olap_Etl_Connection_String@ClientApi\Operations\AddEtl.cs /}
  
To connect to a cloud instance, see the [Olap ETL article about ongoing tasks](../../../../server/ongoing-tasks/etl/olap#ongoing-tasks-olap-etl).  
  
The following code sample is for a connection string to Amazon AWS. If you use Google or Microsoft cloud servers, change the parameters accordingly.   
{CODE olap_Etl_AWS_connection_string@ClientApi\Operations\AddEtl.cs /}


## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
