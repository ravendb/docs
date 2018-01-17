# Operations : How to put indexes?

**PutIndexesOperation** is used to insert indexes into a database.

### Syntax

{CODE put_1_0@ClientApi\Operations\Indexes\Put.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexToAdd** | params [IndexDefinition](../../../glossary/index-definition)\[\] | Definitions of indexes |

| Return Value | |
| ------------- | ----- |
| PutIndexResult[] | List of created indexes |

### Example I

{CODE put_1_1@ClientApi\Operations\Indexes\Put.cs /}

### Example II

{CODE put_1_2@ClientApi\Operations\Indexes\Put.cs /}

## Related articles

- [How to **reset index**?](../../../client-api/operations/maintenance/reset-index-operation)
- [How to **get index**?](../../../client-api/operations/maintenance/get-index-operation)
- [How to **delete index**?](../../../client-api/operations/maintenance/delete-index-operation)
