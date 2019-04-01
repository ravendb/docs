# Operations: Server: How to Restore a Database from the Backup

To restore a database from its backup, use **RestoreBackupOperation**. 

{NOTE This article describes restoring a database using a C# client. You can also restore a database using [RavenDB Studio](../../../studio/server/databases/create-new-database/from-backup). /}

## Syntax

{CODE:csharp restore_1@ClientApi\Operations\Server\Restore.cs /}

{CODE:csharp restore_2@ClientApi\Operations\Server\Restore.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Database name to create during the restore operation |
| **BackupLocation** | string | Directory containing backup files |
| **LastFileNameToRestore** | string | Used for partial restore |
| **DataDirectory** | string | Optional: Database data directory |
| **EncryptionKey** | string | Encryption key used for restore |

##Example

{CODE:csharp restore_3@ClientApi\Operations\Server\Restore.cs /}
