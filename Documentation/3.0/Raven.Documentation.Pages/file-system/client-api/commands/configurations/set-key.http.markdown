#Commands: SetKey

The **PUT** operation is used to store a JSON formatted object as [a configuration item](../../../configurations) under the specified key. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/config?name={name}  \
	-X PUT \
	-d "{json-data}"
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| JSON formatted object |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The name under which the configuration item will be stored |

### Response

| Status code | Description |
| ----------- | - |
| `201` | Created |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

Put a file under name `/movies/intro.avi` with `AllowRead` metadata:

{CODE-BLOCK:json}
curl \
	-X PUT http://localhost:8080/fs/NorthwindFS/config?name=descriptions/intro.avi \
    -d "{Author: 'Hibernathing Rhinos'}"
< HTTP/1.1 201 Created

{CODE-BLOCK/}
