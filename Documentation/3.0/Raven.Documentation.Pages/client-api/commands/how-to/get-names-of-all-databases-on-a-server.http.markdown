# Commands : How to get names of all databases on a server?

## Syntax

{CODE-BLOCK:json}
  curl -X GET http://{serverName}/databases/?pageSize={pageSize}&start={start}
{CODE-BLOCK/}

### Request

| Query parameters | Required | |
| ------------- | -- | ---- |
| **pageSize** | int | Maximum number of records that will be downloaded |
| **start** | int | Number of records that should be skipped. Default: `0` |

### Response

| Status code | |
| ----------- | - |
| `200` | OK |

| Return Value | |
| ------------- | ------------- |
| payload | array of database names |

## Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases?pageSize=8&start=0" 
< HTTP/1.1 200 OK
["db2","disabled","Documentation","http","NewNorthwind","NorthWind","rep1","sample"]
{CODE-BLOCK/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
