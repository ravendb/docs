# Sharding: Restore
---

{NOTE: }

* A sharded database's backup is made of a set of backup files that were 
  created by the database's shards.  
* To restore a sharded database, we need to provide 
  [RestoreBackupOperation](../../../../client-api/operations/maintenance/backup/restore) 
  with the locations of the backup files.  
* We must restore the database in order: first shard, second shard, 
  and so on. Failing to do so will create a functional database but 
  documents will be misplaced in the wrong buckets and shards.  
* Backup files may be restored from shard machines that stored them 
  locally, or from remote locations like an AWS S3 bucket or an Azure 
  destination.  
* A backed up sharded database can be restored in part or in full, 
  to a sharded or a non-sharded database.  
* `.ravendbdump` and backup files can be imported into a sharded database.  

* In this page:  
  * [Restore](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore)  
     * [Restoring a Database: Configuration and Execution](../../../../)  
     * [RestoreBackupOperation](../../../../)  
     * [RestoreBackupConfigurationBase](../../../../)  
     * [SingleShardRestoreSetting](../../../../)  
  * [Import](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#import)  

{NOTE/}

---

{PANEL: Restore}

To restore a sharded database, provide 
[RestoreBackupOperation](../../../../client-api/operations/maintenance/backup/restore) 
with paths to the locations of the backup files, in the original shards order.  

#### Restoring a Database: Configuration and Execution

* To restore the database:  
  {CODE-BLOCK:json}
var restoreOperation = new RestoreBackupOperation(new RestoreBackupConfiguration
  {
     DatabaseName = databaseName,
     ShardRestoreSettings = settings
  });
  {CODE-BLOCK/}

    * Configure a `RestoreBackupConfiguration` instance.  
    * Define all shard-related information within the instance's `ShardRestoreSettings` property 
      including, for each shard, the shard number, node tag, and backup file/s folder name.  
    * Pass the defined `RestoreBackupConfiguration` instance to `RestoreBackupOperation` for execution.  


---

#### `RestoreBackupOperation`
This is the restore operation.
{CODE restore_RestoreBackupOperation@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

---
#### `RestoreBackupConfigurationBase`
This is the restore operation configuration.  
{CODE restore_RestoreBackupConfigurationBase@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | `string` | Name for the new database. |
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | `string` | [Last incremental backup file](../../../../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | `string` | The new database data directory. <br> **Default folder: Under the "Databases" folder, in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | `string` | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | `boolean` | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | `boolean` | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
    | **Type** | `RestoreType` | Restore from local or cloud storage <br> {CODE restore_RestoreType@Sharding\Server\OngoingTasks\BackupAndRestore.cs /} |
    | **ShardRestoreSettings** | `ShardedRestoreSettings` | a dictionary of `SingleShardRestoreSetting` instances defining the shard files to restore <br> {CODE restore_ShardedRestoreSettings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /} |
    | **BackupEncryptionSettings** | `BackupEncryptionSettings` | [Backup Encryption Settings](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key) |
  
    {WARNING: }
    * Verify that RavenDB has full access to the backup-files and database folders.
    * Make sure your server has permissions to read from `BackupLocation` and write to `DataDirectory`.
    {WARNING/}

#### `SingleShardRestoreSetting`
{CODE restore_SingleShardRestoreSetting@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

{PANEL/}

{PANEL: Import}

`.ravendbdump` files, as well as full and incremental backups, 
can be imported into a sharded database using 
[studio](../../../../studio/database/tasks/import-data/import-data-file) 
or [smuggler](../../../../client-api/smuggler/what-is-smuggler#import).  

This is helpful, for example, when we wan to create a new database 
out of a single shard, or to restore only a part of a database.  
  
{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  

