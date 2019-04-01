#Commands: StartCompact

The **POST** method initializes the compaction of the indicated file system. This operation makes the file system offline for the time of compaction.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/admin/fs/compact?filesystem={filesystemName}  \
	-X POST 
    -d ""
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Empty |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **filesystemName** | Yes | The name of a file system to compact |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | The operation identifier |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	http://localhost:8080/admin/fs/compact?filesystem=NorthwindFS  \
	-X POST \
	-d ""
< HTTP/1.1 200 OK

{"OperationId":1}

{CODE-BLOCK/}
