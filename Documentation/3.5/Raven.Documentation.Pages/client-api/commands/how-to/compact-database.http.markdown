# Commands: How to compact database?

To compact a database, please use compact endpoint.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/admin/compact?database={databaseName} \
	-X POST
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **databaseName** | Yes | Name of a database to compact |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | Operation id |

<hr />

## Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/admin/compact?database=NorthWind"
< HTTP/1.1 200 OK
{"OperationId":1}
{CODE-BLOCK/}

## Remarks

Compacting operation is executed **asynchronously** and during this operation the **database** will be **offline**.

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   

