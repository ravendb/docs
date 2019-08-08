# Commands: Documents: Delete

---
{NOTE: }  

* Use this endpoint with the `PUT` method to upload documents to the database:  
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
| **id** | ID of the document to be deleted. | Required |

| Headers | Description | Required |
| - | - | - |
| **If-Match** | Expected [change vector](../../../server/clustering/replication/change-vector). If it matches the server-side change vector the document is deleted, if they don't match a concurrency exception is thrown. | Optional |

### Response

| Status Code | Description |
| - | - |
| 204 ("No Content") | The document was successfully deleted |
| 409 ("Conflict") | The change vector submitted did not match the server side change vector. A concurrency exception was thrown. |

### Example

{CODE delete_sample@ClientApi\Commands\Documents\Delete.cs /}

## Related Articles

### Commands 

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)

### Server

- [Change Vector](../../../server/clustering/replication/change-vector)
