#Commands: GetKeyNamesAsync

The **GET** method on `/config` endpoint returns names of all stored configurations.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/config?start={start}&pageSize={pageSize}  \
	-X GET 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | No | The number of results that should be skipped |
| **pageSize** | No | The maximum number of results that will be returned |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON Array** | The array of existing configuration names |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/config
< HTTP/1.1 200 OK
["Raven/Sequences/Raven/Etag","Raven/Synchronization/VersionHilo"]
{CODE-BLOCK/}
