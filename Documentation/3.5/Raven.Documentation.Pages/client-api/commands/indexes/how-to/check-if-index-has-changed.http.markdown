# Commands: Indexes: How to check if an index has changed?

**IndexHasChanged** will let you check if the given index definition differs from the one on a server. This might be useful when you want to check the prior index deployment, if index will be overwritten, and if indexing data will be lost.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName}?op=hasChanged \
	-X POST
	-d @indexDefinition.txt
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| [IndexDefinition](../../../../glossary/index-definition) |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Name** | index name |
| **Changed** | true/false |

<hr />

### Example I

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals?op=hasChanged" 
	-d @indexDefinition.txt
< HTTP/1.1 200 OK
{"Name":"Orders/Totals","Changed":false}
{CODE-BLOCK/}

## Related articles

- [GetIndex](../../../../client-api/commands/indexes/get)  
- [PutIndex](../../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../../client-api/commands/indexes/delete)  
