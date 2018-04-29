# Operations : How to Put Indexes

**PutIndexesOperation** is used to insert indexes into a database.

### Syntax

{CODE:java put_1_0@ClientApi\Operations\Indexes\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexToAdd** | `IndexDefinition...` | Definitions of indexes |

| Return Value | |
| ------------- | ----- |
| PutIndexResult[] | List of created indexes |

### Example I

{CODE:java put_1_1@ClientApi\Operations\Indexes\Put.java /}

### Example II

{CODE:java put_1_2@ClientApi\Operations\Indexes\Put.java /}

## Related Articles

- [How to **reset index**?](../../../../client-api/operations/maintenance/indexes/reset-index)
- [How to **get index**?](../../../../client-api/operations/maintenance/indexes/get-index)
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
