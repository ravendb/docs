# Attachments : How to update an attachment metadata only?

**UpdateAttachmentMetadata** is used to update an attachment metadata in a database.

## Syntax

{CODE-BLOCK:json}
  curl -X POST http://{serverName}/databases/{databaseName}/static/{key}
{CODE-BLOCK/}

### Request

| Query parameters | Required |  |
| ------------- | -- | ---- |
| **key** | Yes |  key under which attachment is stored |

| Headers | Required | |
| --------| ------- | --- |
| any header | No |  metadata |
| etag | No | current attachment etag, used for concurrency checks |

### Response

| Status code | |
| ----------- | - |
| `200` | OK |

| Return Value | |
| ------------- | ------------- |
| payload | etag |

## Example


{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/sample/static/sea.jpg" \
	--header "Description: Holidays"

< HTTP/1.1 200 OK
< ETag: "02000000-0000-0002-0000-000000000003"
"02000000-0000-0002-0000-000000000003"
{CODE-BLOCK/}

## Related articles

- [How to **get** attachment **metadata** only?](../../../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [PutAttachment](../../../../client-api/commands/attachments/put)  