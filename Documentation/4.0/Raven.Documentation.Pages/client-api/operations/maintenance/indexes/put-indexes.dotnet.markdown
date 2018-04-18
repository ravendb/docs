# Operations : How to Put Indexes

**PutIndexesOperation** is used to insert indexes into a database.

### Syntax

{CODE put_1_0@ClientApi\Operations\Indexes\Put.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexToAdd** | `params IndexDefinition[]` | Definitions of indexes |

| Return Value | |
| ------------- | ----- |
| PutIndexResult[] | List of created indexes |

### Example I

{CODE put_1_1@ClientApi\Operations\Indexes\Put.cs /}

### Example II

{CODE put_1_2@ClientApi\Operations\Indexes\Put.cs /}

## Related Articles

- [How to **reset index**?](../../../../client-api/operations/maintenance/indexes/reset-index)
- [How to **get index**?](../../../../client-api/operations/maintenance/indexes/get-index)
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
