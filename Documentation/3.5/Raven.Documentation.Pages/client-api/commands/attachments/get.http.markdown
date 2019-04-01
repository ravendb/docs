# Attachments: Get

There are few methods that allow you to download attachments from a database:   
- [GetAttachment](../../../client-api/commands/attachments/get#getattachment)   
- [GetAttachments](../../../client-api/commands/attachments/get#getattachments)   

{PANEL:**GetAttachment**}

**GetAttachment** can be used to download a single attachment.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/{key} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes | key of the attachment you want to download |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | attachment stream |

| Header | Description |
| -------- | - |
| **ETag** | attachment ETag |
| any header | metadata |

<hr />

### Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/sample/static/sea.jpg" 

< HTTP/1.1 200 OK
< ETag: "02000000-0000-0002-0000-000000000001"
< Description: Holidays 2014
here goes payload
{CODE-BLOCK/}

{PANEL/}
{PANEL:GetAttachments}

**GetAttachments** can be used to download attachment information for multiple attachments.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/? \
		&pageSize={pageSize} \
		&etag={etag} \
		&start={start} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | Yes | Indicates how many attachments should be skipped |
| **etag** | Yes | ETag from which to start |
| **pageSize** | Yes | maximum number of attachments that will be downloaded |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | list of AttachmentInformation |

<hr />

### Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/sample/static/?pageSize=128&etag=00000000-0000-0000-0000-000000000000&start=0"
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

- [How to **get** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../client-api/commands/attachments/put)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  
