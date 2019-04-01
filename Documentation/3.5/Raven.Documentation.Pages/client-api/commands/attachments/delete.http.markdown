# Attachments: Delete

**DeleteAttachment** is used to remove an attachment from a database.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/{key} \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required |  Description |
| ------------- | -- | ---- |
| **key** | Yes | key of an attachment to delete |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No | current attachment etag, used for concurrency checks |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No content |

<hr />

## Example

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/sample/static/sea.jpg" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

## Related articles

- [GetAttachment](../../../client-api/commands/attachments/get)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
