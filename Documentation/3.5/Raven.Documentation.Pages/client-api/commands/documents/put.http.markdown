# Commands: Documents: Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/databases/{databaseName}/docs/{key}  \
	-X PUT \
	-d @jsonData.txt  \
	--header "anyKey:anyValue" \
    --header "If-None-Match:{etag}" 
&nbsp;
curl \
	http://{serverUrl}/databases/{databaseName}/docs \
	-X POST \
	-d @jsonData.txt  \
	--header "anyKey:anyValue" \	
    --header "If-None-Match:{etag}" 
{CODE-BLOCK/}

### Request

| Method | Description |
| -------| - |
| `POST` | document key isn't passed in url and is auto-generated on server side |
| `PUT` | document key is passed in url |

| Payload |
| ------- |
| Json document data |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | for `PUT` | unique key under which document will be stored |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No |  Used to pass document `Etag` |
| Any other header | No | Used to pass document metadata |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK - for `POST` |
| `201` | Created - for `PUT` |
| `409` | When concurrency exception occurred |

| Return Value | Description |
| ------------- | ------------- |
| **Key** | unique key under which document was stored |
| **Etag** | stored document etag |

<hr />

## Examples

Put document under key `items/1` with  `CreatedBy` and `ItemOwner` in metadata.

{CODE-BLOCK:json}
curl \
	-X PUT http://localhost:8080/docs/items/1  \
	-d "{ }"  \
	--header "CreatedBy:Adam Smith" \
	--header "ItemOwner:John Davis"
< HTTP/1.1 201 Created
{"Key":"items/1","ETag":"01000000-0000-0001-0000-000000000004"}
{CODE-BLOCK/}

Put document and auto-generate key on server side.

{CODE-BLOCK:json}
curl -X POST http://localhost:8080/docs \
	-d "{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }"
< HTTP/1.1 200 OK
{"Key":"156b3ed5-ad89-47d9-96fa-ba11811b9fad","ETag":"01000000-0000-0001-0000-000000000004"}
{CODE-BLOCK/}

Attempting to put document with invalid etag ends up with conflict.

{CODE-BLOCK:json}
curl -X PUT http://localhost:8080/docs/user/100  \
	-d "{ }" 
< HTTP/1.1 201 Created
{"Key":"user/100","ETag":"01000000-0000-0008-0000-00000000000A"}
&nbsp;
curl -X PUT http://localhost:8080/docs/user/100 \
	-d "{ }" \
	--header "If-None-Match:01000000-0000-0008-0000-0000000000CC" 
< HTTP/1.1 409 Conflict
{
	"Url":"/docs/user/100",
	"ActualETag":"01000000-0000-0008-0000-00000000000A",
	"ExpectedETag":"01000000-0000-0008-0000-000000000010",
	"Error":"PUT attempted on document 'user/100' using a non current etag"
}
{CODE-BLOCK/}


## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
