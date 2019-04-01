# Commands: How to start backup or restore operations?

To start or restore backup use `StartBackup` or `StartRestore` operations respectively.

{PANEL:StartBackup}

{CODE backup_restore_1@ClientApi\Commands\HowTo\StartBackupRestoreOperations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **backupLocation** | string | Path to directory where backup will be stored. |
| **databaseDocument** | [DatabaseDocument](../../../glossary/database-document) | Database configuration document that will be stored with backup in 'Database.Document' file. Pass `null` to use the one from `<system>` database.<br />**WARNING**: Database configuration document may contain sensitive data which will be decrypted and stored in backup. |
| **incremental** | bool | Indicates if backup is incremental. |
| **databaseName** | string | Name of a database that will be backed up. |

### Example

{CODE backup_restore_3@ClientApi\Commands\HowTo\StartBackupRestoreOperations.cs /}

{PANEL/}

{PANEL:StartRestore}

{CODE backup_restore_2@ClientApi\Commands\HowTo\StartBackupRestoreOperations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **restoreRequest** | [DatabaseRestoreRequest](../../../glossary/database-restore-request) | Restore information |

## Example

{CODE backup_restore_4@ClientApi\Commands\HowTo\StartBackupRestoreOperations.cs /}

{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to **compact** database?](../../../client-api/commands/how-to/compact-database)     
