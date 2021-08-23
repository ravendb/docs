# Operations: Server: How to Restore a Database from the Backup

To restore a database from its backup, use **RestoreBackupOperation**. 

{NOTE This article describes restoring a database using a java client. You can also restore a database using [RavenDB Studio](../../../studio/server/databases/create-new-database/from-backup). /}

## Syntax

{CODE:java constructor@ClientApi\Operations\Server\restoreBackup.java /}

{CODE:java get_set@ClientApi\Operations\Server\restoreBackup.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | String | Database name to create during the restore operation |
| **lastFileNameToRestore** | String | Used for partial restore |
| **dataDirectory** | String | Optional: Database data directory |
| **encryptionKey** | String | Encryption key used for restore |
| **disableOngoingTasks** | boolean | Disable on doing tasks |
| **skipIndexes** | boolean | Skip the indexes|

##Example

{CODE:java example_java@ClientApi\Operations\Server\restoreBackup.java /}

## Related Articles

**Studio Articles**:   
[Create a Database : From Backup](../../../studio/server/databases/create-new-database/from-backup)      
[Create a Database : General Flow](../../../studio/server/databases/create-new-database/general-flow)           
[Create a Database : Encrypted](../../../studio/server/databases/create-new-database/encrypted)        
[The Backup Task](../../../studio/database/tasks/backup-task)      

**Client Articles**:  
[Restore](../../../client-api/operations/maintenance/backup/restore)   
[What Is Smuggler](../../../client-api/smuggler/what-is-smuggler)   
[Backup](../../../client-api/operations/maintenance/backup/backup)

**Server Articles**:  
[Backup Overview](../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../migration/server/data-migration) 
