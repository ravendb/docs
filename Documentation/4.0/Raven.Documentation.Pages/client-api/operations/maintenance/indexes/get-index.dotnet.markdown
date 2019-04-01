# Operations: How to Get an Index

**GetIndexOperation** is used to retrieve an index definition from a database.

### Syntax

{CODE get_1_0@ClientApi\Operations\Indexes\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index |

| Return Value | |
| ------------- | ----- |
| `IndexDefinition` | Instance of IndexDefinition representing index. |

### Example

{CODE get_1_1@ClientApi\Operations\Indexes\Get.cs /}

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
