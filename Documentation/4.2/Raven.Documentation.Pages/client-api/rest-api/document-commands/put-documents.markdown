# Put Documents

---

{NOTE: }  

* Use this endpoint with the **`PUT`** method to upload a new document to the database, or update an existing one:  
`<server URL>/databases/<database name>/docs`  

* In this page:  
  * [Examples](../../../client-api/rest-api/document-commands/put-documents#examples)  
  * [Request Format](../../../client-api/rest-api/document-commands/put-documents#request-format)  
  * [Response Format](../../../client-api/rest-api/document-commands/put-documents#response-format)  

{NOTE/}  

---

{PANEL: Examples}

These are cURL requests to a database named "Example" on our [playground server](http://live-test.ravendb.net):  

#### 1) Store new document "person/1-A" in the collection "People".  

{CODE-BLOCK: bash}
curl -X PUT http://live-test.ravendb.net/databases/Example/docs?id=person/1-A
-d "{ 
    \"FirstName\":\"Jane\", 
    \"LastName\":\"Doe\",
    \"Age\":42,
    \"@metadata\":{
        \"@collection\":\"People\"
    }
}"
{CODE-BLOCK/}
Linebreaks are added for clarity.

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

#### 2) Update the document `person/1-A`.  

{CODE-BLOCK: bash}
curl -X PUT http://live-test.ravendb.net/databases/Example/docs?id=person/1-A
--header If-Match: A:1-L8hp6eYcA02dkVIEifGfKg
-d "{ 
    \"FirstName\":\"John\", 
    \"LastName\":\"Smith\",
    \"Age\":24,
    \"@metadata\":{
        \"@collection\": \"People\"
    }
}"
{CODE-BLOCK/}

Response is the same as the previous response except for the updated change vector:  

{CODE-BLOCK: Http}
HTTP/1.1 201
status: 201
Server: nginx
Date: Tue, 27 Aug 2019 10:59:54 GMT
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

{PANEL: Request Format}

This is the general form of a cURL request:  

{CODE-BLOCK: batch}
curl -X PUT <server URL>/databases/<database name>/docs?id=<document ID>
--header If-Match: <expected change vector>
-d "<JSON document>"
{CODE-BLOCK/}

| Query Parameter | Description | Required |
| - | - | - |
| **id** | Unique ID under which the new document will be stored, or the ID of an existing document to be updated. | Yes |

| Headers | Description | Required |
| - | - | - |
| **If-Match** | When updating an existing document, this header passes the document's expected [change vector](../../../server/clustering/replication/change-vector). If this change vector doesn't match the document's server-side change vector, a concurrency exception is thrown. | No |

#### Request body

The body contains a JSON document. This will replace the existing document with the specified ID if one exists, otherwise it 
will become a new document with the specified ID.  

{CODE-BLOCK: javascript}
{
    \"<field>\": \"<value>\",
    ...
    \"@metadata\": {
        \"@collection\": \"<collection name>\",
        ...
    }
}
{CODE-BLOCK/}

When updating an existing document, you'll need to include its [collection](../../../client-api/faq/what-is-a-collection) 
name in the metadata or an exception will be thrown. Exceptions to this rule are documents in the collection `@empty` - 
i.e. not in any collection. A document's collection cannot be modified.  

cURL syntax requires that all double quotes `"` within the request body be escaped: `\"`.Alternatively, you can save the 
document as a file and pass the path to that file as the request body:  

{CODE-BLOCK: batch}
curl -X PUT <server URL>/databases/<database name>/docs?id=<document ID>
-d <@path/yourDocument.txt>
{CODE-BLOCK/}


{PANEL/}

{PANEL: Response Format}

The response body is JSON and contains the document ID and current [change vector](../../../server/clustering/replication/change-vector):

{CODE-BLOCK: javascript}
{
    "Id": "<document ID>",
    "ChangeVector": "<current change vector>"
}
{CODE-BLOCK/}

| Header | Description |
| - | - |
| **Content-Type** | MIME media type and character encoding. This should always be: `application/json; charset=utf-8`. |
| **Raven-Server-Version** | Version of RavenDB the responding server is running |

| HTTP Status Code | Description |
| - | - |
| `201` | The document was successfully stored / updated |
| `409` | The change vector submitted did not match the server-side change vector. A concurrency exception is thrown. |
| `500` | Server error. For example: when the submitted document's collection tag does not match the specified document's collection tag. |

{PANEL/}

## Related Articles  

### Client API  

- [Get All Documents](../../../client-api/rest-api/document-commands/get-all-documents)  
- [Get Documents by ID](../../../client-api/rest-api/document-commands/get-documents-by-id)  
- [Get Documents by Prefix](../../../client-api/rest-api/document-commands/get-documents-by-prefix)  
- [Delete Document](../../../client-api/rest-api/document-commands/delete-document)  
- [Batch Commands](../../../client-api/rest-api/document-commands/batch-commands)  
- [What is a Collection](../../../client-api/faq/what-is-a-collection)  

### Server  

- [Change Vector](../../../server/clustering/replication/change-vector)  
