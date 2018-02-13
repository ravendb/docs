# Operations : How to Reset ETL

ETL is processing documents from the point where the last batch finished. To start the processing from the very beginning you can reset the ETL by using **ResetEltOperation**.

## Syntax

{CODE reset_etl_1@ClientApi\Operations\ResetEtl.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **configurationName** | string | ETL configuration name |
| **transformationName** | string | Name of ETL transformation |

## Example

{CODE reset_etl_2@ClientApi\Operations\ResetEtl.cs /}
