# Commands: Indexes: How to get index terms?

**GetTerms** will retrieve all stored terms for a field of an index.

## Syntax

{CODE:java get_terms_1@ClientApi\Commands\Indexes\HowTo\GetTerms.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | Name of an index |
| **field** | String | Index field |
| **fromValue** | String | Starting point for a query, used for paging |
| **pageSize** | int | Maximum number of terms that will be returned |

| Return Value | |
| ------------- | ----- |
| String[] | Array of index terms. |

## Example

{CODE:java get_terms_2@ClientApi\Commands\Indexes\HowTo\GetTerms.java /}

## Related articles

- [How to **reset index**?](../../../../client-api/commands/indexes/how-to/reset-index)   
- [How to **get index merge suggestions**?](../../../../client-api/commands/indexes/how-to/get-index-merge-suggestions)   
