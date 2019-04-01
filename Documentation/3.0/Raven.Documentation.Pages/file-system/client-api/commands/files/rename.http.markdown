#Commands: Rename

The **PATCH** method is used to change the file name. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/files/{name}?rename={newName}  \
	-X PATCH
    --header "If-None-Match:{etag}" 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The name of the file that you want to change |
| **rename** | Yes | The new name of a file |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No |  Used to pass the file `Etag` |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content status means that the rename operation processed successfully |
| `404` | The file was not found |
| `405` | The concurrency exception occurred |
| `420` | The synchronization exception occurred |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any value |

<hr />

## Examples

Execute the following request to change the name `/movies/intro.avi` to `/movies/introduction.avi`

{CODE-BLOCK:json}
curl \
	-X PATCH http://localhost:8080/fs/NorthwindFS/files/movies/intro.avi?rename=/movies/introduction.avi
< HTTP/1.1 204 No Content

{CODE-BLOCK/}
