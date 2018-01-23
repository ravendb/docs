# Operations : How to get index errors?

**GetIndexErrorsOperation** is used to return errors encounted during document indexing. 

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
