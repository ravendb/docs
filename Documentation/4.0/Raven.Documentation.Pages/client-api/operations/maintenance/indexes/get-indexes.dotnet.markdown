# Operations : How to Get Indexes

**GetIndexesOperation** is used to retrieve multiple index definitions from a database.

### Syntax

{CODE get_2_0@ClientApi\Operations\Indexes\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | string | Number of indexes that should be skipped |
| **pageSize** | int | Maximum number of indexes that will be retrieved  |

| Return Value | |
| ------------- | ----- |
| `IndexDefinition` | Instance of IndexDefinition representing index. |

### Example

{CODE get_2_1@ClientApi\Operations\Indexes\Get.cs /}

## Related Articles

- [How to **get indexes names**?](../../../../client-api/operations/maintenance/indexes/get-index-names)
- [How to **put indexes**?](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
