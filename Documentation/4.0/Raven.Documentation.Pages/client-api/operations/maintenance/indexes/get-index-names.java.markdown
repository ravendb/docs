# Operations : How to get Index Names

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

- [How to **get indexes**?](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to **put indexes**?](../../../../client-api/operations/maintenance/indexes/put-indexes)
- [How to **delete index**?](../../../../client-api/operations/maintenance/indexes/delete-index)
