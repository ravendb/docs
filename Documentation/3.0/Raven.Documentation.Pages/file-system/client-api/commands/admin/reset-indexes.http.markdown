#Commands: ResetIndexes

The **POST** method forces RavenFS to rebuild Lucene indexes from scratch.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{filesystemName}/admin/reset-index  \
	-X POST \
    -d ""
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Empty |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X POST http://localhost:8080/fs/NorthwindFS/admin/reset-index \
    -d ""
< HTTP/1.1 200 OK

{CODE-BLOCK/}
