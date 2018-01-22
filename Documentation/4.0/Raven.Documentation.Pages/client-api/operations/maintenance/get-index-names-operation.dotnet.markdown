# Operations : How to get Index Names

**GetIndexNamesOperation** is used to retrieve multiple index names from a database.

### Syntax

{CODE get_3_0@ClientApi\Operations\Indexes\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | string | Number of index names that should be skipped |
| **pageSize** | int | Maximum number of index names that will be retrieved |

| Return Value | |
| ------------- | ----- |
| string[] | This methods returns an array of index **name** as a result. |

### Example

{CODE get_3_1@ClientApi\Operations\Indexes\Get.cs /}


## Related Articles

- [How to **get indexes**?](../../../client-api/operations/get-indexes-operation)
- [How to **put indexes**?](../../../client-api/operations/put-indexes-operation)
- [How to **delete index**?](../../../client-api/operations/delete-index-operation)
