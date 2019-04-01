# Operations: How to Update ETL

You can modify ETL task by using **UpdateEtlOperation**.

## Syntax

{CODE update_etl_operation@ClientApi\Operations\UpdateEtl.cs /}

| Parameters | | |
| ------------- | ----- | ---- |
| **taskId** | long | Current ETL task ID | 
| **configuration** | `EtlConfiguration<T>` | ETL configuration where `T` is connection string type |

## Example

{CODE update_etl_example@ClientApi\Operations\UpdateEtl.cs /}

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
