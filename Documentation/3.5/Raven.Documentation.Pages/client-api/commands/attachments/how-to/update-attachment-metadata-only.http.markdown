# Attachments: How to Update Only Attachment Metadata 

**UpdateAttachmentMetadata** is used to update an attachment's metadata in a database.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/static/{key} \
	-X POST
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **key** | Yes |  key under which the attachment is stored |

| Header | Required | Description |
| --------| ------- | --- |
| any header | No |  metadata |
| **If-None-Match** | No | current attachment etag, used for concurrency checks |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | etag |

<hr />

## Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/sample/static/sea.jpg" \
	--header "Description: Holidays"

< HTTP/1.1 200 OK
< ETag: "02000000-0000-0002-0000-000000000003"
"02000000-0000-0002-0000-000000000003"
{CODE-BLOCK/}

## Related Articles

- [How to **get** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  
