# Operations : How to Get an Index

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

- [How to **get indexes**?](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to **put indexes**?](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
