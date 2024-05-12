# Put Indexes Operation

**PutIndexesOperation** is used to insert indexes into a database.

### Syntax

{CODE:java put_1_0@ClientApi\Operations\Maintenance\Indexes\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexToAdd** | `IndexDefinition...` | Definitions of indexes |

| Return Value | |
| ------------- | ----- |
| PutIndexResult[] | List of created indexes |

### Example I

{CODE:java put_1_1@ClientApi\Operations\Maintenance\Indexes\Put.java /}

### Example II

{CODE:java put_1_2@ClientApi\Operations\Maintenance\Indexes\Put.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
