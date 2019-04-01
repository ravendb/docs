# Commands: How to get server build number?

To check with what version of server you are working use `build/version` endpoint.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/build/version \
	-X GET 
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **ProductVersion** | String representing current product version e.g. `"ceacabf"` |
| **BuildVersion** |  String indicating current build version e.g. `"35123"` |

<hr />

## Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/build/version" 
< HTTP/1.1 200 OK
{"ProductVersion":"cdc39ac","BuildVersion":"13"}
{CODE-BLOCK/}


## Remarks

`BuildVersion` for custom builds is `"13"`
