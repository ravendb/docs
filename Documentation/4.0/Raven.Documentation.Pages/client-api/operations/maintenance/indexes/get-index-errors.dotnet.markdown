# Operations: How to Get Index Errors

**GetIndexErrorsOperation** is used to return errors encountered during document indexing. 

## Syntax

{CODE errors_1@ClientApi\Operations\IndexError.cs /}

{CODE errors_2@ClientApi\Operations\IndexError.cs /}

{CODE errors_3@ClientApi\Operations\IndexError.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Name** | string | Index name |
| **Errors** | IndexingError\[\] | List of indexing errors |

## Example I

{CODE errors_4@ClientApi\Operations\IndexError.cs /}

## Example II

{CODE errors_5@ClientApi\Operations\IndexError.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Debugging Index Errors](../../../../indexes/troubleshooting/debugging-index-errors)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
