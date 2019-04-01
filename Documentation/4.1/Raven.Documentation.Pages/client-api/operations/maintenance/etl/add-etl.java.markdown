# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE:java add_etl_operation@ClientApi\Operations\AddEtl.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL

{CODE:java add_raven_etl@ClientApi\Operations\AddEtl.java /}

## Example - Add Sql ETL

{CODE:java add_sql_etl@ClientApi\Operations\AddEtl.java /}

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
