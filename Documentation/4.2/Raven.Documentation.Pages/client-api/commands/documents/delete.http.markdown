# Commands: Documents: Delete

---
{NOTE: }  

* Use this endpoint with the `DELETE` method to delete one document from the database:  
`http://[server URL]/databases/[database name]/docs`

* In this page:    
  * [Request and Response Formats](../../../client-api/commands/documents/delete#request-and-response-formats)
      * [Request](../../../client-api/commands/documents/delete#request)
      * [Response](../../../client-api/commands/documents/delete#response)
      * [Example](../../../client-api/commands/documents/delete#example)

{NOTE/}  

---

{PANEL: Request and Response Formats}

### Request

{CODE-BLOCK: bash}
curl \
    'http://[server URL]/databases/[database name]/docs?id=[document ID]' \
    -X DELETE \
    --header 'If-Match: [expected change vector]'
{CODE-BLOCK/}

| Query Parameters | Description | Required |
| - | - | - |
| **id** | ID of a document to be deleted. Only one document can be deleted per request. | Required |

| Headers | Description | Required |
| - | - | - |
| **If-Match** | Expected [change vector](../../../server/clustering/replication/change-vector). If it matches the server-side change vector the document is deleted, if they don't match a concurrency exception is thrown. | Optional |

### Response

| Header | Description |
| - | - |
| **status** | Http status code |
| **Server** | Web server |
| **Date** | Date and time of response (UTC) |
| **Content-Type** | MIME media type and character encoding |
| **Raven-Server-Version** | Version or RavenDB the responding server is running |

| HTTP Status Code | Description |
| - | - |
| `204` | The document was successfully deleted |
| `409` | The change vector submitted did not match the server-side change vector. A concurrency exception was thrown. |

### Example

An example request sent to the RavenDB playground server:

{CODE-BLOCK: bash}
curl -X DELETE \
  'http://live-test.ravendb.net/databases/Demo/docs?id=employees/1-A' \
{CODE-BLOCK/}

Response:

{CODE-BLOCK: Http}
HTTP/1.1 204
status: 204
Server: nginx
Date: Tue, 27 Aug 2019 11:40:12 GMT
Connection: keep-alive
Raven-Server-Version: 4.2.3.42
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)

### Server

- [Change Vector](../../../server/clustering/replication/change-vector)
