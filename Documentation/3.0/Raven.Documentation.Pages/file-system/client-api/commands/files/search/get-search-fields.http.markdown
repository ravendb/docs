#Commands: GetSearchFields

The **GET** method is used to retrieve the list of all available field names to build a query. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/search/terms?start={start}&pageSize={pageSize}  \
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
| **Array** | JSON formatted array of indexed terms |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/search/terms
< HTTP/1.1 200 OK
[
    "__key",
    "__fileName",
    "__rfileName",
    "__directory",
    "__rdirectory",
    "__directoryName",
    "__rdirectoryName",
    "__modified",
    "__level",
    "RavenFS-Size",
    "Raven-Creation-Date",
    "Last-Modified",
    "Raven-Last-Modified",
    "Creation-Date",
    "Raven-Synchronization-Version",
    "Raven-Synchronization-Source",
    "ETag",
    "__size",
    "__size_numeric",
    "Content-MD5",
    "Content-Length"
]
{CODE-BLOCK/}
