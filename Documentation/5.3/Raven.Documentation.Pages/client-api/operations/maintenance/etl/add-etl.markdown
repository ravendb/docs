# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL Task

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

## Example - Add Sql ETL Task

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

## Example - Add OLAP ETL Task

{CODE add_olap_etl@ClientApi\Operations\AddEtl.cs /}

## Example - Add Elasticsearch ETL Task

* **Add Elasticsearch Connection String**  
  {CODE create-connection-string@ClientApi\Operations\AddEtl.cs /}

* **Add Elasticsearch ETL Task**  
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
