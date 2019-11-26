# Delete a Document

---

{NOTE: }  

* Use this endpoint with the `**DELETE**` method to delete one document from the database:  
`<server URL>/databases/<database name>/docs?id=<document ID>`  

* In this page:    
    * [Example](../../../client-api/rest-api/document-commands/delete-document#example)  
    * [Request Format](../../../client-api/rest-api/document-commands/delete-document#request-format)  
    * [Response Format](../../../client-api/rest-api/document-commands/delete-document#response-format)  

{NOTE/}  

---

{PANEL: Example}

This is a cURL request to delete the document "employees/1-A" from a database named "Example" on our 
[playground server](http://live-test.ravendb.net):  

{CODE-BLOCK: bash}
curl -X DELETE "http://live-test.ravendb.net/databases/Example/docs?id=employees/1-A"
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

{PANEL: Request Format}

This is the general format of the cURL request:  

{CODE-BLOCK: bash}
curl -X DELETE "<server URL>/databases/<database name>/docs?id=<document ID>"
--header "If-Match: <expected change vector>"
{CODE-BLOCK/}

| Query Parameters | Description | Required |
| - | - | - |
| **id** | ID of a document to be deleted. | Yes |

| Headers | Description | Required |
| - | - | - |
| **If-Match** | Expected [change vector](../../../server/clustering/replication/change-vector). If it matches the server-side change vector the document is deleted, if they don't match a concurrency exception is thrown. | No |

{PANEL/}

{PANEL: Response Format}

| Header | Description |
| - | - |
| **Content-Type** | MIME media type and character encoding. This should always be: `application/json; charset=utf-8`. |
| **Raven-Server-Version** | Version or RavenDB the responding server is running |

| HTTP Status Code | Description |
| - | - |
| `204` | The document was successfully deleted, _or_ no document with the specified ID exists. |
| `409` | The change vector submitted did not match the server-side change vector. A concurrency exception is thrown. |

{PANEL/}

## Related Articles  

### Client API  

##### Commands

- [Documents: Delete](../../../client-api/commands/documents/delete)

##### REST API

- [Get All Documents](../../../client-api/rest-api/document-commands/get-all-documents)  
- [Get Documents by ID](../../../client-api/rest-api/document-commands/get-documents-by-id)  
- [Get Documents by Prefix](../../../client-api/rest-api/document-commands/get-documents-by-prefix)  
- [Put a Document](../../../client-api/rest-api/document-commands/put-documents)  
- [Batch Commands](../../../client-api/rest-api/document-commands/batch-commands)  
<br/>
### Server  

- [Change Vector](../../../server/clustering/replication/change-vector)  
