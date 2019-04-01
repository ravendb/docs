# Commands: Documents: Delete

**Delete** is used to remove a document from a database.

### Syntax

{CODE-BLOCK:json}
 curl \
	http://{serverName}/databases/{databaseName}/docs/{key} \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes | key of a document to be deleted |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No |  current document etag, used for concurrency checks |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No content |

<hr />

### Example I

Delete document `products/9999`.

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/NorthWind/docs/products%2F9999" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}


## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
