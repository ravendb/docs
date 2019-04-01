# Commands: How to get database configuration?

`GetDatabaseConfiguration` from admin database commands can be used to retrieve database configuration.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/debug/config \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |


