# Operations: How to Add ETL

You can add ETL task by using **AddEtlOperation**.

## Syntax

{CODE add_etl_operation@ClientApi\Operations\AddEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example - Add Raven ETL

{CODE add_raven_etl@ClientApi\Operations\AddEtl.cs /}

## Example - Add Sql ETL

{CODE add_sql_etl@ClientApi\Operations\AddEtl.cs /}

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
