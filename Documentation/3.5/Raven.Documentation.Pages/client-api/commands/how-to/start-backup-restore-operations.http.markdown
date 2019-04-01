# Commands: How to start backup or restore operations?

{PANEL:StartBackup}

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/admin/backup?incremental={incremental} \
	-X POST 
{CODE-BLOCK/}

### Request

| Payload | Description |
| ------- | - | 
| **BackupLocation** | Location of backup |
| **DatabaseDocument** | [DatabaseDocument](../../../glossary/database-document) that will be stored with backup in 'Database.Document' file. Pass `null` to use the one from `<system>` database.<br />**WARNING**: Database configuration document may contain sensitive data which will be decrypted and stored in backup. |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **incremental** | No | Indicates if backup is incremental. |

### Response

| Status code | Description |
| ----------- | - |
| `201` | OK |

<hr />

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/Northwind/admin/backup?incremental=false" \
 -d "{\"BackupLocation\":\"c:\\temp\\backup\\Northwind\\\",\"DatabaseDocument\":{\"SecuredSettings\":{},\"Settings\":{},\"Disabled\":false,\"Id\":null}}"
< HTTP/1.1 201 Created
{CODE-BLOCK/}

{PANEL/}

{PANEL:StartRestore}


{CODE-BLOCK:json} 
curl \
	http://{serverName}/admin/restore \
	-X POST
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| [DatabaseRestoreRequest](../../../glossary/database-restore-request) |

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
curl -X POST "http://localhost:8080/admin/restore" \
 -d "{\"DatabaseName\":\"NewNorthwind\",\"BackupLocation\":\"c:\\temp\\backup\\Northwind\\\",\"IndexesLocation\":null,\"RestoreStartTimeout\":null,\"DatabaseLocation\":\"~\\Databases\\NewNorthwind\\\",\"Defrag\":false,\"JournalsLocation\":null}"
< HTTP/1.1 200 OK
{"OperationId":2}
{CODE-BLOCK/}


{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to **compact** database?](../../../client-api/commands/how-to/compact-database)     
