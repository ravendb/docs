# Get Index Errors Operation

**GetIndexErrorsOperation** is used to return errors encountered during document indexing. 

## Syntax

{CODE:java errors_1@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.java /}

{CODE:java errors_2@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.java /}

{CODE:java errors_3@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.java /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Name** | String | Index name |
| **Errors** | IndexingError\[\] | List of indexing errors |

## Example I

{CODE:java errors_4@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.java /}

## Example II

{CODE:java errors_5@ClientApi\Operations\Maintenance\Indexes\GetIndexErrors.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Debugging Index Errors](../../../../indexes/troubleshooting/debugging-index-errors)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
