#Commands: DeleteKey

The **DELETE** method is used to remove a configuration stored under the specified key.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/config?name={name}  \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The configuration name |

### Response

| Status code | Description |
| ----------- | - |
| `204` | NoContent |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X DELETE http://localhost:8080/fs/NorthwindFS/config?name=descriptions/intro.avi
< HTTP/1.1 204 NoContent

{CODE-BLOCK/}
