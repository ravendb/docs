# Operations: How to Add ETL

In this page:

* [Syntax](../../../../client-api/operations/maintenance/etl/add-etl#syntax)  
* [Example - Add Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-raven-etl)  
    * [Connection String for Raven ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-raven-etl)  
* [Example - Add Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#example---add-sql-etl)  
    * [Connection String for Sql ETL](../../../../client-api/operations/maintenance/etl/add-etl#connection-string-for-sql-etl)  



You can add ETL task by using **AddEtlOperation**.

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

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
