# Commands: Indexes: Delete

**DeleteIndex** is used to remove an index from a database.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName} \
	-X DELETE 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description  |
| ------------- | -- | ---- |
| **indexName** | Yes | name of an index to delete |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No content |

<hr />

### Example I

Delete index `Orders/Totals`.

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}


## Related articles

- [GetIndex](../../../client-api/commands/indexes/get)  
- [PutIndex](../../../client-api/commands/indexes/put)  
