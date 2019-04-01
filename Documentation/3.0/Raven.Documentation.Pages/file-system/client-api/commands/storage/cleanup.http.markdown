#Commands: CleanUp

The **POST** method forces to run a background task that will clean up files marked as deleted. Read [Background tasks](../../../server/background-tasks) article for details.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/storage/cleanup  \
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
| `204` | NoContent |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X POST http://localhost:8080/fs/NorthwindFS/storage/cleanup  \
	-d ""
< HTTP/1.1 204 NoContent

{CODE-BLOCK/}
