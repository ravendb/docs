# Commands: Transformers: Delete

**DeleteTransformer** is used to remove a transformer from a database.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/transformers/{transformerName} \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **transformerName** | Yes | name of a transformer |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No content |

<hr />

## Example

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/NorthWind/transformers/Order/Statistics" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

## Related articles

- [GetTransformer](../../../client-api/commands/transformers/get)  
- [PutTransformer](../../../client-api/commands/transformers/put)  
