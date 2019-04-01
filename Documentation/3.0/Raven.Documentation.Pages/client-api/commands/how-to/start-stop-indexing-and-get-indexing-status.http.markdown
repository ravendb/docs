# Commands: How to start or stop indexing and get indexing status?

Following endpoints have been created to enable user to toggle indexing and retrieve indexing status:   
- [StartIndexing](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#startindexing)   
- [StopIndexing](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#stopindexing)   
- [GetIndexingStatus](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#getindexingstatus)

{PANEL:StartIndexing}

Starts indexing, if it was previously stopped.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/admin/startIndexing? \
	&concurrency={concurrency} \
	-X POST 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **concurrency** | No | if set then maximum number of parallel indexing tasks will be set to this value |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content |

<hr />

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/admin/StartIndexing" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}

{PANEL:StopIndexing}

Stops indexing, if it was running.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/admin/StopIndexing \
	-X POST 
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content |

<hr />

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/admin/StopIndexing" 
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}

{PANEL:GetIndexingStatus}

Retrieves current status of the indexing.

### Syntax

{CODE-BLOCK:json}
  curl -X GET http://{serverName}/databases/{databaseName}/admin/IndexingStatus
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **IndexingStatus** | `Indexing` or `Paused` |

<hr />

### Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/admin/IndexingStatus" 
< HTTP/1.1 200 OK
{"IndexingStatus":"Indexing"}
{CODE-BLOCK/}

{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
