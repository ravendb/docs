# Operations: How to Get Indexes

**GetIndexesOperation** is used to retrieve multiple index definitions from a database.

### Syntax

{CODE:java get_2_0@ClientApi\Operations\Indexes\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | Number of indexes that should be skipped |
| **pageSize** | int | Maximum number of indexes that will be retrieved  |

| Return Value | |
| ------------- | ----- |
| `IndexDefinition` | Instance of IndexDefinition representing index. |

### Example

{CODE:java get_2_1@ClientApi\Operations\Indexes\Get.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Put Indexes](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
