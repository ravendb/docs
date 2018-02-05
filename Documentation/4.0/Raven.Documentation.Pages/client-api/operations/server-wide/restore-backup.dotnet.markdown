# Operations : Server : How to Restore Database from Backup?

To restore database from backup use **RestoreBackupOperation**. 

{NOTE This article describes restoring database using C# client. You can also restore database using [RavenDB Studio](../../../studio/server/databases/create-new-database/from-backup). /}

## Syntax

{CODE:csharp restore_1@ClientApi\Operations\Server\Restore.cs /}

{CODE:csharp restore_2@ClientApi\Operations\Server\Restore.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Database name to create during restore operation |
| **BackupLocation** | string | Directory containing backup files |
| **LastFileNameToRestore** | string | Used to partial restore |
| **DataDirectory** | string | Optional: Database data directory |
| **EncryptionKey** | string | Encryption key used for restore |

##Example

{CODE:csharp restore_3@ClientApi\Operations\Server\Restore.cs /}
