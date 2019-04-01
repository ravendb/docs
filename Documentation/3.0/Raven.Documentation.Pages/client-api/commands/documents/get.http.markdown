# Commands: Documents: Get

There are few methods that allow you to retrieve documents from a database:   
- [Get](../../../client-api/commands/documents/get#get)   
- [Get - multiple documents](../../../client-api/commands/documents/get#get---multiple-documents)   
- [GetDocuments](../../../client-api/commands/documents/get#getdocuments)   
- [StartsWith](../../../client-api/commands/documents/get#startswith)  

{PANEL:Get}

**Get** can be used to retrieve a single document.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/databases/{databaseName}/docs?id={key}  \
	-X GET \
	--header "If-None-Match:{etag}" 
&nbsp;
curl \
	http://{serverUrl}/databases/{databaseName}/docs/{key}  \
	-X GET \
	--header "If-None-Match:{etag}" 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes | unique key under which document is stored |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No |  Used to pass document `Etag` |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |
| `404` | Not found |
| `304` | Not modified |

| Return Value | Description |
| ------------- | ------------- |
| payload | json representing document |

| Header | Description |
| -------- | - |
| **ETag** | document ETag |
| **&#95;&#95;document_id** | Document id |
| **Last-Modified** | Date of last modification |

<hr />

### Example

Get document under key `user/100`. 

{CODE-BLOCK:json}
curl -X GET http://localhost:8080/docs/user/100
< HTTP/1.1 200 OK
< Last-Modified: Thu, 06 Nov 2014 10:58:52 GMT
< ETag: "01000000-0000-0008-0000-00000000000A"
< __document_id: user/100
{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }

{CODE-BLOCK/}

{PANEL/}

{PANEL:Get - multiple documents}

**Get** can also be used to retrieve a list of documents.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/databases/{databaseName}/queries/? \
		metadata-only={metadataOnly}& \
		include={includePath}& \
		transformer={transformerName}& \
		tp-{param}={value}& \
		id={documentKey} \
	-X GET
&nbsp;
curl \
	http://{serverUrl}/databases/{databaseName}/queries/? \
		metadata-only={metadataOnly}& \
		include={includePath}& \
		transformer={transformerName}& \
		tp-{param}={value}& \
		id={documentKey} \
	-X POST
{CODE-BLOCK/}

### Request

| Method | Description |
| -------| - |
| `GET` | document ids length < 1024 |
| `PUT` | document ids length > 1024 (pass them as payload) |

| Query parameter | Required | Multiple allowed | Description |
| ------------- | -- | ---- |
| **id** | Yes | Yes | document id to load |
| **include** | No | Yes | include paths |
| **transformer** | No | No | transformerName to use |
| tp-{param} | No | Yes | Transformer parameter |
| **metadata-only** | No | No | Fetch only metadata |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Results** | List of requested documents |
| **Includes** | List of included documents |

<hr />

### Example I

Get documents with ids: `products/1` and `products/2`.

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/http/queries/?id=products%2F1&id=products%2F2" 
< HTTP/1.1 200 OK
{
	"Results":
	[
		{"Name":"orange","Price":0.0,"@metadata":...},
		{"Name":"apple","Price":0.0, "@metadata":...}
	],
	"Includes":
	[]
}

{CODE-BLOCK/}


### Example II - using includes

Get documents with ids: `products/1` and `products/2`. Include `Category`.

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/queries/?include=Category&id=products%2F1&id=products%2F2" 
< HTTP/1.1 200 OK
{"Results":[
		{"Name":"Chai","Category":"categories/1", ... },
		{"Name":"Chang","Category":"categories/1", ... }
	],
 "Includes":[
		{"Name":"Beverages",...}
	]
}
{CODE-BLOCK/}

<hr />

### Example III - missing documents

Assuming that `products/9999` does not exist. 

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/queries/?&include=Category&id=products%2F1&id=products%2F9999&id=products%2F2" 
< HTTP/1.1 200 OK
{"Results":[
		{"Name":"Chai","Category":"categories/1", ... },
		null,
		{"Name":"Chang","Category":"categories/1", ... }
	],
 "Includes":[]
}
{CODE-BLOCK/}

{PANEL/}

{PANEL:GetDocuments}

**GetDocuments** can be used to retrieve multiple documents.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/databases/{databaseName}/docs? \
		start={start}&  \
		pageSize={pageSize}& \
		metadata-only={metadataOnly}
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | No | number of documents that should be skipped |
| **pageSize** | No | maximum number of documents that will be retrieved |
| **metadata-only** | No | specifies if only document metadata should be returned |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | List of json documents |

<hr />

### Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/docs?start=0&pageSize=32" 
< HTTP/1.1 200 OK
[ json, json, ... ]
{CODE-BLOCK/}

{PANEL/}

{PANEL:StartsWith}

**StartsWith** can be used to retrieve multiple documents for a specified key prefix.

### Syntax

{CODE-BLOCK:json}
curl  \
	http://{serverName}/databases/{databaseName}/docs? \
		startsWith={startsWith}& \
		matches={matches}& \
		exclude={exclude}& \
		start={start}& \
		pageSize={pageSize}& \
		metadata-only={metadata}& \
		skipAfter={skipAfter}& \
		transformer={transformer}& \
		tp-{param}={value} \
	-X GET 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Multiple allowed | Description |
| ------------- | -- | ---- |
| **startsWith** | Yes | No | prefix for which documents should be returned |
| **matches** | No | No | separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **exclude** | No | No | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **start** | No | No | number of documents that should be skipped |
| **pageSize** | No | No | maximum number of documents that will be retrieved |
| **metadataOnly** | No | No | specifies if only document metadata should be returned |
| **skipAfter** | No | No | skip document fetching until given key is found and return documents after that key (default: `null`) |
| **transformer** | No | No |name of a transformer that should be used to transform the results |
| tp-{param} | No | Yes | parameters that will be passed to transformer |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |
| `409` | Conflict |

| Return Value | Description |
| ------------- | ------------- |
| payload | List of json documents |

<hr />

### Example I

Return up to 128 documents with key that starts with 'products'. 

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/docs?startsWith=products%2F&matches=&exclude=&start=0&pageSize=128" 
< HTTP/1.1 200 OK
[ jsonDocument, jsonDocument, ... ]
{CODE-BLOCK/}

### Example II

Return up to 128 documents with key that starts with 'products/' and rest of the key begins with "1" or "2" e.g. `products/10`, `products/25`

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/docs?startsWith=products%2F&matches=1&#42;%7C2&#42;&exclude=&start=0&pageSize=128" 
< HTTP/1.1 200 OK
[ jsonDocument, jsonDocument, ... ]
{CODE-BLOCK/}

### Example III

Return up to 128 documents with key that starts with 'products/' and rest of the key have length of 3, begins and ends with "1" and contains any character at 2nd position e.g. `products/101`, `products/1B1`. 

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/docs?startsWith=products%2F&matches=1%3F1&exclude=&start=0&pageSize=128" 
< HTTP/1.1 200 OK
[ jsonDocument, jsonDocument, ... ]
{CODE-BLOCK/}

{PANEL/}

## Related articles

- [How to **get** document **metadata** only?](../../../client-api/commands/documents/how-to/get-document-metadata-only)  
- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)   
- [How to **stream** documents?](../../../client-api/commands/documents/stream)   
