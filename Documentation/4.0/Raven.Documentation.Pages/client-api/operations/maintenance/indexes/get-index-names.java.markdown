# Operations: How to Get Index Names

**GetIndexNamesOperation** is used to retrieve multiple index names from a database.

### Syntax

{CODE:java get_3_0@ClientApi\Operations\Indexes\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | Number of index names that should be skipped |
| **pageSize** | int | Maximum number of index names that will be retrieved |

| Return Value | |
| ------------- | ----- |
| String[] | This methods returns an array of index **name** as a result. |

### Example

{CODE:java get_3_1@ClientApi\Operations\Indexes\Get.java /}


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
