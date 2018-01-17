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

- [GetIndexesOperation](../../../client-api/operations/get-indexes-operation)
- [PutIndexesOperation](../../../client-api/operations/put-indexes-operation)
- [DeleteIndexOperation](../../../client-api/operations/delete-index-operation)
