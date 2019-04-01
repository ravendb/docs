#Commands: RetryRenaming

The **POST** method runs a background task that will resume unaccomplished file renames. Read [Background tasks](../../../server/background-tasks) article for details.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/storage/retryRenaming  \
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
	-X POST http://localhost:8080/fs/NorthwindFS/storage/retryRenaming  \
	-d ""
< HTTP/1.1 204 NoContent

{CODE-BLOCK/}
