# Operations : How to get indexes?

**GetIndexesOperation** is used to retrieve multiple index definitions from a database.

### Syntax

{CODE get_2_0@ClientApi\Operations\Indexes\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | string | Number of indexes that should be skipped |
| **pageSize** | int | Maximum number of indexes that will be retrieved  |

| Return Value | |
| ------------- | ----- |
| [IndexDefinition](../../../glossary/index-definition) | Instance of IndexDefinition representing index. |

### Example

{CODE get_2_1@ClientApi\Operations\Indexes\Get.cs /}

## Related articles

- [GetIndexesNames](../../../client-api/operations/get-index-names-operation)
- [PutIndexesOperation](../../../client-api/operations/put-indexes-operation)
- [DeleteIndexOperation](../../../client-api/operations/delete-index-operation)
