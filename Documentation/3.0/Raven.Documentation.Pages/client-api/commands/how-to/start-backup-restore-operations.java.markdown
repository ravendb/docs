# Commands: How to start backup or restore operations?

To start or restore backup use `startBackup` or `startRestore` operations respectively.

{PANEL:StartBackup}

{CODE:java backup_restore_1@ClientApi\Commands\HowTo\StartBackupRestoreOperations.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **backupLocation** | String | Path to directory where backup will be stored. |
| **databaseDocument** | [DatabaseDocument](../../../glossary/database-document) | Database configuration document that will be stored with backup in 'Database.Document' file. Pass `null` to use the one from `<system>` database.<br />**WARNING**: Database configuration document may contain sensitive data which will be decrypted and stored in backup. |
| **incremental** | boolean | Indicates if backup is incremental. |
| **databaseName** | String | Name of a database that will be backed up. |

### Example

{CODE:java backup_restore_3@ClientApi\Commands\HowTo\StartBackupRestoreOperations.java /}

{PANEL/}

{PANEL:StartRestore}

{CODE:java backup_restore_2@ClientApi\Commands\HowTo\StartBackupRestoreOperations.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **restoreRequest** | [DatabaseRestoreRequest](../../../glossary/database-restore-request) | Restore information |

## Example

{CODE:java backup_restore_4@ClientApi\Commands\HowTo\StartBackupRestoreOperations.java /}

{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to **compact** database?](../../../client-api/commands/how-to/compact-database)     
