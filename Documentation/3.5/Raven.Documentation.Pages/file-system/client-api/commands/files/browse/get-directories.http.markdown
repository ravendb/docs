#Commands: GetDirectories

The **GET** method is designated to retrieve the paths of subdirectories of a specified directory. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/folders/subdirectories/{directory}?start={start}&pageSize={pageSize}  \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **directory** | Yes | The directory path (empty value means the root directory) |
| **start** | No | The number of results that should be skipped (for paging purposes) |
| **pageSize** | No | The max number of results to get |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Array** | The response consists of array of subdirectory paths. |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/folders/subdirectories
< HTTP/1.1 200 OK
["/books","/movies","/pdfs"]
{CODE-BLOCK/}
