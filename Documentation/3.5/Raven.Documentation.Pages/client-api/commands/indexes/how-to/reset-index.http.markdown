# Commands: Indexes: How to reset index?

**ResetIndex** will remove all indexing data from a server for a given index so the indexation can start from scratch for that index.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName} \
	-X RESET 
{CODE-BLOCK/}

### Request

| Query parameter | Required |  Description |
| ------------- | -- | ---- |
| **indexName** | Yes | name of an index to reset |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

<hr />

### Example I

Reset index `Orders/Totals`.

{CODE-BLOCK:json}
curl -X RESET "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals" 
< HTTP/1.1 200 OK
{"Reset":"Orders/Totals"}
{CODE-BLOCK/}

## Related articles

- [GetIndex](../../../../client-api/commands/indexes/get)  
- [PutIndex](../../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../../client-api/commands/indexes/delete)  
