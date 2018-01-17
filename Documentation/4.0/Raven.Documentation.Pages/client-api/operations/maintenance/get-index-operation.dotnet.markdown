# Operations : How to get index?

**GetIndexOperation** is used to retrieve an index definition from a database.

### Syntax

{CODE get_1_0@ClientApi\Operations\Indexes\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index |

| Return Value | |
| ------------- | ----- |
| [IndexDefinition](../../../glossary/index-definition) | Instance of IndexDefinition representing index. |

### Example

{CODE get_1_1@ClientApi\Operations\Indexes\Get.cs /}

## Related articles

- [How to **get indexes**?](../../../client-api/operations/get-indexes-operation)
- [How to **put indexes**?](../../../client-api/operations/put-indexes-operation)
- [How to **delete index**?](../../../client-api/operations/delete-index-operation)
