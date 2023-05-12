# Operations: Server: How to Restore a Database from the Backup

* To restore a database from its backup, use `RestoreBackupOperation`.  
* A backup can also be restored using [Studio](../../../studio/database/create-new-database/from-backup).  

## Syntax

{CODE:nodejs restore_1@ClientApi\Operations\Server\Restore.js /}

{CODE:nodejs restore_2@ClientApi\Operations\Server\Restore.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | string | Database name to create during the restore operation |
| **lastFileNameToRestore** | string | Used for partial restore |
| **dataDirectory** | string | Optional: Database data directory |
| **encryptionKey** | string | Encryption key used for restore |
| **disableOngoingTasks** | boolean | true/false to disable/enable Ongoing Tasks|
| **skipIndexes** | boolean | true/false to disable/enable indexes import|
| **type** | RestoreType | Encryption key used for restore |
| **backupEncryptionSettings** | BackupEncryptionSettings | Backup encryption settings |

##Example

{CODE:nodejs restore_3@ClientApi\Operations\Server\Restore.js /}

## Related Articles

### Studio Articles
- [Create DB: From Backup](../../../studio/database/create-new-database/from-backup)  
- [Create DB: General Flow](../../../studio/database/create-new-database/general-flow)  
- [Create DB: Encrypted](../../../studio/database/create-new-database/encrypted)  
- [The Backup Task](../../../studio/database/tasks/backup-task)  

### Client Articles
- [Restore](../../../client-api/operations/maintenance/backup/restore)  
- [What Is Smuggler](../../../client-api/smuggler/what-is-smuggler)  
- [Backup](../../../client-api/operations/maintenance/backup/backup)  

### Server Articles
- [Backup Overview](../../../server/ongoing-tasks/backup-overview)  

### Migration Articles
- [Migration](../../../migration/server/data-migration)  
