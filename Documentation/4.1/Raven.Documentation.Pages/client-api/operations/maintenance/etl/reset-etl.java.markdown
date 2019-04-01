# Operations: How to Reset ETL

ETL is processing documents from the point where the last batch finished. To start the processing from the very beginning you can reset the ETL by using **ResetEtlOperation**.

## Syntax

{CODE:java reset_etl_1@ClientApi\Operations\ResetEtl.java /}

| Parameters | | |
| ------------- | ----- | ---- |
| **configurationName** | String | ETL configuration name |
| **transformationName** | String | Name of ETL transformation |

## Example

{CODE:java reset_etl_2@ClientApi\Operations\ResetEtl.java /}

## Related Articles

### ETL

- [Basics](../../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL](../../../../server/ongoing-tasks/etl/sql)

### Studio

- [RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
