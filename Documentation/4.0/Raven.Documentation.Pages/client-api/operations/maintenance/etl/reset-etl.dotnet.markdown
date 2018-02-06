# Operations : How to reset ETL?

ETL process continues to send documents where it last finished. To reset pointer to last sent document and start from the beginning you should use **ResetEtlOperation**.

## Syntax

{CODE reset_etl_1@ClientApi\Operations\ResetEtl.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **configurationName** | string | ETL configuration name |
| **transformationName** | string | Name of ETL transformation name |

## Example

{CODE reset_etl_2@ClientApi\Operations\ResetEtl.cs /}
