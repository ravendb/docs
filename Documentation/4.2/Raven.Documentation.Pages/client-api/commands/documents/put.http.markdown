# Commands: Documents: Put

---
{NOTE: }  

* Use this endpoint with the `PUT` method to upload documents to the database:  
`http://[server URL]/databases/[database name]/docs`  

* In this page:  
  * [Request and Response Formats](../../../client-api/commands/documents/put#request-and-response-formats)  
      * [Request](../../../client-api/commands/documents/put#request)  
      * [Response](../../../client-api/commands/documents/put#response)  
      * [Examples](../../../client-api/commands/documents/put#examples)  

{NOTE/}  

---

{PANEL: Request and Response Formats}

### Request

{CODE-BLOCK: bash}
curl -X PUT \
    'http://[server URL]/databases/[database name]/docs?id=[document ID]' \
    --header 'If-Match: [expected change vector]' \
    -d [JSON document]
{CODE-BLOCK/}

| Query Parameters | Description | Required |
| - | - | - |
| **id** | Unique ID under which the new document will be stored, or the ID of an existing document to be updated. | Required |

| Headers | Description | Required |
| - | - | - |
| **If-Match** | When updating an existing document, this header passes the document's expected [change vector](../../../server/clustering/replication/change-vector). If this change vector doesn't match the document's server-side change vector, a concurrency exception is thrown. | Optional |

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
| `201` | The document was successfully stored / the *existing* document was successfully updated |
| `409` | The change vector submitted did not match the server-side change vector. A concurrency exception was thrown. |

### Examples

Example requests sent to the RavenDB playground server:  
1) Store new document "person/1-A" in the collection "People".  

{CODE-BLOCK: bash}
curl -X PUT \
    'http://live-test.ravendb.net/databases/Demo/docs?id=person/1-A' \
    -d '{ 
    "FirstName":"Jane", 
    "LastName":"Doe",
    "Age":42,
    "@metadata":{
		"@collection": "People"
	}
}'
{CODE-BLOCK/}

Response:  

{CODE-BLOCK: Http}
HTTP/1.1 201
status: 201
Server: nginx
Date: Tue, 27 Aug 2019 10:58:28 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
Vary: Accept-Encoding
Raven-Server-Version: 4.2.3.42

{
    "Id":"person/1-A",
    "ChangeVector":"A:1"
}
{CODE-BLOCK/}

2) Update that same document.  

{CODE-BLOCK: bash}
curl \
    'http://live-test.ravendb.net/databases/Demo/docs?id=person/1-A' \
    -X PUT \
    --header 'If-Match: A:1' \
    -d '{ 
    "FirstName":"John", 
    "LastName":"Smith",
    "Age":24,
}'
{CODE-BLOCK/}

Response is identical except for the updated change vector:  

{CODE-BLOCK: Http}
HTTP/1.1 201
status: 201
Server: nginx
Date: Tue, 27 Aug 2019 10:58:28 GMT
Content-Type: application/json; charset=utf-8
Transfer-Encoding: chunked
Connection: keep-alive
Content-Encoding: gzip
Vary: Accept-Encoding
Raven-Server-Version: 4.2.3.42

{
    "Id":"person/1-A",
    "ChangeVector":"A:3"
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Client API

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)

### Server

- [Change Vector](../../../server/clustering/replication/change-vector)
