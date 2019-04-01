#Commands: Search

The **GET** method retrieves the names of configurations that starts with a specified prefix. 

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/config/search?prefix={prefix}&start={start}&pageSize={pageSize}  \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **prefix** | Yes | The prefix value with which the name of a configuration has to start |
| **start** | No | The number of results that should be skipped |
| **pageSize** | No | The maximum number of results that will be returned (Default: 25) |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON** | JSON object representing [configuration search results](../../../../glossary/configuration-search-results)|

<hr />

## Example

In order to get first 25 configuration names which keys start with `descriptions/`` prefix you need to create the following request:

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs/NorthwindFS/config/search?prefix=descriptions/
< HTTP/1.1 200 OK
{
    "ConfigNames":["descriptions/intro.avi"],
    "TotalCount":1,
    "Start":0,
    "PageSize":25
}

{CODE-BLOCK/}
