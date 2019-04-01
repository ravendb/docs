#Commands: GetKey

The **PUT** operation is used to retrieve an object stored as [a configuration item](../../../configurations) in RavenFS.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/config?name={name}  \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The name of an configuration item |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON** | The configuration object |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/config?name=descriptions/intro.avi
< HTTP/1.1 200 OK
{"Author":"Hibernathing Rhinos"}
{CODE-BLOCK/}
