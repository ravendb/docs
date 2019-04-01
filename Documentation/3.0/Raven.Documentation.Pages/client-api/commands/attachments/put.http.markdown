# Attachments: Put

**PutAttachment** is used to insert or update an attachment in a database.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/{attachmentName} \
	-X PUT
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Attachment data |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No | current attachment etag, used for concurrency checks |
| any header | No |  Metadata |


### Response

| Status code | Description |
| ----------- | - |
| `201` | Created |

| Header | Description |
| -------- | - |
| **ETag** | Attachment ETag |

<hr />

### Example I

{CODE-BLOCK:json}
curl -X PUT "http://localhost:8080/databases/NorthWind/static/albums/holidays/sea.jpg"  \
	--header "Description: Holidays 2014" \
	-d @sea.jpg

< HTTP/1.1 201 Created
< ETag: "02000000-0000-0004-0000-000000000002"
< Location: /static/albums/holidays/sea.jpg
{CODE-BLOCK/}

## Related articles

- [How to **update** attachment **metadata** only?](../../../client-api/commands/attachments/how-to/update-attachment-metadata-only)  
- [GetAttachment](../../../client-api/commands/attachments/get)  
- [DeleteAttachment](../../../client-api/commands/attachments/delete)  
