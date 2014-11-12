# Attachments : Delete

**DeleteAttachment** is used to remove an attachment from a database.

## Syntax

{CODE-BLOCK:json}
  curl -X DELETE http://{serverName}/databases/{databaseName}/static/{key}
{CODE-BLOCK/}

### Request

| Query parameters | Required |  |
| ------------- | -- | ---- |
| **key** | Yes | key of an attachment to delete |

| Headers | Required | |
| --------| ------- | --- |
| **etag** | No | current attachment etag, used for concurrency checks |

### Response

| Status code | |
| ----------- | - |
| `204` | No content |

## Example

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/sample/static/sea.jpg" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

## Related articles

- [GetAttachment](../../../client-api/commands/attachments/get)  
- [PutAttachment](../../../client-api/commands/attachments/put)  