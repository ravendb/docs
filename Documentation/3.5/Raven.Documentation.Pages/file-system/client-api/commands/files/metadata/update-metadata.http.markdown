#Commands: UpdateMetadata

The **POST** operation is used if you need to change just the file's metadata without any modification to its content. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/files/{name}  \
	-X POST \
    --header "Content-Length:0"
	--header "anyKey:anyValue" \
    --header "If-None-Match:{etag}"
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Empty |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The name of modified file |

| Header | Required | Description |
| --------| ------- | --- |
| **Content-Length** | Yes |  Needed to accomplish the `POST` request |
| **If-None-Match** | No |  Used to pass the file `Etag` |
| Any other header | No | Used to pass metadata records |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content status means that operation completed successfully |
| `405` | The concurrency exception occurred |
| `420` | The synchronization exception occurred |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

In order to change metadata of `/movies/intro.avi` by setting `AllowRead` to `None` value, create the `POST` request as follow:

{CODE-BLOCK:json}
curl \
	-X POST http://localhost:8080/fs/NorthwindFS/files/movies/intro.avi  \
	--header "Content-Length:0" \
    --header "AllowRead:None"
< HTTP/1.1 204 NoContent

{CODE-BLOCK/}
