# Commands: How to start or stop indexing and get indexing status?

Following endpoints have been created to enable user to toggle reducing:   
- [StartReducing](../../../client-api/commands/how-to/start-stop-reducing#startreducing)   
- [StopReducing](../../../client-api/commands/how-to/start-stop-reducing#stopreducing)   

{PANEL:StartReducing}

Starts reducing, if it was previously stopped.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/admin/startReducing \
	-X POST \
    -d ""
{CODE-BLOCK/}

| Payload |
| ------- |
| Empty |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content |

<hr />

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/admin/StartReducing" -d ""
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}

{PANEL:StopReducing}

Stops reducing, if it was running.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/admin/StopReducing \
	-X POST 
    -d ""
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content |

<hr />

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/admin/StopReducing" -d ""
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
