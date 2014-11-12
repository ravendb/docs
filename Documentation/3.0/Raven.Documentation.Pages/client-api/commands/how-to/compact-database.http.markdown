# Commands : How to compact database?

To compact a database, please use compact endpoint.

## Syntax

{CODE-BLOCK:json}
  curl -X POST http://{serverName}/admin/compact?database={databaseName}
{CODE-BLOCK/}

### Request

| Query parameters | Required | |
| ------------- | -- | ---- |
| **databaseName** | Yes | Name of a database to compact |

### Response

| Status code | |
| ----------- | - |
| `200` | OK |

| Return Value | |
| ------------- | ------------- |
| OperationId | Operation id |

## Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/admin/compact?database=NorthWind"
< HTTP/1.1 200 OK
{"OperationId":1}
{CODE-BLOCK/}

## Remarks

Currently **only esent** storage supports compaction.

Compacting operation is executed **asynchronously** and during this operation the **database** will be **offline**.

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   

