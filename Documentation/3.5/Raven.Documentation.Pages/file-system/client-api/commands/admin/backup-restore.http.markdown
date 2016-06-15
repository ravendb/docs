#Backup and restore

Use **POST** methods to start or restore backup.

{NOTE: Non blocking operations}
Execution of the backup or restore request simply starts the operation on the server and returns immediately. 
The request does not wait for the operation to complete.
{NOTE/}

{PANEL:Start backup}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/admin/backup?incremental={incremental}  \
	-X POST \
	--d "{backupRequest}"
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| JSON formatted [FilesystemBackupRequest](../../../../glossary/file-system-backup-request) |

{WARNING: Backup and sensitive data}
The file system configuration document may contain sensitive data which will be decrypted and stored in the backup. 
{WARNING/}

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **fileSystemName** | Yes | The name of the file system to backup |
| **incremental** | No | Indicates if it should be the incremental backup |

### Response

| Status code | Description |
| ----------- | - |
| `201` | Created |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

### Example

{CODE-BLOCK:json}
curl \
    -X POST http://localhost:8080/fs/NorthwindFS/admin/backup?incremental=false \
    -d "{'BackupLocation':'c:\\temp\\backup\\NorthwindFS'}"

< HTTP/1.1 201 Created

{CODE-BLOCK/}
If you are interested in checking the current backup status you can retrieve it by [getting](../configurations/get-key) `Raven/Backup/Status` configuration item.

{PANEL/}


{PANEL:Start restore}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/admin/fs/restore  \
	-X POST \
	-d "{restoreRequest}"
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| JSON formatted [FilesystemRestoreRequest](../../../../glossary/file-system-restore-request) |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | The operation identifier |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	http://localhost:8080/admin/fs/restore  \
	-X POST \
	-d "{'FilesystemName':'NewNorthwindFS','BackupLocation':'c:\\temp\\backup\\NorthwindFS','IndexesLocation':null,'RestoreStartTimeout':null,'FilesystemLocation':'~\\FileSystem\\NewNorthwindFS','Defrag':false,'JournalsLocation':null}"
< HTTP/1.1 200 OK

{"OperationId":1}
{CODE-BLOCK/}
{PANEL/}
