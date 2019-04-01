# Attachments: How to get attachment metadata only?

There are few methods that allow you to download attachment metadata from a database:   
- [HeadAttachment](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#head)   
- [GetAttachmentHeadersStartingWith](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only#getattachmentheadersstartingwith)   

{PANEL:**HeadAttachment**}

**HeadAttachment** can be used to download attachment metadata for a single attachment.

### Syntax


{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/{key} \
	-X HEAD
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes | key of the attachment you want to download metadata for |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Header | Description |
| -------- | - |
| any header | attachment metadata |

<hr />

### Example

{CODE-BLOCK:json}
curl -X HEAD "http://localhost:8080/databases/sample/static/sea.jpg"
< HTTP/1.1 200 OK
< ETag: "02000000-0000-0002-0000-000000000002"
< Description: Holidays 2014
{CODE-BLOCK/}

{PANEL/}

{PANEL:**GetAttachmentHeadersStartingWith**}

**GetAttachmentHeadersStartingWith** can be used to download attachment metadata for a multiple attachments.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static? \
		&startsWith={startsWith}  \
		&start={start} \
		&pageSize={pageSize} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **idPrefix** | Yes | prefix for which attachments should be returned |
| **start** | No | number of attachments that should be skipped |
| **pageSize** | No | maximum number of attachments that will be returned |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | list of [AttachmentInformation](../../../../glossary/attachment-information) |

<hr />

### Example


{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/sample/static/?startsWith=albums/&pageSize=128&start=0"
< HTTP/1.1 200 OK
[
	{
		"Size":4,
		"Key":"sea.jpg",
		"Metadata":{"Description":"Holidays 2014","Content-Type":"application/json; charset=UTF-8"},
		"Etag":"02000000-0000-0002-0000-000000000001"
	},
	... another attachment information ...
]
{CODE-BLOCK/}

{PANEL/}


## Related articles

- [How to **update** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  
- [GetAttachment](../../../../client-api/commands/attachments/get)  
